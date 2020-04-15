using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;

namespace NWN.FinalFantasy.Service.PerkService
{
    public class PerkLevel
    {
        public int Price { get; set; }
        public string Description { get; set; }
        public List<Feat> GrantedFeats { get; set; }
        public List<IPerkPurchaseRequirement> PurchaseRequirements { get; set; }
        public List<IPerkActivationRequirement> EffectiveLevelRequirements { get; set; }
        public List<IPerkActivationRequirement> ActivationRequirements { get; set; }

        public PerkLevel()
        {
            GrantedFeats = new List<Feat>();
            PurchaseRequirements = new List<IPerkPurchaseRequirement>();
            EffectiveLevelRequirements = new List<IPerkActivationRequirement>();
            ActivationRequirements = new List<IPerkActivationRequirement>();
        }
    }
}
