using System.Collections.Generic;
using System.Linq;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Registry;
using static NWN._;

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
            var playerID = GetGlobalID(player);
            var jobType = GetClassByPosition(ClassPosition.First, player);
            var abilityList = AbilityRegistry.GetByJob(jobType);
            var level = GetTotalLevel(player);

            // Ensure the player has mastered the ability and meets the level requirement.
            // If it's not mastered or player fails to meet requirements, remove it from the list of abilities to add.
            for(int x = abilityList.Count-1; x >= 0; x--)
            {
                var ability = abilityList.ElementAt(x);
                var progress = AbilityProgressRepo.Get(playerID, ability.Feat);
                var definition = AbilityRegistry.Get(ability.Feat);
                var matchingJob = definition.JobRequirements.SingleOrDefault(j => j.Job == jobType);

                if (progress.AP < definition.APRequired || // Missing AP
                    matchingJob == null || // Not a valid job
                    level < matchingJob.Level) // Level too low.
                {
                    abilityList.RemoveAt(x);
                }
            }

            var allFeats = new List<Feat>(DefaultFeats);
            allFeats.AddRange(abilityList.Select(m => m.Feat));

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
                if (GetHasFeat(feat, player)) continue;

                NWNXCreature.AddFeatByLevel(player, feat, 1);
            }
        }
    }
}
