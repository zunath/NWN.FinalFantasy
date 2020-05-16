using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Feature.PerkDefinition
{
    public class SpecialistPerkDefinition : IPerkListDefinition
    {
        public Dictionary<PerkType, PerkDetail> BuildPerks()
        {
            var builder = new PerkBuilder();
            LionHeart(builder);

            return builder.Build();
        }

        private static void LionHeart(PerkBuilder builder)
        {
            builder.Create(PerkCategoryType.Specialist, PerkType.LionHeart)
                .Name("Lion Heart")
                .Description("Grants +25 AC to all nearby party members for 30 seconds.")

                .AddPerkLevel()
                .Description("Grants the Lion Heart ability.")
                .RequirementSkill(SkillType.Swordplay, 50)
                .RequirementSkill(SkillType.Gunblade, 50)
                .RequirementSkill(SkillType.HeavyArmor, 50)
                .RequirementQuest("a_specialists_test")
                .Price(15)
                .GrantsFeat(Feat.LionHeart);
        }
    }
}