using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Job.Registry;
using Serilog;

namespace NWN.FinalFantasy.Job
{
    public class JobRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Job...");
            XPChart.Register();
            AbilityRegistry.Register();
            JobRegistry.Register();
            RatingRegistry.Register();
        }
    }
}
