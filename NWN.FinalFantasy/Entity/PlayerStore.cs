using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Entity
{
    public class PlayerStore: EntityBase
    {
        public PlayerStore()
        {
            ItemsForSale = new Dictionary<string, PlayerStoreItem>();
            DateLeaseExpires = DateTime.UtcNow;
            IsOpen = false;
        }

        public string StoreName { get; set; }
        public Dictionary<string, PlayerStoreItem> ItemsForSale { get; set; }
        public override string KeyPrefix => "PlayerStore";
        public DateTime DateLeaseExpires { get; set; }
        public bool IsOpen { get; set; }
    }

    public class PlayerStoreItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int StackSize { get; set; }
        public string Data { get; set; }
    }
}
