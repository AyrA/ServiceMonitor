using System;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ServiceMonitor.BuiltinPlugins
{
    public class ServicePlugin : IDisposable
    {
        private const int MAGIC = 0x1E6DD5F1;

        private string FullServiceName = null;

        public static string BaseName => "Service status";

        public string Name
        {
            get
            {
                var TempName = FullServiceName;
                if (TempName == null)
                {
                    TempName = ServiceName;
                }
                if (string.IsNullOrEmpty(ComputerName) || ComputerName == ".")
                {
                    return $"{BaseName}: {TempName}";
                }
                return $"{BaseName}: {TempName} on {ComputerName}";
            }
        }

        public string ServiceName { get; set; }

        public string ComputerName { get; set; }

        public ServiceControllerStatus ExpectedStatus { get; set; } = ServiceControllerStatus.Running;

        public DateTime NextCheck { get; private set; } = DateTime.UtcNow;

        public int Interval { get; set; } = 5;

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
            using (var Frm = new FrmServicePluginConfig(this))
            {
                if (Frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Frm.ShadowPlugin.Validate();
                        ServiceName = Frm.ShadowPlugin.ServiceName;
                        ComputerName = Frm.ShadowPlugin.ComputerName;
                        ExpectedStatus = Frm.ShadowPlugin.ExpectedStatus;
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
            ServiceController Service;
            try
            {
                if (string.IsNullOrWhiteSpace(ComputerName) || ComputerName == ".")
                {
                    Service = new ServiceController(ServiceName);
                }
                else
                {
                    Service = new ServiceController(ServiceName, ComputerName);
                }
            }
            catch
            {
                throw;
            }
            using (Service)
            {
                if (Service.Status != ExpectedStatus)
                {
                    throw new Exception($"Service {ServiceName} is not in expected state. Expected: {ExpectedStatus}; Current: {Service.Status}");
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
                    ServiceName = BR.ReadString();
                    ComputerName = BR.ReadString();
                    ExpectedStatus = (ServiceControllerStatus)BR.ReadInt32();
                    Interval = BR.ReadInt32();
                    if (string.IsNullOrWhiteSpace(ComputerName))
                    {
                        ComputerName = null;
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
                    BW.Write(ServiceName);
                    BW.Write(string.IsNullOrWhiteSpace(ComputerName) ? string.Empty : ComputerName);
                    BW.Write((int)ExpectedStatus);
                    BW.Write(Interval);
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
            if (string.IsNullOrWhiteSpace(ServiceName))
            {
                throw new InvalidOperationException("Invalid service name");
            }
            else
            {
                //Try to get display name, substitute with service name if not available
                try
                {
                    using (var S = new ServiceController(ServiceName))
                    {
                        FullServiceName = S.DisplayName;
                    }
                    if (string.IsNullOrWhiteSpace(FullServiceName))
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    FullServiceName = ServiceName;
                }
            }
            if (!Enum.IsDefined(typeof(ServiceControllerStatus), ExpectedStatus))
            {
                throw new InvalidOperationException($"{nameof(ExpectedStatus)} is invalid");
            }
            if (Interval <= 0)
            {
                throw new InvalidOperationException("Interval is not properly set");
            }
        }
    }
}
