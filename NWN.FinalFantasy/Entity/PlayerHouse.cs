﻿using System;
using System.Collections.Generic;
using MessagePack;
using NWN.FinalFantasy.Enumeration;

namespace NWN.FinalFantasy.Entity
{
    [MessagePackObject(true)]
    public class PlayerHouse: EntityBase
    {
        public PlayerHouse()
        {
            Furnitures = new Dictionary<string, PlayerHouseFurniture>();
        }

        public override string KeyPrefix => "PlayerHouse";

        public string CustomName { get; set; }
        public PlayerHouseType HouseType { get; set; }
        public Dictionary<string, PlayerHouseFurniture> Furnitures { get; set; }
        public Dictionary<string, PlayerHousePermission> PlayerPermissions { get; set; }
    }

    [MessagePackObject(true)]
    public class PlayerHouseFurniture
    {
        public FurnitureType FurnitureType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Orientation { get; set; }
        public string CustomName { get; set; }
    }

    [MessagePackObject(true)]
    public class PlayerHousePermission
    {
        public bool CanEnter { get; set; }
        public bool CanPlaceFurniture { get; set; }
        public bool CanAdjustPermissions { get; set; }
    }
}
