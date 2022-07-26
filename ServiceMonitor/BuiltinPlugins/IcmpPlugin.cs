using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceMonitor.BuiltinPlugins
{
    public class IcmpPlugin : IDisposable
    {
        private const int MAGIC = 0x2401450F;

        public static string BaseName => "ICMP Ping";

        public string Name
        {
            get
            {
                return $"{BaseName}: {HostName}";
            }
        }

        public string HostName { get; set; }

        public string LastStatus { get; private set; }

        public DateTime NextCheck { get; private set; } = DateTime.UtcNow;

        public int Interval { get; set; } = 5;
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
            using (var Frm = new FrmIcmpPluginConfig(this))
            {
                if (Frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Frm.ShadowPlugin.Validate();
                        HostName = Frm.ShadowPlugin.HostName;
                        Interval = Frm.ShadowPlugin.Interval;
                        Timeout = Frm.ShadowPlugin.Timeout;
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
            using (var P = new Ping())
            {
                try
                {
                    var Response = P.Send(HostName, Timeout);
                    if (Response.Status != IPStatus.Success)
                    {
                        throw new PingException($"Ping to {Response.Address} failed with status {Response.Status}");
                    }
                    LastStatus = $"{Response.Address} OK; RTT: {Response.RoundtripTime}";
                }
                catch
                {
                    LastStatus = null;
                    throw;
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
                    HostName = BR.ReadString();
                    Interval = BR.ReadInt32();
                    Timeout = BR.ReadInt32();
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
                    BW.Write(HostName);
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
            var CheckResult = Uri.CheckHostName(HostName);
            if (CheckResult == UriHostNameType.Unknown)
            {
                throw new InvalidOperationException("Invalid hostname or IP address");
            }
            if (Interval <= 0)
            {
                throw new InvalidOperationException($"{nameof(Interval)} is not properly set");
            }
            if (Timeout <= 0)
            {
                throw new InvalidOperationException($"{nameof(Timeout)} is not properly set");
            }
        }
    }
}
