using System;
using MessagePack;

namespace NWN.FinalFantasy.Entity
{
    [Union(1, typeof(Account))]
    [Union(2, typeof(AuthorizedDM))]
    [Union(3, typeof(BugReport))]
    [Union(4, typeof(InventoryItem))]
    [Union(5, typeof(Player))]
    [Union(6, typeof(PlayerHouse))]
    [Union(7, typeof(PlayerStore))]
    [Union(8, typeof(PlayerTripleTriad))]
    [Union(9, typeof(ServerConfiguration))]
    public abstract class EntityBase
    {
        public Guid ID { get; set; }
        public DateTime DateCreated { get; set; }
        public abstract string KeyPrefix { get; }

        protected EntityBase()
        {
            ID = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
        }
    }
}