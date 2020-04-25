using System;

namespace NWN.FinalFantasy.Enumeration
{
    public enum PerkCategoryType
    {
        [PerkCategory("Invalid", false)]
        Invalid = 0,
        [PerkCategory("General", true)]
        General = 1,
        [PerkCategory("Knight", true)]
        Knight = 2,
        [PerkCategory("Monk", true)]
        Monk = 3,
        [PerkCategory("Thief", true)]
        Thief = 4,
        [PerkCategory("Black Mage", true)]
        BlackMage = 5,
        [PerkCategory("White Mage", true)]
        WhiteMage = 6,
        [PerkCategory("Red Mage", true)]
        RedMage = 7,
        [PerkCategory("Ranger", true)]
        Ranger = 8,
        [PerkCategory("Ninja", true)]
        Ninja = 9,
        [PerkCategory("Specialist", true)]
        Specialist = 10,
        [PerkCategory("Sniper", true)]
        Sniper = 11,
        [PerkCategory("Dark Knight", true)]
        DarkKnight = 12
    }

    public class PerkCategoryAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public PerkCategoryAttribute(string name, bool isActive)
        {
            Name = name;
            IsActive = isActive;
        }
    }
}
