using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.JobDefinition
{
    internal class Blacksmith : JobDefinitionBase
    {
        public Blacksmith()
        {
            Name = "Blacksmith";
            Description = "Crafter which specializes in creating weapons and armor.";
            CallSign = "BSM";
            GF = GuardianForce.None;
            Class = ClassType.Blacksmith;
            Package = Package.Blacksmith;

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