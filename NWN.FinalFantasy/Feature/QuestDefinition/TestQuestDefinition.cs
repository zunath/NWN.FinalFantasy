using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.QuestService;

namespace NWN.FinalFantasy.Feature.QuestDefinition
{
    public class TestQuestDefinition: IQuestListDefinition
    {
        public Dictionary<string, QuestDetail> BuildQuests()
        {
            var builder = new QuestBuilder()
                .Create("myQuestId", "Test Quest", "test_quest")
                .AddState()
                .AddKillObjective(NPCGroupType.TestGroup, 2)
                .AddState()
                .AddCollectItemObjective("quest_item", 3)
                .AddState();

            return builder.Build();
        }
    }
}
