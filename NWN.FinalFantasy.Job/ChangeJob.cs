using System;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Job
{
    internal class ChangeJob
    {
        public static void Main(JobType newJob)
        {
            var player = NWGameObject.OBJECT_SELF;
            var playerID = GetGlobalID(player);
            var entity = PlayerRepo.Get(playerID);
            var oldJob = entity.CurrentJob;
            
            Console.WriteLine("playerID = " + playerID + ", oldJob = " + oldJob + ", newJob = " + newJob);

            MessageHub.Instance.Publish(new JobChanged(player, oldJob, newJob));
        }
    }
}
