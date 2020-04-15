using System;
using System.Globalization;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.PerkService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Object = NWN.FinalFantasy.Core.NWNX.Object;

namespace NWN.FinalFantasy.Feature
{
    public static class UsePerkFeat
    {
        // Variable names for queued abilities.
        private const string ActiveAbilityName = "ACTIVE_ABILITY";
        private const string ActiveAbilityIdName = "ACTIVE_ABILITY_ID";
        private const string ActiveAbilityFeatIdName = "ACTIVE_ABILITY_FEAT_ID";
        private const string ActiveAbilityEffectivePerkLevelName = "ACTIVE_ABILITY_EFFECTIVE_PERK_LEVEL";

        /// <summary>
        /// When a creature uses any feat, this will check and see if the feat is registered with the perk system.
        /// If it is, requirements to use the feat will be checked and then the ability will activate.
        /// If there are errors at any point in this process, the creature will be notified and the execution will end.
        /// </summary>
        [NWNEventHandler("feat_use_bef")]
        public static void UseFeat()
        {
            var activator = OBJECT_SELF;
            var target = Object.StringToObject(Events.GetEventData("TARGET_OBJECT_ID"));
            var feat = (Feat)Convert.ToInt32(Events.GetEventData("FEAT_ID"));
            var perkType = Perk.GetPerkByFeat(feat);
            var perk = Perk.GetPerkDetails(perkType);

            // Feat isn't registered to a perk.
            if (!Perk.IsFeatRegisteredToPerk(feat))
            {
                return;
            }

            // Creature cannot use the feat.
            var effectivePerkLevel = Perk.GetEffectivePerkLevel(activator, perk.Type);
            if (!CanUsePerkFeat(activator, target, perk, effectivePerkLevel))
            {
                return;
            }

            Messaging.SendMessageNearbyToPlayers(activator, $"{GetName(activator)} readies {perk.Name}.");

            // Weapon abilties are queued for the next time the activator's attack lands on an enemy.
            if (perk.ActivationType == PerkActivationType.Weapon)
            {
                QueueWeaponAbility(activator, perk, feat, effectivePerkLevel);
            }
            // All other abilities are funneled through the same process.
            else 
            {
                ActivateAbility(activator, perk, feat, effectivePerkLevel);
            }
        }

        /// <summary>
        /// Checks whether a creature can activate the perk feat.
        /// </summary>
        /// <param name="activator">The activator of the perk feat.</param>
        /// <param name="target">The target of the perk feat.</param>
        /// <param name="perk">The perk details</param>
        /// <param name="effectivePerkLevel">The activator's effective perk level.</param>
        /// <returns>true if successful, false otherwise</returns>
        private static bool CanUsePerkFeat(uint activator, uint target, PerkDetail perk, int effectivePerkLevel)
        {
            // Must have at least one level in the perk.
            if (effectivePerkLevel <= 0)
            {
                SendMessageToPC(activator, "You do not meet the prerequisites to use this ability.");
                return false;
            }

            // Activator is dead.
            if (GetCurrentHitPoints(activator) <= 0)
            {
                SendMessageToPC(activator, "You are dead.");
                return false;
            }

            // Not commandable
            if (!GetCommandable(activator))
            {
                SendMessageToPC(activator, "You cannot take actions at this time.");
                return false;
            }

            // Must be within line of sight.
            if (!LineOfSightObject(activator, target) == false)
            {
                SendMessageToPC(activator, "You cannot see your target.");
                return false;
            }

            // Perk-specific requirement checks
            var perkLevel = perk.PerkLevels[effectivePerkLevel];
            foreach (var req in perkLevel.ActivationRequirements)
            {
                var requirementError = req.CheckRequirements(activator);
                if (!string.IsNullOrWhiteSpace(requirementError))
                {
                    SendMessageToPC(activator, requirementError);
                    return false;
                }
            }

            // Check if ability is on a recast timer still.
            if (IsOnRecastDelay(activator, perk.RecastGroup))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if a recast delay has not expired yet.
        /// Returns false if there is no recast delay or the time has already passed.
        /// </summary>
        /// <param name="creature">The creature to check</param>
        /// <param name="recastGroup">The recast group to check</param>
        /// <returns>true if recast delay hasn't passed. false otherwise</returns>
        public static bool IsOnRecastDelay(uint creature, RecastGroup recastGroup)
        {
            if (GetIsDM(creature)) return false;
            var now = DateTime.UtcNow;

            // Players
            if (GetIsPC(creature) && !GetIsDMPossessed(creature))
            {
                var playerId = GetObjectUUID(creature);
                var dbPlayer = DB.Get<Entity.Player>(playerId);

                if (!dbPlayer.RecastTimes.ContainsKey(recastGroup)) return false;

                if (now >= dbPlayer.RecastTimes[recastGroup])
                {
                    return true;
                }
                else
                {
                    string timeToWait = Time.GetTimeToWaitLongIntervals(now, dbPlayer.RecastTimes[recastGroup], false);
                    SendMessageToPC(creature, $"This ability can be used in {timeToWait}.");
                    return false;
                }
            }
            // NPCs and DM-possessed NPCs
            else
            {
                string unlockDate = GetLocalString(creature, $"ABILITY_RECAST_ID_{(int)recastGroup}");
                if (string.IsNullOrWhiteSpace(unlockDate))
                {
                    return false;
                }
                else
                {
                    var dateTime = DateTime.ParseExact(unlockDate, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                    if (now >= dateTime)
                    {
                        return true;
                    }
                    else
                    {
                        string timeToWait = Time.GetTimeToWaitLongIntervals(now, dateTime, false);
                        SendMessageToPC(creature, $"This ability can be used in {timeToWait}.");
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Applies effects to the activator for each requirement.
        /// Depending on the ability type, this may be called before or after the ability has finished.
        /// </summary>
        /// <param name="activator">The activator of the ability.</param>
        /// <param name="perk">The perk details</param>
        /// <param name="effectivePerkLevel">The effective perk level.</param>
        private static void ApplyRequirementEffects(uint activator, PerkDetail perk, int effectivePerkLevel)
        {
            var perkLevel = perk.PerkLevels[effectivePerkLevel];

            foreach (var req in perkLevel.ActivationRequirements)
            {
                req.AfterActivationAction(activator);
            }
        }

        /// <summary>
        /// Handles casting abilities. These can be combat-related or casting-related and may or may not have a casting delay.
        /// Requirement reductions (MP, STM, etc) are applied after the casting has completed.
        /// In the event there is no casting delay, the reductions are applied immediately.
        /// </summary>
        /// <param name="activator">The creature activating the ability.</param>
        /// <param name="perk">The perk details</param>
        /// <param name="feat">The feat being activated</param>
        /// <param name="effectivePerkLevel">The activator's effective perk level</param>
        private static void ActivateAbility(uint activator, PerkDetail perk, Feat feat, int effectivePerkLevel)
        {
        }

        /// <summary>
        /// Handles queuing a weapon ability for the activator's next attack.
        /// Local variables are set on the activator which are picked up the next time the activator's weapon hits a target.
        /// If the activator does not hit a target within 30 seconds, the queued ability wears off automatically.
        /// Requirement reductions (MP, STM, etc) are applied as soon as the ability is queued.
        /// </summary>
        /// <param name="activator">The creature activating the ability.</param>
        /// <param name="perk">The perk details</param>
        /// <param name="feat">The feat being activated</param>
        /// <param name="effectivePerkLevel">The activator's effective perk level</param>
        private static void QueueWeaponAbility(uint activator, PerkDetail perk, Feat feat, int effectivePerkLevel)
        {
            var abilityId = Guid.NewGuid().ToString();
            // Assign local variables which will be picked up on the next weapon OnHit event by this player.
            SetLocalInt(activator, ActiveAbilityName, (int)perk.Type);
            SetLocalString(activator, ActiveAbilityIdName, abilityId);
            SetLocalInt(activator, ActiveAbilityFeatIdName, (int)feat);
            SetLocalInt(activator, ActiveAbilityEffectivePerkLevelName, effectivePerkLevel);

            ApplyRequirementEffects(activator, perk, effectivePerkLevel);

            // todo: apply a cooldown

            // Activator must attack within 30 seconds after queueing or else it wears off.
            DelayCommand(30.0f, () =>
            {
                if (GetLocalString(activator, ActiveAbilityIdName) != abilityId) return;

                // Remove the local variables.
                DeleteLocalInt(activator, ActiveAbilityName);
                DeleteLocalString(activator, ActiveAbilityIdName);
                DeleteLocalInt(activator, ActiveAbilityFeatIdName);
                DeleteLocalInt(activator, ActiveAbilityEffectivePerkLevelName);

                // Notify the activator and nearby players
                SendMessageToPC(activator, $"Your weapon ability {perk.Name} is no longer queued.");
                Messaging.SendMessageNearbyToPlayers(activator, $"{GetName(activator)} not longer has weapon ability {perk.Name} readied.");
            });
        }

        // todo: onhit event which fires off the queued weapon ability


        // todo: module on enter event which removes the temporary queued weapon ability variables, just in case the 30 second delay doesn't fire.
    }
}
