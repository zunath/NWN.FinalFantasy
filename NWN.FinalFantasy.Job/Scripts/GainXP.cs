using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Event;
using NWN.FinalFantasy.Job.Registry;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts
{
    /// <summary>
    /// Handles saving job XP changes and performs level ups as necessary.
    /// </summary>
    public class GainXP: IScript
    {
        public void Main()
        {
            var data = Script.GetScriptData<JobXPGained>();
            var player = data.Creature;
            var playerID = GetGlobalID(player);
            var jobType = GetClassByPosition(ClassPosition.First, player);
            var job = JobRepo.Get(playerID, jobType);

            var xp = GetXP(player);
            var currentLevel = XPChart.GetLevelByXP(xp);
            var jobLevel = job.Level;

            NWNXCreature.SetLevelByPosition(player, ClassPosition.First, currentLevel);

            // Update the DB record
            job.Level = currentLevel;
            job.XP = xp;
            JobRepo.Set(playerID, jobType, job);

            if (currentLevel != jobLevel)
            {
                Publish.CustomEvent(player, JobEventPrefix.OnLeveledUp, new LeveledUp(player));
            }
        }
    }
}
