'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'treeGrid'])
    .controller('TeleMarketingDashboardController', function ($scope, $http, $location, $window, $log, $rootScope, $filter, $q, FileSaver, Blob) {
    var deferred = $q.defer();
    $scope.TLMDashboardModel = {
        ReportType: '',
        FromDate: '',
        ToDate: '',
        Month: '',
        Year: '',
        Telemarketer: ''
    };
    $scope.TLMSummary = {
        TotalProjects: '0',
        TotalPushSale: '0',
        TotalOutlet: '0',
        TotalCurrMonth: '0',
        TotalGrandTotal: '0',
        ArchiveTarget: '0',
        AverageArchive: '0'
    }
    $scope.CurrentMonth = '';
    var tree;
    $scope.extensionArr = [{ key: 'xls', value: 'xls' }, { key: 'xlsx', value: 'xlsx' }];
    $scope.extension = '';
    /*========== GET CUSTOMER CODE BY DAY / MONTH / YEAR ===========*/
    $scope.GetSummaryInfo = function () {
        $('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/DashboardTelemarketing/GetSummaryInfo',
            headers: [{ 'Content-Type': 'application/json' }],
            params: null
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            if (!IsNullOrEmpty(response.data.TLMDashboardDto)) {
                
            }
        }, function (response) {
            $('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    }

    // Export Report
    $scope.ExportReport = function (ReportType) {
        var fileName = '';
        $('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/DashboardTelemarketing/ExportReport',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.TLMDashboardModel,
            responseType: 'blob'
        }).then(function (response) {
            console.log(response.data.type);
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileName);
        }, function (response) {
            $('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    }

    /*===================================== BEGIN GOOGLE CHART ========================================*/
    $scope.LoadPieChartChannels = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(CustomerChannelChart);
    }


    $scope.LoadStackedChartChannel = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(StatusChannelsStackedChart);
    }
    /*===================================== END GOOGLE CHART ========================================*/

    /*================== TREE GRID ===================*/

    var tree;

    var rawTreeData = [
        { "DemographicId": 1, "ParentId": null, "Name": "United States of America", "Description": "United States of America", "Area": 9826675, "Population": 318212000, "TimeZone": "UTC -5 to -10" },
        { "DemographicId": 2, "ParentId": 1, "Name": "California", "Description": "The Tech State", "Area": 423970, "Population": 38340000, "TimeZone": "Pacific Time" },
        { "DemographicId": 3, "ParentId": 2, "Name": "San Francisco", "Description": "The happening city", "Area": 231, "Population": 837442, "TimeZone": "PST" },
        { "DemographicId": 4, "ParentId": 2, "Name": "Los Angeles", "Description": "Disco city", "Area": 503, "Population": 3904657, "TimeZone": "PST" },
        { "DemographicId": 5, "ParentId": 1, "Name": "Illinois", "Description": "Not so cool", "Area": 57914, "Population": 12882135, "TimeZone": "Central Time Zone" },
        { "DemographicId": 6, "ParentId": 5, "Name": "Chicago", "Description": "Financial City", "Area": 234, "Population": 2695598, "TimeZone": "CST" },
        { "DemographicId": 7, "ParentId": 1, "Name": "Texas", "Description": "Rances, Oil & Gas", "Area": 268581, "Population": 26448193, "TimeZone": "Mountain" },
        { "DemographicId": 8, "ParentId": 1, "Name": "New York", "Description": "The largest diverse city", "Area": 141300, "Population": 19651127, "TimeZone": "Eastern Time Zone" },
        { "DemographicId": 14, "ParentId": 8, "Name": "Manhattan", "Description": "Time Square is the place", "Area": 269.403, "Population": 0, "TimeZone": "EST" },
        { "DemographicId": 15, "ParentId": 14, "Name": "Manhattan City", "Description": "Manhattan island", "Area": 33.77, "Population": 0, "TimeZone": "EST" },
        { "DemographicId": 16, "ParentId": 14, "Name": "Time Square", "Description": "Time Square for new year", "Area": 269.40, "Population": 0, "TimeZone": "EST" },
        { "DemographicId": 17, "ParentId": 8, "Name": "Niagra water fall", "Description": "Close to Canada", "Area": 65.7, "Population": 0, "TimeZone": "EST" },
        { "DemographicId": 18, "ParentId": 8, "Name": "Long Island", "Description": "Harbour to Atlantic", "Area": 362.9, "Population": 0, "TimeZone": "EST" },
        { "DemographicId": 51, "ParentId": 1, "Name": "All_Other", "Description": "All_Other demographics", "Area": 0, "Population": 0, "TimeZone": 0 },
        { "DemographicId": 201, "ParentId": null, "Name": "India", "Description": "Hydrabad tech city", "Area": 9826675, "Population": 318212000, "TimeZone": "IST" },
        { "DemographicId": 301, "ParentId": null, "Name": "Bangladesh", "Description": "Country of love", "Area": 9826675, "Population": 318212000, "TimeZone": "BST" }
    ];


    var myTreeData = getTree(rawTreeData, 'DemographicId', 'ParentId');

    $scope.tree_data = myTreeData;
    $scope.my_tree = tree = {};
    $scope.expanding_property = "Name";
    $scope.col_defs = [
        { field: "Description" },
        { field: "Area" },
        { field: "Population" },
        { field: "TimeZone", displayName: "Time Zone" }
    ];
    $scope.my_tree_handler = function (branch) {
        console.log('you clicked on', branch)
    }

    function getTree(data, primaryIdName, parentIdName) {
        if (!data || data.length == 0 || !primaryIdName || !parentIdName)
            return [];

        var tree = [],
            rootIds = [],
            item = data[0],
            primaryKey = item[primaryIdName],
            treeObjs = {},
            parentId,
            parent,
            len = data.length,
            i = 0;

        while (i < len) {
            item = data[i++];
            primaryKey = item[primaryIdName];
            treeObjs[primaryKey] = item;
            parentId = item[parentIdName];

            if (parentId) {
                parent = treeObjs[parentId];

                if (parent.children) {
                    parent.children.push(item);
                }
                else {
                    parent.children = [item];
                }
            }
            else {
                rootIds.push(primaryKey);
            }
        }

        for (var i = 0; i < rootIds.length; i++) {
            tree.push(treeObjs[rootIds[i]]);
        };

        return tree;
    }
    

    /*===== INIT PAGE =====*/
    $scope.initPage = function () {
        $scope.GetSummaryInfo();
        $scope.CurrentMonth = GetCurrentDate().substr(3, 8);
    }
    $scope.initPage();
    /*===== INIT PAGE =====*/

})