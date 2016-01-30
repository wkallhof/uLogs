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

    //App state
    $scope.initialLoad = false;
   
    /*
    * Handles when the user selects a log file from the dropdown.
    * Calls out to the uLogs API to get the logs for the given file. Updates
    * the counts and sets the table data for the new logs
    */
    $scope.onFileChange = function () {
        uLogsApi.getLogsForFile($scope.selectedFile).then(function (response) {
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


    /*
    * Initial load function to set loaded state
    */
    $scope.initLoad = function () {
        if (!$scope.initialLoad) {
            //Get the available log files to view log entries for.
            uLogsApi.getAvailableFiles()
                .then(function (response) {
                    $scope.files = response.data;
                    $scope.initialLoad = true;
                });
        }
    }

    $(function () {
        $scope.$tab = $('a:contains("Trace Logs")');

        //If we have a tab, set the click handler so we only
        //load the content on tab click. 
        if ($scope.$tab && $scope.$tab.length > 0) {
            $scope.$tab.on('click', $scope.initLoad.bind(this));
        }
        else {
            $scope.initLoad();
        }
    });

})

/*
* uLOGS API
* -----------------------------------------------------
* Resource to handle making requests to the backoffice API to fetch logs and dates
*/
angular.module("umbraco.resources").factory("uLogsApi", function ($http) {
    return {
        getLogsForFile: function (file) {
            return $http.get("backoffice/uLogs/uLogsApi/GetLogsForFile", { params:file});
        },

        getAvailableFiles: function () {
            return $http.get("backoffice/uLogs/uLogsApi/GetAvailableFiles");
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