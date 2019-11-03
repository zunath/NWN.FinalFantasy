using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using static NWN._;

namespace NWN.FinalFantasy.Persistence.PlayerLocation
{
    public class LoadPlayerLocation
    {
        public static void Main()
        {
            var player = GetEnteringObject();
            var area = GetArea(player);
            var areaTag = GetTag(area);

            // Must be a player entering the OOC entry area, otherwise we exit early.
            if (!GetIsPlayer(player) || areaTag != "ooc_area") return;

            var playerID = GetGlobalID(player);
            var entity = DB.Get<Player>(playerID);
            var locationArea = GetAreaByResRef(entity.LocationAreaResref);
            var position = Vector(entity.LocationX, entity.LocationY, entity.LocationZ);

            var location = Location(locationArea, position, entity.LocationOrientation);

            AssignCommand(player, () =>
            {
                ActionJumpToLocation(location);
            });
        }

    }
}
