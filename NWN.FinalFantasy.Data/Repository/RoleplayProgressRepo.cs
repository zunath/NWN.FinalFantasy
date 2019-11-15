using System;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class RoleplayProgressRepo
    {
        private static string BuildKey(Guid playerID)
        {
            return $"Roleplay:{playerID}";
        }

        public static RoleplayProgress Get(Guid playerID)
        {
            var key = BuildKey(playerID);

            if (!DB.Exists(key))
            {
                var entity = new RoleplayProgress();

                Set(playerID, entity);
                return entity;
            }

            return DB.Get<RoleplayProgress>(key);
        }

        public static void Set(Guid playerID, RoleplayProgress entity)
        {
            var key = BuildKey(playerID);
            DB.Set(key, entity);
        }
    }
}
