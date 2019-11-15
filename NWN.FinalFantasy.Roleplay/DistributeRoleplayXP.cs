using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Roleplay
{
    /// <summary>
    /// Attempts to distribute roleplay XP to all logged-in players.
    /// </summary>
    internal class DistributeRoleplayXP
    {
        public static void Main()
        {
            var module = GetModule();
            var ticks = GetLocalInt(module, "RP_SYSTEM_TICKS") + 1;

            // Is it time to process RP points?
            if (ticks >= 300) // 300 ticks * 6 seconds per HB = 1800 seconds = 30 minutes
            {
                var player = GetFirstPC();
                while (GetIsObjectValid(player))
                {
                    ProcessPlayerRoleplayXP(player);

                    player = GetNextPC();
                }

                ticks = 0;
            }

            SetLocalInt(module, "RP_SYSTEM_TICKS", ticks);
        }

        private static void ProcessPlayerRoleplayXP(NWGameObject player)
        {
            if (!GetIsPlayer(player)) return;

            var playerID = GetGlobalID(player);
            var progress = RoleplayProgressRepo.Get(playerID);
            if (progress.RPPoints >= 50)
            {
                const int BaseXP = 1500;
                int delta = progress.RPPoints - 50;
                int bonusXP = delta * 25;
                int xp = BaseXP + bonusXP;

                progress.RPPoints = 0;
                progress.TotalXPGained += (ulong)xp;
                RoleplayProgressRepo.Set(playerID, progress);
                GiveJobXP(player, xp);

                SendMessageToPC(player, $"You gained {xp} roleplay XP.");
            }
        }
    }
}
