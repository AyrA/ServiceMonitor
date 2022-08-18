using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace ServiceMonitor
{
    public static class PluginManager
    {
        private const int CONFIG_VERSION = 1;

        private static Thread T;

        public static readonly string ConfigFile = Path.Combine(Application.StartupPath, "config.bin");

        public static readonly List<PluginStatus> Plugins = new List<PluginStatus>();

        public static void StartTestLoop()
        {
            if (T != null)
            {
                throw new InvalidOperationException("Test loop already running");
            }
            T = new Thread(delegate ()
            {
                while (T != null)
                {
                    var DT = DateTime.UtcNow;
                    foreach (var PI in Plugins.Where(m => m.Enabled))
                    {
                        if (!PI.IsTesting)
                        {
                            try
                            {
                                PI.BeginTest(true);
                            }
                            catch
                            {
                                //NOOP
                            }
                        }
                    }
                    //Wait until the next second
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }
            })
            {
                IsBackground = true
            };
            T.Start();
        }

        public static void StopTestLoop()
        {
            var TTest = T;
            T = null;
            if (TTest != null)
            {
                TTest.Join();
            }
        }

        public static PluginInfo[] FindPlugins(string DllFile, bool SkipInvalid = false)
        {
            var Ret = new List<PluginInfo>();
            //Loading plugins in a separate domain allows us to unload them again.
            var AD = AppDomain.CreateDomain("PluginDetection");
            var A = AD.Load(System.IO.File.ReadAllBytes(DllFile));
            var PluginTypes = A.GetExportedTypes();
            AppDomain.Unload(AD);

            foreach (var T in PluginTypes)
            {
                if (T.Name.EndsWith("Plugin") && T.Name.Length > "Plugin".Length)
                {
                    try
                    {
                        //Try to instantiate the plugin to validate it
                        var P = new Plugin(T);
                        Ret.Add(new PluginInfo()
                        {
                            BaseName = P.BaseName,
                            PluginType = T,
                            AssemblyFile = DllFile
                        });
                    }
                    catch (PluginException)
                    {
                        if (!SkipInvalid)
                        {
                            throw;
                        }
                    }
                }
            }
            return Ret.ToArray();
        }

        public static Plugin LoadPlugin(string PluginName, string DllFile)
        {
            if (string.IsNullOrEmpty(PluginName))
            {
                throw new ArgumentException($"'{nameof(PluginName)}' cannot be null or empty.", nameof(PluginName));
            }

            if (string.IsNullOrEmpty(DllFile))
            {
                throw new ArgumentException($"'{nameof(DllFile)}' cannot be null or empty.", nameof(DllFile));
            }

            var A = Assembly.LoadFrom(DllFile);
            return new Plugin(A.GetType(PluginName));
        }

        public static Plugin LoadPlugin(PluginInfo Info)
        {
            if (Info is null)
            {
                throw new ArgumentNullException(nameof(Info));
            }
            return LoadPlugin(Info.PluginType.FullName, Info.AssemblyFile);
        }

        public static void SavePluginData()
        {
            using (var MS = new MemoryStream())
            {
                using (var BW = new BinaryWriter(MS))
                {
                    var All = Plugins.ToArray();
                    BW.Write(CONFIG_VERSION);
                    BW.Write(All.Length);
                    foreach (var PI in All)
                    {
                        var Data = PI.Plugin.Save();
                        if (Data == null)
                        {
                            Data = new byte[0];
                        }
                        if (Data.Length > 0)
                        {
                            Data = NativeMethods.Encrypt(Data);
                        }
                        BW.Write(PI.Plugin.PluginType.FullName);
                        BW.Write(PI.Enabled);
                        BW.Write(Data.Length);
                        BW.Write(Data);
                    }
                    BW.Flush();
                    using (var FS = File.Create(ConfigFile))
                    {
                        MS.Position = 0;
                        MS.CopyTo(FS);
                    }
                }
            }
        }

        public static void RestorePlugins(PluginInfo[] KnownPlugins)
        {
            if (KnownPlugins is null)
            {
                throw new ArgumentNullException(nameof(KnownPlugins));
            }
            if (KnownPlugins.Length == 0)
            {
                throw new ArgumentException("Array contains no entries", nameof(KnownPlugins));
            }

            var TempList = new List<PluginStatus>();
            FileStream FS;
            try
            {
                FS = File.OpenRead(ConfigFile);
            }
            catch
            {
                return;
            }
            using (FS)
            {
                using (var BR = new BinaryReader(FS))
                {
                    var Version = BR.ReadInt32();
                    if (Version != CONFIG_VERSION)
                    {
                        throw new InvalidDataException($"Config version {Version} is not supported");
                    }
                    var Count = BR.ReadInt32();
                    while (Count-- > 0)
                    {
                        var Name = BR.ReadString();
                        var Enabled = BR.ReadBoolean();
                        var Data = BR.ReadBytes(BR.ReadInt32());
                        if (Data.Length > 0)
                        {
                            Data = NativeMethods.Decrypt(Data);
                        }
                        var PluginType = KnownPlugins.FirstOrDefault(m => m.PluginType.FullName == Name);
                        if (PluginType != null)
                        {
                            try
                            {
                                var Plugin = LoadPlugin(PluginType);
                                Plugin.Load(Data);
                                Plugin.Start();
                                TempList.Add(new PluginStatus(Plugin) { Enabled = Enabled });
                            }
                            catch
                            {
                                //NOOP
                            }
                        }
                    }
                }
            }

            foreach (var PI in Plugins)
            {
                PI.Plugin.Dispose();
            }
            Plugins.Clear();
            Plugins.AddRange(TempList);
        }
    }

    public class PluginInfo
    {
        public string BaseName { get; set; }
        public Type PluginType { get; set; }
        public string AssemblyFile { get; set; }

        public override string ToString()
        {
            return $"Plugin information: Name={PluginType.FullName} File={AssemblyFile}";
        }
    }
}
