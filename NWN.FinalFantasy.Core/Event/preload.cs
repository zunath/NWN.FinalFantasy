using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Startup;
using Serilog;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    internal static class preload
    {
        internal static void Main()
        {
            ConfigureLogger();
            EventRegistration.Register();
            AssemblyLoader.LoadAssemblies();
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
