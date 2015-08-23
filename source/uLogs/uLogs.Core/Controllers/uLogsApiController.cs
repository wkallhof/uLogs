using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using uLogs.Providers;
using System.Collections.Generic;
using System;
using System.Linq;
using uLogs.Models;

namespace uLogs.Controllers
{
    /// <summary>
    /// Plugin API controller used to handle requests for
    /// Log Dates and Log Entries. 
    /// </summary>
    [PluginController("uLogs")]
    public class uLogsApiController : UmbracoAuthorizedApiController
    {
        private readonly ILogDataProvider _dataProvider;

        public uLogsApiController()
        {
            this._dataProvider = uLogsResolver.GetLogDateProviderInstance();
        }

        public IEnumerable<LogDataItem> GetLogsForDate(DateTime date)
        {
            return _dataProvider.GetLogData(date);
        }

        public IEnumerable<DateTime> GetAvailableDates()
        {
            return this._dataProvider.GetLogDates().OrderByDescending(x => x);
        }
    }

}