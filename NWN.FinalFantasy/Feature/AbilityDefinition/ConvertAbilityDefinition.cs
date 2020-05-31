using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Player = NWN.FinalFantasy.Entity.Player;
using Random = System.Random;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class ConvertAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            Convert(builder);

            return builder.Build();
        }

        private static void Convert(AbilityBuilder builder)
        {
            builder.Create(Feat.Convert, PerkType.Convert)
                .Name("Convert")
                .HasRecastDelay(RecastGroup.Convert, 600f)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (!GetIsPC(activator) || GetIsDM(activator))
                    {
                        return "Only players may use this ability.";
                    }

                    var playerId = GetObjectUUID(activator);
                    var dbPlayer = DB.Get<Player>(playerId);

                    if (dbPlayer.MP <= 0)
                    {
                        return "Your MP is too low to convert.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    var playerId = GetObjectUUID(activator);
                    var dbPlayer = DB.Get<Player>(playerId);

                    var currentHP = GetCurrentHitPoints(activator);
                    var currentMP = dbPlayer.MP;

                    // Set MP to 0 then do a restore by the player's HP.
                    dbPlayer.MP = 0;
                    Stat.RestoreMP(activator, dbPlayer, currentHP);
                    DB.Set(playerId, dbPlayer);
                    
                    // Current HP is higher than MP. Deal damage.
                    if (currentHP > currentMP)
                    {
                        var damageAmount = currentHP - currentMP;
                        ApplyEffectToObject(DurationType.Instant, EffectDamage(damageAmount), activator);
                    }
                    // Current HP is lower than MP. Heal damage.
                    else if (currentHP < currentMP)
                    {
                        var recoverAmount = currentMP - currentHP;
                        ApplyEffectToObject(DurationType.Instant, EffectHeal(recoverAmount), activator);
                    }

                    ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(VisualEffect.Vfx_Imp_Good_Help), activator);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 5);
                    Enmity.ModifyEnmityOnAll(activator, 10);
                });
        }
    }
}
