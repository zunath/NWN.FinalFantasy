using System.Collections.Generic;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Migration.Server;

namespace NWN.FinalFantasy.Migration
{
    internal class ServerMigrationRunner
    {
        private static readonly Queue<IServerMigration> _registeredMigrations = new Queue<IServerMigration>();

        static ServerMigrationRunner()
        {
            _registeredMigrations.Enqueue(new ServerVersion1(1));
        }

        public static void Run()
        {
            var serverConfig = ServerConfigurationRepo.Get();

            // Iterate over the registered migrations. If server is below the migration version, that migration will be executed.
            // If server is at or above that version, nothing will happen and they will move to the next migration in the list.
            foreach (var migration in _registeredMigrations)
            {
                if (serverConfig.Version < migration.Version)
                {
                    migration.RunMigration();
                    serverConfig.Version = migration.Version;
                    ServerConfigurationRepo.Set(serverConfig);
                }
            }
        }
    }
}
