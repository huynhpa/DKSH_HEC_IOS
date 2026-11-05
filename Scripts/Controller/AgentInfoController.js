'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination'])
.controller('AgentInfoController', function ($scope, $http, $location, $window, $log, $rootScope, $q) {
    var deferred = $q.defer();
    $scope.ProfileDto = {
        ProfileID: ''
        , FullName: ''
        , UserLogin: ''
        , EmployeeID: ''
        , Email: ''
        , ImagePath: ''
        , JobTitle: ''
        , Password: ''
        , NewPassword: ''
        , ConfirmPassword: ''
        , contracts: ''
    };
    $scope.ImageFile = '';
    $scope.UserSession = {
        UserId: '', UserName: '', IsAdmin: ''
    }

    $scope.ChangePassword = function () {
        if (IsNullOrEmpty($scope.ProfileDto.Password)) {
            ErrorMessage('Password is required');
            return false;
        }
        if (IsNullOrEmpty($scope.ProfileDto.NewPassword)) {
            ErrorMessage('New Password is required');
            return false;
        }
        if (IsNullOrEmpty($scope.ProfileDto.ConfirmPassword)) {
            ErrorMessage('Confirm password is required');
            return false;
        }
        if ($scope.ProfileDto.NewPassword !== $scope.ProfileDto.ConfirmPassword) {
            ErrorMessage('Confirm password does not match');
            return false;
        } else {
            $.confirm({
                title: 'Confirm!',
                type: 'orange',
                content: 'Do you want to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            angular.element('#Loading').LoadingOverlay("show");
                            deferred.notify();
                            $http({
                                method: 'POST',
                                url: '/AgentInfo/ChangePassword',
                                headers: [{ 'Content-Type': 'application/json' }],
                                params: $scope.ProfileDto
                            }).then(function (response) {
                                angular.element('#Loading').LoadingOverlay("hide");
                                deferred.resolve(response);
                                $scope.ProfileDto = { Password: '', NewPassword: '', ConfirmPassword: '' };
                                if (response.data.code === 'SUCCESS') {
                                    $.alert({
                                        title: 'Message!',
                                        type: 'green',
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
                                } else if (response.data.code === 'ERROR') {
                                    ErrorMessage(response.data.message);
                                }                                
                            }, function (response) {
                                ErrorMessage(response.data);
                                angular.element('#Loading').LoadingOverlay("hide");
                                return deferred.reject(response.data);
                            });
                            return deferred.promise;
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });            
        }
    }

    $scope.GetUserProfile = function () {
        $scope.ProfileDto.UserId = $scope.UserSession.UserId;
        $http({
            method: 'GET',
            url: '/AgentInfo/GetUserProfile',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ProfileDto
        }).then(function (response) {
            deferred.resolve(response);
            console.log(response.data);
            if (!IsNullOrEmpty(response.data)) {
                $scope.ProfileDto = response.data;
            }
        }, function (response) {
            console.error(response.data);
            return deferred.reject(response.data);
        });
    }

    $scope.UpdateProfile = function () {
        if (!IsNullOrEmpty($scope.ProfileDto.NewPassword) && IsNullOrEmpty($scope.ProfileDto.ConfirmPassword)) {
            ErrorMessage('Confirm password is required');
            return false;
        }
        if (!IsNullOrEmpty($scope.ProfileDto.NewPassword) && ($scope.ProfileDto.NewPassword !== $scope.ProfileDto.ConfirmPassword)) {
            ErrorMessage('Confirm password does not match');
            return false;
        } else {
            $http({
                method: 'POST',
                url: '/AgentInfo/UpdateProfile',
                headers: [{ 'Content-Type': 'application/json' }],
                params: $scope.ProfileDto
            }).then(function (response) {
                if (response.data.code === 'SUCCESS') {
                    SuccessMessage(response.data.message);
                } else if (response.data.code === 'ERROR') {
                    ErrorMessage(response.data.message);
                }
            }), function (response) {
                ErrorMessage(response.data.message);
                console.log(response.data);
            };
        }
        
    }

    /*====== INIT PAGE ======*/
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
        $scope.UserSession.IsAdmin = $('#IsAdmin').val();
        $scope.GetUserProfile();
    }
    $scope.initPage();
    /* ===== INIT PAGE ====== */
}).directive("fileinput", [function () {
    return {
        scope: {
            fileinput: "=",
            filepreview: "="
        },
        link: function (scope, element, attributes) {
            element.bind("change", function (changeEvent) {
                scope.fileinput = changeEvent.target.files[0];
                var reader = new FileReader();
                reader.onload = function (loadEvent) {
                    scope.$apply(function () {
                        scope.filepreview = loadEvent.target.result;
                    });
                }
                reader.readAsDataURL(scope.fileinput);
            });
        }
    }
}]);