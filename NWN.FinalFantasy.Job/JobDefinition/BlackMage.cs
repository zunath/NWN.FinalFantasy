using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class BlackMage: JobDefinitionBase
    {
        public BlackMage()
        {
            Name = "Black Mage";
            Description = "Magic user which specializes in destructive black magic.";
            CallSign = "BLM";
            GF = GuardianForce.Ramuh;
            Class = ClassType.BlackMage;

            HPRating = ProficiencyRating.B;
            MPRating = ProficiencyRating.E;
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
                BaseItemType.Lightmace
            });
        }
    }
}
