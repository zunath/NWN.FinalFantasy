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
        [Skill(SkillCategoryType.Melee, "Longsword", 100, true, "Ability to use longswords.", true)]
        Longsword = 4,
        [Skill(SkillCategoryType.Ability, "Chivalry", 100, true, "Ability to use shield bash, cleave, and other knight-related actions.", true)]
        Chivalry = 5,
        [Skill(SkillCategoryType.Melee, "Knuckles", 100, true, "Ability to use fist knuckle weapons.", true)]
        Knuckles = 6,
        [Skill(SkillCategoryType.Ability, "Chi", 100, true, "Ability to use self-buff and restoration techniques.", true)]
        Chi = 7,
        [Skill(SkillCategoryType.Melee, "Dagger", 100, true, "Ability to use daggers.", true)]
        Dagger = 8,
        [Skill(SkillCategoryType.Ability, "Thievery", 100, true, "Ability to use stealth, steal, and other thievery-related actions.", true)]
        Thievery = 9,
        [Skill(SkillCategoryType.Melee, "Staff", 100, true, "Ability to use staves.", true)]
        Staff = 10,
        [Skill(SkillCategoryType.Ability, "Black Magic", 100, true, "Ability to use fire, blizzard, and other black magic-related actions.", true)]
        BlackMagic = 11,
        [Skill(SkillCategoryType.Melee, "Rod", 100, true, "Ability to use rods.", true)]
        Rod = 12,
        [Skill(SkillCategoryType.Ability, "White Magic", 100, true, "Ability to use cure, raise, and other white magic-related actions.", true)]
        WhiteMagic = 13,
        [Skill(SkillCategoryType.Ranged, "Longbow", 100, true, "Ability to use longbows.", true)]
        Longbow = 14,
        [Skill(SkillCategoryType.Ability, "Archery", 100, true, "Ability to use barrage, aim, and other archery-related actions.", true)]
        Archery = 15

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
