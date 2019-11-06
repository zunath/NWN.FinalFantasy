using System;
using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Data.Entity
{
    public class Cooldown: EntityBase
    {
        public DateTime DateUnlocked { get; set; }
        public Feat Feat { get; set; }
    }
}
