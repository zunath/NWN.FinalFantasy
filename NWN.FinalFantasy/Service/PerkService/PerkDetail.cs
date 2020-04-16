using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Service.PerkService
{
    public delegate void PerkImpactAction(uint activator, uint target, int effectivePerkLevel);
    public class PerkDetail
    {
        public PerkCategoryType Category { get; set; }
        public PerkType Type { get; set; }
        public RecastGroup RecastGroup { get; set; }
        public PerkActivationType ActivationType { get; set; }
        public VisualEffect ActivationVisualEffect { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float ActivationDelay { get; set; }
        public float RecastDelay { get; set; }
        public bool IsActive { get; set; }
        public PerkImpactAction ImpactAction { get; set; }

        public Dictionary<int, PerkLevel> PerkLevels { get; set; }

        public PerkDetail()
        {
            ActivationVisualEffect = VisualEffect.None;
            PerkLevels = new Dictionary<int, PerkLevel>();
        }
    }
}
