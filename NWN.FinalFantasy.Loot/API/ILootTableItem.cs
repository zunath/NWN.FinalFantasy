namespace NWN.FinalFantasy.Loot.API
{
    internal interface ILootTableItem
    {
        string Resref { get; set; }
        int MaxQuantity { get; set; }
        int Weight { get; set; }
    }
}
