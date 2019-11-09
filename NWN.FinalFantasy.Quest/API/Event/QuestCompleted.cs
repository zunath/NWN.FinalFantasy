namespace NWN.FinalFantasy.Quest.API.Event
{
    public class QuestCompleted
    {
        public NWGameObject Player { get; set; }
        public string QuestID { get; set; }

        public QuestCompleted(NWGameObject player, string questID)
        {
            Player = player;
            QuestID = questID;
        }
    }
}
