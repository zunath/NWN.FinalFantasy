using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class Carpenter : JobDefinitionBase
    {
        public Carpenter()
        {
            Name = "Carpenter";
            Description = "Crafter which specializes in creating furniture and other wood-based products.";
            CallSign = "CPN";
            GF = GuardianForce.None;
            Class = ClassType.Carpenter;
            Package = Package.Carpenter;

            HPRating = ProficiencyRating.D;
            MPRating = ProficiencyRating.Zero;
            ACRating = ProficiencyRating.E;
            BABRating = ProficiencyRating.E;

            STRRating = ProficiencyRating.D;
            DEXRating = ProficiencyRating.E;
            CONRating = ProficiencyRating.C;
            INTRating = ProficiencyRating.E;
            WISRating = ProficiencyRating.E;
            CHARating = ProficiencyRating.E;
        }
    }
}