'use strict';
var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination','ngFileSaver','lr.upload'])
dkshApp.controller('ProductInformationController', function ($scope, $http, $location, $window, $log, $q, upload, FileSaver, Blob) {
    var deferred = $q.defer();
    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = {
        Supplier: '', ProductID: '', ProductName: '', PrdHierarchy: '', Category: '',
        Brand: '', ProductGroup: '', ProductSubGroup: '', VAT: '',
        SellingPrice: '', GenericName: '', UnitOfMeasure: '', SalesUnit: ''
    }
    $scope.ProductModel = {
        Supplier: '', ProductID: '', ProductName: '', PrdHierarchy: '', Category: '',
        Brand: '', ProductGroup: '', ProductSubGroup: '', VAT: '',
        SellingPrice: '', GenericName: '', UnitOfMeasure: '', SalesUnit: ''
    };
    $scope.ListProduct = [];
    $scope.LastUpdateDate = '';
    $scope.Suppliers = []; // SELECT OPTIONS Suppliers
    $scope.ProductFile = '';
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
    $scope.SearchProduct = function () {
        $('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/MedicalDevided/SearchProduct',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListProduct = response.data;
            $scope.LastUpdateDate = $scope.ListProduct.length > 0 ? $scope.ListProduct[0].UpdateDate : '';
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            $('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            $('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });        
    }

    /*============= Get Suppliers ==============*/
    $scope.GetSuppliers = function () {
        $http({
            method: 'GET',
            url: '/MedicalDevided/GetSuppliers',
            headers: [{ 'Content-Type': 'application/json' }],
            params: null
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.Suppliers = response.data;
        }, function (response) {
            console.log(response.data);
            return deferred.reject(response.data);
        });
    }

    /*============= Upload customer ==============*/
    $scope.UploadProduct = function () {
        if (IsNullOrEmpty($('#txtProductFile').val())) {
            WarningMessage('There is no file to upload');
            return false;
        } else {
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: 'Upload data may be take a few minutes. Do you want to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-green-leaf',
                        action: function () {
                            $scope.SaveProductInformation();
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
    }
    $scope.SaveProductInformation = function () {
        $('#Loading').LoadingOverlay("show");
        upload({
            url: '/MedicalDevided/UploadProduct',
            method: 'POST',
            data: {
                aFile: $scope.ProductFile
            }
        }).then(function (response) {
            $('#Loading').LoadingOverlay("hide");
            if (response.data.code === 'SUCCESS') {
                $.alert({
                    title: 'Message',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $('#mdProductInformation').modal('hide');
                                $scope.SearchProduct();
                            }
                        }
                    }
                });
            } else {
                ErrorMessage(response.data.message);
            }
        }, function (response) {
            $('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data.message);
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
        $scope.ProductInfo = {};
        $http({
            method: 'GET',
            url: '/MedicalDevided/ProductInformationTemplate',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ProductInfo,
            responseType: 'blob'
        }).then(function (response) {
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'ProductInformation.xlsx');
        }, function (response) {
            ErrorMessage('Unexpected Error while getting data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
    }


    /*========== INIT PAGE ===========*/
    $scope.initPage = function () {
        $scope.GetSuppliers();
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