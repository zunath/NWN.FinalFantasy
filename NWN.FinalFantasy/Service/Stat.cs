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
            entity.MP += amount;

            if (entity.MP > entity.MaxMP)
                entity.MP = entity.MaxMP;
        }

        /// <summary>
        /// Restores an entity's Stamina by a specified amount.
        /// This method will not persist the changes so be sure you call DB.Set after calling this.
        /// </summary>
        /// <param name="entity">The entity to modify.</param>
        /// <param name="amount">The amount of Stamina to restore.</param>
        public static void RestoreStamina(Player entity, int amount)
        {
            entity.Stamina += amount;

            if (entity.Stamina > entity.MaxStamina)
                entity.Stamina = entity.MaxStamina;
        }
    }
}
