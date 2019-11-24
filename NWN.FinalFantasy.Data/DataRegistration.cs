using Newtonsoft.Json;
using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Data
{
    public class DataRegistration: IFeatureRegistration
    {
        private static void ConfigureJsonFormatter()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }

        public void Register()
        {
            Log.Information("Registering Data...");
            ConfigureJsonFormatter();
        }
    }
}
