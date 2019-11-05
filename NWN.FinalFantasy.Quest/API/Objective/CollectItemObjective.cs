using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Quest.API.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Quest.API.Objective
{
    internal class CollectItemObjective: IQuestObjective
    {
        private readonly string _resref;
        private readonly int _quantity;

        public CollectItemObjective(string resref, int quantity)
        {
            _resref = resref;
            _quantity = quantity;
        }

        public void Initialize(NWGameObject player, string questID)
        {
            var playerID = GetGlobalID(player);
            var status = QuestProgressRepo.Get(playerID, questID);

            var itemProgress = new QuestProgress.ItemProgress
            {
                Resref = _resref,
                Remaining = _quantity
            };

            status.ItemProgresses.Add(itemProgress);
            QuestProgressRepo.Set(playerID, status);
        }

        public bool IsComplete(NWGameObject player, string questID)
        {
            var playerID = GetGlobalID(player);
            var status = QuestProgressRepo.Get(playerID, questID);

            foreach (var progress in status.ItemProgresses)
            {
                if (progress.Remaining > 0)
                    return false;
            }

            return true;
        }
    }
}
