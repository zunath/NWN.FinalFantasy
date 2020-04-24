using System;

namespace NWN.FinalFantasy.Enumeration
{
    public enum AchievementType
    {
        [Achievement("Invalid", "Invalid", false)]
        Invalid = 0

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
