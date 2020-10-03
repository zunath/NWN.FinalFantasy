﻿using System;
using MessagePack;

namespace NWN.FinalFantasy.Entity
{
    [MessagePackObject(true)]
    public class ServerConfiguration: EntityBase
    {
        public ServerConfiguration()
        {
            MigrationVersion = 0;
            LastRestart = DateTime.MinValue;
        }

        public int MigrationVersion { get; set; }
        public DateTime LastRestart { get; set; }
        public override string KeyPrefix => "ServerConfiguration";
    }
}
