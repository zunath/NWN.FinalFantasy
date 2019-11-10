using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class Ranger: JobDefinitionBase
    {
        public Ranger()
        {
            Name = "Ranger";
            Description = "Combatant which specializes in attacking from afar with ranged attacks.";
            CallSign = "RNG";
            GF = GuardianForce.Valefor;
            Class = ClassType.Ranger;
            Package = Package.Ranger;

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
                BaseItemType.ShortBow,
                BaseItemType.Longbow,
                BaseItemType.LightCrossbow,
                BaseItemType.HeavyCrossbow,
                BaseItemType.Shortsword,
                BaseItemType.HandAxe,
                BaseItemType.ThrowingAxe
            });
        }
    }
}
