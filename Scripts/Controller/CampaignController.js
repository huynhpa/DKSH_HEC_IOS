'use strict';
var dkshApp = angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'ngSanitize'])
dkshApp.controller('CampaignController', function ($scope, $http, $location, $window, $log, $q, FileSaver, Blob, $sce, $sanitize) {
    var deferred = $q.defer();

    /*========= MODEL, PARAM =========*/
    $scope.totalItems = 0; // Get total row
    $scope.SearchParam = { ProjectID: '', CreatedDate: '', CreatedBy: '', Status: '1', ServiceName: '' };
    $scope.ListServiceType = [];
    $scope.ServiceTypeDto = {
        ID: ''
        ,ServiceCode: ''
        ,ServiceName: ''
        ,CurrentID: ''
        ,StartAlert: ''
        ,EndAlert: ''
        ,IsActive: true
        ,CreatedBy: ''
        ,CreatedDate: ''
        ,UpdatedBy: ''
        ,UpdatedDate: ''
        ,IsDeleted: ''
        ,ProjectID: ''
        ,ProjectIDList: ''
        ,ProductCode: ''
        ,Client: ''
        ,StartDate: ''
        ,EndDate: ''
        ,CustomerApplied: ''
    };
    $scope.ProjectList = [];
    /*========= PAGING =========*/
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['20', '50', '100']
    };
    $scope.isUpdate = false;
    $scope.callback = function () { };
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['20', '50', '100']
        };
    },
    /*========= PAGING =========*/

    // SEARCH
    $scope.Search = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Campaign/Search',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.ListServiceType = response.data.ServiceTypeList;
            $scope.totalItems = response.data.ServiceTypeList.length;
            $scope.SetPaging();            
        }, function (response) {            
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });

    },
    /*========== RESET ===========*/
    $scope.Reset = function () {
        $scope.SearchParam = { ProjectID: '', CreatedDate: '', CreatedBy: '', Status: '1' };
    },

    /*========== Get Campaign ==========*/
    $scope.GetCampaign = function () {        
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Campaign/GetCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            $scope.ListServiceType = response.data.ServiceTypeList;
            $scope.ProjectList = response.data.ProjectList;
            $scope.totalItems = response.data.ServiceTypeList.length;
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /* get campaign detail */
    $scope.CampaignDetail = function (item) {
        $scope.isUpdate = true;
        $scope.ServiceTypeDto = item;
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Campaign/CampaignDetail',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ServiceTypeDto
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            if (!IsNullOrEmpty(response.data)) {
                $scope.ServiceTypeDto = response.data;
            }            
            $('#txtStartAlert').summernote('code', $scope.ServiceTypeDto.StartAlert);
            $('#txtEndAlert').summernote('code', $scope.ServiceTypeDto.EndAlert);
            $('#mdCampaign').modal({ backdrop: 'static' }, 'show');
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
        
    },

    /* InsertCampaign */
    $scope.InsertCampaign = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'POST',
            url: '/Campaign/InsertCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            data: $scope.ServiceTypeDto
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            if (response.data.code === 'SUCCESS') {
                $.alert({
                    title: 'Message!',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $('#mdCampaign').modal('hide');
                                $scope.Search();
                            }
                        }
                    }
                });
            } else if (response.data.code === 'ERROR') {
                ErrorMessage(response.data.message);
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
            }
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /* UpdateCampaign */
    $scope.UpdateCampaign = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'POST',
            url: '/Campaign/UpdateCampaign',
            headers: [{ 'Content-Type': 'application/json' }],
            data: $scope.ServiceTypeDto
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            deferred.resolve(response);
            if (response.data.code === 'SUCCESS') {
                $.alert({
                    title: 'Message!',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $('#mdCampaign').modal('hide');
                                $scope.Search();
                            }
                        }
                    }
                });
            } else if (response.data.code === 'ERROR') {
                ErrorMessage(response.data.message);
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
            }
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    /* save campaign information */
    $scope.SaveCampaign = function () {
        if (IsNullOrEmpty($scope.ServiceTypeDto.ServiceName)) {
            ErrorMessage('Campaign name is required');
            return false;
        }
        //if (IsNullOrEmpty($scope.ServiceTypeDto.ProjectIDList)) {
        //    ErrorMessage('Project is required');
        //    return false;
        //}
        else {
            $scope.ServiceTypeDto.StartAlert = $('#txtStartAlert').summernote('code');
            $scope.ServiceTypeDto.EndAlert = $('#txtEndAlert').summernote('code');
            if (!IsNullOrEmpty($scope.ServiceTypeDto.ProjectIDList)) {
                $scope.ServiceTypeDto.ProjectIDList = $scope.ServiceTypeDto.ProjectIDList.toString();
            }
            else if ($scope.isUpdate === true && IsNullOrEmpty($scope.ServiceTypeDto.ProjectIDList)) {
                $scope.ServiceTypeDto.ProjectIDList = $scope.ServiceTypeDto.ProjectID;
            }
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: 'Do you want to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-green',
                        action: function () {
                            if ($scope.isUpdate) {
                                $scope.UpdateCampaign();
                            } else {
                                $scope.InsertCampaign();
                            }
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        } 
        
    },

    $scope.DeleteCampaign = function (obj) {
        $scope.ServiceTypeDto = obj;
        $.confirm({
            title: 'Confirm!',
            type: 'red',
            content: 'Do you want to delete Campaign ' + obj.ServiceName + '</b>?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        deferred.notify();
                        $http({
                            method: 'POST',
                            url: '/Campaign/DeleteCampaign',
                            headers: [{ 'Content-Type': 'application/json' }],
                            data: $scope.ServiceTypeDto
                        }).then(function (response) {
                            deferred.resolve(response);
                            if (response.data.code == "DELETE") {
                                $.alert({
                                    title: 'Message!',
                                    type: 'green',
                                    content: response.data.message,
                                    buttons: {
                                        ok: {
                                            btnClass: 'btn-green',
                                            action: function () {
                                                $scope.Search();
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
                            }
                        }, function (response) {
                            ErrorMessage('Unexpected error while delete data!!');
                            return deferred.reject(response.data);
                        });
                    }
                }, cancel: { btnClass: 'btn-pinterest', action: function () { } }
            }
        });
    },



    $scope.ClearModel = function () {
        $('#txtStartAlert').summernote('code', '');
        $('#txtEndAlert').summernote('code', '');
        $scope.isUpdate = false;
        $scope.ServiceTypeDto = {
            ID: ''
            , ServiceCode: ''
            , ServiceName: ''
            , CurrentID: ''
            , StartAlert: ''
            , EndAlert: ''
            , IsActive: true
            , CreatedBy: ''
            , CreatedDate: ''
            , UpdatedBy: ''
            , UpdatedDate: ''
            , IsDeleted: ''
            , ProjectID: ''
            , ProjectIDList: ''
            , ProductCode: ''
            , Client: ''
            , StartDate: ''
            , EndDate: ''
            , CustomerApplied: ''
        };
    },

    /*========== INIT PAGE ===========*/
    $scope.UserSession = { UserId: '', UserName: '', IsAdmin: '' };
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();
        $scope.GetCampaign();
    },
    $scope.initPage();

    /*========== INIT PAGE ===========*/

}).filter('unsafe', function ($sce) {
    return function (val) {
        return $sce.trustAsHtml(val);
    };
});