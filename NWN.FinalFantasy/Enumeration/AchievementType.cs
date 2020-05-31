using System;

namespace NWN.FinalFantasy.Enumeration
{
    public enum AchievementType
    {
        [Achievement("Invalid", "Invalid", false)]
        Invalid = 0,
        [Achievement("Kill Enemies I", "Kill 10 enemies.", true)]
        KillEnemies1 = 1,
        [Achievement("Kill Enemies II", "Kill 50 enemies.", true)]
        KillEnemies2 = 2,
        [Achievement("Kill Enemies III", "Kill 500 enemies.", true)]
        KillEnemies3 = 3,
        [Achievement("Kill Enemies IV", "Kill 2,000 enemies.", true)]
        KillEnemies4 = 4,
        [Achievement("Kill Enemies V", "Kill 10,000 enemies.", true)]
        KillEnemies5 = 5,
        [Achievement("Kill Enemies VI", "Kill 100,000 enemies.", true)]
        KillEnemies6 = 6,
        [Achievement("Learn Perks I", "Learn 1 Perk", true)]
        LearnPerks1 = 7,
        [Achievement("Learn Perks II", "Learn 20 Perks", true)]
        LearnPerks2 = 8,
        [Achievement("Learn Perks III", "Learn 50 Perks", true)]
        LearnPerks3 = 9,
        [Achievement("Learn Perks IV", "Learn 100 Perks", true)]
        LearnPerks4 = 10,
        [Achievement("Learn Perks V", "Learn 500 Perks", true)]
        LearnPerks5 = 11,
        [Achievement("Gain Skill Points I", "Gain 1 Skill Point", true)]
        GainSkills1 = 12,
        [Achievement("Gain Skill Points II", "Gain 50 Skill Points", true)]
        GainSkills2 = 13,
        [Achievement("Gain Skill Points III", "Gain 150 Skill Points", true)]
        GainSkills3 = 14,
        [Achievement("Gain Skill Points IV", "Gain 250 Skill Points", true)]
        GainSkills4 = 15,
        [Achievement("Gain Skill Points V", "Gain 500 Skill Points", true)]
        GainSkills5 = 16,
        [Achievement("Gain Skill Points VI", "Gain 1000 Skill Points", true)]
        GainSkills6 = 17,
    }

    public class AchievementAttribute: Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public AchievementAttribute(string name, string description, bool isActive)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
        }
    }
}
