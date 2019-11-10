using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Quest.API.Contracts;
using static NWN._;

namespace NWN.FinalFantasy.Quest.API.Prerequisite
{
    internal class RequiredQuestPrerequisite: IQuestPrerequisite
    {
        private readonly string _questID;

        public RequiredQuestPrerequisite(string questID)
        {
            _questID = questID;
        }

        public bool MeetsPrerequisite(NWGameObject player)
        {
            var playerID = GetGlobalID(player);
            var quest = QuestProgressRepo.Get(playerID, _questID);
            return quest.TimesCompleted > 0;
        }
    }
}
