'use strict';
var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'ngSanitize', 'lr.upload'])
dkshApp.controller('MonitorFormController', function ($scope, $http, $location, $window, $log, $q, upload, FileSaver, Blob, $sce, $sanitize) {
    var deferred = $q.defer();

    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = {
        
    };
    $scope.RecordFile = '';
    $scope.MonitorFormList = [];
    $scope.MonitorHistoryList = [];
    /*========= PAGING =========*/
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['20', '50', '100']
    };
    $scope.callback = function () { }
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['20', '50', '100']
        };
    }
    /*========= PAGING =========*/

    // SEARCH
    $scope.Search = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/MonitorForm/SearchMonitor',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.MonitorFormList = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });

    }

    $scope.MonitorHistory = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/MonitorForm/MonitorHistory',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.MonitorHistoryList = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });

    }

    
    /*========== RESET ===========*/
    $scope.Reset = function () {
        $scope.SearchParam = {
            
        };
    }

    /*========== INIT PAGE ===========*/
    $scope.UserSession = { UserId: '', UserName: '', IsAdmin: '' };
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();
    }
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