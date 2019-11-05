using System;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public class QuestProgressRepo
    {
        private static string BuildKey(Guid playerID, string questID)
        {
            return $"Quest:{playerID}:{questID}";
        }

        public static QuestProgress Get(Guid playerID, string questID)
        {
            var key = BuildKey(playerID, questID);

            if (!DB.Exists(key))
            {
                var entity = new QuestProgress
                {
                    QuestID = questID
                };

                Set(playerID, entity);
                return entity;
            }

            return DB.Get<QuestProgress>(key);
        }

        public static void Set(Guid playerID, QuestProgress entity)
        {
            var key = BuildKey(playerID, entity.QuestID);
            DB.Set(key, entity);
        }
    }
}
