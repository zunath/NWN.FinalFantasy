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
        [NWNEventHandler("mod_heartbeat")]
        public static void HandleNaturalRegeneration()
        {
            for(var player = GetFirstPC(); GetIsObjectValid(player); player = GetNextPC())
            {
                var playerId = GetObjectUUID(player);
                var dbPlayer = DB.Get<Player>(playerId);
                dbPlayer.RegenerationTick++;

                if(dbPlayer.RegenerationTick >= NumberRequiredTicks)
                {
                    var conModifier = GetAbilityModifier(Ability.Constitution, player);
                    var chaModifier = GetAbilityModifier(Ability.Charisma, player);
                    var strModifier = GetAbilityModifier(Ability.Strength, player);
                    var hpAmount = BaseNaturalHPRegeneration + (conModifier > 0 ? conModifier : 0);
                    var mpAmount = BaseNaturalMPRegeneration + (chaModifier > 0 ? chaModifier : 0);
                    var staminaAmount = BaseNaturalStaminaRegeneration + (strModifier > 0 ? strModifier : 0);

                    Stat.RestoreMP(dbPlayer, mpAmount);
                    Stat.RestoreStamina(dbPlayer, staminaAmount);

                    if (hpAmount > 0)
                    {
                        ApplyEffectToObject(DurationType.Instant, EffectHeal(hpAmount), player);
                    }

                    dbPlayer.RegenerationTick = 0;
                }

                DB.Set(playerId, dbPlayer);
            }
        }
    }
}
