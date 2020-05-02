using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Service.AbilityService;

namespace NWN.FinalFantasy.Feature.AbilityDefinition
{
    public class TestAbilityListDefinition: IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities()
        {
            var builder = new AbilityBuilder();

            return builder.Build();
        }
    }
}
