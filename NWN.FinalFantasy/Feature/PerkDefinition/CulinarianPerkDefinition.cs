using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class CulinarianPerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            CookingRecipes(builder);

            return builder.Build();
        }

        private static void CookingRecipes(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Culinarian, PerkType.CookingRecipes)
                .Name("Cooking Recipes")
                .Description("Unlocks cooking recipes.")

                .AddPerkLevel()
                .Description("Unlocks tier 1 cooking recipes.")
                .Price(2)

                .AddPerkLevel()
                .Description("Unlocks tier 2 cooking recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Cooking, 10)

                .AddPerkLevel()
                .Description("Unlocks tier 3 cooking recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Cooking, 20)

                .AddPerkLevel()
                .Description("Unlocks tier 4 cooking recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Cooking, 30)

                .AddPerkLevel()
                .Description("Unlocks tier 5 cooking recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Cooking, 40);

        }
    }
}
