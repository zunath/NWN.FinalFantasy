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
        private const int MaxCharsInScriptName = 16;
        private const int ScriptHandled = 0;
        private const int ScriptNotHandled = -1;

        private delegate int ConditionalScriptDelegate();

        private static Dictionary<string, List<Action>> _scripts;
        private static Dictionary<string, List<ConditionalScriptDelegate>> _conditionalScripts;
        private readonly string _script;

        private static DateTime _last1SecondIntervalCall = DateTime.UtcNow;

        public NWNEventHandler(string script)
        {
            _script = script;
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

        private static int RunScripts(string script)
        {
            if (_conditionalScripts.ContainsKey(script))
            {
                foreach (var action in _conditionalScripts[script])
                {
                    return action.Invoke();
                }
            }
            else if (_scripts.ContainsKey(script))
            {
                foreach (var action in _scripts[script])
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

                return ScriptHandled;
            }

            return ScriptNotHandled;
        }

        public static int OnRunScript(string script, uint oidSelf)
        {
            var retVal = RunScripts(script);

            if (retVal == -1) return ScriptNotHandled;
            else return retVal;
        }

        public static void OnStart()
        {
            Console.WriteLine("Registering scripts...");
            LoadHandlersFromAssembly();
            Console.WriteLine("Scripts registered successfully.");
        }

        public static void LoadHandlersFromAssembly()
        {
            _scripts = new Dictionary<string, List<Action>>();
            _conditionalScripts = new Dictionary<string, List<ConditionalScriptDelegate>>();

            var handlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(NWNEventHandler), false).Length > 0)
                .ToArray();

            foreach (var mi in handlers)
            {
                foreach (var attr in mi.GetCustomAttributes(typeof(NWNEventHandler), false))
                {
                    var script = ((NWNEventHandler)attr)._script;
                    if (script.Length > MaxCharsInScriptName || script.Length == 0)
                    {
                        Console.WriteLine($"Script name '{script}' is invalid on method {mi.Name}.");
                        throw new ApplicationException();
                    }

                    // If the return type is an int, it is assumed to be a conditional script.
                    if (mi.ReturnType == typeof(int))
                    {
                        var del = (ConditionalScriptDelegate)mi.CreateDelegate(typeof(ConditionalScriptDelegate));

                        if (!_conditionalScripts.ContainsKey(script))
                            _conditionalScripts[script] = new List<ConditionalScriptDelegate>();

                        _conditionalScripts[script].Add(del);

                        Console.WriteLine($"Registered method '{del.Method.Name}' to conditional script: {script}");
                    }
                    // Otherwise it's a normal script.
                    else
                    {
                        var del = (Action)mi.CreateDelegate(typeof(Action));

                        if (!_scripts.ContainsKey(script))
                            _scripts[script] = new List<Action>();

                        _scripts[script].Add(del);

                        Console.WriteLine($"Registered method '{del.Method.Name}' to script: {script}");
                    }

                }
            }
        }
    }
}