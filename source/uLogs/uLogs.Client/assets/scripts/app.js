app.requires.push('ngTable');

/*
* uLOGS CONTROLLER
* -----------------------------------------------------
* Main uLogs controller used to render out the uLogs developer section
*/
angular.module("umbraco").controller("uLogsController", function ($scope, $filter, uLogsApi, ngTableParams) {

    //Count variables used to update filter buttons
    $scope.allCount = 0;
    $scope.infoCount = 0;
    $scope.warnCount = 0;
    $scope.errorCount = 0;

    //Tracks the selected filter type
    $scope.selectedFilter = "";

    /*
    * Get the available log dates to view log entries for.
    * TODO : Update to only run this once the developer tab is clicked
    */
    uLogsApi.getAvailableDates().then(function (response) {
        $scope.dates = response.data;
    });

    /*
    * Handles when the user selects a log date from the dropdown.
    * Calls out to the uLogs API to get the logs for the given date. Updates
    * the counts and sets the table data for the new logs
    */
    $scope.onDateChange = function () {
        uLogsApi.getLogsForDate($scope.selectedDate).then(function (response) {
            $scope.logItems = response.data;

            $scope.allCount = $scope.logItems.length;
            $scope.infoCount = $filter('filter')($scope.logItems, { Level: "INFO" }).length;
            $scope.warnCount = $filter('filter')($scope.logItems, { Level: "WARN" }).length;
            $scope.errorCount = $filter('filter')($scope.logItems, { Level: "ERROR" }).length;

            $scope.tableParams.total($scope.logItems.length);
            $scope.tableParams.reload();
        });
    };

    /*
    * Handler for when a Level filter is clicked.
    * Takes in the level filter type and adds it to
    * the table filter
    */
    $scope.onLevelFilterClick = function (filterText) {
        $scope.selectedFilter = filterText;
        $scope.tableParams.filter().Level = filterText;
    };

    /*
    * Defines a new ngTable. 
    */
    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 10,          // count per page
        sorting: {
            Date: 'desc'     // initial sorting
        },
        filter: {
            Message: ''       // initial filter
        }
    }, {
        total: 0,
        getData: function ($defer, params) {

            //Do we have logItems yet?
            var data = $scope.logItems || [];

            //Do we have a search term set in the search box?
            //If so, filter the log message for that text
            var searchTerm = params.filter().Search;
            var searchedData = searchTerm ?
                data.filter(function (logItem) {
                    return logItem.Message.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1;
                }) : data;

            //Do we have a level set? Filter by the selected level
            var filteredData = params.filter().Level ?
                    $filter('filter')(searchedData, { Level: params.filter().Level }) :
                    searchedData;

            //Are we ordering the results?
            var orderedData = params.sorting() ?
                    $filter('orderBy')(filteredData, params.orderBy()) :
                    filteredData;

            //Set totals and page counts
            params.total(orderedData.length);
            $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
        }
    })

})

/*
* uLOGS API
* -----------------------------------------------------
* Resource to handle making requests to the backoffice API to fetch logs and dates
*/
angular.module("umbraco.resources").factory("uLogsApi", function ($http) {
    return {
        getLogsForDate: function (date) {
            return $http.get("backoffice/uLogs/uLogsApi/GetLogsForDate?date=" + encodeURIComponent(date));
        },

        getAvailableDates: function()
        {
            return $http.get("backoffice/uLogs/uLogsApi/GetAvailableDates");
        }
    };
});

/*
* SNIPPET DIRECTIVE
* -----------------------------------------------------
* Creates a new <snippet> directive that handles setting a <pre> and
* <code> structure with syntax highlighting
*/
angular.module('umbraco').directive('snippet', ['$timeout', '$interpolate', function ($timeout, $interpolate) {
        "use strict";
        return {
            restrict: 'E',
            template: '<pre><code ng-transclude></code></pre>',
            replace: true,
            transclude: true,
            link: function (scope, elm) {
                var tmp = $interpolate(elm.find('code').text())(scope);
                tmp = tmp.replace(/`/g, '');
                var highlight = hljs.highlight('prolog', tmp);
                elm.find('code').html(highlight.value);
            }
        };
}]);