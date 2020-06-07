using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class LeatherworkerPerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            LeathercraftRecipes(builder);

            return builder.Build();
        }

        private static void LeathercraftRecipes(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Leatherworker, PerkType.LeathercraftRecipes)
                .Name("Leathercraft Recipes")
                .Description("Unlocks leathercraft recipes.")

                .AddPerkLevel()
                .Description("Unlocks tier 1 leathercraft recipes.")
                .Price(2)

                .AddPerkLevel()
                .Description("Unlocks tier 2 leathercraft recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Leathercraft, 10)

                .AddPerkLevel()
                .Description("Unlocks tier 3 leathercraft recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Leathercraft, 20)

                .AddPerkLevel()
                .Description("Unlocks tier 4 leathercraft recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Leathercraft, 30)

                .AddPerkLevel()
                .Description("Unlocks tier 5 leathercraft recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Leathercraft, 40);

        }
    }
}
