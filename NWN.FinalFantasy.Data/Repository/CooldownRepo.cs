using System;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class CooldownRepo
    {
        private static string BuildKey(Guid playerID, Feat feat)
        {
            return $"Cooldown:{playerID}:{feat}";
        }

        public static Cooldown Get(Guid playerID, Feat feat)
        {
            var key = BuildKey(playerID, feat);

            if (!DB.Exists(key))
            {
                var entity = new Cooldown
                {
                    DateUnlocked = DateTime.UtcNow,
                    Feat = feat
                };

                Set(playerID, entity);
                return entity;
            }

            return DB.Get<Cooldown>(key);
        }

        public static void Set(Guid playerID, Cooldown entity)
        {
            var key = BuildKey(playerID, entity.Feat);
            DB.Set(key, entity);
        }
    }
}
