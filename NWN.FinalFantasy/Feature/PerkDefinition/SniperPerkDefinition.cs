using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class SniperPerkDefinition : IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            EagleEyeShot(builder);
            SnapShot(builder);
            ArmShot(builder);
            EyeShot(builder);

            return builder.Build();
        }

        private static void EagleEyeShot(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Sniper, PerkType.EagleEyeShot)
                .Name("Eagle Eye Shot")
                .Description("Your next ranged attack will deal 5 times normal damage.")

                .AddPerkLevel()
                .Description("Grants the Eagle Eye Shot ability.")
                .RequirementSkill(SkillType.Rifle, 50)
                .RequirementSkill(SkillType.Marksmanship, 50)
                .RequirementSkill(SkillType.LightArmor, 50)
                .RequirementQuest("a_snipers_test")
                .Price(15)
                .GrantsFeat(Feat.EagleEyeShot);
        }
        private static void SnapShot(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Sniper, PerkType.SnapShot)
                .Name("Snap Shot")
                .Description("You take a free action shot.")

                .AddPerkLevel()
                .Description("Grants the Snap Shot ability.")
                .RequirementSkill(SkillType.Marksmanship, 5)
                .Price(3)
                .GrantsFeat(Feat.SnapShot);
        }
        private static void ArmShot(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Sniper, PerkType.SnapShot)
                .Name("Arm Shot")
                .Description("Your next ranged attack will decrease the enemies attack bonus.")

                .AddPerkLevel()
                .Description("Grants the Eye Shot ability.")
                .RequirementSkill(SkillType.Marksmanship, 10)
                .Price(3)
                .GrantsFeat(Feat.SnapShot);
        }
        private static void EyeShot(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Sniper, PerkType.EyeShot)
                .Name("Eye Shot")
                .Description("Your next ranged attack will deal 1.5 times normal damage and decrease the enemies attack bonus.")

                .AddPerkLevel()
                .Description("Grants the Eye Shot ability.")
                .RequirementSkill(SkillType.Marksmanship, 25)
                .Price(4)
                .GrantsFeat(Feat.SnapShot);
        }
    }
}