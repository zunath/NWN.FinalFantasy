﻿using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.QuestService;

namespace NWN.FinalFantasy.Feature.QuestDefinition
{
    public class TestQuestDefinition: IQuestListDefinition
    {
        public Dictionary<string, QuestDetail> BuildQuests()
        {
            var builder = new QuestBuilder()
                .Create("myQuestId", "Test Quest")
                .AddState()
                .SetStateJournalText("Go kill some stuff")
                .AddKillObjective(NPCGroupType.TestGroup, 2)

                .AddState()
                .SetStateJournalText("go collect some quest items")
                .AddCollectItemObjective("quest_item", 3)

                .AddState()
                .SetStateJournalText("go talk to the npc");

            return builder.Build();
        }
    }
}
