using System.Linq;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.QuestService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.SnippetDefinition
{
    public static class QuestSnippetDefinition
    {
        /// <summary>
        /// Snippet which checks whether a player has a quest.
        /// </summary>
        /// <param name="player">The player to check</param>
        /// <param name="args">Arguments provided by conversation builder.</param>
        /// <returns>true if player has a quest, false otherwise</returns>
        [Snippet("condition-has-quest")]
        public static bool ConditionHasQuest(uint player, string[] args)
        {
            if (args.Length <= 0)
            {
                const string Error = "'condition-has-quest' requires a questId argument.";
                SendMessageToPC(player, Error);
                Log.Write(LogGroup.Error, Error);
                return false;
            }

            var questId = args[0];
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            return dbPlayer.Quests.ContainsKey(questId);
        }

        /// <summary>
        /// Snippet which accepts a quest for a player.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <param name="args">Arguments provided by conversation builder.</param>
        [Snippet("action-accept-quest")]
        public static void AcceptQuest(uint player, string[] args)
        {
            if (args.Length <= 0)
            {
                const string Error = "'action-accept-quest' requires a questId argument.";
                SendMessageToPC(player, Error);
                Log.Write(LogGroup.Error, Error);
                return;
            }

            var questId = args[0];
            Quest.AcceptQuest(player, questId);
        }

        /// <summary>
        /// Snippet which advances a quest for a player.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <param name="args">Arguments provided by conversation builder.</param>
        [Snippet("action-advance-quest")]
        public static void AdvanceQuest(uint player, string[] args)
        {
            if (args.Length <= 0)
            {
                const string Error = "'action-advance-quest' requires a questId argument.";
                SendMessageToPC(player, Error);
                Log.Write(LogGroup.Error, Error);
                return;
            }

            var questId = args[0];
            Quest.AdvanceQuest(player, OBJECT_SELF, questId);
        }


        /// <summary>
        /// Spawns a container and forces the player to open it. They are then instructed to insert any quest items inside.
        /// </summary>
        /// <param name="player">The player we're requesting items from</param>
        /// <param name="args">Arguments provided by conversation builder.</param>
        [Snippet("action-request-quest-items")]
        public static void RequestItemsFromPlayer(uint player, string[] args)
        {
            if (!GetIsPC(player) || GetIsDM(player)) return;

            if (args.Length <= 0)
            {
                const string Error = "'action-request-quest-items' requires a questId argument.";
                SendMessageToPC(player, Error);
                Log.Write(LogGroup.Error, Error);
                return;
            }

            var questId = args[0];
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            if (!dbPlayer.Quests.ContainsKey(questId))
            {
                SendMessageToPC(player, "You have not accepted this quest yet.");
                return;
            }

            var quest = dbPlayer.Quests[questId];
            var questDetail = Quest.GetQuestById(questId);
            var questState = questDetail.States[quest.CurrentState];

            // Ensure there's at least one "Collect Item" objective on this quest state.
            var hasCollectItemObjective = questState.GetObjectives().OfType<CollectItemObjective>().Any();

            // The only time this should happen is if the quest is misconfigured.
            if (!hasCollectItemObjective)
            {
                SendMessageToPC(player, "There are no items to turn in for this quest. This is likely a bug. Please let the staff know.");
                return;
            }

            var collector = CreateObject(ObjectType.Placeable, "qst_item_collect", GetLocation(player));
            SetLocalObject(collector, "QUEST_OWNER", OBJECT_SELF);
            SetLocalString(collector, "QUEST_ID", questId);

            AssignCommand(collector, () => SetFacingPoint(GetPosition(player)));
            AssignCommand(player, () => ActionInteractObject(collector));
        }
    }
}
