using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NWN.FinalFantasy.Core.Startup
{
    /// <summary>
    /// Responsible for loading associated assemblies in the location configured in the AppSettings.json file.
    /// </summary>
    internal class AssemblyLoader
    {
        private static readonly List<Assembly> _assemblies = new List<Assembly>();

        /// <summary>
        /// Loads all DLL assemblies in the path configured in the app settings file.
        /// </summary>
        public static void LoadAssemblies()
        {
            var settings = new ApplicationSettings();

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var files = Directory.GetFiles(settings.DllDirectory, settings.DllSearchPattern);

            foreach(var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (fileName.StartsWith(assemblyName)) continue;
                var assembly = Assembly.LoadFile(file);
                _assemblies.Add(assembly);

                RunStartup(assembly);
            }
        }

        /// <summary>
        /// Executes the script named Startup, if it's found in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search</param>
        private static void RunStartup(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var matching = types.SingleOrDefault(x => x.Namespace + "." + x.Name == x.Namespace + ".Startup");
            if (matching == null) return;

            var method = matching.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);
            if (method == null) return;

            method.Invoke(null, null);
        }

        /// <summary>
        /// Finds a specified type throughout all loaded assemblies.
        /// </summary>
        /// <param name="namespace">The assembly to look for</param>
        /// <returns></returns>
        public static Type FindType(string @namespace)
        {
            foreach (var assembly in _assemblies)
            {
                var types = assembly.GetTypes();
                var matching = types.SingleOrDefault(x => x.Namespace + "." + x.Name == @namespace);

                if (matching == null) continue;

                return matching;
            }

            throw new Exception($"Could not find script with namespace {@namespace}");
        }
    }
}
