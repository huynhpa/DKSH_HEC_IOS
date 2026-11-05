'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'lr.upload'])
.controller('CallingTimeController', function ($scope, $http, $location, $window, $log, $rootScope, $q, upload, FileSaver, Blob) {
    var deferred = $q.defer();
    $scope.CallingTimeList = [];
    $scope.totalItems = 0;
    $scope.CallingTimeDto = {
        id: ''
        , CustomerCode: ''
        , PhoneNumber: ''
        , Priority1: ''
        , Priority2: ''
    };

    $scope.SearchCallingTime = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/CallingTime/SearchCallingTime',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CallingTimeDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.CallingTimeList = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    }

    $scope.SetCallingTime = function () {
        if (IsNullOrEmpty($scope.CallingTimeDto.CustomerCode)) {
            ErrorMessage('Please input customer code');
            return false;
        } else {
            $.confirm({
                title: 'Confirm!',
                type: 'blue',
                content: 'Do you wanna to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            angular.element('#Loading').LoadingOverlay("show");
                            $scope.CallingTimeDto.Priority1 = $('#dtFromTime1').val() + ' - ' + $('#dtToTime1').val();
                            $scope.CallingTimeDto.Priority2 = $('#dtFromTime2').val() + ' - ' + $('#dtToTime2').val();
                            deferred.notify();
                            $http({
                                method: 'POST',
                                url: '/CallingTime/SetCallingTime',
                                headers: [{ 'Content-Type': 'application/json' }],
                                params: $scope.CallingTimeDto
                            }).then(function (response) {
                                deferred.resolve(response);
                                if (response.data.code === 'ERROR') {
                                    ErrorMessage(response.data.message);
                                } else if (response.data.code === 'SUCCESS') {
                                    SuccessMessage(response.data.message);
                                }
                                angular.element('#Loading').LoadingOverlay("hide");
                            }, function (response) {
                                ErrorMessage(response.data);
                                angular.element('#Loading').LoadingOverlay("hide");
                                return deferred.reject(response.data);
                            });
                            return deferred.promise;
                        }
                    }, cancel: { btnClass: 'btn-pinterest', action: function () { } }
                }
            });
        }        
    }

    $scope.Clear = function () {
        $scope.CallingTimeDto = {
            id: ''
            , CustomerCode: ''
            , PhoneNumber: ''
            , Priority1: ''
            , Priority2: ''
        };
    }


    /*=========== PAGINATION=========== */
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
    /*=========== PAGINATION=========== */
    /*====== INIT PAGE ======*/
    $scope.initPage = function () {
        $scope.SearchCallingTime();
    }
    $scope.initPage();
    /* ===== INIT PAGE ====== */
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