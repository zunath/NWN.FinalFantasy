using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal abstract class JobDefinitionBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CallSign { get; set; }
        public GuardianForce GF { get; set; }
        public ClassType Class { get; set; }
        public Package Package { get; set; }

        public ProficiencyRating HPRating { get; set; }
        public ProficiencyRating MPRating { get; set; }
        public ProficiencyRating ACRating { get; set; }
        public ProficiencyRating BABRating { get; set; }
        public ProficiencyRating STRRating { get; set; }
        public ProficiencyRating DEXRating { get; set; }
        public ProficiencyRating CONRating { get; set; }
        public ProficiencyRating INTRating { get; set; }
        public ProficiencyRating WISRating { get; set; }
        public ProficiencyRating CHARating { get; set; }

        public List<BaseItemType> WeaponTypes { get; } = new List<BaseItemType>();
    }
}
