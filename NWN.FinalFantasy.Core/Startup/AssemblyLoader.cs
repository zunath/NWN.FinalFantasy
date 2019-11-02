using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Serilog;

namespace NWN.FinalFantasy.Core.Startup
{
    internal class AssemblyLoader
    {
        private static readonly List<Assembly> _assemblies = new List<Assembly>();

        public static void LoadAssemblies()
        {
            var settings = ApplicationSettings.Instance;

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var files = Directory.GetFiles(settings.DllDirectory, settings.DllSearchPattern);

            foreach(var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (fileName.StartsWith(assemblyName)) continue;
                var assembly = Assembly.LoadFile(file);
                _assemblies.Add(assembly);
            }
        }

        public static Type FindType(string @namespace)
        {
            foreach (var assembly in _assemblies)
            {
                Log.Information("Checking assembly: " + assembly.FullName);
                Log.Information("Namespace: " + @namespace);

                var types = assembly.GetTypes();
                var matching = types.SingleOrDefault(x =>
                {
                    Log.Information("name = " + x.Namespace + "." + x.Name);
                    return x.Namespace + "." + x.Name == @namespace;
                });

                if (matching == null) continue;

                return matching;
            }

            throw new Exception($"Could not find script with namespace {@namespace}");
        }
    }
}
