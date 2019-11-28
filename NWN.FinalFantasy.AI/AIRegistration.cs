using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.AI
{
    public class AIRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Starting AI Feature...");
        }
    }
}
