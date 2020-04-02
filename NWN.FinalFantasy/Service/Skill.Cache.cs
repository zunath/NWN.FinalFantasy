using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Extension;

namespace NWN.FinalFantasy.Service
{
    public static partial class Skill
    {
        private static readonly HashSet<SkillCategoryType> _categoriesWithSkillContributing = new HashSet<SkillCategoryType>();
        private static readonly Dictionary<SkillCategoryType, SkillCategoryAttribute> _categories = new Dictionary<SkillCategoryType, SkillCategoryAttribute>();
        private static readonly Dictionary<SkillCategoryType, List<SkillType>> _skillsByCategory = new Dictionary<SkillCategoryType, List<SkillType>>();
        private static readonly Dictionary<SkillType, SkillAttribute> _skills = new Dictionary<SkillType, SkillAttribute>();
        private static readonly Dictionary<SkillType, SkillAttribute> _skillsContributingToCap = new Dictionary<SkillType, SkillAttribute>();

        [NWNEventHandler("mod_load")]
        public static void CacheData()
        {
            Console.WriteLine("Caching skill data.");
            // Initialize the list of categories.
            var categories = Enum.GetValues(typeof(SkillCategoryType)).Cast<SkillCategoryType>();
            foreach (var category in categories)
            {
                _categories[category] = category.GetAttribute<SkillCategoryType, SkillCategoryAttribute>();
                _skillsByCategory[category] = new List<SkillType>();
            }

            // Organize skills to make later reads quicker.
            var skills = Enum.GetValues(typeof(SkillType)).Cast<SkillType>();
            foreach (var skill in skills)
            {
                var attr = skill.GetAttribute<SkillType, SkillAttribute>();
                _skills[skill] = attr;

                if (attr.ContributesToSkillCap)
                {
                    _skillsContributingToCap[skill] = attr;

                    if (!_categoriesWithSkillContributing.Contains(attr.Category))
                        _categoriesWithSkillContributing.Add(attr.Category);
                }

                _skillsByCategory[attr.Category].Add(skill);
            }
        }

        /// <summary>
        /// Retrieves a list of available skills.
        /// </summary>
        /// <returns>A list of available skills.</returns>
        public static Dictionary<SkillType, SkillAttribute> GetSkills()
        {
            return _skills.ToDictionary(x => x.Key, y => y.Value);
        }

        /// <summary>
        /// Retrieves details about a specific skill.
        /// </summary>
        /// <param name="skillType">The skill whose details we will retrieve.</param>
        /// <returns>An object containing details about a skill.</returns>
        public static SkillAttribute GetSkillDetails(SkillType skillType)
        {
            return _skills[skillType];
        }
    }
}
