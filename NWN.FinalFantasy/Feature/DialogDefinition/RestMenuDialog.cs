using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Dialog = NWN.FinalFantasy.Service.Dialog;
using Player = NWN.FinalFantasy.Entity.Player;
using Skill = NWN.FinalFantasy.Service.Skill;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class RestMenuDialog : DialogBase
    {
        /// <summary>
        /// When a player uses the "Open Rest Menu" feat, open the rest menu dialog conversation.
        /// </summary>
        [NWNEventHandler("feat_use_bef")]
        public static void UseOpenRestMenuFeat()
        {
            var feat = (Feat)Convert.ToInt32(Events.GetEventData("FEAT_ID"));
            if (feat != Feat.OpenRestMenu) return;

            Dialog.StartConversation(OBJECT_SELF, OBJECT_SELF, "RestMenuDialog");
        }

        private const string MainPageId = "MAIN_PAGE";

        public override PlayerDialog SetUp(uint player)
        {
            var dialog = DialogBuilder.Create()
                .AddPage(MainPageId, MainPageInit)
                .Build();

            return dialog;
        }

        /// <summary>
        /// Builds the Main Page.
        /// </summary>
        /// <param name="page">The page to build.</param>
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
            page.AddResponse("View Achievements", () => SwitchConversation("ViewAchievementsDialog"));
            page.AddResponse("View Recipes", () => SwitchConversation("RecipeDialog"));
            page.AddResponse("Modify Item Appearance", () => SwitchConversation("ModifyItemAppearanceDialog"));
            page.AddResponse("Player Settings", () => SwitchConversation("PlayerSettingsDialog"));
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
