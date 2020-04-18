using System.Collections.Generic;

namespace NWN.FinalFantasy.Service.QuestService
{
    public interface IQuestListDefinition
    {
        public Dictionary<string, QuestDetail> BuildQuests();
    }
}
