'use strict';


var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'lr.upload'])
dkshApp.controller('WallboardController', function ($scope, $http, $location, $window, $log, $q, FileSaver, Blob, upload, $interval) {
    var deferred = $q.defer();

    /*========= MODEL, PARAM =========*/
    $scope.IsViewCall = true;
    $scope.IsViewPerformance = false;
    $scope.SearchParam = {
        FromDate: GetCurrentDate(), ToDate: GetCurrentDate()        
    },
    $scope.ptime = new Date().toLocaleTimeString();
    $scope.pdate = GetCurrentDate();
    $interval(function () {
        $scope.ptime = new Date().toLocaleTimeString();
    }, 1000);
    $scope.UserSession = {
        UserId: '', UserName: '', IsAdmin: ''
    },
    $scope.WallboardDto = {
        TeleSale: {}, TeleCSO: {}, VAS: {}, AchivedPerformance: [], FocusCampaign: []
    },
    /*========== INIT PAGE ===========*/
    $scope.InitializeWallboard = function () {
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';
        $http({
            method: 'GET',
            url: '/Wallboard/InitializeWallboard',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.WallboardDto = response.data;
            console.log($scope.WallboardDto);
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $interval(function () {
        $scope.SaveRecords();
    }, 1800000);


    $interval(function () {
        $scope.SaveVASCampaign();
    }, 3600000);

    $scope.SaveVASCampaign = function () {
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';
        $http({
            method: 'POST',
            url: '/Wallboard/SaveVASCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },
    
    $interval(function () {
        if ($scope.IsViewCall === true) {
            $scope.IsViewCall = false;
            $scope.IsViewPerformance = true;
        } else if ($scope.IsViewCall === false) {
            $scope.IsViewCall = true;
            $scope.IsViewPerformance = false;
        }        
        $scope.InitializeWallboard();
    }, 100000);

    $scope.SaveRecords = function () {
        $scope.SearchParam.FromDate += ' 01:00 AM';
        $scope.SearchParam.ToDate += ' 11:59 PM';
        $http({
            method: 'POST',
            url: '/Wallboard/SaveRecords',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.InitializeWallboard();
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();    
        //$scope.SaveVASCampaign();
        $scope.SaveRecords();
    },
    $scope.initPage();

    /*========== INIT PAGE ===========*/

}).filter('unsafe', function ($sce) {
    return function (val) {
        return $sce.trustAsHtml(val);
    };
}).directive('uploadFile', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var file_uploaded = $parse(attrs.uploadFile);
            element.bind('change', function () {
                scope.$apply(function () {
                    file_uploaded.assign(scope, element[0].files[0]);
                });
            });
        }
    };
}]);