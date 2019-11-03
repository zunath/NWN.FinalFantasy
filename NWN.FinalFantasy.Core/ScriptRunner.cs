using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Core.Startup;
using Serilog;
using static NWN._;

namespace NWN.FinalFantasy.Core
{
    internal static class ScriptRunner
    {
        private static readonly Dictionary<string, MethodInfo> _cachedScripts = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// Runs all scripts matching a given prefix, in the order they are found.
        /// </summary>
        /// <param name="caller">The object whose scripts we're checking</param>
        /// <param name="scriptPrefix">The prefix to look for.</param>
        internal static void RunScriptEvents(NWGameObject caller, string scriptPrefix)
        {
            var scripts = GetMatchingVariables(caller, scriptPrefix);

            foreach (var script in scripts)
            {
                try
                {
                    RunScript(script, caller);
                }
                catch(Exception ex)
                {
                    Audit.Write(AuditGroup.Error, ex.ToMessageAndCompleteStacktrace());
                }
            }
        }

        /// <summary>
        /// Retrieves all local string variables on an object matching a given prefix.
        /// Returns a list containing the scripts to run sequentially.
        /// </summary>
        /// <param name="target">The object to pull variables from</param>
        /// <param name="prefix">The prefix to look for</param>
        /// <returns>A list of scripts to run, ordered from lowest to highest</returns>
        internal static IEnumerable<string> GetMatchingVariables(NWGameObject target, string prefix)
        {
            var variableCount = NWNXObject.GetLocalVariableCount(target);
            var variableList = new SortedList<int, string>();

            for (int x = 0; x < variableCount; x++)
            {
                var variable = NWNXObject.GetLocalVariable(target, x);
                if (variable.Type == LocalVariableType.String &&
                    variable.Key.StartsWith(prefix))
                {
                    // If the rest of the variable key can be converted to an int, add it to the list.
                    var skipCharacters = prefix.Length;
                    var orderSubstring = variable.Key.Substring(skipCharacters);

                    if (!int.TryParse(orderSubstring, out var order))
                    {
                        // Couldn't parse an integer out of the key name. Move to the next variable.
                        continue;
                    }

                    // If the variable ID has already been assigned, skip to the next variable.
                    if (variableList.ContainsKey(order))
                    {
                        Log.Warning($"Variable '{prefix}' for ID {order} already exists. Ignoring second entry.");
                        continue;
                    }

                    // Add the script to the list.
                    var value = GetLocalString(target, variable.Key);
                    variableList.Add(order, value);
                }
            }

            return variableList.Values.ToList();
        }

        /// <summary>
        /// Runs a C# script's Main() method.
        /// </summary>
        /// <param name="script">Name of the script's namespace</param>
        /// <param name="caller">The caller of this script</param>
        private static void RunScript(string script, NWGameObject caller)
        {
            if (!_cachedScripts.ContainsKey(script))
            {
                var settings = ApplicationSettings.Get();
                var scriptNamespace = settings.NamespaceRoot + "." + script;

                var type = Type.GetType(scriptNamespace);
                if (type == null)
                {
                    // Check the loaded assemblies for the type.
                    type = AssemblyLoader.FindType(scriptNamespace);

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

            _cachedScripts[script].Invoke(null, null);
        }
    }
}
