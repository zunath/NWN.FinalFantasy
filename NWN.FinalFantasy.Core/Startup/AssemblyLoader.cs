using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using NWN.FinalFantasy.Core.Logging;

namespace NWN.FinalFantasy.Core.Startup
{
    /// <summary>
    /// Responsible for loading associated assemblies in the location configured via environment variables.
    /// </summary>
    public class AssemblyLoader
    {
        private static readonly List<Assembly> _assemblies = new List<Assembly>();

        /// <summary>
        /// Loads all DLL assemblies in the path configured in the app settings file.
        /// </summary>
        internal static void LoadAssemblies()
        {
            var settings = ApplicationSettings.Get();
            var files = Directory.GetFiles(settings.DllDirectory, settings.DllSearchPattern);

            foreach(var file in files)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                _assemblies.Add(assembly);
            }

            foreach (var assembly in _assemblies)
            {
                RunStartup(assembly);
            }
        }

        /// <summary>
        /// Executes the script named Startup, if it's found in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search</param>
        private static void RunStartup(Assembly assembly)
        {
            try
            {
                var types = assembly.GetTypes();
                var matching = types.SingleOrDefault(x => x.Namespace + "." + x.Name == x.Namespace + ".Startup");
                if (matching == null) return;

                var method = matching.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);
                if (method == null) return;

                method.Invoke(null, null);
            }
            catch(Exception ex)
            {
                Audit.Write(AuditGroup.Error, ex.ToMessageAndCompleteStacktrace());
            }
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

            return null;
        }

        /// <summary>
        /// Finds all of the specified types of a given interface type T.
        /// </summary>
        /// <typeparam name="T">The type of interface to search for.</typeparam>
        /// <returns>A list of types which implement type T.</returns>
        public static List<Type> GetAllImplementingInterface<T>()
        {
            if(!typeof(T).IsInterface)
                throw new Exception("T must be an interface type.");

            var type = typeof(T);
            return _assemblies.SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
                .ToList();
        }
    }
}
