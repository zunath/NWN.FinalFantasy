using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Data.Entity
{
    public class QuestProgress: EntityBase
    {
        public class ItemProgress
        {
            public string Resref { get; set; }
            public int Remaining { get; set; }
        }

        public class KillProgress
        {
            public string NPCGroupID { get; set; }
            public int Remaining { get; set; }
        }

        public string QuestID { get; set; }
        public int CurrentState { get; set; }
        public int TimesCompleted { get; set; }

        public List<KillProgress> KillProgresses { get; set; } = new List<KillProgress>();
        public List<ItemProgress> ItemProgresses { get; set; } = new List<ItemProgress>();
    }
}
