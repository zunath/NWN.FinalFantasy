using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class RestMenuDialog: DialogBase
    {
        public override PlayerDialog SetUp(uint player)
        {
            PlayerDialog dialog = new PlayerDialog("MainPage");
            DialogPage mainPage = new DialogPage(
                BuildMainPageHeader(player),
                "View Skills",
                "View Perks",
                "View Blueprints",
                "View Key Items",
                "Modify Item Appearance",
                "Open Trash Can (Destroy Items)");

            dialog.AddPage("MainPage", mainPage);

            return dialog;
        }

        public override void Initialize()
        {
        }

        public override void DoAction(uint player, string pageName, int responseID)
        {
        }

        public override void Back(uint player, string beforeMovePage, string afterMovePage)
        {
        }

        public override void EndDialog()
        {
        }


        private string BuildMainPageHeader(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            // Get all player skills and then sum them up by the rank.
            var totalSkillCount = dbPlayer.TotalSPAcquired;

            var playerName = GetName(player);
            string header = ColorToken.Green("Name: ") + playerName + "\n";
            header += ColorToken.Green("Skill Points: ") + totalSkillCount + " / " + Skill.SkillCap + "\n";
            header += ColorToken.Green("Unallocated SP: ") + dbPlayer.UnallocatedSP + "\n";
            header += ColorToken.Green("Unallocated XP: ") + dbPlayer.UnallocatedXP + "\n";

            return header;
        }
    }
}
