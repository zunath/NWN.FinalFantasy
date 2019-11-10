using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Quest.API.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Quest.API.Objective
{
    internal class KillTargetObjective: IQuestObjective
    {
        private readonly string _group;
        private readonly int _amount;

        public KillTargetObjective(string group, int amount)
        {
            _group = group;
            _amount = amount;
        }

        public void Initialize(NWGameObject player, string questID)
        {
            var playerID = GetGlobalID(player);
            var status = QuestProgressRepo.Get(playerID, questID);
            var progress = new QuestProgress.KillProgress
            {
                NPCGroupID = _group,
                Remaining = _amount
            };

            status.KillProgresses.Add(progress);
            QuestProgressRepo.Set(playerID, status);
        }

        public bool IsComplete(NWGameObject player, string questID)
        {
            var playerID = GetGlobalID(player);
            var status = QuestProgressRepo.Get(playerID, questID);

            foreach (var progress in status.KillProgresses)
            {
                if (progress.Remaining > 0)
                    return false;
            }

            return true;
        }
    }
}
