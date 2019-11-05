using System;

namespace NWN.FinalFantasy.Data.Entity
{
    public class QuestProgress: EntityBase
    {
        public string QuestID { get; set; }
        public int CurrentState { get; set; }
        public int TimesCompleted { get; set; }
    }
}
