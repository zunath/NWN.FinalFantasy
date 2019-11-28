using NWN.FinalFantasy.Core.Contracts;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Map
{
    public class LoadMapProgression: IScript
    {
        public void Main()
        {
            var player = GetEnteringObject();

            if (!GetIsPlayer(player)) return;

            var area = GetArea(player);
            var areaResref = GetResRef(area);
            var playerID = GetGlobalID(player);
            var progression = MapProgressionRepo.Get(playerID, areaResref);

            if(!string.IsNullOrWhiteSpace(progression.Progression))
                NWNXPlayer.SetAreaExplorationState(player, area, progression.Progression);
        }
    }
}
