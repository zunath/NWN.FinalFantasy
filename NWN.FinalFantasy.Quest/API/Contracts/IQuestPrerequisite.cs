namespace NWN.FinalFantasy.Quest.API.Contracts
{
    public interface IQuestPrerequisite
    {
        bool MeetsPrerequisite(NWGameObject player);
    }
}
