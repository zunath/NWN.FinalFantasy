using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;

namespace NWN.FinalFantasy.Service.SpawnService
{
    public class SpawnTableBuilder
    {
        private readonly Dictionary<int, SpawnTable> SpawnTables = new Dictionary<int, SpawnTable>();

        private SpawnTable ActiveTable { get; set; }
        private SpawnObject ActiveSpawn { get; set; }


        /// <summary>
        /// Creates a new spawn table with the specified Id
        /// </summary>
        /// <param name="spawnTableId">The spawn table Id to create the table with</param>
        /// <param name="name">The name of the spawn table. This is purely for the programmer's benefit. Not used by the system.</param>
        /// <returns>A spawn table builder with the configured settings.</returns>
        public static SpawnTableBuilder Create(int spawnTableId, string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = $"Spawn Table {spawnTableId}";

            var builder = new SpawnTableBuilder();
            builder.ActiveTable = new SpawnTable(name);
            builder.SpawnTables[spawnTableId] = builder.ActiveTable;

            return builder;
        }

        /// <summary>
        /// Sets the number of minutes before a respawn takes place.
        /// Values less than 1 will default to 1.
        /// </summary>
        /// <param name="minutes">The number of minutes before a respawn takes place.</param>
        public void RespawnDelay(int minutes)
        {
            if (minutes < 1) minutes = 1;
            ActiveTable.RespawnDelayMinutes = minutes;
        }

        /// <summary>
        /// Adds a new spawn object to this spawn table.
        /// </summary>
        /// <param name="type">The object type to spawn</param>
        /// <param name="resref">The resref of the object</param>
        /// <returns>A spawn table builder with the configured settings</returns>
        public SpawnTableBuilder AddSpawn(ObjectType type, string resref)
        {
            ActiveSpawn = new SpawnObject
            {
                Type = type,
                Resref = resref,
                Weight = 10
            };
            ActiveTable.Spawns.Add(ActiveSpawn);

            return this;
        }

        /// <summary>
        /// Sets the frequency of a spawn. This modifies the likelihood of this particular object spawning
        /// based on the weight of all other objects in the same table.
        /// In laymen's terms, the higher this number is, the more likely it will appear.
        /// </summary>
        /// <param name="frequency">The frequency to set.</param>
        /// <returns>A spawn table builder with the configured settings</returns>
        public SpawnTableBuilder WithFrequency(int frequency)
        {
            if (frequency < 1) frequency = 1;

            ActiveSpawn.Weight = frequency;
            return this;
        }

        /// <summary>
        /// Builds a dictionary of spawn tables.
        /// </summary>
        /// <returns>A dictionary of spawn tables</returns>
        public Dictionary<int, SpawnTable> Build()
        {
            return SpawnTables;
        }
    }
}
