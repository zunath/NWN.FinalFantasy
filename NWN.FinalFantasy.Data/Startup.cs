using Newtonsoft.Json;

namespace NWN.FinalFantasy.Data
{
    public class Startup
    {
        public static void Main()
        {
            ConfigureJsonFormatter();
        }

        private static void ConfigureJsonFormatter()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }
    }
}
