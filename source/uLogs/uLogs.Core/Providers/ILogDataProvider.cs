using System;
using System.Collections.Generic;
using uLogs.Models;

namespace uLogs.Providers
{
    /// <summary>
    /// Data Provider for the uLogs trace log viewer. 
    /// </summary>
    public interface ILogDataProvider
    {
        /// <summary>
        /// Get the log entries for a given log date
        /// </summary>
        /// <param name="date">Date to get logs for</param>
        /// <returns>Collection of Log Data Items</returns>
        IEnumerable<LogDataItem> GetLogData(DateTime date);

        /// <summary>
        /// Get the available log dates
        /// </summary>
        /// <returns>Collection of DateTimes</returns>
        IEnumerable<DateTime> GetLogDates();
    }
}
