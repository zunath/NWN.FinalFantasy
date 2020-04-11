using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.SpawnService
{
    public interface ISpawnTableDefinition
    {
        /// <summary>
        /// Creates a dictionary of spawn tables to be stored in the cache.
        /// </summary>
        /// <returns>A dictionary of spawn tables.</returns>
        public Dictionary<int, SpawnTable> BuildSpawnTables();
    }
}
