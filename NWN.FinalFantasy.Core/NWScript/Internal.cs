using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using NWN.FinalFantasy.Core.NWScript;

// ReSharper disable once CheckNamespace
namespace NWN
{
    public partial class Internal
    {
        public const uint OBJECT_INVALID = 0x7F000000;
        public static NWGameObject OBJECT_SELF { get; set; } = OBJECT_INVALID;

        public delegate void MainLoopHandlerDelegate(ulong frame);
        public delegate int RunScriptHandlerDelegate(string script, uint oid);
        public delegate void ClosureHandlerDelegate(ulong eid, uint oid);

        [StructLayout(LayoutKind.Sequential)]
        public struct AllHandlers
        {
            public MainLoopHandlerDelegate MainLoop;
            public RunScriptHandlerDelegate RunScript;
            public ClosureHandlerDelegate Closure;
        }

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetFunctionPointerDelegate(string name);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void RegisterHandlersDelegate(IntPtr handlers, uint size);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallBuiltInDelegate(int id);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushIntegerDelegate(int value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushFloatDelegate(float value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushStringDelegate(string value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushStringUTF8Delegate(string value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushObjectDelegate(uint value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushVectorDelegate(Vector value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushEffectDelegate(IntPtr value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushEventDelegate(IntPtr value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushLocationDelegate(IntPtr value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushTalentDelegate(IntPtr value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StackPushItemPropertyDelegate(IntPtr value);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int StackPopIntegerDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate float StackPopFloatDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string StackPopStringDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string StackPopStringUTF8Delegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint StackPopObjectDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Vector StackPopVectorDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr StackPopEffectDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr StackPopEventDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr StackPopLocationDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr StackPopTalentDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr StackPopItemPropertyDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FreeEffectDelegate(IntPtr ptr);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FreeEventDelegate(IntPtr ptr);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FreeLocationDelegate(IntPtr ptr);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FreeTalentDelegate(IntPtr ptr);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FreeItemPropertyDelegate(IntPtr ptr);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ClosureAssignCommandDelegate(uint oid, ulong eventId);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ClosureDelayCommandDelegate(uint oid, float duration, ulong eventId);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ClosureActionDoCommandDelegate(uint oid, ulong eventId);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nwnxSetFunctionDelegate(string plugin, string function);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nwnxPushIntDelegate(int n);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nwnxPushFloatDelegate(float f);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nwnxPushObjectDelegate(uint o);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nwnxPushStringDelegate(string s);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nwnxPushStringUTF8Delegate(string s);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nwnxPushEffectDelegate(IntPtr e);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nwnxPushItemPropertyDelegate(IntPtr ip);
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int nwnxPopIntDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate float nwnxPopFloatDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint nwnxPopObjectDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string nwnxPopStringDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string nwnxPopStringUTF8Delegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr nwnxPopEffectDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr nwnxPopItemPropertyDelegate();
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void nwnxCallFunctionDelegate();

        [StructLayout(LayoutKind.Sequential)]
        public struct BootstrapArgs
        {
            public GetFunctionPointerDelegate GetFunctionPointer;
            public RegisterHandlersDelegate RegisterHandlers;
            public CallBuiltInDelegate CallBuiltIn;
            public StackPushIntegerDelegate StackPushInteger;
            public StackPushFloatDelegate StackPushFloat;
            public StackPushStringDelegate StackPushString;
            public StackPushStringUTF8Delegate StackPushStringUTF8;
            public StackPushObjectDelegate StackPushObject;
            public StackPushVectorDelegate StackPushVector;
            public StackPushEffectDelegate StackPushEffect;
            public StackPushEventDelegate StackPushEvent;
            public StackPushLocationDelegate StackPushLocation;
            public StackPushTalentDelegate StackPushTalent;
            public StackPushItemPropertyDelegate StackPushItemProperty;
            public StackPopIntegerDelegate StackPopInteger;
            public StackPopFloatDelegate StackPopFloat;
            public StackPopStringDelegate StackPopString;
            public StackPopStringUTF8Delegate StackPopStringUTF8;
            public StackPopObjectDelegate StackPopObject;
            public StackPopVectorDelegate StackPopVector;
            public StackPopEffectDelegate StackPopEffect;
            public StackPopEventDelegate StackPopEvent;
            public StackPopLocationDelegate StackPopLocation;
            public StackPopTalentDelegate StackPopTalent;
            public StackPopItemPropertyDelegate StackPopItemProperty;
            public FreeEffectDelegate FreeEffect;
            public FreeEventDelegate FreeEvent;
            public FreeLocationDelegate FreeLocation;
            public FreeTalentDelegate FreeTalent;
            public FreeItemPropertyDelegate FreeItemProperty;
            public ClosureAssignCommandDelegate ClosureAssignCommand;
            public ClosureDelayCommandDelegate ClosureDelayCommand;
            public ClosureActionDoCommandDelegate ClosureActionDoCommand;
            public nwnxSetFunctionDelegate nwnxSetFunction;
            public nwnxPushIntDelegate nwnxPushInt;
            public nwnxPushFloatDelegate nwnxPushFloat;
            public nwnxPushObjectDelegate nwnxPushObject;
            public nwnxPushStringDelegate nwnxPushString;
            public nwnxPushStringUTF8Delegate nwnxPushStringUTF8;
            public nwnxPushEffectDelegate nwnxPushEffect;
            public nwnxPushItemPropertyDelegate nwnxPushItemProperty;
            public nwnxPopIntDelegate nwnxPopInt;
            public nwnxPopFloatDelegate nwnxPopFloat;
            public nwnxPopObjectDelegate nwnxPopObject;
            public nwnxPopStringDelegate nwnxPopString;
            public nwnxPopStringUTF8Delegate nwnxPopStringUTF8;
            public nwnxPopEffectDelegate nwnxPopEffect;
            public nwnxPopItemPropertyDelegate nwnxPopItemProperty;
            public nwnxCallFunctionDelegate nwnxCallFunction;
        }
        public static BootstrapArgs NativeFunctions;

        public static void RegisterHandlers(AllHandlers handlers)
        {
            var size = Marshal.SizeOf(typeof(AllHandlers));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(handlers, ptr, false);
            NativeFunctions.RegisterHandlers(ptr, (uint)size);
            Marshal.FreeHGlobal(ptr);
        }


        public static readonly Stack<ScriptContext> ScriptContexts = new Stack<ScriptContext>();
        

        private struct Closure
        {
            public NWGameObject OwnerObject;
            public ActionDelegate Run;
        }
        private static ulong NextEventId = 0;
        private static readonly Dictionary<ulong, Closure> Closures = new Dictionary<ulong, Closure>();

        public static void OnClosure(ulong eid, uint oidSelf)
        {
            try
            {
                Closures[eid].Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Closures.Remove(eid);
        }

        public static void ClosureAssignCommand(NWGameObject obj, ActionDelegate func)
        {
            if (NativeFunctions.ClosureAssignCommand(obj.Self, NextEventId) != 0)
            {
                Closures.Add(NextEventId++, new Closure { OwnerObject = obj, Run = func });
            }
        }

        public static void ClosureDelayCommand(NWGameObject obj, float duration, ActionDelegate func)
        {
            if (NativeFunctions.ClosureDelayCommand(obj.Self, duration, NextEventId) != 0)
            {
                Closures.Add(NextEventId++, new Closure { OwnerObject = obj, Run = func });
            }
        }

        public static void ClosureActionDoCommand(NWGameObject obj, ActionDelegate func)
        {
            if (NativeFunctions.ClosureActionDoCommand(obj.Self, NextEventId) != 0)
            {
                Closures.Add(NextEventId++, new Closure { OwnerObject = obj, Run = func });
            }
        }
    }
}