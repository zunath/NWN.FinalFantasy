using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts
{
    /// <summary>
    /// This is a workaround to prevent the server from crashing on log out.
    /// Sets the player's level back to what their current job level is.
    /// </summary>
    internal class SetLevelToNormal
    {
        public static void Main()
        {
            NWGameObject player = GetEnteringObject();
            if (!GetIsPlayer(player)) return;

            var playerID = GetGlobalID(player);
            var jobType = GetClassByPosition(ClassPosition.First, player);
            var job = JobRepo.Get(playerID, jobType);

            NWNXCreature.SetLevelByPosition(player, ClassPosition.First, job.Level);
        }
    }
}
