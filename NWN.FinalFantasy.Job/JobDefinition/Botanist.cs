using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class Botanist : JobDefinitionBase
    {
        public Botanist()
        {
            Name = "Botanist";
            Description = "Gatherer which specializes in harvesting herbs and wood.";
            CallSign = "BOT";
            GF = GuardianForce.None;
            Class = ClassType.Botanist;
            Package = Package.Botanist;

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