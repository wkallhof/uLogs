using System;
using System.Linq;
using uLogs.Providers;
using System.Configuration;

namespace uLogs
{
    /// <summary>
    /// Static resolver for paths and provider implementations for uLogs
    /// </summary>
    public static class uLogsResolver
    {
        private static string _logsDirectory;
        private static string _logFileName;
        private static Type _logDataProviderType;
        private static Type _logFileProviderType;

        static uLogsResolver()
        {
            _logsDirectory = ConfigurationManager.AppSettings["uLogs.LogsDirectory"] ?? "~/App_Data/Logs";
            _logFileName = ConfigurationManager.AppSettings["uLogs.LogFileName"] ?? "UmbracoTraceLog.txt";

            _logDataProviderType = typeof(UmbracoLogDataProvider);
            _logFileProviderType = typeof(UmbracoLogFileProvider);
        }

        /// <summary>
        /// The Directory that the default LogFileProvider
        /// uses to find log files (ex. ~/App_Data/Logs)
        /// </summary>
        public static string LogsDirectory
        {
            get
            {
                return _logsDirectory;
            }
            set
            {
                _logsDirectory = value;
            }
        }

        /// <summary>
        /// The Log File Name that the default LogFileProvider
        /// uses to find log files (ex. UmbracoTraceLog.txt)
        /// </summary>
        public static string LogFileName
        {
            get
            {
                return _logFileName;
            }
            set
            {
                _logFileName = value;
            }
        }

        /// <summary>
        /// Set the Log Data Provider used to parse log files. Must inherit
        /// ILogDataProvider.
        /// </summary>
        /// <param name="type">Custom ILogDataProvider type</param>
        public static void SetLogDataProviderType(Type type)
        {
            ValidateType<ILogDataProvider>(type);
            _logDataProviderType = type;
        }

        /// <summary>
        /// Set the Log File Provider used to find log files. Must inherit
        /// ILogFileProvider.
        /// </summary>
        /// <param name="type">Custom ILogFileProvider type</param>
        public static void SetLogFileProviderType(Type type)
        {
            ValidateType<ILogFileProvider>(type);
            _logFileProviderType = type;
        }

        /// <summary>
        /// Get a new instance of the currently set LogFileProvider
        /// </summary>
        /// <returns>new ILogFileProvider instance</returns>
        public static ILogFileProvider GetLogFileProviderInstance()
        {
            return GetProviderInstance<ILogFileProvider>(_logFileProviderType);
        }

        /// <summary>
        /// Get a new instance of the currently set ILogDataProvider
        /// </summary>
        /// <returns>new ILogDataProvider instance</returns>
        public static ILogDataProvider GetLogDateProviderInstance()
        {
            return GetProviderInstance<ILogDataProvider>(_logDataProviderType);
        }

        /// <summary>
        /// Returns an instance of the default provider instance.
        /// </summary>
        private static T GetProviderInstance<T>(Type value) where T : class
        {

            var instance = Activator.CreateInstance(value);
            var result = instance as T;
            if (result == null)
            {
                throw new InvalidOperationException("Could not create an instance of " + value + " for the default " + typeof(T).Name);
            }
            return result;
        }

        /// <summary>
        /// Ensures that the type passed inherits a specific interface
        /// </summary>
        /// <param name="type">Type to validate</param>
        private static void ValidateType<T>(Type type)
        {
            if (!type.GetInterfaces().Contains(typeof(T)))
            {
                throw new InvalidOperationException("The Type specified (" + type + ") is not of type " + typeof(T));
            }
        }
    }
}
