using System;

namespace NWN.FinalFantasy.Enumeration
{
    public enum SkillType
    {
        [Skill(SkillCategoryType.Invalid, "Unknown", 0, false, "Unused in-game.", false)]
        Unknown = 0,
        [Skill(SkillCategoryType.Armor, "Heavy Armor", 100, true, "Ability to use heavy armor.", true)]
        HeavyArmor = 1,
        [Skill(SkillCategoryType.Armor, "Light Armor", 100, true, "Ability to use light armor.", true)]
        LightArmor = 2,
        [Skill(SkillCategoryType.Armor, "Mystic Armor", 100, true, "Ability to use mystic armor.", true)]
        MysticArmor = 3,

    }

    public class SkillAttribute : Attribute
    {
        public SkillCategoryType Category { get; set; }
        public string Name { get; set; }
        public int MaxRank { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public bool ContributesToSkillCap { get; set; }

        public SkillAttribute(SkillCategoryType category, string name, int maxRank, bool isActive, string description, bool contributesToSkillCap)
        {
            Category = category;
            Name = name;
            MaxRank = maxRank;
            IsActive = isActive;
            Description = description;
            ContributesToSkillCap = contributesToSkillCap;
        }
    }
}
