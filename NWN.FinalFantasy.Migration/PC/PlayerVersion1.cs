using System;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Entity;
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
            entity.CurrentJob = GetClassByPosition(ClassPosition.First, player);
            PlayerRepo.Set(entity);

            InitializeJobs(playerID);
        }

        private void InitializeJobs(Guid playerID)
        {
            JobRepo.Set(playerID, ClassType.Warrior, new Data.Entity.Job());
            JobRepo.Set(playerID, ClassType.Monk, new Data.Entity.Job());
            JobRepo.Set(playerID, ClassType.WhiteMage, new Data.Entity.Job());
            JobRepo.Set(playerID, ClassType.BlackMage, new Data.Entity.Job());
            JobRepo.Set(playerID, ClassType.Thief, new Data.Entity.Job());
            JobRepo.Set(playerID, ClassType.Ranger, new Data.Entity.Job());


        }

    }
}
