using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enum;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Player = NWN.FinalFantasy.Entity.Player;

namespace NWN.FinalFantasy.Service
{
    public class Stat
    {
        /// <summary>
        /// Restores an entity's MP by a specified amount.
        /// This method will not persist the changes so be sure you call DB.Set after calling this.
        /// </summary>
        /// <param name="entity">The entity to modify.</param>
        /// <param name="amount">The amount of MP to restore.</param>
        public static void RestoreMP(Player entity, int amount)
        {
            if (amount <= 0) return;

            entity.MP += amount;

            if (entity.MP > entity.MaxMP)
                entity.MP = entity.MaxMP;
        }

        /// <summary>
        /// Reduces an entity's MP by a specified amount.
        /// If player would fall below 0 MP, they will be reduced to 0 instead.
        /// This method will not persist the changes so be sure you call DB.Set after calling this.
        /// </summary>
        /// <param name="entity">The entity to modify</param>
        /// <param name="reduceBy">The amount of MP to reduce by.</param>
        public static void ReduceMP(Player entity, int reduceBy)
        {
            if (reduceBy <= 0) return;

            entity.MP -= reduceBy;

            if (entity.MP < 0)
                entity.MP = 0;
        }

        /// <summary>
        /// Restores an entity's Stamina by a specified amount.
        /// This method will not persist the changes so be sure you call DB.Set after calling this.
        /// </summary>
        /// <param name="entity">The entity to modify.</param>
        /// <param name="amount">The amount of Stamina to restore.</param>
        public static void RestoreStamina(Player entity, int amount)
        {
            if (amount <= 0) return;

            entity.Stamina += amount;

            if (entity.Stamina > entity.MaxStamina)
                entity.Stamina = entity.MaxStamina;
        }

        /// <summary>
        /// Reduces an entity's Stamina by a specified amount.
        /// If player would fall below 0 stamina, they will be reduced to 0 instead.
        /// This method will not persist the changes so be sure you call DB.Set after calling this.
        /// </summary>
        /// <param name="entity">The entity to modify</param>
        /// <param name="reduceBy">The amount of Stamina to reduce by.</param>
        public static void ReduceStamina(Player entity, int reduceBy)
        {
            if (reduceBy <= 0) return;

            entity.Stamina -= reduceBy;

            if (entity.Stamina < 0)
                entity.Stamina = 0;
        }

        /// <summary>
        /// Increases or decreases a player's HP by a specified amount.
        /// There is a cap of 255 HP per NWN level. Players are auto-leveled to 5 by default, so this
        /// gives 255 * 5 = 1275 HP maximum. If the player's HP would go over this amount, it will be set to 1275.
        /// This method will not persist the changes so be sure you call DB.Set after calling this.
        /// </summary>
        /// <param name="entity">The entity to modify</param>
        /// <param name="player">The player to adjust</param>
        /// <param name="adjustBy">The amount to adjust by.</param>
        public static void AdjustMaxHP(Player entity, uint player, int adjustBy)
        {
            const int MaxHPPerLevel = 255;
            entity.MaxHP += adjustBy;
            var nwnLevelCount = GetLevelByPosition(1, player) +
                                GetLevelByPosition(2, player) +
                                GetLevelByPosition(3, player);

            var hpToApply = entity.MaxHP;

            // All levels must have at least 1 HP, so apply those right now.
            for (var nwnLevel = 1; nwnLevel <= nwnLevelCount; nwnLevel++)
            {
                hpToApply--;
                Creature.SetMaxHitPointsByLevel(player, nwnLevel, 1);
            }

            // It's possible for the MaxHP value to be a negative if builders misuse item properties, etc.
            // Players cannot go under 'nwnLevel' HP, so we apply that first. If our HP to apply is zero, we don't want to
            // do any more logic with HP application.
            if (hpToApply > 0)
            {
                // Apply the remaining HP.
                for (var nwnLevel = 1; nwnLevel <= nwnLevelCount; nwnLevel++)
                {
                    if (hpToApply > MaxHPPerLevel) // Levels can only contain a max of 255 HP
                    {
                        Creature.SetMaxHitPointsByLevel(player, nwnLevel, 255);
                        hpToApply -= 254;
                    }
                    else // Remaining value gets set to the level. (<255 hp)
                    {
                        Creature.SetMaxHitPointsByLevel(player, nwnLevel, hpToApply + 1);
                        break;
                    }
                }
            }

            // If player's current HP is higher than max, deal the difference in damage to bring them back down to their new maximum.
            var currentHP = GetCurrentHitPoints(player);
            var maxHP = GetMaxHitPoints(player);
            if (currentHP > maxHP)
            {
                var damage = EffectDamage(currentHP - maxHP);
                ApplyEffectToObject(DurationType.Instant, damage, player);
            }
        }

        /// <summary>
        /// Modifies a player's maximum MP by a certain amount.
        /// This method will not persist the changes so be sure you call DB.Set after calling this.
        /// </summary>
        /// <param name="entity">The entity to modify</param>
        /// <param name="adjustBy">The amount to adjust by</param>
        public static void AdjustMaxMP(Player entity, int adjustBy)
        {
            // Note: It's possible for Max MP to drop to a negative number. This is expected to ensure calculations stay in sync.
            // If there are any visual indicators (GUI elements for example) be sure to account for this scenario.
            entity.MaxMP += adjustBy;

            if (entity.MP > entity.MaxMP)
                entity.MP = entity.MaxMP;

            // Current MP, however, should never drop below zero.
            if (entity.MP < 0)
                entity.MP = 0;
        }

        /// <summary>
        /// Modifies a player's maximum STM by a certain amount.
        /// This method will not persist the changes so be sure you call DB.Set after calling this.
        /// </summary>
        /// <param name="entity">The entity to modify</param>
        /// <param name="adjustBy">The amount to adjust by</param>
        public static void AdjustMaxSTM(Player entity, int adjustBy)
        {
            // Note: It's possible for Max STM to drop to a negative number. This is expected to ensure calculations stay in sync.
            // If there are any visual indicators (GUI elements for example) be sure to account for this scenario.
            entity.MaxStamina += adjustBy;

            if (entity.Stamina > entity.MaxStamina)
                entity.Stamina = entity.MaxStamina;

            // Current STM, however, should never drop below zero.
            if (entity.Stamina < 0)
                entity.Stamina = 0;
        }
    }
}
