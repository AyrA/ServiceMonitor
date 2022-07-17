using System;
using System.Linq;
using System.Reflection;

namespace ServiceMonitor
{
    /// <summary>
    /// Generic wrapper around a plugin
    /// </summary>
    public class Plugin : IDisposable
    {
        private bool Disposed = false;

        private readonly object BasePlugin;
        private readonly Type PluginType;

        private readonly MethodInfo StartFunc;
        private readonly MethodInfo StopFunc;
        private readonly MethodInfo LoadFunc;
        private readonly MethodInfo SaveFunc;
        private readonly MethodInfo ConfigFunc;
        private readonly MethodInfo TestFunc;

        private readonly PropertyInfo NameProp;

        //private readonly EventInfo TestingEvent;
        //private readonly EventInfo TestCompleteEvent;

        public string Name { get => (string)NameProp.GetValue(BasePlugin); }

        public Plugin(Type PluginType)
        {
            this.PluginType = PluginType ?? throw new ArgumentNullException(nameof(PluginType));

            //Check if the base type is IDisposable
            if (!PluginType.GetInterfaces().Contains(typeof(IDisposable)))
            {
                throw new PluginException($"{PluginType.Name} doeen't implements {nameof(IDisposable)}");
            }
            //Try to invoke constructor
            var Constructor = PluginType.GetConstructor(Type.EmptyTypes);
            if (Constructor != null)
            {
                throw new PluginException($"{PluginType.Name} does not contain a parameterless constructor");
            }
            try
            {
                BasePlugin = Constructor.Invoke(null);
            }
            catch (Exception ex)
            {
                throw new PluginException($"Failed to call constructor {PluginType.Name}(). See inner exception for details", ex);
            }
            //Attach functions
            StartFunc = PluginType.GetMethod("Start", Type.EmptyTypes);
            StopFunc = PluginType.GetMethod("Stop", Type.EmptyTypes);
            ConfigFunc = PluginType.GetMethod("Config", Type.EmptyTypes);
            LoadFunc = PluginType.GetMethod("Load", new Type[] { typeof(byte[]) });
            SaveFunc = PluginType.GetMethod("Save", Type.EmptyTypes);
            TestFunc = PluginType.GetMethod("Test", Type.EmptyTypes);

            try
            {
                var NameProp = PluginType.GetProperty("Name");
                if (NameProp == null)
                {
                    throw new PluginException("Plugin has no Name property");
                }
                if (!NameProp.CanRead)
                {
                    throw new PluginException($"Name property of {PluginType.Name} is not readable");
                }
                var Getter = NameProp.GetMethod;
                if (Getter == null || !Getter.IsPublic)
                {
                    throw new PluginException($"get() of Name of {PluginType.Name} not found or not public");
                }
                //Try to retrieve value now to catch potential exception
                NameProp.GetValue(Getter.IsStatic ? null : BasePlugin);
                //make property accessible through our own Name property
                this.NameProp = NameProp;
            }
            catch (PluginException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PluginException($"Failed to read Name property of {PluginType.Name}. See inner exception for details", ex);
            }
            //Attach events
            //TestingEvent = PluginType.GetEvent("Testing");
            //TestCompleteEvent = PluginType.GetEvent("TestComplete");

            //Check if all functions were found
            if (StartFunc == null)
            {
                throw new PluginException($"{PluginType.Name} lacks Start()");
            }
            if (StopFunc == null)
            {
                throw new PluginException($"{PluginType.Name} lacks Stop()");
            }
            if (ConfigFunc == null)
            {
                throw new PluginException($"{PluginType.Name} lacks Config()");
            }
            if (LoadFunc == null)
            {
                throw new PluginException($"{PluginType.Name} lacks Load(byte[])");
            }
            if (SaveFunc == null)
            {
                throw new PluginException($"{PluginType.Name} lacks Save()");
            }
            if (TestFunc == null)
            {
                throw new PluginException($"{PluginType.Name} lacks Test()");
            }

            if (SaveFunc.ReturnType != typeof(byte[]))
            {
                throw new PluginException($"{PluginType.Name}.Save() does not return byte[]");
            }
        }

        public void Start()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            StartFunc.Invoke(BasePlugin, null);
        }

        public void Stop()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            StopFunc.Invoke(BasePlugin, null);
        }

        public void Config()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            ConfigFunc.Invoke(BasePlugin, null);
        }

        public void Load(byte[] Data)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            LoadFunc.Invoke(BasePlugin, new object[] { Data });
        }

        public byte[] Save()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            return (byte[])SaveFunc.Invoke(BasePlugin, null);
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                Stop();
                (BasePlugin as IDisposable).Dispose();
            }
        }
    }
}
