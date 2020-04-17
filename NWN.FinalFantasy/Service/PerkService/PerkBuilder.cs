using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Service.PerkService
{
    public class PerkBuilder
    {
        private readonly Dictionary<PerkType, PerkDetail> _perks = new Dictionary<PerkType, PerkDetail>();
        private PerkDetail _activePerk;
        private PerkLevel _activeLevel;

        /// <summary>
        /// Creates a new perk.
        /// </summary>
        /// <param name="category">The category under which this perk is grouped.</param>
        /// <param name="type">The type of perk.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder Create(PerkCategoryType category, PerkType type)
        {
            _activeLevel = null;

            _activePerk = new PerkDetail
            {
                Category = category,
                Type = type,
                IsActive = true
            };
            _perks[type] = _activePerk;

            return this;
        }

        /// <summary>
        /// Sets the name of a perk which will be displayed to the player.
        /// </summary>
        /// <param name="name">The name of the perk.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder Name(string name)
        {
            _activePerk.Name = name;
            return this;
        }

        /// <summary>
        /// Sets the description of the perk or the perk level.
        /// </summary>
        /// <param name="description">The description to set.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder Description(string description)
        {
            if(_activeLevel == null)
            {
                _activePerk.Description = description;
            }
            else
            {
                _activeLevel.Description = description;
            }

            return this;
        }

        /// <summary>
        /// Deactivates the perk which will prevent players from purchasing and using it.
        /// </summary>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder Inactive()
        {
            _activePerk.IsActive = false;
            return this;
        }

        /// <summary>
        /// Creates a new perk level on the active perk we're building.
        /// </summary>
        /// <param name="level">The new perk level.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder AddPerkLevel(int level)
        {
            _activeLevel = new PerkLevel();
            _activePerk.PerkLevels[level] = _activeLevel;

            return this;
        }

        /// <summary>
        /// Assigns a recast group on the active perk we're building.
        /// Calling this more than once will replace the previous recast group.
        /// </summary>
        /// <param name="recastGroup">The recast group to set.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder UsesRecastGroup(RecastGroup recastGroup)
        {
            _activePerk.RecastGroup = recastGroup;

            return this;
        }

        /// <summary>
        /// Assigns an activation type on the active perk we're building.
        /// Calling this more than once will replace the previous activation type.
        /// </summary>
        /// <param name="activationType">The activation type to set.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder UsesActivationType(PerkActivationType activationType)
        {
            _activePerk.ActivationType = activationType;

            return this;
        }

        /// <summary>
        /// Assigns a visual effect to the caster of the spell. This will display while casting.
        /// Calling this more than once will replace the previous visual effect.
        /// </summary>
        /// <param name="vfx">The visual effect to display.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder DisplaysVisualEffectWhenActivating(VisualEffect vfx = VisualEffect.Vfx_Dur_Elemental_Shield)
        {
            _activePerk.ActivationVisualEffect = vfx;

            return this;
        }

        /// <summary>
        /// Assigns an impact action on the active perk we're building.
        /// Calling this more than once will replace the previous action.
        /// Impact actions are fired when a perk is used. The timing of when it fires depends on the activation type.
        /// For example, "Casted" perks fire the impact action at the end of the casting phase.
        /// While "Queued" perks fire the impact action on the next weapon hit.
        /// </summary>
        /// <param name="action">The action to fire when a perk impacts a target.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder HasImpactAction(PerkImpactAction action)
        {
            _activePerk.ImpactAction = action;

            return this;
        }

        /// <summary>
        /// Assigns an activation delay on the active perk we're building.
        /// This is typically used for casting times.
        /// Calling this more than once will replace the previous activation delay.
        /// </summary>
        /// <param name="delay">The amount of time to delay, in seconds.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder HasActivationDelay(float delay)
        {
            _activePerk.ActivationDelay = delay;

            return this;
        }

        /// <summary>
        /// Assigns a recast delay on the active perk we're building.
        /// This prevents the ability from being used again until the specified time has passed.
        /// Calling this more than once will replace the previous recast delay.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public PerkBuilder HasRecastDelay(float delay)
        {
            _activePerk.RecastDelay = delay;

            return this;
        }

        /// <summary>
        /// Sets the amount of SP it costs to purchase this perk level.
        /// </summary>
        /// <param name="price">The price to purchase this perk level.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder Price(int price)
        {
            _activeLevel.Price = price;
            return this;
        }

        /// <summary>
        /// Adds a feat to grant to the player when the perk is purchased.
        /// </summary>
        /// <param name="feat">The feat to grant</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder GrantsFeat(Feat feat)
        {
            _activeLevel.GrantedFeats.Add(feat);
            return this;
        }

        /// <summary>
        /// Adds a skill requirement to purchase and use the perk.
        /// </summary>
        /// <param name="skill">The skill to require</param>
        /// <param name="requiredRank">The number of ranks to require</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder RequirementSkill(SkillType skill, int requiredRank)
        {
            var requirement = new PerkSkillRequirement(skill, requiredRank);
            _activeLevel.PurchaseRequirements.Add(requirement);
            _activeLevel.EffectiveLevelRequirements.Add(requirement);
            _activeLevel.ActivationRequirements.Add(requirement);

            return this;
        }

        /// <summary>
        /// Adds an MP requirement to use the perk at this level.
        /// </summary>
        /// <param name="requiredMP">The amount of MP needed to use this perk at this level.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder RequirementMP(int requiredMP)
        {
            var requirement = new PerkMPRequirement(requiredMP);
            _activeLevel.ActivationRequirements.Add(requirement);
            return this;
        }

        /// <summary>
        /// Adds a stamina requirement to use the perk at this level.
        /// </summary>
        /// <param name="requiredSTM">The amount of STM needed to use this perk at this level.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder RequirementStamina(int requiredSTM)
        {
            var requirement = new PerkStaminaRequirement(requiredSTM);
            _activeLevel.ActivationRequirements.Add(requirement);
            return this;
        }

        /// <summary>
        /// Adds an action to run when an item is equipped and the player has this perk.
        /// </summary>
        /// <param name="equipAction">The action to run when an item is equipped.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder TriggerEquippedItem(PerkTriggerEquippedUnequippedAction equipAction)
        {
            _activePerk.EquippedTriggers.Add(equipAction);
            return this;
        }

        /// <summary>
        /// Adds an action to run when an item is unequipped and the player has this perk.
        /// </summary>
        /// <param name="unequipAction">The action to run when an item is unequipped.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder TriggerUnequippedItem(PerkTriggerEquippedUnequippedAction unequipAction)
        {
            _activePerk.EquippedTriggers.Add(unequipAction);
            return this;
        }

        /// <summary>
        /// Adds an action to run when this perk is purchased.
        /// </summary>
        /// <param name="purchaseAction">The action to run when this perk is purchased.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder TriggerPurchase(PerkTriggerPurchasedRefundedAction purchaseAction)
        {
            _activePerk.PurchasedTriggers.Add(purchaseAction);
            return this;
        }

        /// <summary>
        /// Adds an action to run when this perk is refunded.
        /// </summary>
        /// <param name="refundAction">The action to run when this perk is refunded.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder TriggerRefund(PerkTriggerPurchasedRefundedAction refundAction)
        {
            _activePerk.RefundedTriggers.Add(refundAction);
            return this;
        }

        /// <summary>
        /// Returns a built list of perks.
        /// </summary>
        /// <returns>A list of built perks.</returns>
        public Dictionary<PerkType, PerkDetail> Build()
        {
            return _perks;
        }
    }
}
