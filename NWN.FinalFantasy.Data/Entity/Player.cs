﻿using NWN.FinalFantasy.Core.Enumerations;

namespace NWN.FinalFantasy.Data.Entity
{
    public class Player: EntityBase
    {
        public int Version { get; set; }
        public JobType CurrentJob { get; set; }
        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int MP { get; set; }
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
        public bool IsDeleted { get; set; }
    }
}
