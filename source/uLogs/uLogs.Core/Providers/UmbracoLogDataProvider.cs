using System;
using System.Linq;
using System.Collections.Generic;
using uLogs.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace uLogs.Providers
{
    /// <summary>
    /// Default Umbraco Log Data Provider. Handles parsing out log entries
    /// in a single log file.
    /// </summary>
    public class UmbracoLogDataProvider : ILogDataProvider
    {
        private readonly ILogFileProvider _fileProvider;
        private const string LOG_LINE_REGEX_FORMAT = @"^{0}\s{1}\s{2}\s+{3}\s-\s{4}";

        //Parse the Date from the log entry
        private const string LOG_DATE_PATTERN = @"(?<date>[0-9 -]+\s[0-9 :]+)[0-9 ,]+";
        //Parse out the ID section
        private const string LOG_ID_PATTERN = @"(?<id>\[.+\])";
        //Parse out the Log Level
        private const string LOG_LEVEL_PATTERN = @"(?<level>[A-Z]+)";
        //Parse out the Log Scope / Caller
        private const string LOG_SCOPE_PATTERN = @"(?<scope>[a-zA-Z \.]+)";
        //Parse out the Log Message
        private const string LOG_MESSAGE_PATTERN = @"(?<message>.*)";

        private IEnumerable<LogFile> _logFiles;

        public UmbracoLogDataProvider()
        {
            this._fileProvider = uLogsResolver.GetLogFileProviderInstance();
            this._logFiles = this._fileProvider.GetLogFiles();
        }

        /// <summary>
        /// Get the log entries for a given date. The date corresponds to a
        /// single log file.
        /// </summary>
        /// <param name="date">Date to fetch logs for</param>
        /// <returns>List of log files</returns>
        public IEnumerable<LogDataItem> GetLogData(DateTime date)
        {
            //Get the log file for the given date
            var file = this._logFiles
                .FirstOrDefault(x => x.Date.Equals(date));

            //If it doesn't exist, throw exception
            if (file == null || string.IsNullOrWhiteSpace(file.Path) || !File.Exists(file.Path))
                throw new Exception("Unable to locate a log file for the given date");

            //Return list of log entries
            return this.ParseLogsFromFile(file.Path);
        }

        /// <summary>
        /// Parses the logs from a given file path
        /// </summary>
        /// <param name="path">Path to file to parse logs from</param>
        /// <returns>Collection of Log Data Items</returns>
        private IEnumerable<LogDataItem> ParseLogsFromFile(string path)
        {
            //Read in the text and split by newline
            string log = File.ReadAllText(path);
            var lines = log.Split('\n');

            //Format the regex expression
            var lineRegex = string.Format(LOG_LINE_REGEX_FORMAT, 
                LOG_DATE_PATTERN, 
                LOG_ID_PATTERN, 
                LOG_LEVEL_PATTERN, 
                LOG_SCOPE_PATTERN, 
                LOG_MESSAGE_PATTERN);

            var regex = new Regex(lineRegex);
            var logItems = new List<LogDataItem>();

            foreach(var line in lines)
            {
                //Check for a line match
                var match = regex.Match(line);
                if (!match.Success)
                {
                    //Log regex didn't match. This is assumed to be a continuation from the
                    //previous log message so add it to the last found log and continue
                    if(logItems.Count > 0)
                    {
                        var lastItem = logItems.LastOrDefault();
                        lastItem.Message += line;
                    }
                    continue;
                }

                //Parse the date group out
                DateTime date;
                if (!DateTime.TryParse(match.Groups["date"].Value, out date)) return null;

                //Return new log item
                logItems.Add(new LogDataItem()
                {
                    Date = date,
                    Id = match.Groups["id"].Value,
                    Level = match.Groups["level"].Value,
                    Message = match.Groups["scope"].Value + " : " + match.Groups["message"].Value
                });
            }

            return logItems;
        }

        /// <summary>
        /// Get the available log dates to read logs from
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DateTime> GetLogDates()
        {
            return this._logFiles
                .Select(x => x.Date)
                .OrderBy(x => x);
        }
    }
}
