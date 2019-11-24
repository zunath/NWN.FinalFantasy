using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Death
{
    public class DeathRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Death...");
        }
    }
}
