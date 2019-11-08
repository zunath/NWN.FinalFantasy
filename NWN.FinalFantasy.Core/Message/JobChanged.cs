using NWN.FinalFantasy.Core.Enumerations;

namespace NWN.FinalFantasy.Core.Message
{
    public class JobChanged
    {
        public NWGameObject Player { get; set; }
        public JobType OldJob { get; set; }
        public JobType NewJob { get; set; }

        public JobChanged(NWGameObject player, JobType oldJob, JobType newJob)
        {
            Player = player;
            OldJob = oldJob;
            NewJob = newJob;
        }
    }
}
