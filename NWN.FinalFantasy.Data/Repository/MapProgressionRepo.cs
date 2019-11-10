using System;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class MapProgressionRepo
    {
        private static string BuildKey(Guid playerID, string areaResref)
        {
            return $"MapProgression:{playerID}:{areaResref}";
        }

        public static MapProgression Get(Guid playerID, string areaResref)
        {
            var key = BuildKey(playerID, areaResref);

            if (!DB.Exists(key))
            {
                var entity = new MapProgression
                {
                    AreaResref = areaResref
                };

                Set(playerID, entity);
                return entity;
            }

            return DB.Get<MapProgression>(key);
        }

        public static void Set(Guid playerID, MapProgression entity)
        {
            var key = BuildKey(playerID, entity.AreaResref);
            DB.Set(key, entity);
        }
    }
}
