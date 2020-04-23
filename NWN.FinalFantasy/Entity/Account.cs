using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Entity
{
    public class Account: EntityBase
    {
        public Account()
        {
            Achievements = new Dictionary<AchievementType, DateTime>();
        }

        public override string KeyPrefix => "Account";

        public Dictionary<AchievementType, DateTime> Achievements { get; set; }
    }
}
