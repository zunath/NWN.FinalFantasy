using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.CraftService;

namespace NWN.FinalFantasy.Feature.RecipeDefinition.BlacksmithingRecipeDefinition
{
    public class CookingRecipes1: IRecipeListDefinition
    {
        public Dictionary<RecipeType, RecipeDetail> BuildRecipes()
        {
            var builder = new RecipeBuilder();
            LongswordRecipes(builder);

            return builder.Build();
        }

        private static void LongswordRecipes(RecipeBuilder builder)
        {
            builder.Create(RecipeType.BasicLongsword, SkillType.Blacksmithing)
                .Category(RecipeCategoryType.Longsword)
                .Name("Basic Longsword")
                .Resref("nw_wswls001")
                .RequirementPerk(PerkType.BlacksmithingRecipes, 1)
                .Component("quest_item", 2);
        }

    }
}
