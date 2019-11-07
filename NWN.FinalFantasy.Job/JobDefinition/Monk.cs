using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class Monk: JobDefinitionBase
    {
        public Monk()
        {
            Name = "Monk";
            Description = "Melee fighter which specializes in fighting unarmed.";
            GF = GuardianForce.Titan;

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
                BaseItemType.Gloves
            });
        }
    }
}
