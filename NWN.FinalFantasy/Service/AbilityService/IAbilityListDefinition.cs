using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;

namespace NWN.FinalFantasy.Service.AbilityService
{
    public interface IAbilityListDefinition
    {
        public Dictionary<Feat, AbilityDetail> BuildAbilities();
    }
}
