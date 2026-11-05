'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination'])
.controller('RoleController', function ($scope, $http, $location, $window, $log, $rootScope, $q) {
    var deferred = $q.defer();
    $scope.isUpdate = false;
    $scope.ListRole = [];

    $scope.SearchParam = { RoleName: '' };
    $scope.RoleModel = {
        ApplicationId: ''
        ,RoleId: ''
        ,RoleName: ''
        ,LoweredRoleName: ''
        ,Description: ''
    };
    $scope.totalItems = 0;
    $scope.ClearModel = function () {
        $scope.isUpdate = false;
        $scope.RoleModel = {
            ApplicationId: ''
            , RoleId: ''
            , RoleName: ''
            , LoweredRoleName: ''
            , Description: ''
        };
    }
    // Search Role
    $scope.SearchRole = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/Role/SearchRole',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListRole = response.data;
            $scope.totalItems = response.data.lenth;
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    }


    // Get Role Detail
    $scope.GetRoleDetail = function (item) {
        deferred.notify();
        $scope.RoleModel.RoleId = item.RoleId;
        $scope.isUpdate = true;
        $http({
            method: 'GET',
            url: '/Role/GetRoleDetail',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.RoleModel
        }).then(function (response) {
            deferred.resolve(response);
            if (!IsNullOrEmpty(response.data)) {
                $scope.RoleModel = response.data;
                $('#mdRole').modal({ backdrop: 'static' }, 'show');
            }
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    }

    // Insert role
    $scope.InsertRole = function () {
        deferred.notify();
        $http({
            method: 'POST',
            url: '/Role/InsertRole',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.RoleModel
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code == "SUCCESS") {
                $.alert({
                    title: 'Message!',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $scope.isUpdate = false;
                                $('#mdRole').modal('hide');
                                $scope.SearchRole();
                            }
                        }
                    }
                });
            } else if (response.data.code == "ERROR") {
                ErrorMessage(response.data.message);
            }
        }, function (response) {
            ErrorMessage("Unexpected error while insert data. Please contact admin!!");
            return deferred.reject(response.data);
        });
    }


    // Update role
    $scope.UpdateRole = function () {
        deferred.notify();
        $http({
            method: 'POST',
            url: '/Role/UpdateRole',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.RoleModel
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code == "SUCCESS") {
                $.alert({
                    title: 'Message!',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $scope.isUpdate = false;
                                $('#mdRole').modal('hide');
                                $scope.SearchRole();
                            }
                        }
                    }
                });
            } else if (response.data.code == "ERROR") {
                ErrorMessage(response.data.message);
            }
        }, function (response) {
            ErrorMessage("Unexpected error while update data. Please contact admin!!");
            return deferred.reject(response.data);
        });
    }
    // Update IsDelete = true
    $scope.DeleteRole = function (obj) {
        $scope.RoleModel = obj;
        $.confirm({
            title: 'Confirm!',
            type: 'red',
            content: 'Do you want to delete role <b>' + obj.RoleName + '</b> ?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        $http({
                            method: 'POST',
                            url: '/Role/DelRole',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.RoleModel
                        }).then(function (response) {
                            deferred.resolve(response);
                            if (response.data.code == "SUCCESS") {
                                $.alert({
                                    title: 'Message!',
                                    type: 'green',
                                    content: response.data.message,
                                    buttons: {
                                        ok: {
                                            btnClass: 'btn-green',
                                            action: function () {
                                                $('#mdRole').modal('hide');
                                                $scope.SearchRole();
                                            }
                                        }
                                    }
                                });
                            } else if (response.data.code === 'ERROR') {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            ErrorMessage("Unexpected error while delete data. Please contact admin!!");
                            return deferred.reject(response.data);
                        });
                    }
                }, cancel: {
                    btnClass: 'btn-pinterest',
                    action: function () { }
                }
            }
        });
        
    }

    // Validation
    $scope.Validate = function () {
        if (IsNullOrEmpty($scope.RoleModel.RoleName)) {
            ErrorMessage('Role name is required');
            return false;
        }
        return true;
    }

    // ACTION SAVE 
    $scope.Save = function () {
        if ($scope.Validate()) {
            $.confirm({
                title: 'Confirm!',
                type: 'blue',
                content: 'Do you want to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            if ($scope.isUpdate) {
                                $scope.UpdateRole();
                            } else {
                                $scope.InsertRole();
                            }
                        }
                    }, cancel: {
                        btnClass: 'btn-pinterest',
                        action: function () { }
                    }
                }
            });            
        }
    }


    /*=========== PAGINATION=========== */
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 20,
        pageLimits: ['20', '50', '100']
    };
    $scope.callback = function () { console.log("Paging setting : ", $scope.settings); }
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 20,
            pageLimits: ['20', '50', '100']
        };
    }
    /*=========== PAGINATION=========== */
    /*====== INIT PAGE ======*/
    $scope.UserSession = { UserId: '', UserName: '', IsAdmin: '' };
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();
        $scope.SearchRole();
    }
    $scope.initPage();
    /* ===== INIT PAGE ====== */
});