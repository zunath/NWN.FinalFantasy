using System;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Migration.PC
{
    public class PlayerVersion1: IPCMigration
    {
        public int Version { get; }

        public PlayerVersion1(int version)
        {
            Version = version;
        }

        public void RunMigration(NWGameObject player)
        {
            var playerID = _.GetGlobalID(player);
            CreateMapPins(playerID);
        }

        private static void CreateMapPins(Guid playerID)
        {
            var mapPins = new EntityList<MapPin>(playerID);
            DB.Set(mapPins);
        }
    }
}
