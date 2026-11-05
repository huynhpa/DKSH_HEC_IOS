'use strict';

var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'lr.upload'])
dkshApp.controller('SFEReportController', function ($scope, $http, $location, $window, $log, $q, FileSaver, Blob, upload) {
    var deferred = $q.defer();

    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = {
        PDACode: '', FromDate: GetCurrentDate(), ToDate: GetCurrentDate(), months: GetCurrentMonth(),
        SFACode: '', Position: '', ASMCode: '', ASMFLMSalesman: '', ConstantType: '', working_month: GetCurrentMonth()
    },
    $scope.SFEReportConstList = [];
    $scope.SFEPerformanceReportList = [];
    $scope.TargetCallFile = '';
    $scope.UserSession = {
        UserId: '', UserName: '', IsAdmin: ''
    },
    /*========= PAGING =========*/
    
        
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['10', '20', '50']
    };
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

    $scope.SFEPerformanceReport = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/SFEReport/SFEPerformanceReport',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.SFEPerformanceReportList = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        }); 
    },

    // Export SFEPerformance
    $scope.ExportSFEPerformance = function () {
        angular.element('#Loading').LoadingOverlay("show");
        var fileNm = 'SFEPerformance_' + $scope.SearchParam.months + '.xlsx';
        $http({
            method: 'GET',
            url: '/SFEReport/ExportSFEPerformance',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, fileNm);
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.UploadTargetCall = function () {
        if (IsNullOrEmpty($('#txtTargetCall').val())) {
            ErrorMessage('There is no file to upload');
            return false;
        } else {
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: 'Do you want to upload?',
                buttons: {
                    submit: {
                        btnClass: 'btn-success',
                        action: function () {
                            $('#Loading').LoadingOverlay("show");
                            upload({
                                url: '/SFEReport/SaveTargetCallTeleSale',
                                method: 'POST',
                                data: {
                                    aFile: $scope.TargetCallFile
                                },
                                params: $scope.SearchParam
                            }).then(function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                if (response.data.code === 'SUCCESS' && response.data.number > 0) {
                                    SuccessMessage(response.data.message);
                                } else if (response.data.code === 'ERROR') {
                                    ErrorMessage(response.data.message);
                                }
                            }, function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                console.error(response.data);
                            });
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            }); 
        }
    },

    $scope.SFEOrgchart = function () {
        $http({
            method: 'GET',
            url: '/SFEReport/SFEOrgchart',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.SFEOrgchartList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /*========== INIT PAGE ===========*/
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();
        $scope.SFEOrgchart();
        $scope.SFEPerformanceReport();
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