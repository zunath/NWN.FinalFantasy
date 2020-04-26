using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class NinjaPerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            DualWield(builder);

            return builder.Build();
        }

        private static void DualWield(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Ninja, PerkType.DualWield)
                .Name("Dual Wield")
                .Description("Grants the ability to equip a one-handed weapon in each hand and improves the effectiveness with each level.")

                .AddPerkLevel()
                .Description("Grants the ability to equip a one-handed weapon in each hand.")
                .Price(4)
                .RequirementSkill(SkillType.Katana, 10)
                .RequirementSkill(SkillType.Ninjitsu, 5)

                .AddPerkLevel()
                .Description("Grants the two-weapon fighting feat which reduces the -6/-10 penalty to -4/-8 when dual wielding.")
                .Price(4)
                .GrantsFeat(Feat.TwoWeaponFighting)
                .RequirementSkill(SkillType.Katana, 15)
                .RequirementSkill(SkillType.Ninjitsu, 10)

                .AddPerkLevel()
                .Description("Grants the ambidexterity feat which reduces the penalty of the off-hand weapon by 4 when dual wielding.")
                .Price(4)
                .GrantsFeat(Feat.Ambidexterity)
                .RequirementSkill(SkillType.Katana, 25)
                .RequirementSkill(SkillType.Ninjitsu, 15)

                .AddPerkLevel()
                .Description("Grants the improved two-weapon fighting feat which grants a second off-hand attack at a penalty of -5 attack roll.")
                .Price(4)
                .GrantsFeat(Feat.ImprovedTwoWeaponFighting)
                .RequirementSkill(SkillType.Katana, 35)
                .RequirementSkill(SkillType.Ninjitsu, 25);
        }
    }
}
