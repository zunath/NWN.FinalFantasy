using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class KnightPerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            LongswordProficiency(builder);

            return builder.Build();
        }

        private static void LongswordProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.LongswordProficiency)
                .Name("Longsword Proficiency")
                .Description("Grants the ability to equip longswords.")
                
                .AddPerkLevel(1)
                .Description("Grants the ability to equip tier 1 longswords.")
                .Price(2)
                
                .AddPerkLevel(2)
                .Description("Grants the ability to equip tier 2 longswords.")
                .Price(2)
                .RequirementSkill(SkillType.Longsword, 10)
                
                .AddPerkLevel(3)
                .Description("Grants the ability to equip tier 3 longswords.")
                .Price(2)
                .RequirementSkill(SkillType.Longsword, 20)
                
                .AddPerkLevel(4)
                .Description("Grants the ability to equip tier 4 longswords.")
                .Price(2)
                .RequirementSkill(SkillType.Longsword, 30)
                
                .AddPerkLevel(5)
                .Description("Grants the ability to equip tier 5 longswords.")
                .Price(2)
                .RequirementSkill(SkillType.Longsword, 40)
                
                .AddPerkLevel(6)
                .Description("Grants the ability to equip tier 6 longswords.")
                .Price(2)
                .RequirementSkill(SkillType.Longsword, 50)
                
                .AddPerkLevel(7)
                .Description("Grants the ability to equip tier 7 longswords.")
                .Price(2)
                .RequirementSkill(SkillType.Longsword, 60)
                
                .AddPerkLevel(8)
                .Description("Grants the ability to equip tier 8 longswords.")
                .Price(2)
                .RequirementSkill(SkillType.Longsword, 70)
                
                .AddPerkLevel(9)
                .Description("Grants the ability to equip tier 9 longswords.")
                .Price(2)
                .RequirementSkill(SkillType.Longsword, 80)
                
                .AddPerkLevel(10)
                .Description("Grants the ability to equip tier 10 longswords.")
                .Price(2)
                .RequirementSkill(SkillType.Longsword, 90);
        }
    }
}
