using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.CraftService;

namespace NWN.FinalFantasy.Feature.RecipeDefinition.LeathercraftRecipeDefinition
{
    public class LeathercraftRecipes5: IRecipeListDefinition
    {
        public Dictionary<RecipeType, RecipeDetail> BuildRecipes()
        {
            var builder = new RecipeBuilder();

            return builder.Build();
        }
    }
}
