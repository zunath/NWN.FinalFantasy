using System;
using System.Collections.Generic;
using System.Linq;
using Random = NWN.FinalFantasy.Core.Utility.Random;

namespace NWN.FinalFantasy.Loot.API
{
    internal class LootTable: List<LootTableItem>, ILootTable
    {
        /// <summary>
        /// Adds an item to this loot table.
        /// </summary>
        /// <param name="resref">The resref of the item</param>
        /// <param name="weightedChance">The weighted chance of the item to drop</param>
        /// <param name="maxQuantity">The max quantity of this item to drop. A random value is selected, not to exceed this value.</param>
        /// <returns>The loot table with the applied changes.</returns>
        public void AddItem(string resref, int weightedChance, int maxQuantity = 1)
        {
            Add(new LootTableItem(resref.ToLower(), maxQuantity, weightedChance));
        }

        /// <summary>
        /// Adds gold to this loot table.
        /// </summary>
        /// <param name="maxAmount">The max amount of gold to drop. A random value is selected, not to exceed this value.</param>
        /// <param name="weightedChance">The weighted chance of the gold to drop</param>
        public void AddGold(int maxAmount, int weightedChance)
        {
            const string GoldResref = "nw_it_gold001";
            Add(new LootTableItem(GoldResref, maxAmount, weightedChance));
        }

        /// <summary>
        /// Retrieves a random item from the loot table.
        /// Throws an exception if there are no items in the loot table.
        /// </summary>
        /// <returns>A random item from the loot table.</returns>
        public ILootTableItem GetRandomItem()
        {
            if(Count <= 0)
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
