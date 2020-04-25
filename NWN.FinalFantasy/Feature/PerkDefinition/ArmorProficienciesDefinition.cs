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
                .RequirementSkill(SkillType.HeavyArmor, 40);
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
                .RequirementSkill(SkillType.LightArmor, 40);
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
                .RequirementSkill(SkillType.MysticArmor, 40);
        }
    }

}
