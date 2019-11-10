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
        private Dictionary<int, List<Feat>> AbilitiesByLevel { get; } = new Dictionary<int, List<Feat>>();

        /// <summary>
        /// Adds an ability for a given level to this job definition.
        /// </summary>
        /// <param name="level">The level to add the ability to.</param>
        /// <param name="type">The type of ability to add for this level.</param>
        public void AddAbility(int level, Feat type)
        {
            if (!AbilitiesByLevel.ContainsKey(level))
            {
                AbilitiesByLevel[level] = new List<Feat>();
            }

            var abilities = AbilitiesByLevel[level];
            abilities.Add(type);
        }

        /// <summary>
        /// Retrieves the list of abilities acquired for this level.
        /// Returns an empty list if no abilities are gained at the specified level.
        /// </summary>
        /// <param name="level">The level to retrieve</param>
        /// <returns>A list of abilities</returns>
        public List<Feat> GetAbilityListByLevel(int level)
        {
            if(!AbilitiesByLevel.ContainsKey(level))
                return new List<Feat>();

            return AbilitiesByLevel[level].ToList();
        }

    }
}
