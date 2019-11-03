using NWN.FinalFantasy.Core.Startup;

namespace NWN.FinalFantasy.Persistence
{
    public class Startup
    {
        public static void Main()
        {
            AreaScriptRegistration.RegisterOnEnterScript("Persistence.PlayerLocation.LoadPlayerLocation");
            AreaScriptRegistration.RegisterOnEnterScript("Persistence.PlayerLocation.SaveOnAreaEnter");
        }
    }
}
