using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using NWN.FinalFantasy.Core.NWScript;
using NWN.FinalFantasy.Location;

namespace NWN.FinalFantasy.Application
{
    public class Bootstrapper
    {
        public static int Bootstrap(IntPtr arg, int argLength)
        {
            if (arg == (IntPtr)0)
            {
                Console.WriteLine("Received NULL bootstrap structure");
                return 1;
            }
            int expectedLength = Marshal.SizeOf(typeof(Internal.BootstrapArgs));
            if (argLength < expectedLength)
            {
                Console.WriteLine($"Received bootstrap structure too small - actual={argLength}, expected={expectedLength}");
                return 1;
            }
            if (argLength > expectedLength)
            {
                Console.WriteLine($"WARNING: Received bootstrap structure bigger than expected - actual={argLength}, expected={expectedLength}");
                Console.WriteLine($"         This usually means that native code version is ahead of the managed code");
            }

            Internal.NativeFunctions = Marshal.PtrToStructure<Internal.BootstrapArgs>(arg);

            Internal.AllHandlers handlers;
            handlers.MainLoop = OnMainLoop;
            handlers.RunScript = OnRunScript;
            handlers.Closure = Internal.OnClosure;
            Internal.RegisterHandlers(handlers);

            try
            {
                Entrypoints.OnStart();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }


        public static void OnMainLoop(ulong frame)
        {
            try
            {
                Entrypoints.OnMainLoop(frame);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static int OnRunScript(string script, uint oidSelf)
        {
            int ret = 0;
            Internal.OBJECT_SELF = oidSelf;
            Internal.ScriptContexts.Push(new ScriptContext { OwnerObject = oidSelf, ScriptName = script });
            try
            {
                ret = Entrypoints.OnRunScript(script, oidSelf);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Internal.ScriptContexts.Pop();
            Internal.OBJECT_SELF = Internal.ScriptContexts.Count == 0 ? Internal.OBJECT_INVALID : Internal.ScriptContexts.Peek().OwnerObject;
            return ret;
        }
    }
}