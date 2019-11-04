using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class ServerConfigurationRepo
    {
        public static ServerConfiguration Get()
        {
            if (!DB.Exists(Keys.ServerConfiguration))
            {
                var config = new ServerConfiguration();
                Set(config);
                return config;
            }

            return DB.Get<ServerConfiguration>(Keys.ServerConfiguration);
        }

        public static void Set(ServerConfiguration entity)
        {
            DB.Set(Keys.ServerConfiguration, entity);
        }
    }
}
