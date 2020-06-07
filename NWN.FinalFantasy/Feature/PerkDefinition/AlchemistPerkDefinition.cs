using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class AlchemistPerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            AlchemistRecipes(builder);

            return builder.Build();
        }

        private static void AlchemistRecipes(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Alchemist, PerkType.AlchemyRecipes)
                .Name("Alchemy Recipes")
                .Description("Unlocks alchemy recipes.")

                .AddPerkLevel()
                .Description("Unlocks tier 1 alchemy recipes.")
                .Price(2)

                .AddPerkLevel()
                .Description("Unlocks tier 2 alchemy recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Alchemy, 10)

                .AddPerkLevel()
                .Description("Unlocks tier 3 alchemy recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Alchemy, 20)

                .AddPerkLevel()
                .Description("Unlocks tier 4 alchemy recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Alchemy, 30)

                .AddPerkLevel()
                .Description("Unlocks tier 5 alchemy recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Alchemy, 40);

        }
    }
}
