using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Logging;

namespace NWN.FinalFantasy.Application
{
    internal class AssemblyLoader
    {
        public static void LoadAssemblies()
        {
            var settings = ApplicationSettings.Get();
            var files = Directory.GetFiles(settings.DllDirectory, settings.DllSearchPattern);

            foreach (var file in files)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                //_assemblies.Add(assembly);

                Console.WriteLine("Loaded ass: " + file);

                //RunStartup(assembly);

            }

            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                Console.WriteLine("ass laoded = " + ass.FullName);
            }

            //foreach (var assembly in _assemblies)
            //{
            //    RunStartup(assembly);
            //}
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
            catch (Exception ex)
            {
                Audit.Write(AuditGroup.Error, ex.ToMessageAndCompleteStacktrace());
            }
        }
    }
}
