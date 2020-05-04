using System;

namespace NWN.FinalFantasy.Service.AbilityService
{
    // Note: Short names are what's displayed on the recast Gui element. They are limited to 14 characters.
    public enum RecastGroup
    {
        [RecastGroup("Invalid", "Invalid")]
        Invalid = 0,
        [RecastGroup("One-Hour Ability", "1-Hr Ability")] 
        OneHourAbility = 1,
        [RecastGroup("Fire", "Fire")]
        Fire = 2,
        [RecastGroup("Ice", "Ice")]
        Ice = 3,
        [RecastGroup("Thunder", "Thunder")]
        Thunder = 4
    }

    public class RecastGroupAttribute: Attribute
    {
        public string Name { get; set; }
        public string ShortName { get; set; }

        public RecastGroupAttribute(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }
    }
}
