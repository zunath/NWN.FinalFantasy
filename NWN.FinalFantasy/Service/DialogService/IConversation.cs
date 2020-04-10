namespace NWN.FinalFantasy.Service.DialogService
{
    public interface IConversation
    {
        PlayerDialog SetUp(uint player);
        void Initialize();
        void DoAction(uint player, string pageName, int responseID);
        void Back(uint player, string beforeMovePage, string afterMovePage);
        void EndDialog();
    }
}
