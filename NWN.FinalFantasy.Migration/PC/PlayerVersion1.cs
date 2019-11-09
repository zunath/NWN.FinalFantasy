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
            JobRepo.Set(playerID, ClassType.Warrior, new Job());
            JobRepo.Set(playerID, ClassType.Monk, new Job());
            JobRepo.Set(playerID, ClassType.WhiteMage, new Job());
            JobRepo.Set(playerID, ClassType.BlackMage, new Job());
            JobRepo.Set(playerID, ClassType.RedMage, new Job());
            JobRepo.Set(playerID, ClassType.Thief, new Job());
            JobRepo.Set(playerID, ClassType.Paladin, new Job());
            JobRepo.Set(playerID, ClassType.DarkKnight, new Job());
            JobRepo.Set(playerID, ClassType.Beastmaster, new Job());
            JobRepo.Set(playerID, ClassType.Bard, new Job());
            JobRepo.Set(playerID, ClassType.Ranger, new Job());
            JobRepo.Set(playerID, ClassType.Samurai, new Job());
            JobRepo.Set(playerID, ClassType.Ninja, new Job());
            JobRepo.Set(playerID, ClassType.Dragoon, new Job());
            JobRepo.Set(playerID, ClassType.Summoner, new Job());
            JobRepo.Set(playerID, ClassType.BlueMage, new Job());
            JobRepo.Set(playerID, ClassType.Corsair, new Job());
            JobRepo.Set(playerID, ClassType.Puppetmaster, new Job());
            JobRepo.Set(playerID, ClassType.Dancer, new Job());
            JobRepo.Set(playerID, ClassType.Scholar, new Job());
            JobRepo.Set(playerID, ClassType.Geomancer, new Job());
            JobRepo.Set(playerID, ClassType.RuneFencer, new Job());
        }

    }
}
