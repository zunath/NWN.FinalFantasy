using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
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
        /// Assigns an activation delay on the active perk we're building.
        /// This is typically used for casting times.
        /// Calling this more than once will replace the previous activation type.
        /// </summary>
        /// <param name="delay">The amount of time to delay, in seconds.</param>
        /// <returns>A perk builder with the configured options</returns>
        public PerkBuilder HasActivationDelay(float delay)
        {
            _activePerk.ActivationDelay = delay;

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
        /// <returns></returns>
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
        /// <returns></returns>
        public PerkBuilder RequirementStamina(int requiredSTM)
        {
            var requirement = new PerkStaminaRequirement(requiredSTM);
            _activeLevel.ActivationRequirements.Add(requirement);
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
