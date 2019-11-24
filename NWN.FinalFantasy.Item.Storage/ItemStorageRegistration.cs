using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Item.Storage
{
    public class ItemStorageRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Item Storage...");
        }
    }
}
