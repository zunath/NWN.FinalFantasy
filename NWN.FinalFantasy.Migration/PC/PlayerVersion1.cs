using System;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Event;
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
            var job = GetClassByPosition(ClassPosition.First, player);
            entity.Name = GetName(player);
            entity.CurrentJob = job;
            PlayerRepo.Set(entity);

            ClearInventory(player);
            InitializeJobs(playerID);
            InitializeSkills(player);
            InitializeSavingThrows(player);
            RemoveNWNSpells(player);

            // Treat this initialization as a job change and a level-up.
            Publish.CustomEvent(player, JobEventPrefix.OnJobChanged, new JobChanged(player, job, job));
            Publish.CustomEvent(player, JobEventPrefix.OnLeveledUp, new LeveledUp(player));
        }

        private void ClearInventory(NWGameObject player)
        {
            for(int slot = 0; slot < NWNConstants.NumberOfInventorySlots; slot++)
            {
                var item = GetItemInSlot((InventorySlot)slot, player);
                if (!GetIsObjectValid(item)) continue;

                DestroyObject(item);
            }

            _.DestroyAllInventoryItems(player);
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

        private void InitializeSkills(NWGameObject player)
        {
            for (int iCurSkill = 1; iCurSkill <= 27; iCurSkill++)
            {
                NWNXCreature.SetSkillRank(player, iCurSkill - 1, 0);
            }
        }

        private void InitializeSavingThrows(NWGameObject player)
        {
            SetFortitudeSavingThrow(player, 0);
            SetReflexSavingThrow(player, 0);
            SetWillSavingThrow(player, 0);
        }

        private void RemoveNWNSpells(NWGameObject player)
        {
            var @class = GetClassByPosition(ClassPosition.First, player);
            for (int index = 0; index <= 255; index++)
            {
                NWNXCreature.RemoveKnownSpell(player, @class, 0, index);
            }
        }
    }
}
