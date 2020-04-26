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
            Invincible(builder);
            ShieldBash(builder);
            Provoke(builder);
            Cleave(builder);
            SpikedDefense(builder);
            ShieldProficiency(builder);
            Cover(builder);
            Defender(builder);
            Ironclad(builder);
            CircleOfScorn(builder);
            
            return builder.Build();
        }

        private static void Invincible(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.Invincible)
                .Name("Invicible")
                .Description("Grants invincibility for 30 seconds and increases enmity toward all nearby targets.")
                
                .AddPerkLevel()
                .Description("Grants the Invincibility ability.")
                .RequirementSkill(SkillType.Chivalry, 50)
                .RequirementSkill(SkillType.HeavyArmor, 50)
                .RequirementSkill(SkillType.Longsword, 50)
                .Price(15);
        }

        private static void ShieldBash(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.ShieldBash)
                .Name("Shield Bash")
                .Description("Deals 1d4 damage and stuns your target for a short period of time.")

                .AddPerkLevel()
                .Description("Grants the Shield Bash ability.")
                .RequirementSkill(SkillType.Chivalry, 5)
                .Price(3);
        }

        private static void Provoke(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.Provoke)
                .Name("Provoke")
                .Description("Goads your target into attacking you.")

                .AddPerkLevel()
                .Description("Goads a single enemy into attacking you.")
                .RequirementSkill(SkillType.Chivalry, 10)
                .Price(3)

                .AddPerkLevel()
                .Description("Goads a group of enemies into attacking you.")
                .RequirementSkill(SkillType.Chivalry, 25)
                .Price(4);
        }

        private static void Cleave(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.Cleave)
                .Name("Cleave")
                .Description("If the character kills an opponent, he gets a free attack against any opponent who is within melee weapon range.")

                .AddPerkLevel()
                .Description("Grants the Cleave ability.")
                .RequirementSkill(SkillType.Chivalry, 10)
                .Price(3);
        }

        private static void SpikedDefense(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.SpikedDefense)
                .Name("Spiked Defense")
                .Description("Grants a temporary damage shield to you.")

                .AddPerkLevel()
                .Description("Grants the Spiked Defense ability.")
                .RequirementSkill(SkillType.Chivalry, 15)
                .Price(3);
        }

        private static void ShieldProficiency(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.ShieldProficiency)
                .Name("Shield Proficiency")
                .Description("Increases your damage reduction when equipped with a shield.")

                .AddPerkLevel()
                .Description("Increases damage reduction by 2% when equipped with a shield.")
                .RequirementSkill(SkillType.HeavyArmor, 5)
                .RequirementSkill(SkillType.Chivalry, 10)
                .Price(3)

                .AddPerkLevel()
                .Description("Increases damage reduction by 4% when equipped with a shield.")
                .RequirementSkill(SkillType.HeavyArmor, 10)
                .RequirementSkill(SkillType.Chivalry, 20)
                .Price(3)

                .AddPerkLevel()
                .Description("Increases damage reduction by 6% when equipped with a shield.")
                .RequirementSkill(SkillType.HeavyArmor, 20)
                .RequirementSkill(SkillType.Chivalry, 30)
                .Price(3)

                .AddPerkLevel()
                .Description("Increases damage reduction by 8% when equipped with a shield.")
                .RequirementSkill(SkillType.HeavyArmor, 30)
                .RequirementSkill(SkillType.Chivalry, 40)
                .Price(3)

                .AddPerkLevel()
                .Description("Increases damage reduction by 10% when equipped with a shield.")
                .RequirementSkill(SkillType.HeavyArmor, 40)
                .RequirementSkill(SkillType.Chivalry, 50)
                .Price(3);
        }

        private static void Cover(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.Cover)
                .Name("Cover")
                .Description("Protects an ally from damage and you receive it instead.")

                .AddPerkLevel()
                .Description("10% of damage is blocked and you receive it instead.")
                .RequirementSkill(SkillType.Chivalry, 10)
                .RequirementSkill(SkillType.Longsword, 5)
                .Price(4)

                .AddPerkLevel()
                .Description("15% of damage is blocked and you receive it instead.")
                .RequirementSkill(SkillType.Chivalry, 20)
                .RequirementSkill(SkillType.Longsword, 10)
                .Price(4)

                .AddPerkLevel()
                .Description("20% of damage is blocked and you receive it instead.")
                .RequirementSkill(SkillType.Chivalry, 30)
                .RequirementSkill(SkillType.Longsword, 15)
                .Price(4)

                .AddPerkLevel()
                .Description("25% of damage is blocked and you receive it instead.")
                .RequirementSkill(SkillType.Chivalry, 40)
                .RequirementSkill(SkillType.Longsword, 20)
                .Price(4);
        }

        private static void Defender(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.Defender)
                .Name("Defender")
                .Description("Increases the damage resistance of your party members within range.")

                .AddPerkLevel()
                .Description("Increases the damage resistance of your party members within range by 3.")
                .RequirementSkill(SkillType.Chivalry, 15)
                .RequirementSkill(SkillType.Longsword, 20)
                .Price(3)

                .AddPerkLevel()
                .Description("Increases the damage resistance of your party members within range by 5.")
                .RequirementSkill(SkillType.Chivalry, 30)
                .RequirementSkill(SkillType.Longsword, 40)
                .Price(3)

                .AddPerkLevel()
                .Description("Increases the damage resistance of your party members within range by 7.")
                .RequirementSkill(SkillType.Chivalry, 45)
                .RequirementSkill(SkillType.Longsword, 50)
                .Price(3);
        }

        private static void Ironclad(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.Ironclad)
                .Name("Ironclad")
                .Description("Increases your damage resistance.")

                .AddPerkLevel()
                .Description("Increases your damage resistance by 4.")
                .RequirementSkill(SkillType.Chivalry, 10)
                .RequirementSkill(SkillType.Longsword, 15)
                .Price(3)

                .AddPerkLevel()
                .Description("Increases your damage resistance by 6.")
                .RequirementSkill(SkillType.Chivalry, 20)
                .RequirementSkill(SkillType.Longsword, 25)
                .Price(3)

                .AddPerkLevel()
                .Description("Increases your damage resistance by 8.")
                .RequirementSkill(SkillType.Chivalry, 40)
                .RequirementSkill(SkillType.Longsword, 45)
                .Price(3);
        }

        private static void CircleOfScorn(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Knight, PerkType.CircleOfScorn)
                .Name("Circle of Scorn")
                .Description("Delivers an attack which damages all nearby enemies.")

                .AddPerkLevel()
                .Description("Delivers an attack which damages all nearby enemies")
                .RequirementSkill(SkillType.Chivalry, 25)
                .RequirementSkill(SkillType.Longsword, 25)
                .Price(6)

                .AddPerkLevel()
                .Description("Delivers an attack which damages all nearby enemies")
                .RequirementSkill(SkillType.Chivalry, 50)
                .RequirementSkill(SkillType.Longsword, 50)
                .Price(8);
        }
    }
}
