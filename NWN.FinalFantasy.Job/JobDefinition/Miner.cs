using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class Miner : JobDefinitionBase
    {
        public Miner()
        {
            Name = "Miner";
            Description = "Gatherer which specializes in harvesting ores and gemstones.";
            CallSign = "MIN";
            GF = GuardianForce.None;
            Class = ClassType.Miner;
            Package = Package.BlackMage;

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