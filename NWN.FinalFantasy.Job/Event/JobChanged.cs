using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Job.Event
{
    public class JobChanged
    {
        public NWGameObject Player { get; set; }
        public ClassType OldJob { get; set; }
        public ClassType NewJob { get; set; }

        public JobChanged(NWGameObject player, ClassType oldJob, ClassType newJob)
        {
            Player = player;
            OldJob = oldJob;
            NewJob = newJob;
        }
    }
}
