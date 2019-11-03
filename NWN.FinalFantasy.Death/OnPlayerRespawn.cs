using System;
using NWN.FinalFantasy.Core.Logging;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using static NWN._;

namespace NWN.FinalFantasy.Death
{
    public class OnPlayerRespawn
    {
        /// <summary>
        /// Restores a respawning player's health to half of maximum,
        /// applies penalties for death, and teleports him or her to
        /// their Home Point.
        /// </summary>
        public static void Main()
        {
            NWGameObject player = GetLastRespawnButtonPresser();
            int maxHP = GetMaxHitPoints(player);

            int amount = maxHP / 2;
            ApplyEffectToObject(DurationType.Instant, EffectResurrection(), player);
            ApplyEffectToObject(DurationType.Instant, EffectHeal(amount), player);

            SendToHomePoint(player);
            var xpLost = ApplyPenalties(player);

            WriteAudit(player, xpLost);
        }

        /// <summary>
        /// Teleports player to his or her last home point.
        /// </summary>
        /// <param name="player">The player to teleport</param>
        private static void SendToHomePoint(NWGameObject player)
        {
            var playerID = GetGlobalID(player);
            var entity = DB.Get<Player>(playerID);
            var area = GetAreaByResRef(entity.RespawnAreaResref);
            var position = Vector(
                entity.RespawnLocationX,
                entity.RespawnLocationY,
                entity.RespawnLocationZ);

            var location = Location(area, position, entity.RespawnLocationOrientation);

            AssignCommand(player, () => ActionJumpToLocation(location));
        }

        /// <summary>
        /// Applies death penalties for a player. If player is at or below level 3 they receive no penalties.
        /// </summary>
        /// <param name="player">The player who we're applying penalties to</param>
        private static int ApplyPenalties(NWGameObject player)
        {
            int totalXP = GetXP(player);
            int level = GetTotalLevel(player);
            if (level <= 3) return 0;

            int penalty = Convert.ToInt32(totalXP * 0.10f);
            totalXP -= penalty;

            SetXP(player, totalXP);

            SendMessageToPC(player, $"{penalty} XP lost");

            return penalty;
        }

        /// <summary>
        /// Writes an audit entry to the Death audit group.
        /// </summary>
        /// <param name="player">The player who respawned</param>
        /// <param name="xpLost">The amount of XP lost</param>
        private static void WriteAudit(NWGameObject player, int xpLost)
        {
            var name = GetName(player);
            var log = $"RESPAWN - {name} - {xpLost} XP lost";

            Audit.Write(AuditGroup.Death, log);
        }
    }
}
