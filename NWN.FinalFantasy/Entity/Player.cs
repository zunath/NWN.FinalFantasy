﻿using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.AbilityService;

namespace NWN.FinalFantasy.Entity
{
    public class Player: EntityBase
    {
        public Player()
        {
            Settings = new PlayerSettings();
            BaseStats = new Dictionary<AbilityType, int>
            {
                {AbilityType.Constitution, 0},
                {AbilityType.Strength, 0},
                {AbilityType.Charisma, 0},
                {AbilityType.Dexterity, 0},
                {AbilityType.Intelligence, 0},
                {AbilityType.Wisdom, 0}
            };
            AdjustedStats = new Dictionary<AbilityType, float>
            {
                {AbilityType.Constitution, 0f},
                {AbilityType.Strength, 0f},
                {AbilityType.Charisma, 0f},
                {AbilityType.Dexterity, 0f},
                {AbilityType.Intelligence, 0f},
                {AbilityType.Wisdom, 0f}
            };
            MapPins = new Dictionary<string, List<MapPin>>();
            MapProgressions = new Dictionary<string, string>();
            RoleplayProgress = new RoleplayProgress();
            Skills = new Dictionary<SkillType, PlayerSkill>();
            Perks = new Dictionary<PerkType, int>();
            RecastTimes = new Dictionary<RecastGroup, DateTime>();
            Quests = new Dictionary<string, PlayerQuest>();
            UnlockedPerks = new Dictionary<PerkType, DateTime>();
            UnlockedRecipes = new Dictionary<RecipeType, DateTime>();
            SeedProgress = new SeedProgress();
        }

        public override string KeyPrefix => "Player";

        public int Version { get; set; }
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int MaxStamina { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Stamina { get; set; }
        public int BAB { get; set; }
        public string LocationAreaResref { get; set; }
        public float LocationX { get; set; }
        public float LocationY { get; set; }
        public float LocationZ { get; set; }
        public float LocationOrientation { get; set; }
        public float RespawnLocationX { get; set; }
        public float RespawnLocationY { get; set; }
        public float RespawnLocationZ { get; set; }
        public float RespawnLocationOrientation { get; set; }
        public string RespawnAreaResref { get; set; }
        public int UnallocatedXP { get; set; }
        public int UnallocatedSP { get; set; }
        public int TotalSPAcquired { get; set; }
        public int RegenerationTick { get; set; }
        public int XPDebt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DatePerkRefundAvailable { get; set; }

        public PlayerSettings Settings { get; set; }
        public Dictionary<AbilityType, int> BaseStats { get; set; }
        public Dictionary<AbilityType, float> AdjustedStats { get; set; }
        public RoleplayProgress RoleplayProgress { get; set; }
        public Dictionary<string, List<MapPin>> MapPins { get; set; }
        public Dictionary<string, string> MapProgressions { get; set; }
        public Dictionary<SkillType, PlayerSkill> Skills { get; set; }
        public Dictionary<PerkType, int> Perks { get; set; }
        public Dictionary<RecastGroup, DateTime> RecastTimes { get; set; }
        public Dictionary<string, PlayerQuest> Quests { get; set; }
        public Dictionary<PerkType, DateTime> UnlockedPerks { get; set; }
        public Dictionary<RecipeType, DateTime> UnlockedRecipes { get; set; }
        public SeedProgress SeedProgress { get; set; }
    }

    public class MapPin
    {
        public int Id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public string Note { get; set; }
    }

    public class RoleplayProgress
    {
        public int RPPoints { get; set; }
        public ulong TotalRPExpGained { get; set; }
        public ulong SpamMessageCount { get; set; }
    }

    public class PlayerSkill
    {
        public int Rank { get; set; }
        public int XP { get; set; }
        public bool IsLocked { get; set; }
    }

    public class PlayerQuest
    {
        public int CurrentState { get; set; }
        public int TimesCompleted { get; set; }
        public DateTime? DateLastCompleted { get; set; }

        public Dictionary<NPCGroupType, int> KillProgresses { get; set; } = new Dictionary<NPCGroupType, int>();
        public Dictionary<string, int> ItemProgresses { get; set; } = new Dictionary<string, int>();
    }

    public class PlayerSettings
    {
        public int? BattleThemeId { get; set; }
        public bool DisplayAchievementNotification { get; set; }

        public PlayerSettings()
        {
            DisplayAchievementNotification = true;
        }
    }

    public class SeedProgress
    {
        public int Rank { get; set; }
        public int Points { get; set; }
    }
}
