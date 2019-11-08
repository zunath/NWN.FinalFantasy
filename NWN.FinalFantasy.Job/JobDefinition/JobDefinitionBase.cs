using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class JobDefinitionBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public GuardianForce GF { get; set; }

        public ProficiencyRating HPRating { get; set; }
        public ProficiencyRating MPRating { get; set; }
        public ProficiencyRating STRRating { get; set; }
        public ProficiencyRating DEXRating { get; set; }
        public ProficiencyRating CONRating { get; set; }
        public ProficiencyRating INTRating { get; set; }
        public ProficiencyRating WISRating { get; set; }
        public ProficiencyRating CHARating { get; set; }

        public List<BaseItemType> WeaponTypes { get; } = new List<BaseItemType>();
        private Dictionary<int, List<AbilityType>> AbilitiesByLevel { get; } = new Dictionary<int, List<AbilityType>>();

        public void AddAbility(int level, AbilityType type)
        {
            if (!AbilitiesByLevel.ContainsKey(level))
            {
                AbilitiesByLevel[level] = new List<AbilityType>();
            }

            var abilities = AbilitiesByLevel[level];
            abilities.Add(type);
        }

    }
}
