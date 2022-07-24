using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Authentication;
using System.Threading;
using System.Windows.Forms;

namespace ServiceMonitor.BuiltinPlugins
{
    public class HttpPlugin : IDisposable
    {
        private const int MAGIC = 0x503CAC08;

        public static string BaseName => "HTTP Status check";

        public string Name
        {
            get
            {
                return $"{BaseName}: {URL.Host}";
            }
        }

        public Uri URL { get; set; } = new Uri("https://example.com/");

        public int StatusCode { get; set; } = 200;

        public DateTime NextCheck { get; private set; } = DateTime.UtcNow;

        public bool IgnoreTlsError { get; set; } = false;

        public int Interval { get; set; } = 60;

        public int TcpTimeout { get; set; } = 5000;

        public void Start()
        {
            NextCheck = DateTime.UtcNow;
        }

        public void Stop()
        {
            //NOOP
        }

        public bool Config()
        {
            using (var Frm = new FrmHttpPluginConfig(this))
            {
                if (Frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Frm.ShadowPlugin.Validate();
                        URL = Frm.ShadowPlugin.URL;
                        Interval = Frm.ShadowPlugin.Interval;
                        TcpTimeout = Frm.ShadowPlugin.TcpTimeout;
                        StatusCode = Frm.ShadowPlugin.StatusCode;
                        IgnoreTlsError = Frm.ShadowPlugin.IgnoreTlsError;
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

            IPAddress[] Addr = null;

            switch (URL.HostNameType)
            {
                case UriHostNameType.Unknown:
                case UriHostNameType.Basic:
                    throw new NotSupportedException($"The host name in the URL {URL} is not supported on this system");
                case UriHostNameType.Dns:
                    Addr = Dns.GetHostAddresses(URL.DnsSafeHost);
                    break;
                case UriHostNameType.IPv4:
                case UriHostNameType.IPv6:
                    Addr = new IPAddress[] { IPAddress.Parse(URL.Host) };
                    break;
            }

            if (Addr is null || Addr.Length == 0)
            {
                throw new Exception($"Failed to resolve {URL.DnsSafeHost}");
            }

            using (var C = new TcpClient())
            {
                Exception error = null;
                var T = new Thread(delegate ()
                {
                    try
                    {
                        C.Connect(Addr, URL.Port);
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                });
                T.Start();
                if (!T.Join(TcpTimeout))
                {
                    throw new TimeoutException("Connecting to the remote host timed out");
                }
                if (error != null)
                {
                    throw error;
                }

                Stream NS = new NetworkStream(C.Client, false);
                if (URL.Scheme.ToUpper() == "HTTPS")
                {
                    var SSL = IgnoreTlsError ? new SslStream(NS, false, delegate { return true; }) : new SslStream(NS, false);
                    try
                    {
                        var Protocol = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                        SSL.AuthenticateAsClient(URL.Host, null, Protocol, true);
                    }
                    catch
                    {
                        SSL.Dispose();
                        throw;
                    }
                    NS = SSL;
                }
                using (NS)
                {
                    var ReqLines = new string[]
                    {
                        $"HEAD {URL.PathAndQuery} HTTP/1.1",
                        $"Host: {URL.Host}",
                        $"User-Agent: AyrA-ServiceMonitor/{Assembly.GetExecutingAssembly().GetName().Version}",
                        "Accept: */*",
                        "Connection: Close"
                    };
                    using (var SW = new StreamWriter(NS))
                    {
                        SW.NewLine = "\r\n";
                        foreach (var S in ReqLines)
                        {
                            SW.WriteLine(S);
                        }
                        SW.WriteLine();
                        SW.Flush();
                        NS.Flush();
                        using (var SR = new StreamReader(NS))
                        {
                            var Header = SR.ReadLine();

                            //Read all remaining headers
                            while (!string.IsNullOrWhiteSpace(SR.ReadLine())) ;

                            //Extract status code
                            var HeaderParts = Header.Split(' ');
                            if (HeaderParts.Length < 2)
                            {
                                throw new ProtocolViolationException($"First line of HTTP response is invalid. Was: {Header}");
                            }
                            if (!int.TryParse(HeaderParts[1], out int ResponseCode))
                            {
                                throw new ProtocolViolationException($"First line of HTTP response is invalid. Expected a status code but got {HeaderParts[1]}");
                            }
                            if (ResponseCode != StatusCode)
                            {
                                throw new Exception($"Test failed. Got code {ResponseCode} but expected {StatusCode}");
                            }
                        }
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
                    URL = new Uri(BR.ReadString());
                    StatusCode = BR.ReadInt32();
                    Interval = BR.ReadInt32();
                    TcpTimeout = BR.ReadInt32();
                    IgnoreTlsError = BR.ReadBoolean();
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
                    BW.Write(URL.AbsoluteUri);
                    BW.Write(StatusCode);
                    BW.Write(Interval);
                    BW.Write(TcpTimeout);
                    BW.Write(IgnoreTlsError);
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
            if (URL == null)
            {
                throw new InvalidOperationException("URL is null");
            }
            if (!URL.IsAbsoluteUri)
            {
                throw new InvalidOperationException("URL is not absolute");
            }
            if (URL.Scheme.ToLower() != "http" && URL.Scheme.ToLower() != "https")
            {
                throw new InvalidOperationException("URL must be HTTP or HTTPS");
            }
            if (Interval <= 0)
            {
                throw new InvalidOperationException("Interval is not properly set");
            }
            if (TcpTimeout <= 0)
            {
                throw new InvalidOperationException("TcpTimeout is not properly set");
            }
            if (StatusCode < 100 || StatusCode > 999)
            {
                throw new InvalidOperationException("HTTP status code is not properly set");
            }
        }
    }
}
