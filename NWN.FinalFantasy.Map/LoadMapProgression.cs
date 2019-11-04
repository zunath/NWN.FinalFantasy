using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public class LoadMapProgression: MapProgressionBase
    {
        public static void Main()
        {
            var player = GetEnteringObject();

            if (!GetIsPlayer(player)) return;

            var area = GetArea(player);
            var areaResref = GetResRef(area);
            var playerID = GetGlobalID(player);
            var key = BuildKey(playerID, areaResref);

            if (DB.Exists(key))
            {
                var progression = DB.Get<MapProgression>(key);
                NWNXPlayer.SetAreaExplorationState(player, area, progression.Progression);
            }
        }
    }
}
