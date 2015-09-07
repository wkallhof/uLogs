using System;

namespace uLogs.Models
{
    /// <summary>
    /// Model representing a single log file
    /// </summary>
    public class LogFile
    {
        public string Path { get; set; }
        public DateTime Date { get; set; }
    }
}
