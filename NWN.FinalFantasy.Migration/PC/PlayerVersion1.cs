using NWN.FinalFantasy.Data.Repository;
using static NWN._;

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
            var playerID = GetGlobalID(player);
            var entity = PlayerRepo.Get(playerID);
            entity.Name = GetName(player);
            PlayerRepo.Set(entity);
        }
    }
}
