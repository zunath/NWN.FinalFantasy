using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Service.PerkService
{
    public class PerkDetail
    {
        public PerkCategoryType Category { get; set; }
        public PerkType Type { get; set; }
        public RecastGroup RecastGroup { get; set; }
        public PerkActivationType ActivationType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float ActivationDelay { get; set; }
        public bool IsActive { get; set; }

        public Dictionary<int, PerkLevel> PerkLevels { get; set; }

        public PerkDetail()
        {
            PerkLevels = new Dictionary<int, PerkLevel>();
        }
    }
}
