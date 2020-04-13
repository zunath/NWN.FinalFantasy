using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWN.FinalFantasy.Service.LootService
{
    public class LootTable : List<LootTableItem>
    {
        /// <summary>
        /// Retrieves a random item from the loot table.
        /// Throws an exception if there are no items in the loot table.
        /// </summary>
        /// <returns>A random item from the loot table.</returns>
        public LootTableItem GetRandomItem()
        {
            if (Count <= 0)
                throw new Exception("No items are in this loot table.");

            int[] weights = new int[Count];
            for (int x = 0; x < Count; x++)
            {
                weights[x] = this.ElementAt(x).Weight;
            }

            int randomIndex = Random.GetRandomWeightedIndex(weights);
            return this.ElementAt(randomIndex);
        }
    }
}