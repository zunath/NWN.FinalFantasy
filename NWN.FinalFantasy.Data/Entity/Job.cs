using NWN.FinalFantasy.Core.Enumerations;

namespace NWN.FinalFantasy.Data.Entity
{
    public class Job: EntityBase
    {
        public int Level { get; set; }
        public int XP { get; set; }

        public Job()
        {
            Level = 1;
        }
    }
}
