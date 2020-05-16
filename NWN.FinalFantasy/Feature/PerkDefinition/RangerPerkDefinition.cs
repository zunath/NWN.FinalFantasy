using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class RangerPerkDefinition: IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            EagleEyeShot(builder);

            return builder.Build();
        }

        private static void EagleEyeShot(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Ranger, PerkType.EagleEyeShot)
                .Name("Eagle Eye Shot")
                .Description("Your next ranged attack will deal 5 times normal damage.")
                
                .AddPerkLevel()
                .Description("Grants the Eagle Eye Shot ability.")
                .RequirementSkill(SkillType.Archery, 50)
                .RequirementSkill(SkillType.Longbow, 50)
                .RequirementSkill(SkillType.LightArmor, 50)
                .RequirementQuest("a_rangers_test")
                .Price(15)
                .GrantsFeat(Feat.EagleEyeShot);
        }
    }
}
