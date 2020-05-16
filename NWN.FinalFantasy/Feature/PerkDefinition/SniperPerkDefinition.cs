using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class SniperPerkDefinition : IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            Gambit(builder);

            return builder.Build();
        }

        private static void Gambit(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Sniper, PerkType.Gambit)
                .Name("Gambit")
                .Description("Randomly applies one of the following effects:\n\n" +
                             "1.) Restores the HP of all nearby party members to maximum.\n" +
                             "2.) Restores the MP and STM of all nearby party members to maximum.\n" +
                             "3.) Grants haste to all nearby party members for 30 seconds.\n" +
                             "4.) Stuns all nearby enemies for 30 seconds.")

                .AddPerkLevel()
                .Description("Grants the Gambit ability.")
                .RequirementSkill(SkillType.Marksmanship, 50)
                .RequirementSkill(SkillType.Rifle, 50)
                .RequirementSkill(SkillType.LightArmor, 50)
                .RequirementQuest("a_snipers_test")
                .Price(15)
                .GrantsFeat(Feat.Gambit);
        }
    }
}