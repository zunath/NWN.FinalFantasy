using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Persistence.PlayerLocation
{
    public abstract class SavePlayerLocation
    {
        public static void Run(NWGameObject player)
        {
            if (!GetIsPlayer(player)) return;

            var area = GetArea(player);
            var position = GetPosition(player);
            var orientation = GetFacing(player);
            var playerID = GetGlobalID(player);
            var entity = PlayerRepo.Get(playerID);

            entity.LocationX = position.X;
            entity.LocationY = position.Y;
            entity.LocationZ = position.Z;
            entity.LocationOrientation = orientation;
            entity.LocationAreaResref = GetResRef(area);

            PlayerRepo.Set(entity);
        }
    }
}
