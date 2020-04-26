using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class RedMagePerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            Manafont(builder);
            Protect(builder);
            TransferMP(builder);
            TransferStamina(builder);
            PiercingStab(builder);
            Blind(builder);
            RecoveryStab(builder);
            Convert(builder);
            Refresh(builder);
            RapierFinesse(builder);
            Jolt(builder);
            PoisonStab(builder);
            ShockSpikes(builder);
            DeliberateStab(builder);

            return builder.Build();
        }

        private static void Manafont(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.Manafont)
                .Name("Manafont")
                .Description("Spells may be cast for free for the next 30 seconds.")

                .AddPerkLevel()
                .Description("Grants the Manafont ability.")
                .RequirementSkill(SkillType.RedMagic, 50)
                .RequirementSkill(SkillType.Rapier, 50)
                .RequirementSkill(SkillType.MysticArmor, 50)
                .Price(15);
        }

        private static void Protect(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.Protect)
                .Name("Protect")
                .Description("");
        }

        private static void TransferMP(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.TransferMP)
                .Name("Transfer MP")
                .Description("");
        }

        private static void TransferStamina(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.TransferStamina)
                .Name("Transfer Stamina")
                .Description("");
        }

        private static void PiercingStab(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.PiercingStab)
                .Name("Piercing Stab")
                .Description("");
        }

        private static void Blind(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.Blind)
                .Name("Blind")
                .Description("");
        }

        private static void RecoveryStab(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.RecoveryStab)
                .Name("Recovery Stab")
                .Description("");
        }

        private static void Convert(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.Convert)
                .Name("Convert")
                .Description("");
        }

        private static void Refresh(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.Refresh)
                .Name("Refresh")
                .Description("");
        }

        private static void RapierFinesse(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.RapierFinesse)
                .Name("Rapier Finesse")
                .Description("");
        }

        private static void Jolt(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.Jolt)
                .Name("Jolt")
                .Description("");
        }

        private static void PoisonStab(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.PoisonStab)
                .Name("Poison Stab")
                .Description("");
        }

        private static void ShockSpikes(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.ShockSpikes)
                .Name("Shock Spikes")
                .Description("");
        }

        private static void DeliberateStab(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.RedMage, PerkType.DeliberateStab)
                .Name("Deliberate Stab")
                .Description("");
        }
    }
}
