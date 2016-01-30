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

        public IEnumerable<LogDataItem> GetLogsForFile(DateTime date, string machine)
        {
            return _dataProvider.GetLogData(date, machine);
        }

        public IEnumerable<LogFile> GetAvailableFiles()
        {
            return this._dataProvider.GetLogFiles().OrderByDescending(x => x.Date);
        }
    }

}