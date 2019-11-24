using System;
using System.Reflection;
using System.Runtime.Loader;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Location;
using Serilog;

namespace NWN.FinalFantasy.Application
{
    /// <summary>
    /// This script is executed before the module OnLoad event and is responsible for all the bootstrapping tasks
    /// necessary for the framework. 
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
            AreaScriptRegistration.Register();

            FeatureRegistration.Register();
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
