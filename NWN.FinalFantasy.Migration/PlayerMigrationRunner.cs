using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Migration.PC;

namespace NWN.FinalFantasy.Migration
{
    internal class PlayerMigrationRunner
    {
        private static readonly Queue<IPCMigration> _registeredMigrations = new Queue<IPCMigration>();

        static PlayerMigrationRunner()
        {
            // All migrations should be assigned here with unique, sequential IDs.
            _registeredMigrations.Enqueue(new PlayerVersion1(1));
        }

        /// <summary>
        /// Handles the migration of player characters.
        /// </summary>
        public static void Main()
        {
            var player = _.GetEnteringObject();
            if (!_.GetIsPlayer(player)) return;

            // Player doesn't have an ID assigned. Generate one and set it on them.
            string tag = _.GetTag(player);
            if (string.IsNullOrWhiteSpace(tag))
            {
                var id = Guid.NewGuid();
                _.SetTag(player, id.ToString());
                SetNewPlayerEntity(id);
            }

            var pcID = _.GetGlobalID(player);

            // Player has an ID but somehow there's no entry in the database. Treat them as a new character.
            if(!DB.Exists<Player>(pcID))
            {
                SetNewPlayerEntity(pcID);
            }

            var pcEntity = DB.Get<Player>(pcID);
            // Iterate over the registered migrations. If player is below the migration version, that migration will be executed upon them.
            // If player is at or above that version, nothing will happen and they will move to the next migration in the list.
            foreach (var migration in _registeredMigrations)
            {
                if (pcEntity.Version < migration.Version)
                {
                    try
                    {
                        migration.RunMigration(player);
                        pcEntity.Version = migration.Version;
                        DB.Set(pcEntity);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Failed to apply migration {migration.GetType().FullName}", ex);
                    }
                }
            }

        }

        private static void SetNewPlayerEntity(Guid id)
        {
            var entity = new Player
            {
                ID = id,
                Version = 0
            };

            DB.Set(entity);
        }
    }
}
