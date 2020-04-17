using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum.VisualEffect;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Service.PerkService
{
    public delegate void PerkImpactAction(uint activator, uint target, int effectivePerkLevel);
    public delegate void PerkTriggerEquippedUnequippedAction(uint player, uint item, PerkType perkType, int effectivePerkLevel);
    public delegate void PerkTriggerPurchasedRefundedAction(uint player, PerkType perkType, int effectivePerkLevel);

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
        public List<PerkTriggerEquippedUnequippedAction> EquippedTriggers { get; set; }
        public List<PerkTriggerEquippedUnequippedAction> UnequippedTriggers { get; set; }
        public List<PerkTriggerPurchasedRefundedAction> PurchasedTriggers { get; set; }
        public List<PerkTriggerPurchasedRefundedAction> RefundedTriggers { get; set; }

        public PerkDetail()
        {
            ActivationVisualEffect = VisualEffect.None;
            PerkLevels = new Dictionary<int, PerkLevel>();

            EquippedTriggers = new List<PerkTriggerEquippedUnequippedAction>();
            UnequippedTriggers = new List<PerkTriggerEquippedUnequippedAction>();
            PurchasedTriggers = new List<PerkTriggerPurchasedRefundedAction>();
            RefundedTriggers = new List<PerkTriggerPurchasedRefundedAction>();
        }
    }
}
