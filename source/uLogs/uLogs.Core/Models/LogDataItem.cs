using System;

namespace uLogs.Models
{
    /// <summary>
    /// Model representing a single log data item
    /// </summary>
    public class LogDataItem
    {
        public DateTime Date { get; set; }
        public string Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
}
