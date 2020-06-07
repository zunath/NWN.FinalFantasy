using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Service.CraftService
{
    public interface IRecipeListDefinition
    {
        public Dictionary<RecipeType, RecipeDetail> BuildRecipes();
    }
}
