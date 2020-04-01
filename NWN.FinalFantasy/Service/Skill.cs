using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Enumeration;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Player = NWN.FinalFantasy.Entity.Player;

namespace NWN.FinalFantasy.Service
{
    public static partial class Skill
    {
        /// <summary>
        /// This is the maximum number of skill points a single character can have at any time.
        /// </summary>
        public const int SkillCap = 500;

        /// <summary>
        /// Gives XP towards a specific skill to a player.
        /// </summary>
        /// <param name="player">The player to give XP to.</param>
        /// <param name="skill">The type of skill to give XP towards.</param>
        /// <param name="xp">The amount of XP to give.</param>
        public static void GiveSkillXP(uint player, SkillType skill, int xp)
        {
            if (skill == SkillType.Unknown || xp <= 0 || !GetIsPC(player) || GetIsDM(player)) return;

            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            var details = GetSkillDetails(skill);
            var pcSkill = dbPlayer.Skills[skill];
            var requiredXP = GetRequiredXP(pcSkill.Rank);
            var receivedRankUp = false;

            // todo: skill decay

            pcSkill.XP += xp;
            SendMessageToPC(player, $"You earned {details.Name} skill experience. ({xp})");

            // Skill is at cap and player would level up.
            // Reduce XP to required amount minus 1 XP.
            if (pcSkill.Rank >= details.MaxRank && pcSkill.XP > requiredXP)
            {
                pcSkill.XP = requiredXP - 1;
            }

            while (pcSkill.XP >= requiredXP)
            {
                receivedRankUp = true;
                pcSkill.XP -= requiredXP;

                if (dbPlayer.TotalSPAcquired < SkillCap && details.ContributesToSkillCap)
                {
                    dbPlayer.UnallocatedSP++;
                    dbPlayer.TotalSPAcquired++;
                }

                pcSkill.Rank++;
                FloatingTextStringOnCreature($"Your {details.Name} skill level increased to rank {pcSkill.Rank}!", player, false);
                requiredXP = GetRequiredXP(pcSkill.Rank);

                if (pcSkill.Rank >= details.MaxRank && pcSkill.XP > requiredXP)
                {
                    pcSkill.XP = requiredXP - 1;
                }

                dbPlayer.Skills[skill] = pcSkill;
            }

            DB.Set(playerId, dbPlayer);

            // Send out an event signifying that a player has received a skill rank increase.
            if(receivedRankUp)
            {
                ExecuteScript("skill_rank_up", player);
            }
        }
    }
}
