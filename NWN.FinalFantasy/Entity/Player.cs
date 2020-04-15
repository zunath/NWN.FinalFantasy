using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service.PerkService;

namespace NWN.FinalFantasy.Entity
{
    public class Player: EntityBase
    {
        public Player()
        {
            MapPins = new Dictionary<string, List<MapPin>>();
            MapProgressions = new Dictionary<string, string>();
            RoleplayProgress = new RoleplayProgress();
            Skills = new Dictionary<SkillType, PlayerSkill>();
            Perks = new Dictionary<PerkType, int>();
            RecastTimes = new Dictionary<RecastGroup, DateTime>();
        }

        public int Version { get; set; }
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int MaxStamina { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Stamina { get; set; }
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

        public RoleplayProgress RoleplayProgress { get; set; }
        public Dictionary<string, List<MapPin>> MapPins { get; set; }
        public Dictionary<string, string> MapProgressions { get; set; }
        public Dictionary<SkillType, PlayerSkill> Skills { get; set; }
        public Dictionary<PerkType, int> Perks { get; set; }
        public Dictionary<RecastGroup, DateTime> RecastTimes { get; set; }
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
}
