using System;
using NWN.FinalFantasy.Core.Enumerations;
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
            PlayerRepo.Set(entity);

            InitializeJobs(playerID);
        }

        private void InitializeJobs(Guid playerID)
        {
            JobRepo.Set(playerID, JobType.Warrior, new Job());
            JobRepo.Set(playerID, JobType.Monk, new Job());
            JobRepo.Set(playerID, JobType.WhiteMage, new Job());
            JobRepo.Set(playerID, JobType.BlackMage, new Job());
            JobRepo.Set(playerID, JobType.RedMage, new Job());
            JobRepo.Set(playerID, JobType.Thief, new Job());
            JobRepo.Set(playerID, JobType.Paladin, new Job());
            JobRepo.Set(playerID, JobType.DarkKnight, new Job());
            JobRepo.Set(playerID, JobType.Beastmaster, new Job());
            JobRepo.Set(playerID, JobType.Bard, new Job());
            JobRepo.Set(playerID, JobType.Ranger, new Job());
            JobRepo.Set(playerID, JobType.Samurai, new Job());
            JobRepo.Set(playerID, JobType.Ninja, new Job());
            JobRepo.Set(playerID, JobType.Dragoon, new Job());
            JobRepo.Set(playerID, JobType.Summoner, new Job());
            JobRepo.Set(playerID, JobType.BlueMage, new Job());
            JobRepo.Set(playerID, JobType.Corsair, new Job());
            JobRepo.Set(playerID, JobType.Puppetmaster, new Job());
            JobRepo.Set(playerID, JobType.Dancer, new Job());
            JobRepo.Set(playerID, JobType.Scholar, new Job());
            JobRepo.Set(playerID, JobType.Geomancer, new Job());
            JobRepo.Set(playerID, JobType.RuneFencer, new Job());
        }

    }
}
