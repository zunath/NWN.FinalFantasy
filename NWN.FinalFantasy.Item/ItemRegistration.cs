using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Item
{
    public class ItemRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Item...");
        }
    }
}
