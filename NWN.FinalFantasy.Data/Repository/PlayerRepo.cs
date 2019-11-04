using System;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class PlayerRepo
    {
        private static string BuildKey(Guid id)
        {
            return $"Player:{id.ToString()}";
        }

        public static Player Get(Guid id)
        {
            var key = BuildKey(id);

            if (!DB.Exists(key))
            {
                var entity = new Player
                {
                    ID = id,
                    Version = 0
                };

                DB.Set(key, entity);

                return entity;
            }

            return DB.Get<Player>(key);
        }

        public static void Set(Player player)
        {
            var key = BuildKey(player.ID);
            DB.Set(key, player);
        }
    }
}
