using NWN.FinalFantasy.Data.Repository;

namespace NWN.FinalFantasy.Location
{
    public abstract class SavePlayerLocation
    {
        public static void Run(NWGameObject player)
        {
            if (!_.GetIsPlayer(player)) return;

            var area = _.GetArea(player);
            var position = _.GetPosition(player);
            var orientation = _.GetFacing(player);
            var playerID = _.GetGlobalID(player);
            var entity = PlayerRepo.Get(playerID);

            entity.LocationX = position.X;
            entity.LocationY = position.Y;
            entity.LocationZ = position.Z;
            entity.LocationOrientation = orientation;
            entity.LocationAreaResref = _.GetResRef(area);

            PlayerRepo.Set(entity);
        }
    }
}
