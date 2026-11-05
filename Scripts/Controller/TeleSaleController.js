'use strict';
var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver'])
dkshApp.controller('TeleSaleController', function ($scope, $http, $location, $window, $log, $q, FileSaver, Blob) {
    var deferred = $q.defer();

    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = { }
    $scope.List = [];
    $scope.CampaignList = [];
    /*========= PAGING =========*/
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 20,
        pageLimits: ['20', '50', '100']
    };
    $scope.callback = function () { }
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 20,
            pageLimits: ['20', '50', '100']
        };
    }
    /*========= PAGING =========*/

    
    /*========== INIT PAGE ===========*/
    $scope.initPage = function () {
    }
    $scope.initPage();

    /*========== INIT PAGE ===========*/

});