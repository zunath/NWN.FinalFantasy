namespace NWN.FinalFantasy.Quest.TestLocation
{
    internal class MyTestQuest: API.Quest
    {
        public MyTestQuest()
        {
            QuestID = "myQuestID";
            Name = "My Quest Name";
            JournalTag = "my_quest_tag";

            AddState()
                .AddObjectiveKillTarget("target_group", 2)
                .AddObjectiveCollectItem("myitem", 4);
        }
    }
}
