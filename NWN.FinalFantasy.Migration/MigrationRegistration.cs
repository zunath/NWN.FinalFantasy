using NWN.FinalFantasy.Core.Contracts;
using Serilog;

namespace NWN.FinalFantasy.Migration
{
    public class MigrationRegistration: IFeatureRegistration
    {
        public void Register()
        {
            Log.Information("Registering Migration...");
            ServerMigrationRunner.Run();
        }
    }
}
