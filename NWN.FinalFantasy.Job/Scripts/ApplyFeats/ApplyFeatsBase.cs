using System;
using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Event;
using NWN.FinalFantasy.Job.Registry;

namespace NWN.FinalFantasy.Job.Scripts.ApplyFeats
{
    internal abstract class ApplyFeatsBase
    {
        /// <summary>
        /// These are feats all players get.
        /// </summary>
        private static readonly Feat[] DefaultFeats =
        {
            Feat.Armor_Proficiency_Light,
            Feat.Armor_Proficiency_Heavy,
            Feat.Armor_Proficiency_Medium,
            Feat.Shield_Proficiency,
            Feat.Weapon_Proficiency_Exotic,
            Feat.Weapon_Proficiency_Martial,
            Feat.Weapon_Proficiency_Simple,
            Feat.Uncanny_Dodge_1
        };

        public static void Apply(NWGameObject player)
        {
            var playerID = _.GetGlobalID(player);
            var jobType = _.GetClassByPosition(ClassPosition.First, player);
            var jobDefinition = JobRegistry.Get(jobType);
            var job = JobRepo.Get(playerID, jobType);
            var abilityList = jobDefinition.GetAbilityListByLevel(job.Level);

            // Ensure the player has mastered the ability.
            // If it's not mastered, remove it from the list of abilities to add.
            for(int x = abilityList.Count-1; x >= 0; x--)
            {
                var feat = abilityList.ElementAt(x);
                var progress = AbilityProgressRepo.Get(playerID, feat);
                var definition = AbilityRegistry.Get(feat);

                if(progress.AP < definition.APRequired)
                    abilityList.RemoveAt(x);
            }

            var allFeats = new List<Feat>(DefaultFeats);
            allFeats.AddRange(abilityList);

            // Remove any feats the player shouldn't have.
            var featCount = NWNXCreature.GetFeatCount(player);
            for (int x = featCount; x >= 0; x--)
            {
                var feat = NWNXCreature.GetFeatByIndex(player, x - 1);
                if(!allFeats.Contains(feat))
                    NWNXCreature.RemoveFeat(player, feat);
            }

            // Add any feats the player needs.
            foreach (var feat in allFeats)
            {
                if (_.GetHasFeat(feat, player)) continue;

                NWNXCreature.AddFeatByLevel(player, feat, 1);
            }
        }
    }
}
