using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Core
{
    internal static partial class Internal
    {
        public const uint OBJECT_INVALID = 0x7F000000;

        private static readonly Stack<ScriptContext> ScriptContexts = new Stack<ScriptContext>();

        private static ulong NextEventId;
        private static readonly Dictionary<ulong, Closure> Closures = new Dictionary<ulong, Closure>();
        public static uint OBJECT_SELF { get; set; } = OBJECT_INVALID;

        public static void OnMainLoop(ulong frame)
        {
            try
            {
                NWNEventHandler.OnMainLoop(frame);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static int OnRunScript(string script, uint oidSelf)
        {
            var ret = 0;
            OBJECT_SELF = oidSelf;
            ScriptContexts.Push(new ScriptContext { OwnerObject = oidSelf, ScriptName = script });
            try
            {
                ret = NWNEventHandler.OnRunScript(script, oidSelf);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            ScriptContexts.Pop();
            OBJECT_SELF = ScriptContexts.Count == 0 ? OBJECT_INVALID : ScriptContexts.Peek().OwnerObject;
            return ret;
        }

        private static void OnClosure(ulong eid, uint oidSelf)
        {
            var old = OBJECT_SELF;
            OBJECT_SELF = oidSelf;
            try
            {
                Closures[eid].Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Closures.Remove(eid);
            OBJECT_SELF = old;
        }

        public static void ClosureAssignCommand(uint obj, ActionDelegate func)
        {
            if (NativeFunctions.ClosureAssignCommand(obj, NextEventId) != 0)
                Closures.Add(NextEventId++, new Closure { OwnerObject = obj, Run = func });
        }

        public static void ClosureDelayCommand(uint obj, float duration, ActionDelegate func)
        {
            if (NativeFunctions.ClosureDelayCommand(obj, duration, NextEventId) != 0)
                Closures.Add(NextEventId++, new Closure { OwnerObject = obj, Run = func });
        }

        public static void ClosureActionDoCommand(uint obj, ActionDelegate func)
        {
            if (NativeFunctions.ClosureActionDoCommand(obj, NextEventId) != 0)
                Closures.Add(NextEventId++, new Closure { OwnerObject = obj, Run = func });
        }

        private struct ScriptContext
        {
            public uint OwnerObject;
            public string ScriptName;
        }

        private struct Closure
        {
            public uint OwnerObject;
            public ActionDelegate Run;
        }
    }
}