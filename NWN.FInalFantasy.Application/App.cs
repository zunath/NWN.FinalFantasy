using System;
using System.Runtime.InteropServices;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.Messaging;
using Serilog;

namespace NWN.FinalFantasy.Application
{
    /// <summary>
    /// This script is executed before the module OnLoad event and is responsible for all the bootstrapping tasks
    /// necessary for the framework. 
    /// </summary>
    internal class App: IApplication
    {
        private ScriptRegistration _scriptRegistration;

        public void OnStart()
        {
            ConfigureLogger();
            RegisterMessageHubErrorHandler();
            Audit.Initialize();

            _scriptRegistration = new ScriptRegistration(OnNWNContextReady);
        }

        public void OnNWNContextReady()
        {
            EventRegistration.Register();
            _scriptRegistration.OnNWNContextReady();
            CustomEventRegistration.Register();
            AreaScriptRegistration.Register();
            FeatureRegistration.Register();
        }

        public void OnMainLoop(ulong frame)
        {
        }

        public int OnRunScript(string script, uint oidSelf)
        {
            return _scriptRegistration.OnRunScript(script);
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static void RegisterMessageHubErrorHandler()
        {
            MessageHub.Instance.RegisterGlobalErrorHandler((guid, exception) => Audit.Write(AuditGroup.Error, exception.ToMessageAndCompleteStacktrace()));
        }


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
            handlers.MainLoop = Internal.OnMainLoop;
            handlers.RunScript = Internal.OnRunScript;
            handlers.Closure = Internal.OnClosure;
            Internal.RegisterHandlers(handlers);

            try
            {
                Internal.Application.OnStart();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }
    }
}
