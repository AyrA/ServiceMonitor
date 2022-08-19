using ServiceMonitor.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ServiceMonitor
{
    static class Program
    {
#if DEBUG
        public static readonly string PluginDir = @"C:\Temp\ServiceMonitor\Plugins";
#else
        public static readonly string PluginDir = Path.Combine(Application.StartupPath, "Plugins");
#endif
        private static FrmMain MainForm = null;
        private static NotifyIcon NFI;

        public static void SetTrayIcon(IEnumerable<PluginStatus> Status)
        {
            var Plugins = Status.ToArray();
            if (Plugins.Length == 0 || Plugins.All(m => !m.Enabled))
            {
                NFI.Icon = Resources.LedCheckOff;
            }
            else
            {
                var ok = Plugins.All(m => !m.Enabled || m.LastError == null);
                NFI.Icon = ok ? Resources.LedCheckOk : Resources.LedCheckError;
            }
        }

        public static void ShowInfo(string Message, string Title, ToolTipIcon Icon)
        {
            if (NFI != null)
            {
                NFI.ShowBalloonTip(10000, Title, Message, Icon);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Load existing plugins
            PluginManager.RestorePlugins(LoadPlugins());
            PluginManager.StartTestLoop();

            //Manual idle event handler so we can start the application with the form in hidden mode
            //We only use this once to set up all other events
            Application.Idle += ApplicationReady;
            //Hide the notify icon before exiting to make sure it won't linger arond
            Application.ApplicationExit += delegate
            {
                NFI.Visible = false;
                NFI.Dispose();
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();
        }

        private static PluginInfo[] LoadPlugins()
        {
            var Plugins = new List<PluginInfo>();

            Plugins.AddRange(PluginManager.FindPlugins(Application.ExecutablePath));

            if (Directory.Exists(PluginDir))
            {
                foreach (var D in Directory.EnumerateDirectories(PluginDir))
                {
                    var PluginName = Path.GetFileName(D);
                    var FullName = Path.Combine(D, PluginName + ".dll");
                    if (File.Exists(FullName))
                    {
                        Plugins.AddRange(PluginManager.FindPlugins(FullName));
                    }
                }
            }
            return Plugins.ToArray();
        }

        private static void ShowForm()
        {
            if (MainForm == null)
            {
                MainForm = new FrmMain();
                MainForm.Disposed += delegate
                {
                    MainForm = null;
                };
                MainForm.SetMenuPlugins(LoadPlugins());
                MainForm.Show();
            }
            else
            {
                MainForm.WindowState = FormWindowState.Normal;
                MainForm.BringToFront();
                MainForm.Show();
                MainForm.Focus();
            }
        }

        private static void ApplicationReady(object sender, EventArgs e)
        {
            //Unregister event immediately
            Application.Idle -= ApplicationReady;
            NFI = new NotifyIcon
            {
                Icon = Resources.LedCheckWarning,
                Visible = true,
                BalloonTipIcon = ToolTipIcon.Info,
                BalloonTipTitle = "Monitoring Status",
                Text = "Service Monitor",
                ContextMenuStrip = new ContextMenuStrip()
            };
            NFI.ContextMenuStrip.Items.Add("&Show").Click += delegate
            {
                ShowForm();
            };
            NFI.ContextMenuStrip.Items.Add("&Exit").Click += delegate
            {
                using (NFI)
                {
                    NFI.Visible = false;
                    Application.Exit();
                }
            };
            NFI.DoubleClick += delegate
            {
                ShowForm();
            };
            SetTrayIcon(PluginManager.Plugins);
        }
    }
}
