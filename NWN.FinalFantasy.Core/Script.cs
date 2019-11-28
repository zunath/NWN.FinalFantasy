using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.Utility;

namespace NWN.FinalFantasy.Core
{
    public static class Script
    {
        private static readonly Dictionary<string, Type> _cachedScripts = new Dictionary<string, Type>();
        private static object _scriptData;
        private static ApplicationSettings _settings;

        public static void CacheScripts()
        {
            _settings = ApplicationSettings.Get();
            var types = TypeFinder.GetTypesImplementingInterface<IScript>();
            foreach (var type in types)
            {
                var key = type.Namespace + "." + type.Name;
                key = key.Replace(_settings.NamespaceRoot + ".", string.Empty);
                Console.WriteLine("Registering type: " + key);
                _cachedScripts[key] = type;
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
            where T : class
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
                    if (!_cachedScripts.ContainsKey(script))
                        throw new Exception("Script '" + script + "' has not been registered. Make sure a public class implementing the " + nameof(IScript) + " interface exists in the code base.");

                    var type = _cachedScripts[script];
                    var method = type.GetMethod("Main");

                    if(method == null)
                        throw new Exception("Main method not found on script: " + script);

                    var instance = Activator.CreateInstance(type);
                    method.Invoke(instance, null);
                }
                catch (Exception ex)
                {
                    Audit.Write(AuditGroup.Error, "SCRIPT ERROR: " + script + " Exception: " + ex.ToMessageAndCompleteStacktrace());
                }
            }
        }
    }
}
