using System.Collections.Generic;
using System.Reflection;

namespace ServiceMonitor
{
    public static class PluginManager
    {
        public static Plugin[] GetPlugins(string DllFile, bool SkipInvalid = false)
        {
            var Ret = new List<Plugin>();
            var PluginTypes = Assembly.LoadFrom(DllFile).GetExportedTypes();

            foreach(var T in PluginTypes)
            {
                if (T.Name.EndsWith("Plugin"))
                {
                    try
                    {
                        Ret.Add(new Plugin(T));
                    }
                    catch(PluginException)
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
    }
}
