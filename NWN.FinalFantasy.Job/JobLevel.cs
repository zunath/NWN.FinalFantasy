using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Job
{
    internal class JobLevel
    {
        public ClassType Job { get; set; }
        public int Level { get; set; }

        public JobLevel(ClassType job, int level)
        {
            Job = job;
            Level = level;
        }
    }
}
