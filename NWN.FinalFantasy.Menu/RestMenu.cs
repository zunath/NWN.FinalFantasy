using NWN.FinalFantasy.Core.Dialog;

namespace NWN.FinalFantasy.Menu
{
    public class RestMenu: ConversationBase
    {
        public override void SetUp(NWGameObject player, PlayerDialog dialog)
        {
            var mainPage = new DialogPage(
                "Hello",
                "response 1",
                "resp 2",
                "resp 3",
                "resp 4",
                "resp 5",
                "resp 6",
                "resp 7",
                "resp 8",
                "resp 9",
                "resp 10",
                "resp 11",
                "resp 12",
                "resp 13",
                "resp 14",
                "resp 15");

            dialog.AddPage("MainPage", mainPage);
        }

        public override void Initialize()
        {
        }

        public override void DoAction(NWGameObject player, string pageName, int responseID)
        {
        }

        public override void Back(NWGameObject player, string beforeMovePage, string afterMovePage)
        {
        }

        public override void EndDialog()
        {
        }
    }
}
