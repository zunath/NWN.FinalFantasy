using System;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class MapPinRepo
    {
        private static string BuildKey(Guid playerID)
        {
            return $"MapPin:{playerID}";
        }

        public static EntityList<MapPin> Get(Guid playerID)
        {
            var key = BuildKey(playerID);

            if (!DB.Exists(key))
            {
                var entity = new EntityList<MapPin>(playerID);

                Set(playerID, entity);
                return entity;
            }

            return DB.GetList<MapPin>(key);
        }

        public static void Set(Guid playerID, EntityList<MapPin> entity)
        {
            var key = BuildKey(playerID);
            DB.Set(key, entity);
        }
    }
}
