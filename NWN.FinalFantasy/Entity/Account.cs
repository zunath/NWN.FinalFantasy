using System;
using System.Collections.Generic;
using MessagePack;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Entity
{
    [MessagePackObject(true)]
    public class Account: EntityBase
    {
        public Account()
        {
            Achievements = new Dictionary<AchievementType, DateTime>();
            AchievementProgress = new AchievementProgress();
        }

        public override string KeyPrefix => "Account";

        public ulong TimesLoggedIn { get; set; }

        public Dictionary<AchievementType, DateTime> Achievements { get; set; }

        public AchievementProgress AchievementProgress { get; set; }
    }

    [MessagePackObject(true)]
    public class AchievementProgress
    {
        public ulong EnemiesKilled { get; set; }
        public ulong PerksLearned { get; set; }
        public ulong SkillsLearned { get; set; }
        public ulong QuestsCompleted { get; set; }
    }
}
