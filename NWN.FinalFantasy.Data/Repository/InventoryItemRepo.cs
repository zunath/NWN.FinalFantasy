using System;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class InventoryItemRepo
    {
        public static EntityList<InventoryItem> Get(string key)
        {
            if (!DB.Exists(key))
            {
                var entities = new EntityList<InventoryItem>(Guid.NewGuid());

                DB.Set(key, entities);
                return entities;
            }

            return DB.GetList<InventoryItem>(key);
        }

        public static void Set(string key, EntityList<InventoryItem> entities)
        {
            DB.Set(key, entities);
        }
    }
}
