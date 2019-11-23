using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.Utility;
using Serilog;
using static NWN._;

namespace NWN.FinalFantasy.Core
{
    public static class Script
    {
        private static readonly Dictionary<string, MethodInfo> _cachedScripts = new Dictionary<string, MethodInfo>();
        private static object _scriptData;

        /// <summary>
        /// Sets data available for scripts to retrieve.
        /// Only available during custom events.
        /// </summary>
        /// <param name="data">The data to store</param>
        public static void SetScriptData(object data)
        {
            _scriptData = data;
        }

        /// <summary>
        /// Clears data available for scripts. Should be called
        /// after all scripts have been executed for this custom event.
        /// </summary>
        public static void ClearScriptData()
        {
            _scriptData = null;
        }

        /// <summary>
        /// Retrieves data stored for this script.
        /// If ignoreNullData is true, no exception will be thrown if null is retrieved.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        /// <param name="ignoreNullData">if true, does not throw a exception if data is null. otherwise it does</param>
        /// <returns>The stored data</returns>
        public static T GetScriptData<T>(bool ignoreNullData)
            where T: class
        {
            if (!ignoreNullData) return GetScriptData<T>();

            return _scriptData as T;
        }

        /// <summary>
        /// Retrieves data stored for this script.
        /// Only available during custom events. 
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        /// <returns>The stored data</returns>
        public static T GetScriptData<T>()
            where T : class
        {
            if (_scriptData == null)
                throw new Exception("Script data does not exist. Is this a custom event?");

            return _scriptData as T;
        }

        /// <summary>
        /// Runs all scripts matching a given prefix, in the order they are found.
        /// </summary>
        /// <param name="caller">The object whose scripts we're checking</param>
        /// <param name="scriptPrefix">The prefix to look for.</param>
        /// <param name="scriptRegistrationObject">If the local variables are stored on a different object than the caller, you can use this argument to dictate where to look for the local variables</param>
        public static void RunScriptEvents(NWGameObject caller, string scriptPrefix, NWGameObject scriptRegistrationObject = null)
        {
            if (scriptRegistrationObject == null)
                scriptRegistrationObject = caller;

            var scripts = LocalVariableTool.FindByPrefix(scriptRegistrationObject, scriptPrefix);

            foreach (var script in scripts)
            {
                try
                {
                    Run(caller, script);
                }
                catch (Exception ex)
                {
                    Audit.Write(AuditGroup.Error, "SCRIPT ERROR: " + script + " Exception: " + ex.ToMessageAndCompleteStacktrace());
                }
            }
        }

        /// <summary>
        /// Runs a C# script's Main() method.
        /// "script" should be specified with the project name.
        /// Example: 'Death.OnPlayerDeath' is valid. Just 'OnPlayerDeath' is not.
        /// Exclude the root namespace when specifying script.
        /// </summary>
        /// <param name="script">Name of the script's namespace</param>
        /// <param name="caller">The caller of this script</param>
        /// <param name="data">Arbitrary data to pass to the script.</param>
        public static void Run<T>(NWGameObject caller, string script, T data)
        {
            if (!_cachedScripts.ContainsKey(script))
            {
                var settings = ApplicationSettings.Get();
                var scriptNamespace = settings.NamespaceRoot + "." + script;

                var type = Type.GetType(scriptNamespace);
                if (type == null)
                {
                    // Check the loaded assemblies for the type.
                    type = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(x => x.DefinedTypes)
                        .SingleOrDefault(x => x.Namespace + "." + x.Name == scriptNamespace);

                    if (type == null)
                    {
                        Log.Warning($"Could not locate script: {scriptNamespace} from caller {GetName(caller)}");
                        return;
                    }
                }

                var method = type.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);
                if (method == null)
                {
                    Log.Warning("Could not locate method 'Main' on script: " + script);
                    return;
                }

                _cachedScripts[script] = method;
            }

            _cachedScripts[script].Invoke(null, data == null ? null : new object[] { data });
        }

        /// <summary>
        /// Runs a C# script's Main() method.
        /// "script" should be specified with the project name.
        /// Example: 'Death.OnPlayerDeath' is valid. Just 'OnPlayerDeath' is not.
        /// Exclude the root namespace when specifying script.
        /// </summary>
        /// <param name="script">Name of the script's namespace</param>
        /// <param name="caller">The caller of this script</param>
        public static void Run(NWGameObject caller, string script)
        {
            Run<object>(caller, script, null);
        }
    }
}
