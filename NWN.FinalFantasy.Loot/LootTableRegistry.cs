using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Loot.API;

namespace NWN.FinalFantasy.Loot
{
    internal static class LootTableRegistry
    {
        private static readonly Dictionary<string, ILootTable> _lootTables = new Dictionary<string, ILootTable>();

        /// <summary>
        /// Adds a loot table to the registry. Loot tables must be registered with unique names.
        /// If a loot table already exists with that name, an exception will be raised.
        /// </summary>
        /// <param name="name">The name of the loot table</param>
        /// <param name="lootTable">The loot table to register</param>
        internal static void Add(string name, LootTable lootTable)
        {
            if (_lootTables.ContainsKey(name))
            {
                throw new Exception($"Loot table with name '{name}' has already been registered. Loot table names must be unique.");
            }

            _lootTables[name] = lootTable;
        }

        /// <summary>
        /// Retrieves a loot table by its unique name.
        /// </summary>
        /// <param name="name">The unique ID of the loot table.</param>
        /// <returns>The registered loot table</returns>
        internal static ILootTable Get(string name)
        {
            if(!_lootTables.ContainsKey(name))
                throw new Exception($"Loot table with name '{name}' has not been registered.");

            return _lootTables[name];
        }
    }
}
