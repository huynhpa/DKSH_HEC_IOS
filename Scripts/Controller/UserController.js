'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination'])
.controller('UserController', function ($scope, $http, $location, $window, $log, $rootScope, $q) {
    var deferred = $q.defer();
    // User to search
    $scope.UserSearch = {
        UserId: '',
        UserName: '',
        RoleId: '',
        Fullname: '',
        Email: '',
        Team: '',
        IsDelete: false,
        IsActive: true,
        JoinDate: null,
        JoinDateFrom: null,
        JoinDateTo: null
    }
    // User To Save
    $scope.UserModel = {
        Id: '',
        UserId: '',
        UserName: '',
        RoleId: '',
        Fullname: '',
        Email: '',
        Pwd: '',
        Team: '',
        JobTitle: '',
        Department: 'Customer Care Center',
        IsDelete: '',
        IsActive: '',
        JoinDate: '',
        IsManager: '',
        CreateBy: '',
        CreateDate: '',
        UpdateBy: '',
        UpdateDate: '',
        ScreenCode: '',
        IsUsing: ''
    }
    $scope.isUpdate = false;
    $scope.ListUser = [{}]; // List user data
    /* Empty User Model */
    $scope.EmptyUserModel = function () {
        $scope.isUpdate = false;
        $scope.UserModel = {
            Id: '',
            UserId: '',
            UserName: '',
            RoleId: '',
            Fullname: '',
            Email: '',
            Team: '',
            JobTitle: '',
            Department: 'Customer Care Center',
            IsDelete: '',
            IsActive: '',
            JoinDate: '',
            IsManager: '',
            CreateBy: '',
            CreateDate: '',
            UpdateBy: '',
            UpdateDate: '',
            ScreenCode: ''
        }
    }

    /*=========== PAGINATION=========== */
    $scope.settings = {
        currentPage: 0,
        offset: 0,
        pageLimit: 10,
        pageLimits: ['20', '50', '100']
    };
    $scope.callback = function () { console.log("Paging setting : ", $scope.settings); }
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['20', '50', '100']
        };
    }
    /*=========== PAGINATION=========== */

    // Search User
    $scope.searchUser = function () {
        $('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/User/SearchUser',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.UserSearch
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            $scope.ListUser = response.data;
            $scope.totalRows = response.data.length;
            $scope.SetPaging();
            $('#Loading').LoadingOverlay("hide");
        }, function (response) {
            $('#Loading').LoadingOverlay("hide");
            ErrorMessage(response.data);
            console.log(response.data);
            return deferred.reject(response.data);
            
        });
    }

    /* GET USER DETAIL */
    $scope.getUserDetail = function (Id) {
        $scope.isUpdate = true;
        $scope.Id = { Id : Id };
        $http({
            method: 'GET',
            url: '/User/GetUserDetail',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.Id
        }).then(function (response) {
            deferred.resolve(response);
            $scope.UserModel = response.data;
        }, function (response) {
            $.alert({
                title: 'Alert!',
                type: 'red',
                content: 'Unexpected error while get data!!',
            });
            return deferred.reject(response.data);
        });

    };
    /* SAVE USER FOR UPDATE AND INSERT */
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
                                $scope.updateUser();
                            }
                            else {
                                $scope.addUser();
                            }
                        }
                    }, cancel: function () { }
                }
            });
        }
    }

    /* ADD USER */
    $scope.addUser = function () {
        $http({
            method: 'POST',
            url: '/User/InsertUser',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.UserModel
        }).then(function (response) {
            deferred.resolve(response);
            console.log("Response : ", response);
            if (response.data.code == 'EXISTS') {
                $.alert({
                    title: 'Alert!',
                    type: 'red',
                    content: response.data.message
                });
            } else if (response.data.code === 'INSERT') {
                $.alert({
                    title: 'Message!',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $('#modalUser').modal('hide');
                                $scope.ListUser = response.data.UserList;
                                $scope.SetPaging();
                            }
                        }
                    }                        
                });
            }
            
        }), function (response) {
            $.alert({
                title: 'Alert!',
                type: 'red',
                btnClass: 'btn-blue',
                content: 'Opps, Something went wrong !!!',
            });
            return deferred.reject(response.data);
        };
        $scope.UserModel = {};
    }
    /* UPDATE USER */
    $scope.updateUser = function () {
        $http({
            method: 'POST',
            url: '/User/UpdateUser',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.UserModel
        }).then(function (response) {
            if (response.data.code === 'UPDATE') {
                $.alert({
                    title: 'Message',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $scope.UserModel = {};
                                $('#modalUser').modal('hide');
                                $scope.ListUser = response.data.UserList;
                                $scope.SetPaging();
                            }
                        }
                    }
                });                    
            } else {
                ErrorMessage(response.data.message);
            }                
        }), function (response) {
            ErrorMessage(response.data.message);
        };
    }
    /* VALIDATE */
    $scope.Validate = function () {
        if (IsNullOrEmpty($scope.UserModel.UserId)) {
            ErrorMessage('User id is required !');
            return false;
        }
        if (IsNullOrEmpty($scope.UserModel.UserName)) {
            ErrorMessage('User name is required !');
            return false;
        }
        if (IsNullOrEmpty($scope.UserModel.RoleId)) {
            ErrorMessage('Role is required !');
            return false;
        }

        if (IsNullOrEmpty($scope.UserModel.Email)) {
            ErrorMessage('Email is required !');
            return false;
        }
        if (IsNullOrEmpty($scope.UserModel.Fullname)) {
            ErrorMessage('Fullname is required !');
            return false;
        }
        if (IsNullOrEmpty($scope.UserModel.Team)) {
            ErrorMessage('Team is required !');
            return false;
        }
        if (IsNullOrEmpty($scope.UserModel.Pwd) && $scope.isUpdate == false) {
            ErrorMessage('Password is required !');
            return false;
        }
        return true;
    }

    /*===== DELETE USER =====*/
    $scope.DeleteUser = function (User) {
        $scope.parameter = {
            Id: User.Id
        }
        $http({
            method: 'POST',
            url: '/User/DeleteUser',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.parameter
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'DELETE') {
                $.alert({
                    title: 'Message!',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-gree',
                            action: function () {
                                $('#modalUser').modal('hide');
                                $scope.ListUser = response.data.UserList;
                                $scope.totalRows = response.data.UserList.length;
                                $scope.SetPaging();
                            }
                        }
                    }
                });
            }                        
        }), function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        };
    }

    /*=============== VIEW USER PROFILE ===============*/
    $scope.ViewProfile = function (item) {
        $scope.param = item;
        $http({
            method: 'GET',
            url: '/User/ViewProfile',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.param
        }).then(function (response) {
            deferred.resolve(response);
        }, function (response) {
            console.log(response.data);
            return deferred.reject(response.data);
        });
    }

    /*================================*/
    $scope.DelUser = function (User) {
        $.confirm({
            title: 'Confirm!',
            type: 'blue',
            content: 'Do you want to delete <b>' + User.UserName + '</b> ?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        $scope.DeleteUser(User);
                    }
                }, cancel: function () { }
            }
        });
        
    }

    /*=========== ACTIVED USER ============*/
    $scope.ActivedUser = function (item) {
        $scope.UserModel = item;
        $.confirm({
            title: 'Confirm!',
            type: 'green',
            content: 'Do you want to actived user <b>' + item.Email + '-' + item.Team + '</b> ?',
            buttons: {
                active: {
                    btnClass: 'btn-blue',
                    action: function () {
                        $http({
                            method: 'POST',
                            url: '/User/ActivedUser',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.UserModel
                        }).then(function (response) {
                            if (response.data.code === 'ACTIVED') {
                                SuccessMessage(response.data.message);
                            } else {
                                $.alert({
                                    title: 'Message!',
                                    type: 'red',
                                    content: response.data.message,
                                    buttons: {
                                        ok: {
                                            btnClass: 'btn-red',
                                            action: function () {
                                                $window.location.hrel = '/Login';
                                            }
                                        }
                                    }
                                });
                            }
                        }), function (response) {
                            ErrorMessage(response.data.message);
                        };
                    }
                }, cancel: function () { }
            }
        });
    }

    /* ===== reset form ===== */
    $scope.resetForm = function () {
        $scope.UserSearch = {};
    }
    $scope.userSession = {
        Id: '',
        Role:''
    }

    $scope.Cancel = function () {
        $scope.UserModel = {};
    }

    //**** ==== ng-init ======******
    $scope.initPage = function () {
        $scope.searchUser();
        $scope.userSession.Id = $('#txtUserLogin').val();
        $scope.userSession.Role = $('#userRoleLogin').val();
    }
    $scope.initPage();
})