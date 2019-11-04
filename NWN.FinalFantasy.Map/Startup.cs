using NWN.FinalFantasy.Core.Startup;

namespace NWN.FinalFantasy.Map
{
    public class Startup
    {
        public static void Main()
        {
            AreaScriptRegistration.RegisterOnEnterScript("Map.AutoExploreArea");
            AreaScriptRegistration.RegisterOnEnterScript("Map.LoadMapProgression");
            AreaScriptRegistration.RegisterOnExitScript("Map.SaveMapProgression");
            AreaScriptRegistration.RegisterOnHeartbeatScript("Map.HideMap");
        }
    }
}
