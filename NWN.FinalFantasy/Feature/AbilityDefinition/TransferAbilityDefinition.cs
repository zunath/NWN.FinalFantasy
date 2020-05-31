using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.AbilityService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class TransferAbilityDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();
            TransferMP1(builder);
            TransferMP2(builder);
            TransferStamina1(builder);
            TransferStamina2(builder);

            return builder.Build();
        }

        private static void TransferMP1(AbilityBuilder builder)
        {
            builder.Create(Feat.TransferMP1, PerkType.TransferMP)
                .Name("Transfer MP I")
                .HasRecastDelay(RecastGroup.Transfer, 60f)
                .RequirementStamina(20)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(4f)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (!GetIsPC(target) || GetIsDM(target))
                    {
                        return "Only players may be targeted with this ability.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    const int BaseTransferAmount = 10;

                    var playerId = GetObjectUUID(target);
                    var dbPlayer = DB.Get<Player>(playerId);
                    Stat.RestoreMP(target, dbPlayer, BaseTransferAmount);
                    DB.Set(playerId, dbPlayer);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 2);
                    Enmity.ModifyEnmityOnAll(activator, 6);
                });
        }

        private static void TransferMP2(AbilityBuilder builder)
        {
            builder.Create(Feat.TransferMP2, PerkType.TransferMP)
                .Name("Transfer MP II")
                .HasRecastDelay(RecastGroup.Transfer, 60f)
                .RequirementStamina(30)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(4f)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (!GetIsPC(target) || GetIsDM(target))
                    {
                        return "Only players may be targeted with this ability.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    const int BaseTransferAmount = 20;

                    var playerId = GetObjectUUID(target);
                    var dbPlayer = DB.Get<Player>(playerId);
                    Stat.RestoreMP(target, dbPlayer, BaseTransferAmount);
                    DB.Set(playerId, dbPlayer);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 2);
                    Enmity.ModifyEnmityOnAll(activator, 8);
                });
        }

        private static void TransferStamina1(AbilityBuilder builder)
        {
            builder.Create(Feat.TransferStamina1, PerkType.TransferStamina)
                .Name("Transfer Stamina I")
                .HasRecastDelay(RecastGroup.Transfer, 60f)
                .RequirementMP(20)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(4f)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (!GetIsPC(target) || GetIsDM(target))
                    {
                        return "Only players may be targeted with this ability.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    const int BaseTransferAmount = 10;

                    var playerId = GetObjectUUID(target);
                    var dbPlayer = DB.Get<Player>(playerId);
                    Stat.RestoreStamina(target, dbPlayer, BaseTransferAmount);
                    DB.Set(playerId, dbPlayer);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 2);
                    Enmity.ModifyEnmityOnAll(activator, 6);
                });
        }

        private static void TransferStamina2(AbilityBuilder builder)
        {
            builder.Create(Feat.TransferStamina2, PerkType.TransferStamina)
                .Name("Transfer Stamina II")
                .HasRecastDelay(RecastGroup.Transfer, 60f)
                .RequirementMP(30)
                .UsesActivationType(AbilityActivationType.Casted)
                .HasActivationDelay(4f)
                .HasCustomValidation((activator, target, level) =>
                {
                    if (!GetIsPC(target) || GetIsDM(target))
                    {
                        return "Only players may be targeted with this ability.";
                    }

                    return string.Empty;
                })
                .HasImpactAction((activator, target, level) =>
                {
                    const int BaseTransferAmount = 20;

                    var playerId = GetObjectUUID(target);
                    var dbPlayer = DB.Get<Player>(playerId);
                    Stat.RestoreStamina(target, dbPlayer, BaseTransferAmount);
                    DB.Set(playerId, dbPlayer);

                    CombatPoint.AddCombatPointToAllTagged(activator, SkillType.RedMagic, 2);
                    Enmity.ModifyEnmityOnAll(activator, 8);
                });
        }
    }
}
