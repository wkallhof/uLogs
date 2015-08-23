using System.Collections.Generic;
using uLogs.Models;

namespace uLogs.Providers
{
    /// <summary>
    /// Log File Provider used to provide available log files for
    /// parsing.
    /// </summary>
    public interface ILogFileProvider
    {
        /// <summary>
        /// Get Log Files for log parsing
        /// </summary>
        /// <returns>Collection of LogFiles</returns>
        IEnumerable<LogFile> GetLogFiles();
    }
}
