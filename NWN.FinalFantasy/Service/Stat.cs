using NWN.FinalFantasy.Entity;

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
    }
}
