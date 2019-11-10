namespace NWN.FinalFantasy.Quest.API.Event
{
    public class QuestAdvanced
    {
        public NWGameObject Player { get; set; }
        public string QuestID { get; set; }
        public int QuestState { get; set; }

        public QuestAdvanced(NWGameObject player, string questID, int questState)
        {
            Player = player;
            QuestID = questID;
            QuestState = questState;
        }
    }
}
