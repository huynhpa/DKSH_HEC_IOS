'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'lr.upload'])
.controller('ImportPhoneNumberController', function ($scope, $http, $location, $window, $log, $rootScope, $q, upload, FileSaver, Blob) {
    var deferred = $q.defer();
    $scope.ListPhoneNumber = [];
    $scope.ListSummary = [];
    $scope.PhoneNumberFile = [];
    $scope.CampaignList = [];
    $scope.PhoneNumberDto = {
        ID: '', IntID: '', PhoneNumber: '', CustName: '', ServiceType: '', PhoneGroup: ''
        , CreatedBy: '', CreatedDate: GetCurrentDate(), UpdatedBy: '', UpdatedDate: ''
        , IsDeleted: '', IsAssigned: '', ProjectId: '', ProjectName: ''
    };

    /*======================*/
    $scope.SearchPhoneNumber = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/ImportPhoneNumber/SearchPhoneNumber',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.PhoneNumberDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListPhoneNumber = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    /* summary phone number */
    $scope.SummaryPhoneNumber = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/ImportPhoneNumber/SummaryPhoneNumber',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.PhoneNumberDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListSummary = response.data;
            $scope.totalItemsSummary = response.data.length;
            $scope.SetPagingSummary();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },
 
    /* Download phonenumber template */
    $scope.PhoneNumberTemplate = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/ImportPhoneNumber/PhoneNumberTemplate',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.PhoneNumberDto,
            responseType: 'blob'
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'PhoneNumberTemplate.xlsx');
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Unexpected error while getting file!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    },

    /* Upload PhoneNumber */
    $scope.UploadPhoneNumber = function () {
        if (IsNullOrEmpty($('#txtPhoneNumber').val())) {
            ErrorMessage('There is no file chosen to upload');
            return false;
        } if (IsNullOrEmpty($scope.PhoneNumberDto.ProjectId)) {
            ErrorMessage('Project is required');
            return false;
        } else {
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: 'Do you want to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-success',
                        action: function () {
                            $('#Loading').LoadingOverlay("show");
                            upload({
                                url: '/ImportPhoneNumber/UploadPhoneNumber',
                                method: 'POST',
                                data: {
                                    aFile: $scope.PhoneNumberFile
                                },
                                params: $scope.PhoneNumberDto
                            }).then(function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                if (response.data.code === 'SUCCESS' && response.data.number > 0) {
                                    $.alert({
                                        title: 'Message',
                                        type: 'green',
                                        content: response.data.message,
                                        buttons: {
                                            ok: {
                                                btnClass: 'btn-green',
                                                action: function () {                                                
                                                    $scope.SearchPhoneNumber();
                                                }
                                            }
                                        }
                                    });
                                } else if (response.data.code === 'SESSION_EXPIRED_MESSAGE') {
                                    $.alert({
                                        title: 'Message!',
                                        type: 'red',
                                        content: response.data.message,
                                        buttons: {
                                            ok: {
                                                btnClass: 'btn-red',
                                                action: function () {
                                                    $window.location.href = '/Login';
                                                }
                                            }
                                        }
                                    });
                                } else {
                                    ErrorMessage(response.data.message);
                                }
                            }, function (response) {
                                $('#Loading').LoadingOverlay("hide");
                                ErrorMessage("The wait operation timed out");
                                console.error(response.data);
                            });
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
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

    // get campaign
    $scope.GetProject = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Campaign/GetCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.ProjectList = response.data.ProjectList;
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /*=========== PAGINATION=========== */
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['20', '50', '100']
    };
    $scope.callback = function () { };
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['20', '50', '100']
        };
    },
    $scope.settingsSummary = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['20', '50', '100']
    };
    $scope.SetPagingSummary = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['20', '50', '100']
        };
    },

    $scope.Clear = function () {
        $('#txtPhoneNumber').val('');
        $scope.PhoneNumberDto = {
            ID: '', IntID: '', PhoneNumber: '', CustName: '', ServiceType: '', PhoneGroup: ''
            , CreatedBy: '', CreatedDate: GetCurrentDate(), UpdatedBy: '', UpdatedDate: ''
            , IsDeleted: '', IsAssigned: '', ProjectId: '', ProjectName: ''
        };
    },
    /*=========== PAGINATION=========== */
    /*====== INIT PAGE ======*/
    $scope.initPage = function () {
        // $scope.GetCampaignList();
        $scope.GetProject();
        // $scope.SearchPhoneNumber();
        $scope.SummaryPhoneNumber();
    },
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