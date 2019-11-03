using System;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
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
            var entity = DB.Get<Player>(playerID);

            entity.LocationX = position.X;
            entity.LocationY = position.Y;
            entity.LocationZ = position.Z;
            entity.LocationOrientation = orientation;
            entity.LocationAreaResref = GetResRef(area);

            DB.Set(entity);
        }
    }
}
