using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.Utility;

namespace NWN.FinalFantasy.Core
{
    public static class Script
    {
        private static readonly Dictionary<string, IScript> _cachedScripts = new Dictionary<string, IScript>();
        private static object _scriptData;
        private static readonly ApplicationSettings _settings;

        static Script()
        {
            _settings = ApplicationSettings.Get();
            var types = TypeFinder.GetTypesImplementingInterface<IScript>();
            foreach (var script in types)
            {
                var instance = (IScript) Activator.CreateInstance(script);
                var key = script.Namespace + "." + script.Name;
                Console.WriteLine("Registering type: " + key);
                _cachedScripts[key] = instance;
            }
        }

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
                    Console.WriteLine("script = " + script);
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
            var key = _settings.NamespaceRoot + "." + script;
            if (!_cachedScripts.ContainsKey(key))
                throw new Exception("Script '" + script + "' has not been registered. Make sure a public class implementing the " + nameof(IScript) + " interface exists in the code base.");

            try
            {
                _cachedScripts[key].Main();
            }
            catch(Exception ex)
            {
                Audit.Write(AuditGroup.Error, "SCRIPT ERROR: " + script + " " + ex.ToMessageAndCompleteStacktrace());
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
        public static void Run(NWGameObject caller, string script)
        {
            Run<object>(caller, script, null);
        }
    }
}
