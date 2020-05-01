using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature
{
    public static class NaturalRegeneration
    {
        private const int BaseNaturalHPRegeneration = 1;
        private const int BaseNaturalMPRegeneration = 1;
        private const int BaseNaturalStaminaRegeneration = 1;
        private const int NumberRequiredTicks = 5; // 5 ticks * 6 seconds = 30 seconds

        /// <summary>
        /// Handles processing natural HP, MP, and Stamina regeneration.
        /// </summary>
        [NWNEventHandler("interval_pc_6s")]
        public static void HandleNaturalRegeneration()
        {
            var player = OBJECT_SELF;
            var playerId = GetObjectUUID(player);
            var dbPlayer = DB.Get<Player>(playerId);
            dbPlayer.RegenerationTick++;

            if (dbPlayer.RegenerationTick >= NumberRequiredTicks)
            {
                var conModifier = GetAbilityModifier(AbilityType.Constitution, player);
                var chaModifier = GetAbilityModifier(AbilityType.Charisma, player);
                var strModifier = GetAbilityModifier(AbilityType.Strength, player);
                var hpAmount = BaseNaturalHPRegeneration + (conModifier > 0 ? conModifier : 0);
                var mpAmount = BaseNaturalMPRegeneration + (chaModifier > 0 ? chaModifier : 0);
                var staminaAmount = BaseNaturalStaminaRegeneration + (strModifier > 0 ? strModifier : 0);

                Stat.RestoreMP(player, dbPlayer, mpAmount);
                Stat.RestoreStamina(player, dbPlayer, staminaAmount);

                if (hpAmount > 0 && GetCurrentHitPoints(player) < GetMaxHitPoints(player))
                {
                    ApplyEffectToObject(DurationType.Instant, EffectHeal(hpAmount), player);
                }

                dbPlayer.RegenerationTick = 0;
            }

            DB.Set(playerId, dbPlayer);
        }
    }
}
