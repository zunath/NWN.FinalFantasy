using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class BlacksmithPerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            BlacksmithingRecipes(builder);

            return builder.Build();
        }

        private static void BlacksmithingRecipes(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Blacksmith, PerkType.BlacksmithingRecipes)
                .Name("Blacksmithing Recipes")
                .Description("Unlocks blacksmithing recipes.")

                .AddPerkLevel()
                .Description("Unlocks tier 1 blacksmithing recipes.")
                .Price(2)

                .AddPerkLevel()
                .Description("Unlocks tier 2 blacksmithing recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Blacksmithing, 10)

                .AddPerkLevel()
                .Description("Unlocks tier 3 blacksmithing recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Blacksmithing, 20)

                .AddPerkLevel()
                .Description("Unlocks tier 4 blacksmithing recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Blacksmithing, 30)

                .AddPerkLevel()
                .Description("Unlocks tier 5 blacksmithing recipes.")
                .Price(2)
                .RequirementSkill(SkillType.Blacksmithing, 40);

        }
    }
}
