using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class Thief: JobDefinitionBase
    {
        public Thief()
        {
            Name = "Thief";
            Description = "Combat specialist which excels at stealth and thievery.";
            CallSign = "THF";
            GF = GuardianForce.Fenrir;
            Class = ClassType.Thief;

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
                BaseItemType.Dagger, 

                BaseItemType.SmallShield
            });
        }
    }
}
