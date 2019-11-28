using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Authorization
{
    public class AuthorizationRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Authorization...");
        }
    }
}
