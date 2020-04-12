using System;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Skill = NWN.FinalFantasy.Service.Skill;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class RestMenuDialog : DialogBase
    {
        public override PlayerDialog SetUp(uint player)
        {
            var dialog = DialogBuilder.Create()
                .AddPage(MainPageInit)
                .Build();

            return dialog;
        }

        private void MainPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            // Get all player skills and then sum them up by the rank.
            var totalSkillCount = dbPlayer.TotalSPAcquired;

            var playerName = GetName(player);
            string header = ColorToken.Green("Name: ") + playerName + "\n";
            header += ColorToken.Green("Skill Points: ") + totalSkillCount + " / " + Skill.SkillCap + "\n";
            header += ColorToken.Green("Unallocated SP: ") + dbPlayer.UnallocatedSP + "\n";
            header += ColorToken.Green("Unallocated XP: ") + dbPlayer.UnallocatedXP + "\n";

            page.Header = header;

            page.AddResponse("View Skills", () => SwitchConversation("ViewSkillsDialog"));
            page.AddResponse("View Perks", () => SwitchConversation("ViewPerksDialog"));
            page.AddResponse("View Blueprints", () => SwitchConversation("ViewBlueprintsDialog"));
            page.AddResponse("View Key Items", () => SwitchConversation("KeyItemsDialog"));
            page.AddResponse("Modify Item Appearance", () => SwitchConversation("ModifyItemAppearanceDialog"));
            page.AddResponse("Open Trash Can (Destroy Items)", () =>
            {
                EndConversation();
                var location = GetLocation(player);
                var trashCan = CreateObject(ObjectType.Placeable, "trash_can", location);

                AssignCommand(player, () => ActionInteractObject(trashCan));
                DelayCommand(0.2f, () => SetUseableFlag(trashCan, false));
            });
        }
    }
}
