using NWN.FinalFantasy.Core;
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
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
