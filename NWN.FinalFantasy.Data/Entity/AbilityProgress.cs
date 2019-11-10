using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Data.Entity
{
    public class AbilityProgress: EntityBase
    {
        public Feat Feat { get; set; }
        public int AP { get; set; }
    }
}
