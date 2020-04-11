using System;

namespace NWN.FinalFantasy.Entity
{
    public class ServerConfiguration: EntityBase
    {
        public ServerConfiguration()
        {
            MigrationVersion = 0;
            LastRestart = DateTime.MinValue;
        }

        public int MigrationVersion { get; set; }
        public DateTime LastRestart { get; set; }
    }
}
