using System;
using System.Runtime.InteropServices;
using NWN.FinalFantasy.Core;
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
        /// <summary>
        /// Runs when the server boots up. The NWN context is not available at this point.
        /// </summary>
        public void OnStart()
        {
            ConfigureLogger();
            RegisterMessageHubErrorHandler();
            Audit.Initialize();

            ScriptRegistration.RegisterStartupAction(OnNWNContextReady);
        }

        /// <summary>
        /// Runs when the NWN context is ready. The NWN context is available and can be used at this point.
        /// </summary>
        public void OnNWNContextReady()
        {
            EventRegistration.Register();
            ScriptRegistration.OnNWNContextReady();
            CustomEventRegistration.Register();
            AreaScriptRegistration.Register();
            FeatureRegistration.Register();
            Script.CacheScripts();
        }

        /// <summary>
        /// Executes once per server frame.
        /// </summary>
        /// <param name="frame"></param>
        public void OnMainLoop(ulong frame)
        {
        }

        /// <summary>
        /// Runs whenever a script is requested by NWN.
        /// </summary>
        /// <param name="script">The name of the script requested.</param>
        /// <param name="oidSelf">The object ID of the object calling this script.</param>
        /// <returns></returns>
        public int OnRunScript(string script, uint oidSelf)
        {
            return ScriptRegistration.OnRunScript(script);
        }

        /// <summary>
        /// Sets up the logger.
        /// </summary>
        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        /// <summary>
        /// Configure the message hub to write an audit in the event of an error.
        /// </summary>
        private static void RegisterMessageHubErrorHandler()
        {
            MessageHub.Instance.RegisterGlobalErrorHandler((guid, exception) => Audit.Write(AuditGroup.Error, exception.ToMessageAndCompleteStacktrace()));
        }

        /// <summary>
        /// Entry point for the NWNX_DotNET plugin.
        /// </summary>
        /// <param name="arg">Pointer to arguments</param>
        /// <param name="argLength">Length of the arguments</param>
        /// <returns></returns>
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
