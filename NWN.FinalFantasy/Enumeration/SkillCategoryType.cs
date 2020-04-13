using System;

namespace NWN.FinalFantasy.Enumeration
{
    public enum SkillCategoryType
    {
        [SkillCategory("Invalid", false, 0)]
        Invalid = 0,
        [SkillCategory("Melee", true, 1)]
        Melee = 1,
        [SkillCategory("Ranged", true, 2)]
        Ranged = 2,
        [SkillCategory("Armor", true, 3)]
        Armor = 3,
        [SkillCategory("Crafting", true, 4)]
        Crafting = 4,
        [SkillCategory("Ability", true, 5)]
        Ability = 5
    }

    public class SkillCategoryAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Sequence { get; set; }

        public SkillCategoryAttribute(string name, bool isActive, int sequence)
        {
            Name = name;
            IsActive = isActive;
            Sequence = sequence;
        }
    }
}