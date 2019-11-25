using System;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Utility;
using Serilog;
using static NWN._;
using Random = NWN.FinalFantasy.Core.Utility.Random;

namespace NWN.FinalFantasy.Loot
{
    public class SpawnLoot: IScript
    {
        // For every loot table:
        //   1.) Look for a chance and attempt variable. Use defaults if not found.
        //   2.) Get the loot table from the cache.
        //   3.) Attempt to spawn an item if random checks are met.
        // Loot tables must be added as local variables on the creature. Prefix is LOOT_TABLE_
        // Values are as follows: Name, Chance, Attempts
        public void Main()
        {
            var creature = NWGameObject.OBJECT_SELF;
            var lootTableEntries = LocalVariableTool.FindByPrefix(creature, "LOOT_TABLE_");

            foreach(var entry in lootTableEntries)
            {
                var data = entry.Split(',');
                string tableName = data[0].Trim();
                int chance = 100;
                int attempts = 1;

                // Second argument: Chance to spawn
                if (data.Length > 1)
                {
                    data[1] = data[1].Trim();
                    if(!int.TryParse(data[1], out chance))
                    {
                        Log.Warning($"Loot Table '{entry}' on {GetName(creature)}, 'Chance' variable could not be processed. Must be an integer.");
                    }
                }

                // Third argument: Attempts to pull from loot table
                if (data.Length > 2)
                {
                    data[2] = data[2].Trim();
                    if (!int.TryParse(data[1], out attempts))
                    {
                        Log.Warning($"Loot Table '{entry}' on {GetName(creature)}, 'Attempts' variable could not be processed. Must be an integer.");
                    }
                }

                // Guards against bad data from builder.
                if (chance > 100)
                    chance = 100;

                if (attempts <= 0)
                    attempts = 1;

                var table = LootTableRegistry.Get(tableName);
                for (int x = 1; x <= attempts; x++)
                {
                    if (Random.D100(1) > chance) continue;

                    var item = table.GetRandomItem();
                    var quantity = Random.Next(item.MaxQuantity) + 1;

                    CreateItemOnObject(item.Resref, creature, quantity);
                }
            }
        }
    }
}
