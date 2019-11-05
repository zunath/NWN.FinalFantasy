using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Core.Startup;
using NWN.FinalFantasy.Core.Utility;
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
            var scripts = LocalVariableTool.FindByPrefix(caller, scriptPrefix);

            foreach (var script in scripts)
            {
                try
                {
                    RunScript(caller, script);
                }
                catch(Exception ex)
                {
                    Audit.Write(AuditGroup.Error, ex.ToMessageAndCompleteStacktrace());
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
        private static void RunScript(NWGameObject caller, string script)
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
