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
        /// Get the log entries for a given log file
        /// </summary>
        /// <param name="date">Date to get logs for</param>
        /// <param name="machine">Machine to get logs for</param>
        /// <returns>Collection of Log Data Items</returns>
        IEnumerable<LogDataItem> GetLogData(DateTime date, string machine);

        /// <summary>
        /// Get the available log files
        /// </summary>
        /// <returns>Collection of LogFiles</returns>
        IEnumerable<LogFile> GetLogFiles();
    }
}
