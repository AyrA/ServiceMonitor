using System;
using System.Threading;

namespace ServiceMonitor
{
    public class PluginStatus
    {
        public event Action<PluginStatus, Exception> TestComplete = delegate { };

        public bool IsTesting => T != null;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                if (value)
                {
                    Plugin.Stop();
                }
                else
                {
                    Plugin.Start();
                }
            }
        }
        public Exception LastError => Plugin.LastError;
        public Plugin Plugin { get; }

        private Thread T = null;
        private bool enabled;

        public PluginStatus(Plugin P)
        {
            Plugin = P ?? throw new ArgumentNullException(nameof(P));
            Enabled = true;
        }

        public bool BeginTest(bool ValidateCheckTime)
        {
            if (T != null)
            {
                throw new InvalidOperationException("Test already ongoing");
            }
            if (Plugin.Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            if (ValidateCheckTime && Plugin.NextCheck.ToUniversalTime() > DateTime.UtcNow)
            {
                return false;
            }
            T = new Thread(delegate ()
            {
                var Previous = LastError;
                Plugin.Test();
                T = null;
                TestComplete(this, Previous);
            })
            {
                IsBackground = true
            };
            T.Start();
            return true;
        }

        public void WaitForResult()
        {
            var TTest = T;
            if (TTest != null)
            {
                TTest.Join();
            }
        }
    }
}