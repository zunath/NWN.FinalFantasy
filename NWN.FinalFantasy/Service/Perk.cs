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
                foreach (var req in detail.EffectiveLevelRequirements)
                {
                    if (string.IsNullOrWhiteSpace(req.CheckRequirements(player))) return level;
                }
            }

            // Otherwise none of the perk level requirements passed. Player's effective level is zero.
            return 0;
        }

        /// <summary>
        /// When an item is equipped, if any of a player's perks has an Equipped Trigger,
        /// run those actions now.
        /// </summary>
        [NWNEventHandler("item_eqp_bef")]
        public static void ApplyEquipTriggers()
        {
            var player = OBJECT_SELF;
            if (!GetIsPC(player) || GetIsDM(player)) return;

            RunEquipUnequipTriggers(player, true);
        }

        /// <summary>
        /// When an item is unequipped, if any of a player's perks has an Unequipped Trigger,
        /// run those actions now.
        /// </summary>
        [NWNEventHandler("item_uneqp_bef")]
        public static void ApplyUnequipTriggers()
        {
            var player = OBJECT_SELF;
            if (!GetIsPC(player) || GetIsDM(player)) return;

            RunEquipUnequipTriggers(player, false);
        }

        /// <summary>
        /// Executes the equip and unequip triggers for all perks, if player has at least one effective level in them.
        /// </summary>
        /// <param name="player">The player object</param>
        /// <param name="isEquip">If true, use the Equipped trigger list, otherwise use the Unequipped trigger list.</param>
        private static void RunEquipUnequipTriggers(uint player, bool isEquip)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            var item = Object.StringToObject(Events.GetEventData("ITEM"));
            var set = isEquip ? _equipTriggers : _unequipTriggers;

            foreach (var (perkType, actionList) in set)
            {
                var playerPerkLevel = GetPlayerPerkLevel(player, dbPlayer, perkType);
                if (playerPerkLevel <= 0) continue;

                foreach (var action in actionList)
                {
                    action(player, item, perkType, playerPerkLevel);
                }
            }
        }
    }
}
