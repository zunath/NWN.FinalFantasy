using System;

namespace NWN.FinalFantasy.Enumeration
{
    public enum PlayerHouseType
    {
        [PlayerHouse("Invalid", 0, 100, 0, "", false)]
        Invalid = 0,
        [PlayerHouse("Basic", 30, 0, 0, "player_layout_1", true)] //todo change seed rank back to 1, and price back to 5000
        Basic = 1,
        [PlayerHouse("Advanced", 50, 3, 150000, "", true)]
        Advanced = 2,
        [PlayerHouse("Superior", 70, 6, 30000, "", true)]
        Superior = 3,
        [PlayerHouse("Master", 100, 10, 50000, "", true)]
        Master = 4
    }

    public class PlayerHouseAttribute : Attribute
    {
        public string Name { get; set; }
        public int FurnitureLimit { get; set; }
        public bool IsActive { get; set; }
        public int RequiredSeedRank { get; set; }
        public int Price { get; set; }
        public string AreaInstanceResref { get; set; }

        public PlayerHouseAttribute(
            string name, 
            int furnitureLimit, 
            int requiredSeedRank,
            int price,
            string areaInstanceResref,
            bool isActive)
        {
            Name = name;
            FurnitureLimit = furnitureLimit;
            RequiredSeedRank = requiredSeedRank;
            Price = price;
            AreaInstanceResref = areaInstanceResref;
            IsActive = isActive;
        }
    }
}
