using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Event;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts
{
    internal class GainXP
    {
        public static void Main()
        {
            var data = Script.GetScriptData<JobXPGained>();
            var player = data.Creature;
            var playerID = GetGlobalID(player);
            var jobType = GetClassByPosition(ClassPosition.First, player);
            var levelBefore = GetLevelByPosition(ClassPosition.First, player);

            // Auto-level the NWN class.
            // These changes get adjusted as part of our auto-level process.
            LevelUpHenchman(player, jobType);
            var levelAfter = GetLevelByPosition(ClassPosition.First, player);
            var job = JobRepo.Get(playerID, jobType);
            job.Level = levelAfter;
            job.XP = GetXP(player);
            JobRepo.Set(playerID, jobType, job);

            if (levelBefore != levelAfter)
            {
                Publish.CustomEvent(player, JobEventPrefix.OnLeveledUp, new LeveledUp(player));
            }
        }
    }
}
