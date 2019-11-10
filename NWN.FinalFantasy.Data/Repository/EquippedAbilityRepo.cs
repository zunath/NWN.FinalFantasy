using System;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class EquippedAbilityRepo
    {
        private static string BuildKey(Guid playerID)
        {
            return $"EquippedAbility:{playerID}";
        }

        public static EntityList<EquippedAbility> Get(Guid playerID)
        {
            var key = BuildKey(playerID);

            if (!DB.Exists(key))
            {
                var entity = new EntityList<EquippedAbility>(playerID);

                Set(playerID, entity);
                return entity;
            }

            return DB.GetList<EquippedAbility>(key);
        }

        public static void Set(Guid playerID, EntityList<EquippedAbility> entity)
        {
            var key = BuildKey(playerID);
            DB.Set(key, entity);
        }
    }
}