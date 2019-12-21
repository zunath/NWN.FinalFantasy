using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Crafting
{
    public class CraftingRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Crafting...");

            RecipeRegistry.Register();
        }
    }
}
