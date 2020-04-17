using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class WeaponProficienciesDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            LongswordProficiency(builder);
            KnucklesProficiency(builder);
            DaggerProficiency(builder);
            StaffProficiency(builder);
            RodProficiency(builder);
            LongbowProficiency(builder);

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

        private static void KnucklesProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Monk, PerkType.KnucklesProficiency)
                .Name("Knuckles Proficiency")
                .Description("Grants the ability to equip knuckles.")

                .AddPerkLevel(1)
                .Description("Grants the ability to equip tier 1 knuckles.")
                .Price(2)

                .AddPerkLevel(2)
                .Description("Grants the ability to equip tier 2 knuckles.")
                .Price(2)
                .RequirementSkill(SkillType.Knuckles, 10)

                .AddPerkLevel(3)
                .Description("Grants the ability to equip tier 3 knuckles.")
                .Price(2)
                .RequirementSkill(SkillType.Knuckles, 20)

                .AddPerkLevel(4)
                .Description("Grants the ability to equip tier 4 knuckles.")
                .Price(2)
                .RequirementSkill(SkillType.Knuckles, 30)

                .AddPerkLevel(5)
                .Description("Grants the ability to equip tier 5 knuckles.")
                .Price(2)
                .RequirementSkill(SkillType.Knuckles, 40)

                .AddPerkLevel(6)
                .Description("Grants the ability to equip tier 6 knuckles.")
                .Price(2)
                .RequirementSkill(SkillType.Knuckles, 50)

                .AddPerkLevel(7)
                .Description("Grants the ability to equip tier 7 knuckles.")
                .Price(2)
                .RequirementSkill(SkillType.Knuckles, 60)

                .AddPerkLevel(8)
                .Description("Grants the ability to equip tier 8 knuckles.")
                .Price(2)
                .RequirementSkill(SkillType.Knuckles, 70)

                .AddPerkLevel(9)
                .Description("Grants the ability to equip tier 9 knuckles.")
                .Price(2)
                .RequirementSkill(SkillType.Knuckles, 80)

                .AddPerkLevel(10)
                .Description("Grants the ability to equip tier 10 knuckles.")
                .Price(2)
                .RequirementSkill(SkillType.Knuckles, 90);
        }


        private static void DaggerProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Thief, PerkType.DaggerProficiency)
                .Name("Dagger Proficiency")
                .Description("Grants the ability to equip daggers.")

                .AddPerkLevel(1)
                .Description("Grants the ability to equip tier 1 daggers.")
                .Price(2)

                .AddPerkLevel(2)
                .Description("Grants the ability to equip tier 2 daggers.")
                .Price(2)
                .RequirementSkill(SkillType.Dagger, 10)

                .AddPerkLevel(3)
                .Description("Grants the ability to equip tier 3 daggers.")
                .Price(2)
                .RequirementSkill(SkillType.Dagger, 20)

                .AddPerkLevel(4)
                .Description("Grants the ability to equip tier 4 daggers.")
                .Price(2)
                .RequirementSkill(SkillType.Dagger, 30)

                .AddPerkLevel(5)
                .Description("Grants the ability to equip tier 5 daggers.")
                .Price(2)
                .RequirementSkill(SkillType.Dagger, 40)

                .AddPerkLevel(6)
                .Description("Grants the ability to equip tier 6 daggers.")
                .Price(2)
                .RequirementSkill(SkillType.Dagger, 50)

                .AddPerkLevel(7)
                .Description("Grants the ability to equip tier 7 daggers.")
                .Price(2)
                .RequirementSkill(SkillType.Dagger, 60)

                .AddPerkLevel(8)
                .Description("Grants the ability to equip tier 8 daggers.")
                .Price(2)
                .RequirementSkill(SkillType.Dagger, 70)

                .AddPerkLevel(9)
                .Description("Grants the ability to equip tier 9 daggers.")
                .Price(2)
                .RequirementSkill(SkillType.Dagger, 80)

                .AddPerkLevel(10)
                .Description("Grants the ability to equip tier 10 daggers.")
                .Price(2)
                .RequirementSkill(SkillType.Dagger, 90);
        }


        private static void StaffProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.BlackMage, PerkType.StaffProficiency)
                .Name("Staff Proficiency")
                .Description("Grants the ability to equip staves.")

                .AddPerkLevel(1)
                .Description("Grants the ability to equip tier 1 staves.")
                .Price(2)

                .AddPerkLevel(2)
                .Description("Grants the ability to equip tier 2 staves.")
                .Price(2)
                .RequirementSkill(SkillType.Staff, 10)

                .AddPerkLevel(3)
                .Description("Grants the ability to equip tier 3 staves.")
                .Price(2)
                .RequirementSkill(SkillType.Staff, 20)

                .AddPerkLevel(4)
                .Description("Grants the ability to equip tier 4 staves.")
                .Price(2)
                .RequirementSkill(SkillType.Staff, 30)

                .AddPerkLevel(5)
                .Description("Grants the ability to equip tier 5 staves.")
                .Price(2)
                .RequirementSkill(SkillType.Staff, 40)

                .AddPerkLevel(6)
                .Description("Grants the ability to equip tier 6 staves.")
                .Price(2)
                .RequirementSkill(SkillType.Staff, 50)

                .AddPerkLevel(7)
                .Description("Grants the ability to equip tier 7 staves.")
                .Price(2)
                .RequirementSkill(SkillType.Staff, 60)

                .AddPerkLevel(8)
                .Description("Grants the ability to equip tier 8 staves.")
                .Price(2)
                .RequirementSkill(SkillType.Staff, 70)

                .AddPerkLevel(9)
                .Description("Grants the ability to equip tier 9 staves.")
                .Price(2)
                .RequirementSkill(SkillType.Staff, 80)

                .AddPerkLevel(10)
                .Description("Grants the ability to equip tier 10 staves.")
                .Price(2)
                .RequirementSkill(SkillType.Staff, 90);
        }


        private static void RodProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.WhiteMage, PerkType.RodProficiency)
                .Name("Rod Proficiency")
                .Description("Grants the ability to equip rods.")

                .AddPerkLevel(1)
                .Description("Grants the ability to equip tier 1 rods.")
                .Price(2)

                .AddPerkLevel(2)
                .Description("Grants the ability to equip tier 2 rods.")
                .Price(2)
                .RequirementSkill(SkillType.Rod, 10)

                .AddPerkLevel(3)
                .Description("Grants the ability to equip tier 3 rods.")
                .Price(2)
                .RequirementSkill(SkillType.Rod, 20)

                .AddPerkLevel(4)
                .Description("Grants the ability to equip tier 4 rods.")
                .Price(2)
                .RequirementSkill(SkillType.Rod, 30)

                .AddPerkLevel(5)
                .Description("Grants the ability to equip tier 5 rods.")
                .Price(2)
                .RequirementSkill(SkillType.Rod, 40)

                .AddPerkLevel(6)
                .Description("Grants the ability to equip tier 6 rods.")
                .Price(2)
                .RequirementSkill(SkillType.Rod, 50)

                .AddPerkLevel(7)
                .Description("Grants the ability to equip tier 7 rods.")
                .Price(2)
                .RequirementSkill(SkillType.Rod, 60)

                .AddPerkLevel(8)
                .Description("Grants the ability to equip tier 8 rods.")
                .Price(2)
                .RequirementSkill(SkillType.Rod, 70)

                .AddPerkLevel(9)
                .Description("Grants the ability to equip tier 9 rods.")
                .Price(2)
                .RequirementSkill(SkillType.Rod, 80)

                .AddPerkLevel(10)
                .Description("Grants the ability to equip tier 10 rods.")
                .Price(2)
                .RequirementSkill(SkillType.Rod, 90);
        }


        private static void LongbowProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Ranger, PerkType.LongbowProficiency)
                .Name("Longbow Proficiency")
                .Description("Grants the ability to equip longbows.")

                .AddPerkLevel(1)
                .Description("Grants the ability to equip tier 1 longbows.")
                .Price(2)

                .AddPerkLevel(2)
                .Description("Grants the ability to equip tier 2 longbows.")
                .Price(2)
                .RequirementSkill(SkillType.Longbow, 10)

                .AddPerkLevel(3)
                .Description("Grants the ability to equip tier 3 longbows.")
                .Price(2)
                .RequirementSkill(SkillType.Longbow, 20)

                .AddPerkLevel(4)
                .Description("Grants the ability to equip tier 4 longbows.")
                .Price(2)
                .RequirementSkill(SkillType.Longbow, 30)

                .AddPerkLevel(5)
                .Description("Grants the ability to equip tier 5 longbows.")
                .Price(2)
                .RequirementSkill(SkillType.Longbow, 40)

                .AddPerkLevel(6)
                .Description("Grants the ability to equip tier 6 longbows.")
                .Price(2)
                .RequirementSkill(SkillType.Longbow, 50)

                .AddPerkLevel(7)
                .Description("Grants the ability to equip tier 7 longbows.")
                .Price(2)
                .RequirementSkill(SkillType.Longbow, 60)

                .AddPerkLevel(8)
                .Description("Grants the ability to equip tier 8 longbows.")
                .Price(2)
                .RequirementSkill(SkillType.Longbow, 70)

                .AddPerkLevel(9)
                .Description("Grants the ability to equip tier 9 longbows.")
                .Price(2)
                .RequirementSkill(SkillType.Longbow, 80)

                .AddPerkLevel(10)
                .Description("Grants the ability to equip tier 10 longbows.")
                .Price(2)
                .RequirementSkill(SkillType.Longbow, 90);
        }
    }
}
