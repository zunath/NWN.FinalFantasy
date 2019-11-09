using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class Warrior: JobDefinitionBase
    {
        public Warrior()
        {
            Name = "Warrior";
            Description = "Melee fighter which specializes in a variety of weapons and armor.";
            CallSign = "WAR";
            GF = GuardianForce.Ifrit;
            Class = ClassType.Warrior;

            HPRating = ProficiencyRating.B;
            MPRating = ProficiencyRating.E;
            ACRating = ProficiencyRating.E;
            BABRating = ProficiencyRating.E;

            STRRating = ProficiencyRating.A;
            DEXRating = ProficiencyRating.B;
            CONRating = ProficiencyRating.A;
            INTRating = ProficiencyRating.E;
            WISRating = ProficiencyRating.E;
            CHARating = ProficiencyRating.C;

            WeaponTypes.AddRange(new []
            {
                BaseItemType.Longsword,
                BaseItemType.GreatSword,
                BaseItemType.GreatAxe,
                BaseItemType.Battleaxe,

                BaseItemType.SmallShield,
                BaseItemType.LargeShield,
                BaseItemType.TowerShield
            });

            AddAbility(1, AbilityType.MightyStrikes);
            AddAbility(3, AbilityType.DefenseBonus1);
        }

    }
}
