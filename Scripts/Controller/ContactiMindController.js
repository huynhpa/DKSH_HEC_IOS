'use strict';
var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination','ngFileSaver','lr.upload'])
dkshApp.controller('ContactiMindController', function ($scope, $http, $location, $window, $log, $q, upload, FileSaver, Blob) {
    var deferred = $q.defer();
    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = {
        cust_code: ''
        , cust_name: ''
        , phone_number: ''
        , addrs: ''
        , district: ''
        , province: ''
        , region: ''
        , channel: ''
        , cgrp_code: ''
        , cgrp_name: ''
    }
    $scope.isUpdate = false;
    $scope.ContactiMindModel = { };
    $scope.ListContactiMind = [];
    $scope.ListContactPhoneNumber = [];

    $scope.ContactiMindFile = '';
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

    /*========== SEARCH ===========*/
    $scope.SearchContactiMind = function () {
        $('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/ContactiMind/SearchContactiMind',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.ListContactiMind = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            $('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            $('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });        
    },


    $scope.ContactPhoneNumber = function (item) {
        $scope.SearchParam.cust_code = item.cust_code;
        $('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/ContactiMind/ContactPhoneNumber',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.ListContactPhoneNumber = response.data;
            $('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            $('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
    },

    $scope.DeletePhoneNumber = function (item) {
        $.confirm({
            title: 'Confirm!',
            type: 'orange',
            content: 'Do you sure want to delete phone number ' + item.phone_number + ' of customer code ' + item.cust_code + ' ?',
            buttons: {
                submit: {
                    btnClass: 'btn-green',
                    action: function () {
                        $scope.SearchParam.cust_code = item.cust_code;
                        $scope.SearchParam.phone_number = item.phone_number;
                        $http({
                            method: 'POST',
                            url: '/ContactiMind/DeletePhoneNumber',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.SearchParam
                        }).then(function (response) {
                            deferred.resolve(response);
                            if (response.data.code === 'SUCCESS') {
                                SuccessMessage(response.data.message);
                            } else if (response.data.code === 'ERROR') {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            ErrorMessage(response.data);
                            return deferred.reject(response.data);
                        });
                    }
                }, cancel: { btnClass: 'btn-pinterest' }
            }
        });        
    },



    /*========== RESET ===========*/
    $scope.resetForm = function () {
        $scope.SearchParam = {};
        $scope.SetPaging();        
    }
    /*========== RESET ===========*/


    /*========== INIT PAGE ===========*/
    $scope.initPage = function () {        
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