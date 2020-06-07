using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service.CraftService
{
    public class RecipeUnlockRequirement: IRecipeRequirement
    {
        private readonly RecipeType _recipe;
        public RecipeUnlockRequirement(RecipeType recipe)
        {
            _recipe = recipe;
        }

        public string CheckRequirements(uint player)
        {
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);

            if (!dbPlayer.UnlockedRecipes.ContainsKey(_recipe))
                return "Recipe must be learned.";

            return string.Empty;
        }

        public string RequirementText => "Recipe must be learned.";
    }
}
