using System;

namespace NWN.FinalFantasy.Data.Entity
{
    public class MapPin: EntityBase
    {
        public string AreaTag { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public string Text { get; set; }
    }
}
