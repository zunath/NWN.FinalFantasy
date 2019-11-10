using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Data.Entity
{
    public class EquippedAbility: EntityBase
    {
        public Feat Feat { get; set; }
        public int Quantity { get; set; }
    }
}
