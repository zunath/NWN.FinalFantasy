using System;
using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public class SaveMapProgression: IScript
    {
        public void Main()
        {
            NWGameObject player = GetExitingObject();

            if (!GetIsPlayer(player)) return;

            var playerID = GetGlobalID(player);
            var area = GetArea(player);
            if(!GetIsObjectValid(area))
                area = NWGameObject.OBJECT_SELF;

            var areaResref = GetResRef(area);

            var progress = NWNXPlayer.GetAreaExplorationState(player, area);

            var progression = MapProgressionRepo.Get(playerID, areaResref);
            progression.Progression = progress;
            MapProgressionRepo.Set(playerID, progression);
        }
    }
}
