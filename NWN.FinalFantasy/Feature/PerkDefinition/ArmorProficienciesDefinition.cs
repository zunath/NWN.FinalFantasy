using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class ArmorProficienciesDefinition: IPerkListDefinition
    {

        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            HeavyArmorProficiency(builder);
            LightArmorProficiency(builder);
            MysticArmorProficiency(builder);

            return builder.Build();
        }

        private static void HeavyArmorProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.General, PerkType.HeavyArmorProficiency)
                .Name("Heavy Armor Proficiency")
                .Description("Grants the ability to equip heavy armor.")

                .AddPerkLevel(1)
                .Description("Grants the ability to equip tier 1 heavy armor.")
                .Price(2)

                .AddPerkLevel(2)
                .Description("Grants the ability to equip tier 2 heavy armor.")
                .Price(2)
                .RequirementSkill(SkillType.HeavyArmor, 10)

                .AddPerkLevel(3)
                .Description("Grants the ability to equip tier 3 heavy armor.")
                .Price(2)
                .RequirementSkill(SkillType.HeavyArmor, 20)

                .AddPerkLevel(4)
                .Description("Grants the ability to equip tier 4 heavy armor.")
                .Price(2)
                .RequirementSkill(SkillType.HeavyArmor, 30)

                .AddPerkLevel(5)
                .Description("Grants the ability to equip tier 5 heavy armor.")
                .Price(2)
                .RequirementSkill(SkillType.HeavyArmor, 40)

                .AddPerkLevel(6)
                .Description("Grants the ability to equip tier 6 heavy armor.")
                .Price(2)
                .RequirementSkill(SkillType.HeavyArmor, 50)

                .AddPerkLevel(7)
                .Description("Grants the ability to equip tier 7 heavy armor.")
                .Price(2)
                .RequirementSkill(SkillType.HeavyArmor, 60)

                .AddPerkLevel(8)
                .Description("Grants the ability to equip tier 8 heavy armor.")
                .Price(2)
                .RequirementSkill(SkillType.HeavyArmor, 70)

                .AddPerkLevel(9)
                .Description("Grants the ability to equip tier 9 heavy armor.")
                .Price(2)
                .RequirementSkill(SkillType.HeavyArmor, 80)

                .AddPerkLevel(10)
                .Description("Grants the ability to equip tier 10 heavy armor.")
                .Price(2)
                .RequirementSkill(SkillType.HeavyArmor, 90);
        }

        private static void LightArmorProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.General, PerkType.LightArmorProficiency)
                .Name("Light Armor Proficiency")
                .Description("Grants the ability to equip light armor.")

                .AddPerkLevel(1)
                .Description("Grants the ability to equip tier 1 light armor.")
                .Price(2)

                .AddPerkLevel(2)
                .Description("Grants the ability to equip tier 2 light armor.")
                .Price(2)
                .RequirementSkill(SkillType.LightArmor, 10)

                .AddPerkLevel(3)
                .Description("Grants the ability to equip tier 3 light armor.")
                .Price(2)
                .RequirementSkill(SkillType.LightArmor, 20)

                .AddPerkLevel(4)
                .Description("Grants the ability to equip tier 4 light armor.")
                .Price(2)
                .RequirementSkill(SkillType.LightArmor, 30)

                .AddPerkLevel(5)
                .Description("Grants the ability to equip tier 5 light armor.")
                .Price(2)
                .RequirementSkill(SkillType.LightArmor, 40)

                .AddPerkLevel(6)
                .Description("Grants the ability to equip tier 6 light armor.")
                .Price(2)
                .RequirementSkill(SkillType.LightArmor, 50)

                .AddPerkLevel(7)
                .Description("Grants the ability to equip tier 7 light armor.")
                .Price(2)
                .RequirementSkill(SkillType.LightArmor, 60)

                .AddPerkLevel(8)
                .Description("Grants the ability to equip tier 8 light armor.")
                .Price(2)
                .RequirementSkill(SkillType.LightArmor, 70)

                .AddPerkLevel(9)
                .Description("Grants the ability to equip tier 9 light armor.")
                .Price(2)
                .RequirementSkill(SkillType.LightArmor, 80)

                .AddPerkLevel(10)
                .Description("Grants the ability to equip tier 10 light armor.")
                .Price(2)
                .RequirementSkill(SkillType.LightArmor, 90);
        }

        private static void MysticArmorProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.General, PerkType.MysticArmorProficiency)
                .Name("Mystic Armor Proficiency")
                .Description("Grants the ability to equip mystic armor.")

                .AddPerkLevel(1)
                .Description("Grants the ability to equip tier 1 mystic armor.")
                .Price(2)

                .AddPerkLevel(2)
                .Description("Grants the ability to equip tier 2 mystic armor.")
                .Price(2)
                .RequirementSkill(SkillType.MysticArmor, 10)

                .AddPerkLevel(3)
                .Description("Grants the ability to equip tier 3 mystic armor.")
                .Price(2)
                .RequirementSkill(SkillType.MysticArmor, 20)

                .AddPerkLevel(4)
                .Description("Grants the ability to equip tier 4 mystic armor.")
                .Price(2)
                .RequirementSkill(SkillType.MysticArmor, 30)

                .AddPerkLevel(5)
                .Description("Grants the ability to equip tier 5 mystic armor.")
                .Price(2)
                .RequirementSkill(SkillType.MysticArmor, 40)

                .AddPerkLevel(6)
                .Description("Grants the ability to equip tier 6 mystic armor.")
                .Price(2)
                .RequirementSkill(SkillType.MysticArmor, 50)

                .AddPerkLevel(7)
                .Description("Grants the ability to equip tier 7 mystic armor.")
                .Price(2)
                .RequirementSkill(SkillType.MysticArmor, 60)

                .AddPerkLevel(8)
                .Description("Grants the ability to equip tier 8 mystic armor.")
                .Price(2)
                .RequirementSkill(SkillType.MysticArmor, 70)

                .AddPerkLevel(9)
                .Description("Grants the ability to equip tier 9 mystic armor.")
                .Price(2)
                .RequirementSkill(SkillType.MysticArmor, 80)

                .AddPerkLevel(10)
                .Description("Grants the ability to equip tier 10 mystic armor.")
                .Price(2)
                .RequirementSkill(SkillType.MysticArmor, 90);
        }
    }

}
