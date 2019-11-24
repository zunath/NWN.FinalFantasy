
using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Loot
{
    public class LootRegistration: IFeatureRegistration
    {
        /// <summary>
        /// Responsible for registering loot tables.
        /// </summary>
        public void Register()
        {
            Log.Information("Registering Loot...");
        }
    }
}
