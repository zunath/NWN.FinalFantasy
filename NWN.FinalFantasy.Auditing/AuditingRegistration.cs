using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Auditing
{
    public class AuditingRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Auditing...");
        }
    }
}
