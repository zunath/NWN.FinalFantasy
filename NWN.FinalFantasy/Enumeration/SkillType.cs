using System;
using NWN.FinalFantasy.Core.NWScript.Enum;

namespace NWN.FinalFantasy.Enumeration
{
    public enum SkillType
    {
        [Skill(SkillCategoryType.Invalid, "Unknown", 0, false, "Unused in-game.", false, Ability.Invalid, Ability.Invalid)]
        Unknown = 0,

        // Ability
        [Skill(SkillCategoryType.Ability, "Chivalry", 50, true, "Ability to use shield bash, cleave, and other knight-related actions.", true, Ability.Invalid, Ability.Invalid)]
        Chivalry = 1,
        [Skill(SkillCategoryType.Ability, "Chi", 50, true, "Ability to use self-buff and restoration techniques.", true, Ability.Invalid, Ability.Invalid)]
        Chi = 2,
        [Skill(SkillCategoryType.Ability, "Thievery", 50, true, "Ability to use stealth, steal, and other thievery-related actions.", true, Ability.Invalid, Ability.Invalid)]
        Thievery = 3,
        [Skill(SkillCategoryType.Ability, "Black Magic", 50, true, "Ability to use fire, blizzard, and other black magic-related actions.", true, Ability.Invalid, Ability.Invalid)]
        BlackMagic = 4,
        [Skill(SkillCategoryType.Ability, "White Magic", 50, true, "Ability to use cure, raise, and other white magic-related actions.", true, Ability.Invalid, Ability.Invalid)]
        WhiteMagic = 5,
        [Skill(SkillCategoryType.Ability, "Red Magic", 50, true, "Ability to use poison, convert, and other red magic-related actions.", true, Ability.Invalid, Ability.Invalid)]
        RedMagic = 6,
        [Skill(SkillCategoryType.Ability, "Archery", 50, true, "Ability to use barrage, aim, and other archery-related actions.", true, Ability.Invalid, Ability.Invalid)]
        Archery = 7,
        [Skill(SkillCategoryType.Ability, "Ninjitsu", 50, true, "Ability to use utsusemi, raiton, and other ninja-related actions.", true, Ability.Invalid, Ability.Invalid)]
        Ninjitsu = 8,
        [Skill(SkillCategoryType.Ability, "Swordplay", 50, true, "Ability to use renzokuken, royal guard, and other specialist-related actions.", true, Ability.Invalid, Ability.Invalid)]
        Swordplay = 9,
        [Skill(SkillCategoryType.Ability, "Marksmanship", 50, true, "Ability to use heat bullet, tranquilizer, and other sniper-related actions..", true, Ability.Invalid, Ability.Invalid)]
        Marksmanship = 10,
        [Skill(SkillCategoryType.Ability, "Darkness", 50, true, "Ability to use souleater, last resort, and other darkness-related actions..", true, Ability.Invalid, Ability.Invalid)]
        Darkness = 11,

        // Armor
        [Skill(SkillCategoryType.Armor, "Heavy Armor", 50, true, "Ability to use heavy armor.", true, Ability.Constitution, Ability.Strength)]
        HeavyArmor = 12,
        [Skill(SkillCategoryType.Armor, "Light Armor", 50, true, "Ability to use light armor.", true, Ability.Constitution, Ability.Dexterity)]
        LightArmor = 13,
        [Skill(SkillCategoryType.Armor, "Mystic Armor", 50, true, "Ability to use mystic armor.", true, Ability.Charisma, Ability.Constitution)]
        MysticArmor = 14,

        // Weapon
        [Skill(SkillCategoryType.Weapon, "Longsword", 50, true, "Ability to use longswords.", true, Ability.Strength, Ability.Constitution)]
        Longsword = 15,
        [Skill(SkillCategoryType.Weapon, "Knuckles", 50, true, "Ability to use fist knuckle weapons.", true, Ability.Strength, Ability.Dexterity)]
        Knuckles = 16,
        [Skill(SkillCategoryType.Weapon, "Dagger", 50, true, "Ability to use daggers.", true, Ability.Dexterity, Ability.Strength)]
        Dagger = 17,
        [Skill(SkillCategoryType.Weapon, "Staff", 50, true, "Ability to use staves.", true, Ability.Intelligence, Ability.Charisma)]
        Staff = 18,
        [Skill(SkillCategoryType.Weapon, "Rod", 50, true, "Ability to use rods.", true, Ability.Wisdom, Ability.Charisma)]
        Rod = 19,
        [Skill(SkillCategoryType.Weapon, "Longbow", 50, true, "Ability to use longbows.", true, Ability.Dexterity, Ability.Wisdom)]
        Longbow = 20,
        [Skill(SkillCategoryType.Weapon, "Katana", 50, true, "Ability to use katanas.", true, Ability.Strength, Ability.Dexterity)]
        Katana = 21,
        [Skill(SkillCategoryType.Weapon, "Gunblade", 50, true, "Ability to use gunblades.", true, Ability.Strength, Ability.Constitution)]
        Gunblade = 22,
        [Skill(SkillCategoryType.Weapon, "Rifle", 50, true, "Ability to use rifles.", true, Ability.Dexterity, Ability.Constitution)]
        Rifle = 23,
        [Skill(SkillCategoryType.Weapon, "Rapier", 50, true, "Ability to use rapiers.", true, Ability.Dexterity, Ability.Intelligence)]
        Rapier = 24,
        [Skill(SkillCategoryType.Weapon, "Great Sword", 50, true, "Ability to use great swords.", true, Ability.Strength, Ability.Intelligence)]
        GreatSword = 25

    }

    public class SkillAttribute : Attribute
    {
        public SkillCategoryType Category { get; set; }
        public string Name { get; set; }
        public int MaxRank { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public bool ContributesToSkillCap { get; set; }
        public Ability PrimaryStat { get; set; }
        public Ability SecondaryStat { get; set; }

        public SkillAttribute(
            SkillCategoryType category, 
            string name, 
            int maxRank, 
            bool isActive, 
            string description, 
            bool contributesToSkillCap, 
            Ability primaryStat, 
            Ability secondaryStat)
        {
            Category = category;
            Name = name;
            MaxRank = maxRank;
            IsActive = isActive;
            Description = description;
            ContributesToSkillCap = contributesToSkillCap;
            PrimaryStat = primaryStat;
            SecondaryStat = secondaryStat;
        }
    }
}
