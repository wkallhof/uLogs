
# uLogs #

![Logo][logoImage]

[![Build status](https://img.shields.io/appveyor/ci/wkallhof/ulogs/master.svg)](https://ci.appveyor.com/project/wkallhof/ulogs/branch/master)

**Note** This package requires Umbraco v7.3.0-RC+

uLogs is a simple to use, yet extensible, Umbraco Trace Log viewer plugin for the Umbraco 7.3 Back-Office that allows you to view logs by date and a variety of refinements. Includes a simple and intuitive refinement system where you can search for specific text in the log message or refine by log levels (INFO, WARN, and ERROR). 

Utilizes [ngTable][ngTableLink] for an AngularJs driven data table which includes ordering by column (Time, Level, and Message) and simple pagination with configurable items per page selector. 



### Getting Started ###

``` Install-Package uLogs -Pre ```

### Usage ###


#### 1. Locate uLogs in the Developer section ####
Navigate to the Umbraco > Developers section. You will find a 'Trace Logs' tab in the right pane. Select this to view the uLogs Trace Log Viewer. From this window you can select a log from the available date dropdown in order to see the logs for the given date.
![Go to Umbraco > Developers > Trace Logs][introImage]

#### 2. Refine and Locate ####
Once you have selected a log date and logs are shown, proceed to refine by log Level (ex. select the ERROR button to narrow the results down to Error logs) or Text search by entering text in the 'Search data' input box. As you type, results will display in real time. 

Once you have located a log, clicking the "View More" link will display a syntax highlighted version of the log message. Clicking the "View Less" button at the end of the message will return the message back to the closed state.
![Refine by Log Level, Text Search, and Pagination][usageImage]


### Extend & Configure ###



__Support:__ [Documentation Wiki](https://github.com/wkallhof/uLogs/wiki), [Issue Logging](https://github.com/wkallhof/uLogs/issues)

[ngTableLink]: https://github.com/esvit/ng-table

[logoImage]: https://raw.githubusercontent.com/wkallhof/uLogs/master/package/uLogsLogo.png  "Logo"
[introImage]: https://raw.githubusercontent.com/wkallhof/uLogs/master/package/Intro.gif  "Intro"
[usageImage]: https://raw.githubusercontent.com/wkallhof/uLogs/master/package/Usage.gif  "Usage"
