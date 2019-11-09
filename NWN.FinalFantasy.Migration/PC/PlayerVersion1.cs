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

            InitializeJobs(playerID);
            InitializeFeats(player);
            InitializeSkills(player);
            InitializeSavingThrows(player);
            RemoveNWNSpells(player);

            Publish.CustomEvent(player, JobEventPrefix.OnJobChanged, new JobChanged(player, job, job));
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

        private void InitializeFeats(NWGameObject player)
        {
            int numberOfFeats = NWNXCreature.GetFeatCount(player);
            for (int currentFeat = numberOfFeats; currentFeat >= 0; currentFeat--)
            {
                NWNXCreature.RemoveFeat(player, NWNXCreature.GetFeatByIndex(player, currentFeat - 1));
            }

            NWNXCreature.AddFeatByLevel(player, Feat.Armor_Proficiency_Light, 1);
            NWNXCreature.AddFeatByLevel(player, Feat.Armor_Proficiency_Medium, 1);
            NWNXCreature.AddFeatByLevel(player, Feat.Armor_Proficiency_Heavy, 1);
            NWNXCreature.AddFeatByLevel(player, Feat.Shield_Proficiency, 1);
            NWNXCreature.AddFeatByLevel(player, Feat.Weapon_Proficiency_Exotic, 1);
            NWNXCreature.AddFeatByLevel(player, Feat.Weapon_Proficiency_Martial, 1);
            NWNXCreature.AddFeatByLevel(player, Feat.Weapon_Proficiency_Simple, 1);
            NWNXCreature.AddFeatByLevel(player, Feat.Uncanny_Dodge_1, 1);
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
