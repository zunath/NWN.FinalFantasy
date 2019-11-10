using System;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Data.Repository;
using NWN.FinalFantasy.Job.Event;
using NWN.FinalFantasy.Job.Registry;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts
{
    /// <summary>
    /// Grants AP to a player
    /// </summary>
    internal class GainAP
    {
        public static void Main()
        {
            var data = Script.GetScriptData<JobXPGained>();
            var player = data.Creature;
            var playerID = GetGlobalID(player);
            var equippedAbilities = EquippedAbilityRepo.Get(playerID);
            int ap = data.Amount / 1000;
            if (ap < 1) 
                ap = 1;

            SendMessageToPC(player, "AP Gained: " + ap);
            foreach (var ability in equippedAbilities.Entities)
            {
                var progress = AbilityProgressRepo.Get(playerID, ability.Feat);
                var definition = AbilityRegistry.Get(ability.Feat);

                // Ability has already been mastered.
                if (progress.AP >= definition.APRequired)
                    continue;

                progress.AP += ap * ability.Quantity;

                if (progress.AP >= definition.APRequired)
                {
                    progress.AP = definition.APRequired;

                    var payload = new AbilityMastered(player, ability.Feat);
                    Publish.CustomEvent(player, JobEventPrefix.OnAbilityMastered, payload);
                }

                AbilityProgressRepo.Set(playerID, progress);
            }

            EquippedAbilityRepo.Set(playerID, equippedAbilities);
        }
    }
}
