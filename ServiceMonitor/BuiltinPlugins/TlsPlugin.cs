using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

namespace ServiceMonitor.BuiltinPlugins
{
    public class TlsPlugin : IDisposable
    {
        private const int MAGIC = 0x216F789B;
        private const SslProtocols SSL_PROTOCOLS = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

        private static readonly Oid SAN = new Oid("2.5.29.17");

        public static string BaseName => "TLS Certificate check";

        public string Name
        {
            get
            {
                return $"{BaseName}: {ConnectHostName}";
            }
        }

        public string ConnectHostName { get; set; }

        public int ConnectPort { get; set; }

        public string CertificateHostName { get; set; }

        public int CertificateLifetimeDays { get; set; }

        public bool IgnoreChainErrors { get; set; }

        public string LastStatus { get; private set; }

        public DateTime NextCheck { get; private set; } = DateTime.UtcNow;

        public int Interval { get; set; } = 24 * 60 * 60;

        public int Timeout { get; set; } = 5000;

        public void Start()
        {
            LastStatus = null;
            NextCheck = DateTime.UtcNow;
        }

        public void Stop()
        {
            LastStatus = null;
        }

        public bool Config()
        {
            using (var Frm = new FrmTlsPluginConfig(this))
            {
                if (Frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Frm.ShadowPlugin.Validate();

                        ConnectHostName = Frm.ShadowPlugin.ConnectHostName;
                        ConnectPort = Frm.ShadowPlugin.ConnectPort;
                        Timeout = Frm.ShadowPlugin.Timeout;

                        CertificateHostName = Frm.ShadowPlugin.CertificateHostName;
                        IgnoreChainErrors = Frm.ShadowPlugin.IgnoreChainErrors;

                        CertificateLifetimeDays = Frm.ShadowPlugin.CertificateLifetimeDays;

                        Interval = Frm.ShadowPlugin.Interval;

                        NextCheck = DateTime.UtcNow.AddSeconds(Interval);
                    }
                    catch
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        public void Test()
        {
            CheckState();
            NextCheck = NextCheck.AddSeconds(Interval);
            using (var C = new TcpClient())
            {
                Exception error = null;
                var T = new Thread(delegate ()
                {
                    try
                    {
                        C.Connect(ConnectHostName, ConnectPort);
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                });
                T.Start();
                if (!T.Join(Timeout))
                {
                    throw new TimeoutException("Connecting to the remote host timed out");
                }
                if (error != null)
                {
                    throw error;
                }

                using (Stream NS = new NetworkStream(C.Client, true))
                {
                    using (var SSL = new SslStream(NS, false, CertificateValidationCallback))
                    {
                        SSL.AuthenticateAsClient(string.IsNullOrWhiteSpace(CertificateHostName) ? ConnectHostName : CertificateHostName, null, SSL_PROTOCOLS, true);
                    }
                }
            }
        }

        public void Load(byte[] data)
        {
            using (var MS = new MemoryStream(data, false))
            {
                using (var BR = new BinaryReader(MS))
                {
                    if (BR.ReadInt32() != MAGIC)
                    {
                        throw new ArgumentException("Data not for this plugin");
                    }
                    ConnectHostName = BR.ReadString();
                    ConnectPort = BR.ReadInt32();
                    CertificateHostName = BR.ReadString();
                    CertificateLifetimeDays = BR.ReadInt32();
                    IgnoreChainErrors = BR.ReadBoolean();
                    Interval = BR.ReadInt32();
                    Timeout = BR.ReadInt32();
                    if (string.IsNullOrEmpty(CertificateHostName))
                    {
                        CertificateHostName = null;
                    }
                }
            }
        }

        public byte[] Save()
        {
            using (var MS = new MemoryStream())
            {
                using (var BW = new BinaryWriter(MS))
                {
                    BW.Write(MAGIC);
                    BW.Write(ConnectHostName);
                    BW.Write(ConnectPort);
                    BW.Write(string.IsNullOrEmpty(CertificateHostName) ? string.Empty : CertificateHostName);
                    BW.Write(CertificateLifetimeDays);
                    BW.Write(IgnoreChainErrors);
                    BW.Write(Interval);
                    BW.Write(Timeout);
                    BW.Flush();
                    return MS.ToArray();
                }
            }
        }

        public void Validate()
        {
            CheckState();
        }

        public void Dispose()
        {
            //NOOP
        }

        private void CheckState()
        {
            var CheckResult = Uri.CheckHostName(ConnectHostName);
            if (CheckResult == UriHostNameType.Unknown)
            {
                throw new InvalidOperationException("Invalid connect hostname or IP address");
            }
            if (CertificateHostName != null)
            {
                CheckResult = Uri.CheckHostName(CertificateHostName);
                if (CheckResult == UriHostNameType.Unknown)
                {
                    throw new InvalidOperationException("Invalid certificate hostname or IP address");
                }
            }
            if (ConnectPort <= ushort.MinValue || ConnectPort >= ushort.MaxValue)
            {
                throw new InvalidOperationException("Port outside of permitted range");
            }
            if (Interval <= 0)
            {
                throw new InvalidOperationException($"{nameof(Interval)} is not properly set");
            }
            if (Timeout <= 0)
            {
                throw new InvalidOperationException($"{nameof(Timeout)} is not properly set");
            }
            if (CertificateLifetimeDays < 0)
            {
                throw new InvalidOperationException($"{nameof(CertificateLifetimeDays)} cannot be negative");
            }
        }

        private static IEnumerable<string> FormatAndSplitName(string Input)
        {
            foreach (var Name in Input.Split(',').Select(m => m.Trim()))
            {
                if (Name.ToUpper().StartsWith("CN="))
                {
                    yield return Name.Substring(3);
                }
                if (Name.ToUpper().StartsWith("DNS NAME="))
                {
                    yield return Name.Substring(9);
                }
                if (Name.ToUpper().StartsWith("IP ADDRESS="))
                {
                    yield return Name.Substring(11);
                }
            }
        }

        private bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            var names = new List<string>();
            if (certificate is X509Certificate2 c2)
            {
                names.AddRange(FormatAndSplitName(c2.SubjectName.Name));
                foreach (var ext in c2.Extensions)
                {
                    if (ext.Oid.Value == SAN.Value)
                    {
                        names.AddRange(FormatAndSplitName(ext.Format(false)));
                    }
                }
            }
            else
            {
                throw new PluginException("Failed to convert certificate instance");
            }
            switch (sslPolicyErrors)
            {
                case SslPolicyErrors.RemoteCertificateNotAvailable:
                    throw new PluginException("Remote server did not send a certificate");
                case SslPolicyErrors.RemoteCertificateNameMismatch:
                    throw new PluginException($"Certificate (names: {string.Join("; ", names.Distinct())}) does not match {CertificateHostName}");
                case SslPolicyErrors.RemoteCertificateChainErrors:
                    if (!IgnoreChainErrors)
                    {
                        if (chain == null || chain.ChainElements.Count < 2)
                        {
                            throw new PluginException("Certificate does not contain a valid chain. Self signed certificate?");
                        }
                        throw new PluginException("Certificate does not contain a valid chain. Custom CA or misconfigured server?");
                    }
                    break;
                default:
                    break;
            }
            //Check date
            var Expires = c2.NotAfter.ToUniversalTime();
            var Now = DateTime.UtcNow;
            var TotalDays = Expires.Subtract(Now).TotalDays;
            if (Expires < Now)
            {
                throw new PluginException($"Certificate expired on {Expires} UTC ({-TotalDays:0.0} days ago)");
            }
            if (Now.AddDays(CertificateLifetimeDays) > Expires)
            {
                throw new PluginException($"Certificate expires soon: {Expires} UTC ({TotalDays:0.0} days)");
            }
            LastStatus = $"Certificate expires on {Expires} UTC ({TotalDays:0.0} days)";
            return sslPolicyErrors == SslPolicyErrors.None || (IgnoreChainErrors && sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors);
        }
    }
}
