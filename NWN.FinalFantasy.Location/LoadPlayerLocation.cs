using NWN.FinalFantasy.Data.Repository;

namespace NWN.FinalFantasy.Location
{
    public class LoadPlayerLocation
    {
        public static void Main()
        {
            var player = _.GetEnteringObject();
            var area = _.GetArea(player);
            var areaTag = _.GetTag(area);

            // Must be a player entering the OOC entry area, otherwise we exit early.
            if (!_.GetIsPlayer(player) || areaTag != "ooc_area") return;

            var playerID = _.GetGlobalID(player);
            var entity = PlayerRepo.Get(playerID);
            var locationArea = _.GetAreaByResRef(entity.LocationAreaResref);
            var position = _.Vector(entity.LocationX, entity.LocationY, entity.LocationZ);

            var location = _.Location(locationArea, position, entity.LocationOrientation);

            _.AssignCommand(player, () =>
            {
                _.ActionJumpToLocation(location);
            });
        }

    }
}
