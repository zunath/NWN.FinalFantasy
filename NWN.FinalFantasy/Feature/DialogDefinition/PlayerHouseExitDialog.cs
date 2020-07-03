using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class PlayerHouseExitDialog: DialogBase
    {
        private const string MainPageId = "MAIN_PAGE";

        public override PlayerDialog SetUp(uint player)
        {
            var builder = new DialogBuilder()
                .AddPage(MainPageId, MainPageInit);


            return builder.Build();
        }

        private void MainPageInit(DialogPage page)
        {
            var player = GetPC();
            page.Header = $"What would you like to do?";
            page.AddResponse("Exit", () =>
            {
                var area = GetArea(OBJECT_SELF);
                Housing.JumpToOriginalLocation(player);

                DelayCommand(6.0f, () => Housing.AttemptCleanUpInstance(area));
            });
        }
    }
}
