using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Data.Repository;
using static NWN._;

namespace NWN.FinalFantasy.Death
{
    public class SetPlayerHomePoint
    {
        public static void Main()
        {
            NWGameObject player = GetLastUsedBy();
            if (!GetIsPlayer(player)) return;

            var position = GetPosition(player);
            var orientation = GetFacing(player);
            var area = GetArea(player);
            var areaResref = GetResRef(area);

            var playerID = GetGlobalID(player);
            var entity = PlayerRepo.Get(playerID);

            entity.RespawnAreaResref = areaResref;
            entity.RespawnLocationOrientation = orientation;
            entity.RespawnLocationX = position.X;
            entity.RespawnLocationY = position.Y;
            entity.RespawnLocationZ = position.Z;

            PlayerRepo.Set(entity);
            SendMessageToPC(player, "Your home point has been set to this location.");
        }
    }
}
