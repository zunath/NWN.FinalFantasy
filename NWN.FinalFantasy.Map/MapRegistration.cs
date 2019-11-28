using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Map
{
    public class MapRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Map...");
        }
    }
}
