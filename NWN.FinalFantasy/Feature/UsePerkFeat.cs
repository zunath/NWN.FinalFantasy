using System;
using System.Globalization;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Bioware;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.Item;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using NWN.FinalFantasy.Service.PerkService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Object = NWN.FinalFantasy.Core.NWNX.Object;

namespace NWN.FinalFantasy.Feature
{
    public static class UsePerkFeat
    {
        private enum ActivationStatus
        {
            Invalid = 0,
            Started = 1,
            Interrupted = 2,
            Completed = 3
        }

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
            if (!Ability.IsFeatRegistered(feat)) return;
            var ability = Ability.GetAbilityDetail(feat);
            
            // Creature cannot use the feat.
            var effectivePerkLevel = Perk.GetEffectivePerkLevel(activator, ability.EffectiveLevelPerkType);
            if (!CanUseAbility(activator, target, ability, effectivePerkLevel))
            {
                return;
            }

            Messaging.SendMessageNearbyToPlayers(activator, $"{GetName(activator)} readies {ability.Name}.");

            // Weapon abilties are queued for the next time the activator's attack lands on an enemy.
            if (ability.ActivationType == AbilityActivationType.Weapon)
            {
                QueueWeaponAbility(activator, ability, feat, effectivePerkLevel);
            }
            // All other abilities are funneled through the same process.
            else 
            {
                ActivateAbility(activator, target, ability, effectivePerkLevel);
            }
        }

        /// <summary>
        /// Checks whether a creature can activate the perk feat.
        /// </summary>
        /// <param name="activator">The activator of the perk feat.</param>
        /// <param name="target">The target of the perk feat.</param>
        /// <param name="ability">The ability details</param>
        /// <param name="effectivePerkLevel">The activator's effective perk level.</param>
        /// <returns>true if successful, false otherwise</returns>
        private static bool CanUseAbility(uint activator, uint target, AbilityDetail ability, int effectivePerkLevel)
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
            foreach (var req in ability.Requirements)
            {
                var requirementError = req.CheckRequirements(activator);
                if (!string.IsNullOrWhiteSpace(requirementError))
                {
                    SendMessageToPC(activator, requirementError);
                    return false;
                }
            }

            // Check if ability is on a recast timer still.
            if (IsOnRecastDelay(activator, ability.RecastGroup))
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
        /// <param name="ability">The ability details</param>
        private static void ApplyRequirementEffects(uint activator, AbilityDetail ability)
        {
            foreach (var req in ability.Requirements)
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
        /// <param name="target">The target of the ability</param>
        /// <param name="ability">The ability details</param>
        /// <param name="effectivePerkLevel">The activator's effective perk level</param>
        private static void ActivateAbility(uint activator, uint target, AbilityDetail ability, int effectivePerkLevel)
        {
            // Activation delay is increased if player is equipped with heavy or light armor.
            float CalculateActivationDelay()
            {
                const float HeavyArmorPenalty = 2.0f;
                const float LightArmorPenalty = 1.5f;

                var armorPenalty = 1.0f;
                var penaltyMessage = string.Empty;
                for (var slot = 0; slot < NumberOfInventorySlots; slot++)
                {
                    var item = GetItemInSlot((InventorySlot) slot, activator);

                    for (var ip = GetFirstItemProperty(item); GetIsItemPropertyValid(ip); ip = GetNextItemProperty(item))
                    {
                        if (GetItemPropertyType(ip) != ItemPropertyType.ArmorType) continue;

                        var armorType = (ArmorType)GetItemPropertySubType(ip);
                        if (armorType == ArmorType.Heavy)
                        {
                            armorPenalty = HeavyArmorPenalty;
                            penaltyMessage = "Heavy armor slows your casting speed by 100%.";
                            break;
                        }
                        else if (armorType == ArmorType.Light)
                        {
                            armorPenalty = LightArmorPenalty;
                            penaltyMessage = "Light armor slows your casting speed by 50%.";
                        }
                    }

                    // If we found heavy armor, we can exit early. Anything else requires us to iterate over the rest of the items.
                    if (armorPenalty >= HeavyArmorPenalty) break;
                }

                // Notify player if needed.
                if (!string.IsNullOrWhiteSpace(penaltyMessage))
                {
                    SendMessageToPC(activator, penaltyMessage);
                }

                return ability.ActivationDelay(activator, target) * armorPenalty;
            }

            // Handles displaying animation and visual effects.
            void ProcessAnimationAndVisualEffects(float delay)
            {
                // Force out of stealth
                if (GetActionMode(activator, ActionMode.Stealth))
                    SetActionMode(activator, ActionMode.Stealth, false);

                AssignCommand(activator, () => ClearAllActions());
                BiowarePosition.TurnToFaceObject(target, activator);

                // Display a casting visual effect if one has been specified.
                if (ability.ActivationVisualEffect != VisualEffect.None)
                {
                    var vfx = TagEffect(EffectVisualEffect(ability.ActivationVisualEffect), "ACTIVATION_VFX");
                    ApplyEffectToObject(DurationType.Temporary, vfx, activator, delay + 0.2f);
                }

                // Casted types play an animation of casting.
                if (ability.ActivationType == AbilityActivationType.Casted)
                {
                    AssignCommand(activator, () => ActionPlayAnimation(Animation.LoopingConjure1, 1.0f, delay - 0.2f));
                }
            }

            // Recursive function which checks if player has moved since starting the casting.
            void CheckForActivationInterruption(string activationId, Vector originalPosition)
            {
                if (!GetIsPC(activator)) return;

                // Completed abilities should no longer run.
                var status = GetLocalInt(activator, activationId);
                if (status == (int) ActivationStatus.Completed || status == (int)ActivationStatus.Invalid) return;
                
                var currentPosition = GetPosition(activator);

                if (currentPosition.X != originalPosition.X ||
                    currentPosition.Y != originalPosition.Y ||
                    currentPosition.Z != originalPosition.Z)
                {
                    for (var effect = GetFirstEffect(activator); GetIsEffectValid(effect); effect = GetNextEffect(activator))
                    {
                        if (GetEffectTag(effect) == "ACTIVATION_VFX")
                        {
                            RemoveEffect(activator, effect);
                        }
                    }

                    Player.StopGuiTimingBar(activator, string.Empty);
                    SendMessageToPC(activator, "Your ability has been interrupted.");
                    return;
                }

                DelayCommand(0.5f, () => CheckForActivationInterruption(activationId, originalPosition));
            }

            // This method is called after the delay of the ability has finished.
            void CompleteActivation(string id, float delay)
            {
                DeleteLocalInt(activator, id);

                // Moved during casting or activator died. Cancel the activation.
                if (GetLocalInt(activator, id) == (int) ActivationStatus.Interrupted || GetCurrentHitPoints(activator) <= 0) return;

                ApplyRequirementEffects(activator, ability);
                ability.ImpactAction?.Invoke(activator, target, effectivePerkLevel);
                ApplyRecastDelay(activator, ability.RecastGroup, delay);
            }

            // Begin the main process
            var activationId = Guid.NewGuid().ToString();
            var activationDelay = CalculateActivationDelay();
            var position = GetPosition(activator);
            ProcessAnimationAndVisualEffects(activationDelay);
            CheckForActivationInterruption(activationId, position);
            SetLocalInt(activator, activationId, (int)ActivationStatus.Started);

            if (activationDelay > 0.0f)
            {
                Player.StartGuiTimingBar(activator, activationDelay, string.Empty);
            }

            DelayCommand(activationDelay, () => CompleteActivation(activationId, activationDelay));
        }

        /// <summary>
        /// Handles queuing a weapon ability for the activator's next attack.
        /// Local variables are set on the activator which are picked up the next time the activator's weapon hits a target.
        /// If the activator does not hit a target within 30 seconds, the queued ability wears off automatically.
        /// Requirement reductions (MP, STM, etc) are applied as soon as the ability is queued.
        /// </summary>
        /// <param name="activator">The creature activating the ability.</param>
        /// <param name="ability">The ability details</param>
        /// <param name="feat">The feat being activated</param>
        /// <param name="effectivePerkLevel">The activator's effective perk level</param>
        private static void QueueWeaponAbility(uint activator, AbilityDetail ability, Feat feat, int effectivePerkLevel)
        {
            var abilityId = Guid.NewGuid().ToString();
            // Assign local variables which will be picked up on the next weapon OnHit event by this player.
            SetLocalInt(activator, ActiveAbilityName, (int)feat);
            SetLocalString(activator, ActiveAbilityIdName, abilityId);
            SetLocalInt(activator, ActiveAbilityFeatIdName, (int)feat);
            SetLocalInt(activator, ActiveAbilityEffectivePerkLevelName, effectivePerkLevel);

            ApplyRequirementEffects(activator, ability);
            ApplyRecastDelay(activator, ability.RecastGroup, ability.RecastDelay(activator));

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
                SendMessageToPC(activator, $"Your weapon ability {ability.Name} is no longer queued.");
                Messaging.SendMessageNearbyToPlayers(activator, $"{GetName(activator)} no longer has weapon ability {ability.Name} readied.");
            });
        }

        /// <summary>
        /// When a player's weapon hits a target, if an ability is queued, that ability will be executed.
        /// </summary>
        [NWNEventHandler("item_on_hit")]
        public static void ProcessQueuedWeaponAbility()
        {
            var activator = OBJECT_SELF;
            if (!GetIsObjectValid(activator)) return;

            var target = GetSpellTargetObject();
            var item = GetSpellCastItem();

            // If this method was triggered by our own armor (from getting hit), return. 
            if (GetBaseItemType(item) == BaseItem.Armor) return;

            var activeWeaponAbility = (Feat)GetLocalInt(activator, ActiveAbilityName);
            var activeAbilityEffectivePerkLevel = GetLocalInt(activator, ActiveAbilityEffectivePerkLevelName);

            if (!Ability.IsFeatRegistered(activeWeaponAbility)) return;

            var perk = Ability.GetAbilityDetail(activeWeaponAbility);
            if (!CanUseAbility(activator, target, perk, activeAbilityEffectivePerkLevel))
            {
                return;
            }

            perk.ImpactAction?.Invoke(activator, target, activeAbilityEffectivePerkLevel);

            DeleteLocalInt(activator, ActiveAbilityName);
            DeleteLocalString(activator, ActiveAbilityIdName);
            DeleteLocalInt(activator, ActiveAbilityFeatIdName);
            DeleteLocalInt(activator, ActiveAbilityEffectivePerkLevelName);
        }

        /// <summary>
        /// Whenever a player enters the server, any temporary variables related to ability execution
        /// will be removed from their PC.
        /// </summary>
        [NWNEventHandler("mod_enter")]
        public static void ClearTemporaryQueuedVariables()
        {
            var player = GetEnteringObject();

            DeleteLocalInt(player, ActiveAbilityName);
            DeleteLocalString(player, ActiveAbilityIdName);
            DeleteLocalInt(player, ActiveAbilityFeatIdName);
            DeleteLocalInt(player, ActiveAbilityEffectivePerkLevelName);
        }

        /// <summary>
        /// Applies a recast delay on a specific recast group.
        /// If group is invalid or delay amount is less than or equal to zero, nothing will happen.
        /// </summary>
        /// <param name="activator">The activator of the ability.</param>
        /// <param name="group">The recast group to put this delay under.</param>
        /// <param name="delaySeconds">The number of seconds to delay.</param>
        private static void ApplyRecastDelay(uint activator, RecastGroup group, float delaySeconds)
        {
            if (!GetIsObjectValid(activator) || group == RecastGroup.Invalid || delaySeconds <= 0.0f) return;

            var recastDate = DateTime.UtcNow.AddSeconds(delaySeconds);

            // NPCs and DM-possessed NPCs
            if (!GetIsPC(activator) || GetIsDMPossessed(activator))
            {
                var recastDateString = recastDate.ToString("yyyy-MM-dd hh:mm:ss");
                SetLocalString(activator, $"ABILITY_RECAST_ID_{(int)group}", recastDateString);
            }
            // Players
            else if (GetIsPC(activator) && !GetIsDM(activator))
            {
                var playerId = GetObjectUUID(activator);
                var dbPlayer = DB.Get<Entity.Player>(playerId);
                dbPlayer.RecastTimes[group] = recastDate;
            }

        }
    }
}
