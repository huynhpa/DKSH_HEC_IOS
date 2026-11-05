'use strict';

var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'ngSanitize'])
dkshApp.controller('OBSDashboardController', function ($scope, $http, $location, $window, $log, $q, FileSaver, Blob, $sce, $sanitize) {
    var deferred = $q.defer();

    $scope.currMon = GetCurrentMonth();
    $scope.SearchParam = {
        Agent: '', Month: GetCurrentMonth(), interval_day: GetLastDayFromNow(1), interval_day_end: GetLastDayFromNow(1),
        Extension: '', TypeReport: 'DAILY', AgentExtension: '',
        FromDate: GetCurrentDate(), ToDate: GetCurrentDate(),
        ServiceType: '', LeaderExtension: '', EmpLevels: ''
    };
    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row    
    /*========= PAGING =========*/
    $scope.TimeRange = [{ value: 'DAILY', label: 'Daily' }, { value: 'MONTHLY', label: 'Monthly' }];
    $scope.OBSDashboardModel = {
        OBSSummary: {
            CallPlacedInCurrentMon: '0', CallPlacedTeleSale: '0',
            CallPlacedTeleMarketing: '0', CallByAgentToday: '0'
        },
        SummaryByCampaign: [],
        SummaryByAgent: [],
        GauChart: [],
        ComboChart: []
    };

    $scope.SupsNameList = [];
    $scope.SaleRepList = [];
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['10', '20', '50']
    },
    $scope.callback = function () { },
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['10', '20', '50']
        };
    },
    
    /*========= PAGING =========*/

    $scope.LoadGaugeChart = function () {
        google.charts.load('current', { 'packages': ['gauge'] });
        google.charts.setOnLoadCallback(drawChart);

        // gauge chart
        function drawChart() {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Label');
            data.addColumn('number', 'Value');
            data.addRow(['Call',$scope.OBSDashboardModel.GauChart[0].actual_call]);            
            data.addRow(['Coverage',$scope.OBSDashboardModel.GauChart[0].actual_coverage]);
            data.addRow(['Actv Customer',$scope.OBSDashboardModel.GauChart[0].active_customer]);
            data.addRow(['Sales',$scope.OBSDashboardModel.GauChart[0].actual_sale]);
            var options = {
                width: 1000, height: 150,
                greenFrom: 0, greenTo: 100, greenColor: "#6AD58E",
                minorTicks: 5
            };
            var chart = new google.visualization.Gauge(document.getElementById('chart_gauge'));
            chart.draw(data, options);
        }
    },

    $scope.LoadComboChart = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawVisualization);
        // combo chart
        function drawVisualization() {
            // Some raw data (not necessarily accurate)
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Month');
            data.addColumn('number', 'Coverage');
            data.addColumn('number', 'Active Customer by rep');
            data.addColumn('number', 'Coveration rate');
            for (var i = 0; i < $scope.OBSDashboardModel.ComboChart.length; i++) {
                data.addRow([$scope.OBSDashboardModel.ComboChart[i].interval_day, $scope.OBSDashboardModel.ComboChart[i].actual_coverage,
                                $scope.OBSDashboardModel.ComboChart[i].active_customer, $scope.OBSDashboardModel.ComboChart[i].coveration_rate]);                
            }

            var options = {
                title: 'Coverage and Active Customer',
                vAxis: { title: 'Coverage and Active Customer' },
                hAxis: { title: 'Day' },
                seriesType: 'bars',
                series: { 2: { type: 'line' } }
            };

            var chart = new google.visualization.ComboChart(document.getElementById('chart_combo'));
            chart.draw(data, options);
        }
    },
    
    
    // DashboardSummary
    $scope.DashboardSummary = function () {        
        console.log($scope.SearchParam);
        $http({
            method: 'GET',
            url: '/Home/DashboardSummary',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.CODE === "ERROR") {
                ErrorMessage(response.data.MESSAGE);
            } else {
                $scope.OBSDashboardModel.OBSSummary = response.data.OBSSummary;
                $scope.OBSDashboardModel.GauChart = response.data.GauChart;
                $scope.OBSDashboardModel.ComboChart = response.data.ComboChart;
                $scope.LoadGaugeChart($scope.OBSDashboardModel.GauChart); 
                $scope.LoadComboChart($scope.OBSDashboardModel.ComboChart); 
            }            
        }, function (response) {            
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.RefreshPerformanceData = function () {
        $.alert({
            title: 'Message!',
            type: 'info',
            content: 'Data process may be take a few minutes. Please wait .......',
            buttons: {
                ok: {
                    btnClass: 'bg-green-active',
                    action: function () {
                        $http({
                            method: 'POST',
                            url: '/Home/RefreshPerformanceData',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.SearchParam
                        }).then(function (response) {
                            deferred.resolve(response);
                            $scope.DashboardSummary();
                        }, function (response) {
                            ErrorMessage(response.data);
                            return deferred.reject(response.data);
                        });
                    }
                }
            }
        });
    },


    $scope.SummaryByCampaign = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Home/SummaryByCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);            
            $scope.OBSDashboardModel.SummaryByCampaign = response.data;
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.SummaryByAgent = function () {
        $scope.SearchParam.ServiceType = $scope.SearchParam.ServiceType.toString();
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Home/SummaryByAgent',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.OBSDashboardModel.SummaryByAgent = response.data;
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.ExportSummaryByCampaign = function () {
        angular.element('#Loading').LoadingOverlay("show");
        var fileNm = "SummaryByAgent-" + $scope.SearchParam.Month + ".xlsx";        
        $http({
            method: 'GET',
            url: '/Home/ExportSummaryByCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileNm);
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            console.error(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.ExportSummaryByAgent = function () {
        angular.element('#Loading').LoadingOverlay("show");
        var fileNm = '';
        if ($scope.SearchParam.TypeReport === "DAILY") {
            fileNm = "SummaryByAgent-" + ($scope.SearchParam.FromDate == $scope.SearchParam.ToDate ? $scope.SearchParam.FromDate : $scope.SearchParam.FromDate + " to " + $scope.SearchParam.ToDate) + ".xlsx";
        }
        else if ($scope.SearchParam.TypeReport === "MONTHLY") {
            fileNm = "SummaryByAgent-" + $scope.SearchParam.Month + ".xlsx";
        }
        $http({
            method: 'GET',
            url: '/Home/ExportSummaryByAgent',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileNm);
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            console.error(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.GetCampaignList = function () {
        $http({
            method: 'GET',
            url: '/SearchCall/GetCampaignList',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.CampaignList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },


    $scope.SupsList = function () {
        $http({
            method: 'GET',
            url: '/Home/SupsList',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.SupsNameList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },


    $scope.SaleRepBySup = function () {
        $http({
            method: 'GET',
            url: '/Home/SaleRepBySup',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.SaleRepList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },    


    /*========== RESET ===========*/
    $scope.Reset = function () {
        $scope.SearchParam = {
            Agent: '', Month: GetCurrentMonth(),
            Extension: '', TypeReport: 'DAILY',
            FromDate: GetCurrentDate(), ToDate: GetCurrentDate(),
            ServiceType: ''
        };
        $('.select2-selection__choice').remove();
    }

    /*========== Get Campaign ==========*/
    /*========== INIT PAGE ===========*/
    $scope.UserSession = { UserId: '', UserName: '' };
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.SearchParam.LeaderExtension = $('#txtLeaderExtension').val();
        if (!IsNullOrEmpty($scope.SearchParam.LeaderExtension)) {
            $scope.SaleRepBySup();
        }
        $scope.SearchParam.AgentExtension = $('#txtAgentExtension').val();
        $scope.DashboardSummary();
        $scope.GetCampaignList();
        $scope.SupsList();
    }
    $scope.initPage();

    /*========== INIT PAGE ===========*/

}).filter('unsafe', function ($sce) {
    return function (val) {
        return $sce.trustAsHtml(val);
    };
});