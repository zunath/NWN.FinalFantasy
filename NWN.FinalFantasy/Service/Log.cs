using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using Serilog;
using Serilog.Core;

namespace NWN.FinalFantasy.Service
{
    public enum LogGroup
    {
        Connection,
        Error,
        Chat,
        DM,
        DMAuthorization,
        Death
    }

    public static class Log
    {
        private static readonly Dictionary<LogGroup, Logger> _loggers = new Dictionary<LogGroup, Logger>();

        /// <summary>
        /// Audits are written asynchronously so it's important to flush everything to disk when the server stops.
        /// Ensure this is called one time when the server stops.
        /// </summary>
        [NWNEventHandler("app_shutdown")]
        public static void OnApplicationShutdown()
        {
            foreach (var logger in _loggers.Values)
            {
                logger.Dispose();
            }
        }

        /// <summary>
        /// Writes a log message to the audit log for a given log group.
        /// </summary>
        /// <param name="group">The group to audit this log under.</param>
        /// <param name="details">The details about the entry which will be written to disk.</param>
        public static void Write(LogGroup group, string details)
        {
            if (!_loggers.ContainsKey(group))
            {
                var settings = ApplicationSettings.Get();

                var path = settings.LogDirectory + group + "/" + group + "_.log";
                var logger = new LoggerConfiguration()
                    .WriteTo.Async(a => a.File(path, rollingInterval: RollingInterval.Day));

                // Errors should also be print to the console.
                if (group == LogGroup.Error)
                {
                    logger.WriteTo.Console();
                }

                _loggers[group] = logger.CreateLogger();
            }

            _loggers[group].Information(details);
        }
    }
}
