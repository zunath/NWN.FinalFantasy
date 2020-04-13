using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;

namespace NWN.FinalFantasy.Service.SpawnService
{
    public class SpawnObject
    {
        public ObjectType Type { get; set; }
        public string Resref { get; set; }
        public int Weight { get; set; }
        
        public List<DayOfWeek> RealWorldDayOfWeekRestriction { get; set; }
        public TimeSpan? RealWorldStartRestriction { get; set; }
        public TimeSpan? RealWorldEndRestriction { get; set; }

        public int GameHourStartRestriction { get; set; }
        public int GameHourEndRestriction { get; set; }

        public SpawnObject()
        {
            RealWorldDayOfWeekRestriction = new List<DayOfWeek>();
            GameHourStartRestriction = -1;
            GameHourEndRestriction = -1;
        }
    }
}
