using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Service.AbilityService
{
    public delegate void AbilityImpactAction(uint activator, uint target, int effectivePerkLevel);
    public delegate float AbilityActivationDelayAction(uint activator, uint target);
    public delegate float AbilityRecastDelayAction(uint activator);

    public class AbilityDetail
    {
        public string Name { get; set; }
        public AbilityImpactAction ImpactAction { get; set; }
        public AbilityActivationDelayAction ActivationDelay { get; set; }
        public AbilityRecastDelayAction RecastDelay { get; set; }
        public List<IAbilityActivationRequirement> Requirements { get; set; }
        public VisualEffect ActivationVisualEffect { get; set; }
        public RecastGroup RecastGroup { get; set; }
        public AbilityActivationType ActivationType { get; set; }
        public PerkType EffectiveLevelPerkType { get; set; }

        public AbilityDetail()
        {
            ActivationVisualEffect = VisualEffect.None;
            Requirements = new List<IAbilityActivationRequirement>();
        }
    }
}
