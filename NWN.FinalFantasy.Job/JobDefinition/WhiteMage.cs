using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class WhiteMage: JobDefinitionBase
    {
        public WhiteMage()
        {
            Name = "White Mage";
            Description = "Magic user which specializes in restorative white magic.";
            CallSign = "WHM";
            GF = GuardianForce.Carbuncle;
            Class = ClassType.WhiteMage;

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
                BaseItemType.Quarterstaff, 
                BaseItemType.LightFlail, 
                BaseItemType.Lightmace,

                BaseItemType.SmallShield
            });
        }
    }
}
