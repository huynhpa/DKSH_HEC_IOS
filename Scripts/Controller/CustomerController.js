'use strict';
var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination','ngFileSaver','lr.upload'])
dkshApp.controller('CustomerController', function ($scope, $http, $location, $window, $log, $q, upload, FileSaver, Blob) {
    var deferred = $q.defer();
    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = {
        CustCode: '', VNZ0Code: '', BravoCode: '', CustName: '', Addr: '', District: '', ZTerm: '', Province: '',
        Region: '', Channel: '', TaxCode: '', CGrpCode: '', IsDelete: false
    }
    $scope.isUpdate = false;
    $scope.CustomerModel = { };
    $scope.ListCustomer = [];
    $scope.LastUpdateDate = '';

    $scope.CustomerFile = '';
    /*========= MODEL, PARAM =========*/


    /*========= PAGING =========*/
    $scope.callback = function () {}
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 15,
            pageLimits: ['20', '50', '100']
        };
    }
    /*========= PAGING =========*/

    

    /*========== DATA PROCESS ===========*/
    /*========== SEARCH ===========*/
    $scope.SearchCustomer = function () {
        $('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Customer/SearchCustomer',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.ListCustomer = response.data;
            $scope.LastUpdateDate = $scope.ListCustomer.length > 0 ? $scope.ListCustomer[0].LastUpdateDate : '';
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            $('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            $('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });        
    }



    /*============= Upload customer ==============*/
    $scope.UploadCustomer = function () {
        $.confirm({
            title: 'Confirm!',
            type: 'orange',
            content: 'Upload data may be take a few minutes. Do you want to save ?',
            buttons: {
                submit: {
                    btnClass: 'btn-success',
                    action: function () {
                        $('#Loading').LoadingOverlay("show");
                        upload({
                            url: '/Customer/UploadCustomer',
                            method: 'POST',
                            data: {
                                aFile: $scope.CustomerFile
                            }
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
                                                $('#mdCustomer').modal('hide');
                                                $scope.SearchCustomer();
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

    $scope.UploadCertification = function () {
        $('#Loading').LoadingOverlay("show");
        upload({
            url: '/Customer/UploadCertification',
            method: 'POST',
            data: {
                aFile: $scope.CustomerFile
            }
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
                                $('#mdCustomer').modal('hide');                                
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
    


    /*========== DATA PROCESS ===========*/

    /*========== RESET ===========*/
    $scope.resetForm = function () {
        $scope.SearchParam = {};
        $scope.SetPaging();
        
    }
    /*========== RESET ===========*/

    $scope.Template = function () {
        $scope.CustomerInfo = {};
        $http({
            method: 'GET',
            url: '/Customer/Template',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.CustomerInfo,
            responseType: 'blob'
        }).then(function (response) {
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'CustomerInformation.xlsx');
        }, function (response) {
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    }


    /*========== INIT PAGE ===========*/
    $scope.initPage = function () {
        //$scope.SearchCustomer();
        $scope.SetPaging();
    }
    $scope.initPage();

    /*========== INIT PAGE ===========*/
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