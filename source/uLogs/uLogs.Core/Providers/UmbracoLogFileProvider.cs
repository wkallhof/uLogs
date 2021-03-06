﻿using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web;
using uLogs.Models;
using System;

namespace uLogs.Providers
{
    /// <summary>
    /// Default Umbraco Log File Provider. 
    /// </summary>
    public class UmbracoLogFileProvider : ILogFileProvider
    {
        private string _logDirectory = uLogsResolver.LogsDirectory;
        private string _logFileName = uLogsResolver.LogFileName;

        /// <summary>
        /// Get the log files from the log directory set. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LogFile> GetLogFiles()
        {
            var server = HttpContext.Current.Server;
            var files = Directory.EnumerateFiles(server.MapPath(_logDirectory));

            if (files == null || !files.Any()) return new List<LogFile>();

            return files
                .Where(x => x.Contains(_logFileName))
                .Select(x => new LogFile()
                {
                    Path = x,
                    Date = this.FetchDateFromFile(x),
                    Machine = this.FetchMachineFromFile(x)
                });

        }

        /// <summary>
        /// Handles parsing the machine name from the file name
        /// </summary>
        /// <param name="file">File to read machine name from</param>
        /// <returns>Name of machine</returns>
        private string FetchMachineFromFile(string file)
        {
            var fileName = Path.GetFileName(file);
            var sections = fileName.Split('.');
            if (sections.Length < 2 || sections[1] == "txt") return string.Empty;

            return sections[1];
        }

        /// <summary>
        /// Handles parsing the date from the datefile. Ex : UmbracoTraceLog.txt.2015-08-04
        /// </summary>
        /// <param name="file">File path to fetch date from</param>
        /// <returns>Set DateTime</returns>
        private DateTime FetchDateFromFile(string file)
        {
            var parsedDate = file.Substring(file.LastIndexOf("."));

            DateTime date;
            if (DateTime.TryParse(parsedDate, out date)) return date;

            return DateTime.Now.Date;
        }
    }
}
