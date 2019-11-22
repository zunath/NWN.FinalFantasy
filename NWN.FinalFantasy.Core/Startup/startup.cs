using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.Messaging;
using Serilog;

namespace NWN.FinalFantasy.Core.Startup
{
    /// <summary>
    /// This script is executed before the module OnLoad event and is responsible for all the bootstrapping tasks
    /// necessary for the framework. This entry point is case sensitive and must be lower case per NWN's naming rules on scripts.
    /// </summary>
    internal static class Startup
    {
        internal static void Main()
        {
            ConfigureLogger();
            RegisterMessageHubErrorHandler();
            Audit.Initialize();
            EventRegistration.Register();
            CustomEventRegistration.Register();
            AssemblyLoader.LoadAssemblies();
            AreaScriptRegistration.Register();
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
    }
}
