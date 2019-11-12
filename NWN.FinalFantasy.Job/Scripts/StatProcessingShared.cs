using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Job.Scripts
{
    internal abstract class StatProcessingShared
    {
        /// <summary>
        /// NWN stores HP as a field under each level object.
        /// The level for each level is 255. Ensure this value doesn't go over the field limit.
        /// </summary>
        protected static void ApplyHP(NWGameObject player, int hp)
        {
            if (hp > 255)
                hp = 255;

            NWNXCreature.SetMaxHitPointsByLevel(player, 1, hp);

            // If player's current HP is higher than their max, apply damage to put them back to their maximum.
            var currentHP = GetCurrentHitPoints(player);
            var maxHP = GetMaxHitPoints(player);
            if (currentHP > maxHP)
            {
                int amount = currentHP - maxHP;
                Effect damage = EffectDamage(amount);
                ApplyEffectToObject(DurationType.Instant, damage, player);
            }

        }
    }
}
