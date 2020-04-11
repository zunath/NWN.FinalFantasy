using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.LootService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;

namespace NWN.FinalFantasy.Feature
{
    public class Loot
    {
        private static readonly Dictionary<string, LootTable> LootTables = new Dictionary<string, LootTable>();

        [NWNEventHandler("mod_load")]
        public static void RegisterLootTables()
        {
            // Get all implementations of spawn table definitions.
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(w => typeof(ILootTableDefinition).IsAssignableFrom(w) && !w.IsInterface && !w.IsAbstract);

            foreach (var type in types)
            {
                var instance = (ILootTableDefinition)Activator.CreateInstance(type);
                var builtTables = instance.BuildLootTables();

                foreach (var table in builtTables)
                {
                    if (string.IsNullOrWhiteSpace(table.Key))
                    {
                        Log.Write(LogGroup.Error, $"Loot table {table.Key} has an invalid key. Values must not be null or white space.");
                        continue;
                    }

                    if (LootTables.ContainsKey(table.Key))
                    {
                        Log.Write(LogGroup.Error, $"Loot table {table.Key} has already been registered. Please make sure all spawn tables use a unique ID.");
                        continue;
                    }

                    LootTables[table.Key] = table.Value;
                }
            }
        }

        [NWNEventHandler("crea_spawn")]
        public static void SpawnLoot()
        {
            var creature = OBJECT_SELF;
            var lootTableEntries = GetLootTableDetails(creature);

            foreach (var entry in lootTableEntries)
            {
                var data = entry.Split(',');
                string tableName = data[0].Trim();
                int chance = 100;
                int attempts = 1;

                // Second argument: Chance to spawn
                if (data.Length > 1)
                {
                    data[1] = data[1].Trim();
                    if (!int.TryParse(data[1], out chance))
                    {
                        Console.WriteLine($"Loot Table '{entry}' on {GetName(creature)}, 'Chance' variable could not be processed. Must be an integer.");
                    }
                }

                // Third argument: Attempts to pull from loot table
                if (data.Length > 2)
                {
                    data[2] = data[2].Trim();
                    if (!int.TryParse(data[1], out attempts))
                    {
                        Console.WriteLine($"Loot Table '{entry}' on {GetName(creature)}, 'Attempts' variable could not be processed. Must be an integer.");
                    }
                }

                // Guards against bad data from builder.
                if (chance > 100)
                    chance = 100;

                if (attempts <= 0)
                    attempts = 1;

                var table = GetLootTableByName(tableName);
                for (int x = 1; x <= attempts; x++)
                {
                    if (Random.D100(1) > chance) continue;

                    var item = table.GetRandomItem();
                    var quantity = Random.Next(item.MaxQuantity) + 1;

                    CreateItemOnObject(item.Resref, creature, quantity);
                }
            }
        }

        /// <summary>
        /// Retrieves a loot table by its unique name.
        /// If name is not registered, an exception will be raised.
        /// </summary>
        /// <param name="name">The name of the loot table to retrieve.</param>
        /// <returns>A loot table matching the specified name.</returns>
        public static LootTable GetLootTableByName(string name)
        {
            if (!LootTables.ContainsKey(name))
                throw new Exception($"Loot table '{name}' is not registered. Did you enter the right name?");

            return LootTables[name];
        }

        /// <summary>
        /// Returns all of the loot table details found on a creature's local variables.
        /// </summary>
        /// <param name="creature">The creature to search.</param>
        /// <returns>A list of loot table details.</returns>
        private static IEnumerable<string> GetLootTableDetails(uint creature)
        {
            var lootTables = new List<string>();

            int index = 1;
            var localVariableName = "LOOT_TABLE_" + index;
            var localVariable = GetLocalString(creature, localVariableName);

            while (!string.IsNullOrWhiteSpace(localVariable))
            {
                localVariable = GetLocalString(creature, localVariableName);
                if (string.IsNullOrWhiteSpace(localVariable)) break;

                lootTables.Add(localVariable);

                index++;
                localVariableName = "LOOT_TABLE_" + index;
            }

            return lootTables;
        }
    }
}
