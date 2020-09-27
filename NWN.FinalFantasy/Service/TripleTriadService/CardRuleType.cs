using System;

namespace NWN.FinalFantasy.Service.TripleTriadService
{
    [Flags]
    public enum CardRuleType
    {
        None = 0,
        Open = 1,
        Same = 2,
        SameWall = 4,
        Elemental = 8,
    }
}
