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
        private readonly object BasePlugin;

        private readonly Action StartFunc;
        private readonly Action StopFunc;
        private readonly Action<byte[]> LoadFunc;
        private readonly Func<byte[]> SaveFunc;
        private readonly Func<bool> ConfigFunc;
        private readonly Action TestFunc;

        private readonly PropertyInfo BaseNameProp;
        private readonly PropertyInfo NameProp;
        private readonly PropertyInfo NextCheckProp;

        public bool Disposed { get; private set; }

        public Type PluginType { get; }

        public Exception LastError { get; private set; }

        public string BaseName
        {
            get
            {
                return (string)BaseNameProp.GetValue(null);
            }
        }

        public string Name
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException(nameof(Plugin));
                }
                return (string)NameProp.GetValue(NameProp.GetMethod.IsStatic ? null : BasePlugin);
            }
        }

        public DateTime NextCheck
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException(nameof(Plugin));
                }
                return (DateTime)NextCheckProp.GetValue(BasePlugin);
            }
        }

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
            if (Constructor == null)
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
            try
            {
                StartFunc = (Action)PluginType.GetMethod("Start", Type.EmptyTypes).CreateDelegate(typeof(Action), BasePlugin);
                StopFunc = (Action)PluginType.GetMethod("Stop", Type.EmptyTypes).CreateDelegate(typeof(Action), BasePlugin);
                ConfigFunc = (Func<bool>)PluginType.GetMethod("Config", Type.EmptyTypes).CreateDelegate(typeof(Func<bool>), BasePlugin);
                LoadFunc = (Action<byte[]>)PluginType.GetMethod("Load", new Type[] { typeof(byte[]) }).CreateDelegate(typeof(Action<byte[]>), BasePlugin);
                SaveFunc = (Func<byte[]>)PluginType.GetMethod("Save", Type.EmptyTypes).CreateDelegate(typeof(Func<byte[]>), BasePlugin);
                TestFunc = (Action)PluginType.GetMethod("Test", Type.EmptyTypes).CreateDelegate(typeof(Action), BasePlugin);
            }
            catch (Exception ex)
            {
                throw new PluginException("Failed to hook up all plugin functions. Function either missing or of invalid type. See inner exception for details.", ex);
            }

            #region Properties

            try
            {
                ValidateProperty("BaseName", typeof(string));
                var Prop = PluginType.GetProperty("BaseName");
                var Getter = Prop.GetMethod;
                if (!Getter.IsStatic)
                {
                    throw new PluginException($"{PluginType.Name}.BaseName is not static");
                }
                //Try to retrieve value now to catch potential exception
                Prop.GetValue(null);
                //make property accessible through our own Name property
                BaseNameProp = Prop;
            }
            catch (PluginException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PluginException($"Failed to read Name property of {PluginType.Name}. See inner exception for details", ex);
            }
            try
            {
                ValidateProperty("Name", typeof(string));
                var Prop = PluginType.GetProperty("Name");
                var Getter = Prop.GetMethod;
                //Try to retrieve value now to catch potential exception
                Prop.GetValue(Getter.IsStatic ? null : BasePlugin);
                //make property accessible through our own Name property
                NameProp = Prop;
            }
            catch (PluginException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PluginException($"Failed to read Name property of {PluginType.Name}. See inner exception for details", ex);
            }
            try
            {
                ValidateProperty("NextCheck", typeof(DateTime));
                var Prop = PluginType.GetProperty("NextCheck");
                //Try to retrieve value now to catch potential exception
                Prop.GetValue(BasePlugin);
                //make property accessible through our own property
                NextCheckProp = Prop;
            }
            catch (PluginException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PluginException($"Failed to read NextCheck property of {PluginType.Name}. See inner exception for details", ex);
            }

            #endregion
        }

        public void Start()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            StartFunc();
        }

        public void Stop()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            StopFunc();
        }

        public bool Config()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            return ConfigFunc();
        }

        public void Test()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            try
            {
                TestFunc();
                LastError = null;
            }
            catch (Exception ex)
            {
                LastError = ex;
            }
        }

        public void Load(byte[] Data)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            LoadFunc(Data);
        }

        public byte[] Save()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }
            return SaveFunc();
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Stop();
                (BasePlugin as IDisposable).Dispose();
                Disposed = true;
            }
        }

        private void ValidateProperty(string PropertyName, Type PropertyType)
        {
            if (string.IsNullOrEmpty(PropertyName))
            {
                throw new ArgumentException($"'{nameof(PropertyName)}' cannot be null or empty.", nameof(PropertyName));
            }

            if (PropertyType is null)
            {
                throw new ArgumentNullException(nameof(PropertyType));
            }

            if (PluginType is null)
            {
                throw new InvalidOperationException("PluginType has not yet been set");
            }

            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(Plugin));
            }

            var Prop = PluginType.GetProperty(PropertyName);
            if (Prop == null)
            {
                throw new PluginException($"Property {PluginType.Name}.{PropertyName} does not exist");
            }
            if (!Prop.CanRead)
            {
                throw new PluginException($"{PluginType.Name}.{PropertyName} is not readable");
            }
            var Getter = Prop.GetMethod;
            if (Getter == null || !Getter.IsPublic)
            {
                throw new PluginException($"{PluginType.Name}.{PropertyName} cannot be read or is not public");
            }
            if (Getter.ReturnType != PropertyType)
            {
                throw new PluginException($"{PluginType.Name}.{PropertyName} is not {PropertyType.Name}");
            }
        }
    }
}
