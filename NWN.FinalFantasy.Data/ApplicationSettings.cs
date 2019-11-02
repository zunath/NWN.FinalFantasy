using System;

namespace NWN.FinalFantasy.Data
{
    internal class ApplicationSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }

        public ApplicationSettings()
        {
            Host = Environment.GetEnvironmentVariable("FF_DATA_HOST");
            Port = Environment.GetEnvironmentVariable("FF_DATA_PORT");
        }
    }
}
