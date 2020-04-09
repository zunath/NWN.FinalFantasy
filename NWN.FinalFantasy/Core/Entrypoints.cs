using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWN.FinalFantasy.Extension;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NWNEventHandler : Attribute
    {
        public const int MaxCharsInScriptName = 16;
        public const int ScriptHandled = 0;
        public const int ScriptNotHandled = -1;

        public static Dictionary<string, List<Action>> Scripts;
        public string Script;

        private static DateTime _last1SecondIntervalCall = DateTime.UtcNow;

        public NWNEventHandler(string script)
        {
            Script = script;
        }

        public static void OnMainLoop(ulong frame)
        {
            RunOneSecondPCIntervalEvent();
        }

        /// <summary>
        /// Fires an event on every player every second.
        /// We do it this way so we don't run into a situation
        /// where we iterate over the player list more than once per second
        /// </summary>
        private static void RunOneSecondPCIntervalEvent()
        {
            var now = DateTime.UtcNow;
            var delta = now - _last1SecondIntervalCall;
            if (delta.Seconds < 1) return;
            _last1SecondIntervalCall = now;

            for (var player = GetFirstPC(); GetIsObjectValid(player); player = GetNextPC())
            {
                Internal.OBJECT_SELF = player;
                RunScripts("interval_pc_1s");
            }
        }

        private static bool RunScripts(string script)
        {
            if (Scripts.ContainsKey(script))
            {
                foreach (var action in Scripts[script])
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        var details = ex.ToMessageAndCompleteStacktrace();
                        Console.WriteLine($"C# Script '{script}' threw an exception. Details: {Environment.NewLine}{Environment.NewLine}{details}");
                    }
                }

                return true;
            }

            return false;
        }

        public static int OnRunScript(string script, uint oidSelf)
        {
            return RunScripts(script) ? ScriptHandled : ScriptNotHandled;
        }

        public static void OnStart()
        {
            Console.WriteLine("Registering scripts...");
            Scripts = GetHandlersFromAssembly();
            Console.WriteLine("Scripts registered successfully.");
        }

        public static Dictionary<string, List<Action>> GetHandlersFromAssembly()
        {
            var result = new Dictionary<string, List<Action>>();
            var handlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(NWNEventHandler), false).Length > 0)
                .ToArray();

            foreach (var mi in handlers)
            {
                var del = (Action)mi.CreateDelegate(typeof(Action));
                foreach (var attr in mi.GetCustomAttributes(typeof(NWNEventHandler), false))
                {
                    var script = ((NWNEventHandler)attr).Script;
                    if (script.Length > MaxCharsInScriptName || script.Length == 0)
                    {
                        Console.WriteLine($"Script name '{script}' is invalid on method {mi.Name}.");
                        throw new ApplicationException();
                    }

                    if(!result.ContainsKey(script))
                        result[script] = new List<Action>();

                    result[script].Add(del);
                    Console.WriteLine($"Registered method '{del.Method.Name}' to script: {script}");
                }
            }

            return result;
        }
    }
}