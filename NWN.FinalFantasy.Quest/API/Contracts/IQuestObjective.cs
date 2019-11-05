namespace NWN.FinalFantasy.Quest.API.Contracts
{
    public interface IQuestObjective
    {
        void Initialize(NWGameObject player, string questID);
        bool IsComplete(NWGameObject player, string questID);
    }
}
