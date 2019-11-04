using System;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public class SaveMapProgression: MapProgressionBase
    {
        public static void Main()
        {
            NWGameObject player = GetExitingObject();

            if (!GetIsPlayer(player)) return;

            var playerID = GetGlobalID(player);
            var area = GetArea(player);
            if(!GetIsObjectValid(area))
                area = NWGameObject.OBJECT_SELF;

            var areaResref = GetResRef(area);

            var key = BuildKey(playerID, areaResref);
            var progress = NWNXPlayer.GetAreaExplorationState(player, area);

            if (DB.Exists(key))
            {
                var progression = DB.Get<MapProgression>(key);
                progression.Progression = progress;
                DB.Set(key, progression);
            }
            else
            {
                var progression = new MapProgression
                {
                    AreaResref = areaResref,
                    Progression = progress
                };
                DB.Set(key, progression);
            }
        }
    }
}
