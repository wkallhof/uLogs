
# uLogs #

![Logo][logoImage]

[![Build status](https://img.shields.io/appveyor/ci/wkallhof/ulogs/master.svg)](https://ci.appveyor.com/project/wkallhof/ulogs/branch/master)

uLogs is a simple to use, yet extensible, Umbraco Trace Log viewer plugin for the Umbraco 7.3 Back-Office that allows you to view logs by date and a variety of refinements. Includes a simple and intuitive refinement system where you can search for specific text in the log message or refine by log levels (INFO, WARN, and ERROR). 

Utilizes [ngTable][ngTableLink] for an AngularJs driven data table which includes ordering by column (Time, Level, and Message) and simple pagination with configurable items per page selector. 

Also takes advantage of [highlight.js][highlightJsLink] to provide syntax highlighting for the logs to improve readability. 

### Getting Started ###

Nuget Package: ` Install-Package uLogs `

### Usage ###

#### 1. Locate uLogs in the Developer section ####
Navigate to the Umbraco > Developers section. You will find a 'Trace Logs' tab in the right pane. Select this to view the uLogs Trace Log Viewer. From this window you can select a log from the available date dropdown in order to see the logs for the given date.
![Go to Umbraco > Developers > Trace Logs][introImage]

#### 2. Refine & Locate ####
Once you have selected a log date and logs are shown, proceed to refine by log Level (ex. select the ERROR button to narrow the results down to Error logs) or Text search by entering text in the 'Search data' input box. As you type, results will display in real time. 

Once you have located a log, clicking the "View More" link will display a syntax highlighted version of the log message. Clicking the "View Less" button at the end of the message will return the message back to the closed state.
![Refine by Log Level, Text Search, and Pagination][usageImage]


### Extend & Configure ###
uLogs provides a variety of configuration options along with allowing you to supply your own Log File and Log Data providers to be used by uLogs. This means you can write your own custom log file provider and/or your own custom log parser and integrate with uLogs to display your log results within the Umbraco Back-Office.

When "code" configuration or extensions are referenced below, the code referenced should be placed in the `protected override void OnApplicationStarting(object sender, EventArgs e)` method generally defined in the `global.asax.cs` file referenced in the solution, but it could exist on any event handler that inherits from `UmbracoApplication`

#### Logs Directory ####
You can provide your own value for where the logs are stored (default : ~/App_Data/Logs). This will be used by the default LogFileProvider to find the logs to parse. You can do this in 1 of 2 ways:
###### 1. App Setting : ######
Add the following App Setting in your `web.config` file
```xml
<add key="uLogs.LogsDirectory" value="~/path/to/custom/directory" />
```
###### 2. Code : ######
Add the following in your `OnApplicationStarting` method
```cs
uLogs.uLogsResolver.LogsDirectory = "~/path/to/custom/directory";
```

#### Log File Name ####
In the event that you have changed the names for the default logs stored by Umbraco or you will implement your own logging, you can provide your own value for the name of the log files to be referenced by uLogs (default : UmbracoTraceLog.txt). This will be used by the default LogFileProvider to find the logs to parse. You can do this in 1 of 2 ways:
###### 1. App Setting : ######
Add the following App Setting in your `web.config` file
```xml
<add key="uLogs.LogFileName" value="MyCustomName.txt" />
```
###### 2. Code : ######
Add the following in your `OnApplicationStarting` method
```cs
uLogs.uLogsResolver.LogFileName = "MyCustomName.txt";
```

#### Log File Provider ####
uLogs allows you to provide your own class that finds logs. This is done by creating a class that inherits from the ILogFileProvider interface and then telling uLogs to use it. 

###### Create your custom log file provider : ######
```cs
public class MyCustomLogFileProvider : ILogFileProvider
{
  /... Implement the Interface Here
}
```
###### Tell uLogs to use it in your `OnApplicationStarting` method ######
```cs
uLogs.uLogsResolver.SetLogFileProviderType(typeof(MyCustomLogFileProvider));
```

#### Log Data Provider ####
uLogs allows you to provide your own class that parses logs. This allows you to make modifications to the Umbraco's Log4Net output format and then update uLogs to parse the new output format. This is done by creating a class that inherits from the ILogDataProvider interface and then telling uLogs to use it.

###### Create your custom log data provider : ######
```cs
public class MyCustomLogDataProvider : ILogDataProvider
{
  /... Implement the Interface Here
}
```
###### Tell uLogs to use it in your `OnApplicationStarting` method ######
```cs
uLogs.uLogsResolver.SetLogDataProviderType(typeof(MyCustomLogDataProvider));
```


__Support:__ [Documentation Wiki](https://github.com/wkallhof/uLogs/wiki), [Issue Logging](https://github.com/wkallhof/uLogs/issues)

[ngTableLink]: https://github.com/esvit/ng-table
[highlightJsLink]: https://github.com/isagalaev/highlight.js
[logoImage]: package/uLogsLogo.png  "Logo"
[introImage]: package/Intro.gif  "Intro"
[usageImage]: package/Usage.gif  "Usage"
