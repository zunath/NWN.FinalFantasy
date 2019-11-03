using System;
using System.Text;

namespace NWN.FinalFantasy.Core.Logging
{
    internal static class ExceptionExtensions
    {
        /// <summary>
        /// Pulls all relevant exception details out and formats them in a human-readable string.
        /// </summary>
        /// <param name="exception">The exception to prettify</param>
        /// <returns>A prettified exception detail string</returns>
        public static string ToMessageAndCompleteStacktrace(this Exception exception)
        {
            Exception e = exception;
            StringBuilder s = new StringBuilder();
            while (e != null)
            {
                s.AppendLine("Exception type: " + e.GetType().FullName);
                s.AppendLine("Message       : " + (e.Message ?? string.Empty));
                s.AppendLine("Stacktrace:");
                s.AppendLine(e.StackTrace);

                s.AppendLine();
                e = e.InnerException;
            }

            return s.ToString();
        }
    }
}