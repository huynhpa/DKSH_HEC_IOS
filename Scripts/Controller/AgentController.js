'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'lr.upload'])
.controller('AgentController', function ($scope, $http, $location, $window, $log, $rootScope, $q, upload, FileSaver, Blob) {
    var deferred = $q.defer();
    $scope.isUpdate = false;
    $scope.ListAgent = [];
    $scope.RoleList = [];
    $scope.RoleByUser = [];
    $scope.SearchParam = { UserName: '', AgentNumber: '', GroupCode: '' };
    $scope.AllRoles = '';
    $scope.UsersModel = {
        ApplicationId: '', UserId: '', UserName: '', LoweredUserName: '', MobileAlias: '', IsAnonymous: '', LastActivityDate: '', IsDeleted: '',
        IsApproved: '', IsLockedOut: '', AgentNumber: '', FullName: '', LastName: '', EmployeeID: '', ProjectName: '', RoleId: ''
    };
    $scope.hrEmployeeDto = {
        ID: '', EmployeeID: '', BranchId: '', WindowsUser: '', AgentNumber: '', FullName: '', LastName: '', Gender: '', GroupID: '', GroupCode: '', DOB: '', CreatedBy: ''
        , CreatedDate: '', UpdatedBy: '', UpdatedDate: '', IsDeleted: '', RoleId: '', Password: '',
        PasswordSalt: '', IsApproved: '', IsLockedOut: '', contracts: '', SFECode: ''
    };
    $scope.MemberDto = {
        ApplicationId: '', UserId: '', Password: '', PasswordFormat: '', PasswordSalt: '', Email: '', LoweredEmail: '', IsApproved: '', IsLockedOut: '', CreateDate: ''
        , LastLoginDate: '', LastPasswordChangedDate: '', LastLockoutDate: '', Comment: '', UserName: '', Extension: '', RoleId: ''
    };
    $scope.ProjectList = [];
    $scope.totalItems = 0;
    $scope.SelectedRole = [];
    // Search Agent
    $scope.SearchAgent = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/Agent/SearchAgent',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListAgent = response.data;
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

    // Get Agent Detail
    $scope.GetAgentDetail = function (id) {
        deferred.notify();
        $scope.UsersModel.UserId = id;
        $scope.isUpdate = true;
        $http({
            method: 'GET',
            url: '/Agent/GetAgentDetail',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.UsersModel
        }).then(function (response) {
            deferred.resolve(response);
            if (!IsNullOrEmpty(response.data)) {
                $scope.UsersModel = response.data.UserDetail;
                $scope.hrEmployeeDto = response.data.hrEmployee;
                $scope.RoleByUser = response.data.RoleByUser;
                $scope.MemberDto.IsApproved = response.data.UserDetail.IsApproved;
                $scope.MemberDto.IsLockedOut = response.data.UserDetail.IsLockedOut;
                console.log("DATA : ", response.data);
                if ($scope.RoleList != null && $scope.RoleList.length > 0 && $scope.RoleByUser.length > 0) {
                    for (var i = 0; i < $scope.RoleByUser.length; i++) {
                        for (var j = 0; j < $scope.RoleList.length; j++) {
                            if ($scope.RoleByUser[i].RoleId === $scope.RoleList[j].RoleId) {
                                $('#role_' + [j]).prop('checked', true);
                                break;
                            }
                        }
                    }
                }
                $('#mdAgent').modal({ backdrop: 'static' }, 'show');
            }
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    }

    // Insert Agent
    $scope.InsertAgent = function () {
        deferred.notify();
        $http({
            method: 'POST',
            url: '/Agent/InsertAgent',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.hrEmployeeDto
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'SUCCESS') {
                $.alert({
                    title: 'Message',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $('#mdAgent').modal('hide');
                                $scope.SelectedRole = [];
                                $scope.SearchAgent();
                            }
                        }
                    }
                });
            } else if (response.data.code === 'ERROR') {
                $.alert({
                    title: 'Error',
                    type: 'red',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $scope.SelectedRole = [];
                            }
                        }
                    }
                });
            } 
        }, function (response) {
            return deferred.reject(response.data);
        });
    }


    // Update Agent
    $scope.UpdateAgent = function () {
        deferred.notify();
        $http({
            method: 'POST',
            url: '/Agent/UpdateAgent',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.hrEmployeeDto
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'SUCCESS') {
                $.alert({
                    title: 'Message',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $('#mdAgent').modal('hide');
                                $scope.SelectedRole = [];
                                $scope.SearchAgent();
                            }
                        }
                    }
                });
            } else if (response.data.code === 'ERROR') {
                $.alert({
                    title: 'Error',
                    type: 'red',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                $scope.SelectedRole = [];
                            }
                        }
                    }
                });
            }    
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    }
    // Update IsDelete = true
    $scope.DeleteAgent = function (obj) {
        $scope.UsersModel = obj;
        $.confirm({
            title: 'Confirm!',
            type: 'red',
            content: 'Do you want to delete Agent <b class="text-blue">' + obj.UserName + '</b>?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        deferred.notify();
                        $http({
                            method: 'POST',
                            url: '/Agent/DeleteAgent',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.UsersModel
                        }).then(function (response) {
                            deferred.resolve(response);
                            if (response.data.code === "DELETE") {
                                $.alert({
                                    title: 'Message!',
                                    type: 'red',
                                    content: response.data.message,
                                    buttons: {
                                        ok: {
                                            btnClass: 'btn-red',
                                            action: function () {
                                                $scope.SearchAgent();
                                            }
                                        }
                                    }
                                });
                                
                            } else if (response.data.code === "ERROR") {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            ErrorMessage('Unexpected error while delete data!!');
                            return deferred.reject(response.data);
                        });
                    }
                }, cancel: { btnClass: 'btn-pinterest', action: function () { } }
            }
        });        
    }

    $scope.ResetPassword = function (obj) {
        $scope.UsersModel = obj;
        $.confirm({
            title: 'Confirm!',
            type: 'red',
            content: 'Do you want to reset password for Agent <b class="text-blue">' + obj.UserName + '</b>?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        deferred.notify();
                        $http({
                            method: 'POST',
                            url: '/Agent/ResetPassword',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.UsersModel
                        }).then(function (response) {
                            deferred.resolve(response);
                            if (response.data.code == "SUCCESS") {
                                SuccessMessage(response.data.message);
                            } else if (response.data.code == "ERROR") {
                                ErrorMessage(response.data.message);
                            }
                        }, function (response) {
                            ErrorMessage('Opp, something went wrong. Please contact admin!!');
                            return deferred.reject(response.data);
                        });
                    }
                }, cancel: { btnClass: 'btn-pinterest', action: function () { } }
            }
        }); 
    }


    // Validation
    $scope.Validate = function () {
        var label = '';
        if ($scope.isUpdate) {
            label = 'Username';
        } else {
            label = 'Login ID';
        }
        if (IsNullOrEmpty($scope.hrEmployeeDto.WindowsUser)) {
            ErrorMessage(label + ' is required');
            return false;
        }
        if (IsNullOrEmpty($scope.hrEmployeeDto.FullName)) {
            ErrorMessage('FullName is required');
            return false;
        }
        if (IsNullOrEmpty($scope.hrEmployeeDto.GroupCode)) {
            ErrorMessage('GroupCode is required');
            return false;
        }
        
        if (!$scope.isUpdate) {
            if (IsNullOrEmpty($('#txtPassword').val())) {
                ErrorMessage('Password is required');
                return false;
            }
            if (IsNullOrEmpty($('#txtRetypePassword').val())) {
                ErrorMessage('Retype password is required');
                return false;
            }
            if ($('#txtRetypePassword').val() !== $('#txtPassword').val()) {
                ErrorMessage('Retype password is does not match');
                return false;
            }
            if (IsNullOrEmpty($scope.hrEmployeeDto.AgentNumber)) {
                ErrorMessage('Extension is required');
                return false;
            }
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
                            for (var i = 0; i < $scope.RoleList.length; i++) {
                                if ($("#role_" + i).prop('checked') == true) {
                                    $scope.SelectedRole.push($scope.RoleList[i].RoleId);
                                }
                            }
                            $scope.hrEmployeeDto.RoleId = $scope.SelectedRole.toString();
                            $scope.hrEmployeeDto.IsApproved = $scope.MemberDto.IsApproved;
                            $scope.hrEmployeeDto.IsLockedOut = $scope.MemberDto.IsLockedOut;
                            if ($scope.isUpdate) {
                                $scope.UpdateAgent();
                            } else {
                                $scope.InsertAgent();
                            }
                        }
                    }, cancel: { btnClass: 'btn-pinterest', action: function() { } }
                }
            });
        }
    },

    // upload assign agent into project
    $scope.UploadAssignAgentProject = function () {
        if (IsNullOrEmpty($('#txtAgent2Project').val())) {
            ErrorMessage('There is no file to upload');
            return false;
        } else {
            $('#Loading').LoadingOverlay("show");
            upload({
                url: '/Agent/UploadAssignAgentProject',
                method: 'POST',
                data: {
                    aFile: $scope.Agent2ProjectFile
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
                                    $scope.CampaignByCustomer();
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
    },

    $scope.AssignAgentProjectTemplate = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'GET',
            url: '/Agent/AssignAgentProjectTemplate',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam,
            responseType: 'blob'
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            var data = new Blob([response.data], { type: response.data.type });
            FileSaver.saveAs(data, 'AssignAgentProject.xlsx');
        }, function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
            ErrorMessage('Opp... Something went wrong. Please contact admin!!');
            return deferred.reject(response.data);
        });
    },

    $scope.GetProjectList = function () {
        $http({
            method: 'GET',
            url: '/SearchCall/GetProjectList',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ProjectList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },

    $scope.GetRoleList = function () {
        $http({
            method: 'GET',
            url: '/Role/GetRoleList',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.SearchParam
        }).then(function (response) {
            deferred.resolve(response);
            $scope.RoleList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            return deferred.reject(response.data);
        });
    },
    /*============================*/
    $scope.ClearParam = function () {
        $scope.SearchParam = { UserName: '', AgentNumber: '', GroupCode: '' };
    },
    /* clear model */
    $scope.ClearModel = function () {
        $scope.isUpdate = false;
        $("[id^='role_']").prop('checked', false);
        $scope.RoleByUser = [];
        $scope.UsersModel = {
            ApplicationId: '', UserId: '', UserName: '', LoweredUserName: '', MobileAlias: '', IsAnonymous: '', LastActivityDate: '', IsDeleted: '',
            IsApproved: '', IsLockedOut: '', AgentNumber: '', FullName: '', LastName: '', EmployeeID: '', ProjectName: '', RoleId: ''
        };
        $scope.hrEmployeeDto = {
            ID: '', EmployeeID: '', BranchId: '', WindowsUser: '', AgentNumber: '', FullName: '', LastName: '',
            Gender: '', GroupCode: '', GroupID: '', DOB: '', CreatedBy: '', contracts: '', SFECode: ''
            , CreatedDate: '', UpdatedBy: '', UpdatedDate: '', IsDeleted: '', RoleId: '', Password: '', PasswordSalt: ''
        };
        $scope.MemberDto = {
            ApplicationId: '', UserId: '', Password: '', PasswordFormat: '', PasswordSalt: '', Email: '', LoweredEmail: '', IsApproved: '', IsLockedOut: '', CreateDate: ''
            , LastLoginDate: '', LastPasswordChangedDate: '', LastLockoutDate: '', Comment: '', UserName: '', Extension: '', RoleId: ''
        };
    }
    /* check | uncheck all roles */
    $scope.CheckAllRole = function () {
        if ($('#chkAll').prop('checked') == false) {
            for (var i = 0; i < $scope.RoleList.length; i++) {
                $("[id^='role_']").prop('checked', false);
            }
        } else {
            for (var i = 0; i < $scope.RoleList.length; i++) {
                $("[id^='role_']").prop('checked', true);
            }
        }
    },

    /*=========== CheckUserInRole ===========*/
    
    $scope.CheckUserInRole = function (item) {
        var IsAdministrator = false;
        $scope.UsersModel = item;
        $http({
            method: 'GET',
            url: '/Agent/CheckUserInRole',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.UsersModel
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'SESSION_EXPIRED_MESSAGE') {
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
                $scope.RoleByUser = response.data.RoleByUser;
                //for (var i = 0; i < $scope.RoleByUser.length; i++) {
                //    if ($scope.RoleByUser[i].RoleName === 'ADMINISTRATOR') {
                //        $scope.IsAdministrator = true;
                //        break;
                //    }
                //}
            }
            return IsAdministrator;
        }, function (response) {
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
    $scope.callback = function () { }
    $scope.SetPaging = function () {
        $scope.settings = {
            currentPage: 0,
            offset: 0,
            pageLimit: 10,
            pageLimits: ['20', '50', '100']
        };
    },
    /*=========== PAGINATION=========== */
    $scope.UserSession = { UserId: '', UserName: '', IsAdmin:'' };
    /*====== INIT PAGE ======*/
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();
        $scope.SearchAgent();
        $scope.GetProjectList();
        $scope.GetRoleList();
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