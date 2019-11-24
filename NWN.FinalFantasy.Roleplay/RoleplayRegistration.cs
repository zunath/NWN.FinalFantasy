using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Roleplay
{
    public class RoleplayRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Roleplay...");
        }
    }
}
