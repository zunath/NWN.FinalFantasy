using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Random = NWN.FinalFantasy.Service.Random;
using Skill = NWN.FinalFantasy.Service.Skill;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class StealAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Steal1(builder);
            Steal2(builder);
            Steal3(builder);
            Steal4(builder);

            return builder.Build();
        }

        /// <summary>
        /// Checks a target's inventory for any items marked as able to be stolen.
        /// If none are found, an error message is returned.
        /// Otherwise an empty string is returned.
        /// </summary>
        /// <param name="target">The target to check.</param>
        /// <returns>An empty string if items are available, otherwise the error message.</returns>
        private static string CheckForStealableItems(uint target)
        {
            if (GetIsPC(target) ||
                GetObjectType(target) != ObjectType.Creature)
            {
                return "You cannot steal from that target.";
            }

            for (var item = GetFirstItemInInventory(target); GetIsObjectValid(item); item = GetNextItemInInventory(target))
            {
                if (GetLocalBool(item, "STEAL_ITEM"))
                    return string.Empty;
            }

            return "Target has no items to steal.";
        }

        private static void AttemptSteal(uint activator, uint target, int baseChance)
        {
            var delta = 0.0f;

            // Players who use this ability have their Thievery skill checked against the CR
            // of the target. Other users simply have a delta of zero.
            if (GetIsPC(activator) && !GetIsDM(activator))
            {
                var playerId = GetObjectUUID(activator);
                var dbPlayer = DB.Get<Player>(playerId);
                var cr = GetChallengeRating(target) * 3;
                var thievery = dbPlayer.Skills[SkillType.Thievery];

                delta = (thievery.Rank - cr) * 0.01f;
            }

            var chance = baseChance + (delta * baseChance);
            if (chance < 1) chance = 1;
            else if (chance > 95) chance = 95;

            if (Random.D100(1) <= chance)
            {
                for (var item = GetFirstItemInInventory(target); GetIsObjectValid(item); item = GetNextItemInInventory(target))
                {
                    // This item can be stolen. Copy it to the thief now.
                    if (GetLocalBool(item, "STEAL_ITEM"))
                    {
                        DeleteLocalBool(item, "STEAL_ITEM");
                        CopyItem(item, activator, true);

                        Messaging.SendMessageNearbyToPlayers(activator, $"{GetName(activator)} successfully steals '{GetName(item)}' from {GetName(target)}.");

                        DestroyObject(item);
                        break;
                    }
                }
            }
            else
            {
                SendMessageToPC(activator, $"You failed to steal from {GetName(target)}.");
            }

            Enmity.ModifyEnmity(target, activator, 5);
        }

        private static void Steal1(AbilityBuilder builder)
        {
            builder.Create(Feat.Steal1, PerkType.Steal)
                .Name("Steal I")
                .HasRecastDelay(RecastGroup.Steal, 60f)
                .RequirementStamina(5)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(3.0f)
                .HasCustomValidation((activator, target, level) => CheckForStealableItems(target))
                .HasImpactAction((activator, target, level) =>
                {
                    const int BaseChance = 15;
                    AttemptSteal(activator, target, BaseChance);
                });
        }
        private static void Steal2(AbilityBuilder builder)
        {
            builder.Create(Feat.Steal2, PerkType.Steal)
                .Name("Steal II")
                .HasRecastDelay(RecastGroup.Steal, 60f)
                .RequirementStamina(15)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(3.0f)
                .HasCustomValidation((activator, target, level) => CheckForStealableItems(target))
                .HasImpactAction((activator, target, level) =>
                {
                    const int BaseChance = 25;
                    AttemptSteal(activator, target, BaseChance);
                });
        }
        private static void Steal3(AbilityBuilder builder)
        {
            builder.Create(Feat.Steal3, PerkType.Steal)
                .Name("Steal III")
                .HasRecastDelay(RecastGroup.Steal, 60f)
                .RequirementStamina(25)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(3.0f)
                .HasCustomValidation((activator, target, level) => CheckForStealableItems(target))
                .HasImpactAction((activator, target, level) =>
                {
                    const int BaseChance = 35;
                    AttemptSteal(activator, target, BaseChance);
                });
        }
        private static void Steal4(AbilityBuilder builder)
        {
            builder.Create(Feat.Steal4, PerkType.Steal)
                .Name("Steal IV")
                .HasRecastDelay(RecastGroup.Steal, 60f)
                .RequirementStamina(40)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(3.0f)
                .HasCustomValidation((activator, target, level) => CheckForStealableItems(target))
                .HasImpactAction((activator, target, level) =>
                {
                    const int BaseChance = 50;
                    AttemptSteal(activator, target, BaseChance);
                });
        }
    }
}
