using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Menu
{
    public class MenuRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Menu...");
        }
    }
}
