using System;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Enumeration;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Object = NWN.FinalFantasy.Core.NWNX.Object;
using Player = NWN.FinalFantasy.Entity.Player;

namespace NWN.FinalFantasy.Service
{
    public static partial class Perk
    {
        /// <summary>
        /// Retrieves the effective perk level of a creature.
        /// On NPCs, this will retrieve the "PERK_LEVEL_{perkId}" variable, where {perkId} is replaced with the ID of the perk.
        /// On PCs, this will retrieve the perk level, taking into account any skill decay.
        /// </summary>
        /// <param name="creature">The creature whose perk level will be retrieved.</param>
        /// <param name="perkType">The type of perk to retrieve.</param>
        /// <returns>The effective perk level of a creature.</returns>
        public static int GetEffectivePerkLevel(uint creature, PerkType perkType)
        {
            if (GetIsDM(creature) && !GetIsDMPossessed(creature)) return 0;

            // Players only
            if (GetIsPC(creature) && !GetIsDMPossessed(creature))
            {
                return GetPlayerPerkLevel(creature, perkType);
            }
            // Creatures or DM-possessed creatures
            else
            {
                return GetLocalInt(creature, $"PERK_LEVEL_{(int) perkType}");
            }
        }

        /// <summary>
        /// Retrieves a player's effective perk level.
        /// This will take into account scenarios where a player's purchased perk level is higher than
        /// what their skill levels allow.
        /// </summary>
        /// <param name="player">The player whose perk level we're retrieving</param>
        /// <param name="perkType">The type of perk we're retrieving</param>
        /// <returns>The player's effective perk level.</returns>
        private static int GetPlayerPerkLevel(uint player, PerkType perkType)
        {
            if (!GetIsPC(player) || GetIsDM(player)) return 0;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            return GetPlayerPerkLevel(player, dbPlayer, perkType);
        }

        /// <summary>
        /// Retrieves a player's effective perk level.
        /// </summary>
        /// <param name="player">The player object</param>
        /// <param name="dbPlayer">The database entity</param>
        /// <param name="perkType">The type of perk</param>
        /// <returns>The effective level for a given player and perk</returns>
        private static int GetPlayerPerkLevel(uint player, Player dbPlayer, PerkType perkType)
        {
            var playerPerkLevel = dbPlayer.Perks.ContainsKey(perkType) ? dbPlayer.Perks[perkType] : 0;

            // Early exit if player doesn't have the perk at all.
            if (playerPerkLevel <= 0) return 0;

            // Retrieve perk levels at or below player's perk level and then order them from highest level to lowest.
            var perk = GetPerkDetails(perkType);
            var perkLevels = perk.PerkLevels
                .Where(x => x.Key <= playerPerkLevel)
                .OrderByDescending(o => o.Key);

            // Iterate over each perk level and check requirements.
            // The first perk level the player passes requirements on is the player's effective level.
            foreach (var (level, detail) in perkLevels)
            {
                // No requirements set for this perk level. Return the level.
                if (detail.EffectiveLevelRequirements.Count <= 0) return level;

                foreach (var req in detail.EffectiveLevelRequirements)
                {
                    if (string.IsNullOrWhiteSpace(req.CheckRequirements(player))) return level;
                }
            }

            // Otherwise none of the perk level requirements passed. Player's effective level is zero.
            return 0;
        }

        /// <summary>
        /// When a perk refund tome is used, start the perk refund conversation.
        /// </summary>
        [NWNEventHandler("item_use_bef")]
        public static void UsePerkRefundTome()
        {
            var player = OBJECT_SELF;
            if (!GetIsPC(player) || GetIsDM(player)) return;
            var item = Object.StringToObject(Events.GetEventData("ITEM_OBJECT_ID"));
            if (GetResRef(item) != "refund_tome") return;

            SetLocalObject(player, "PERK_REFUND_OBJECT", item);
            AssignCommand(player, () => ClearAllActions());
            Dialog.StartConversation(player, player, "PerkRefundDialog");

            // Don't display the "You cannot use this item" message. Skip the event.
            Events.SetEventResult("1"); 
            Events.SkipEvent();
        }
    }
}
