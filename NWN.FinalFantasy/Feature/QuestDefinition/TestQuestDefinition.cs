using System.Collections.Generic;
using NWN.FinalFantasy.Service.QuestService;

namespace NWN.FinalFantasy.Feature.QuestDefinition
{
    public class TestQuestDefinition: IQuestListDefinition
    {
        public Dictionary<string, QuestDetail> BuildQuests()
        {
            var builder = new QuestBuilder();
            return builder.Build();
        }
    }
}
