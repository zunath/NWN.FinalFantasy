namespace NWN.FinalFantasy.Core.Message
{
    public class QuestAccepted
    {
        public NWGameObject Player { get; set; }
        public string QuestID { get; set; }

        public QuestAccepted(NWGameObject player, string questID)
        {
            Player = player;
            QuestID = questID;
        }
    }
}
