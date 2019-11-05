namespace NWN.FinalFantasy.Loot.API
{
    internal interface ILootTable
    {
        /// <summary>
        /// Adds an item to this loot table.
        /// </summary>
        /// <param name="resref">The resref of the item</param>
        /// <param name="weightedChance">The weighted chance of the item to drop</param>
        /// <param name="maxQuantity">The max quantity of this item to drop. A random value is selected, not to exceed this value.</param>
        /// <returns>The loot table with the applied changes.</returns>
        void AddItem(string resref, int weightedChance, int maxQuantity = 1);

        /// <summary>
        /// Adds gold to this loot table.
        /// </summary>
        /// <param name="maxAmount">The max amount of gold to drop. A random value is selected, not to exceed this value.</param>
        /// <param name="weightedChance">The weighted chance of the gold to drop</param>
        void AddGold(int maxAmount, int weightedChance);

        /// <summary>
        /// Retrieves a random item from the loot table.
        /// Throws an exception if there are no items in the loot table.
        /// </summary>
        /// <returns>A random item from the loot table.</returns>
        ILootTableItem GetRandomItem();
    }
}
