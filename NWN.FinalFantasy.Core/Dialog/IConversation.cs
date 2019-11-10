namespace NWN.FinalFantasy.Core.Dialog
{
    public interface IConversation
    {
        void SetUp(NWGameObject player, PlayerDialog dialog);
        void Initialize();
        void DoAction(NWGameObject player, string pageName, int responseID);
        void Back(NWGameObject player, string beforeMovePage, string afterMovePage);
        void EndDialog();
    }
}
