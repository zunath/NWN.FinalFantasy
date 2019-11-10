using System;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public class AbilityProgressRepo
    {
        private static string BuildKey(Guid playerID, Feat feat)
        {
            return $"AbilityProgress:{playerID}:{feat}";
        }

        public static AbilityProgress Get(Guid playerID, Feat feat)
        {
            var key = BuildKey(playerID, feat);

            if (!DB.Exists(key))
            {
                var entity = new AbilityProgress
                {
                    Feat = feat
                };

                Set(playerID, entity);
                return entity;
            }

            return DB.Get<AbilityProgress>(key);
        }

        public static void Set(Guid playerID, AbilityProgress entity)
        {
            var key = BuildKey(playerID, entity.Feat);
            DB.Set(key, entity);
        }
    }
}