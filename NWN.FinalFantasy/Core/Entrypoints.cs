using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWN.FinalFantasy.Extension;

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

        public NWNEventHandler(string script)
        {
            Script = script;
        }

        public static void OnMainLoop(ulong frame)
        {

        }

        public static int OnRunScript(string script, uint oidSelf)
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

                return ScriptHandled;
            }

            return ScriptNotHandled;
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