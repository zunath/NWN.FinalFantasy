using System;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class JobRepo
    {
        private static string BuildKey(Guid playerID, ClassType type)
        {
            return $"Job:{playerID}:{type}";
        }

        public static Job Get(Guid playerID, ClassType type)
        {
            var key = BuildKey(playerID, type);

            if (!DB.Exists(key))
            {
                var entity = new Job();

                Set(playerID, type, entity);
                return entity;
            }

            return DB.Get<Job>(key);
        }

        public static void Set(Guid playerID, ClassType type, Job job)
        {
            var key = BuildKey(playerID, type);
            DB.Set(key, job);
        }
    }
}
