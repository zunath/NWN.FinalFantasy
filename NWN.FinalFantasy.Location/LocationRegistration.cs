using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Location
{
    public class LocationRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Location...");
        }
    }
}
