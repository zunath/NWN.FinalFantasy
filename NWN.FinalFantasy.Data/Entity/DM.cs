using NWN.FinalFantasy.Core.Enumerations;

namespace NWN.FinalFantasy.Data.Entity
{
    public class DM: EntityBase
    {
        public string Name { get; set; }
        public string CDKey { get; set; }
        public AuthorizationLevel Authorization { get; set; }
    }
}
