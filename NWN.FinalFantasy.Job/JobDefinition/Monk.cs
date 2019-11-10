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
            CallSign = "MNK";
            GF = GuardianForce.Titan;
            Class = ClassType.Monk;
            Package = Package.Monk;

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
                BaseItemType.Gloves
            });
        }
    }
}
