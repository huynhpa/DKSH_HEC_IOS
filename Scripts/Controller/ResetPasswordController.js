'use strict';
angular.module('DKSHApp', ['ui.bootstrap'])
.controller('ResetPasswordController', function ($scope, $http, $location, $window, $log, $rootScope, $q) {
    var deferred = $q.defer();

    $scope.ResetPassword = {        
        Email: '',
        NewPassword: '',
        ConfirmPassword: ''
    };

    $scope.Validate = function () {
        if (IsNullOrEmpty($scope.ResetPassword.Email)) {
            ErrorMessage('Email is required !!');
            return false;
        }
        return true;
    }


    /*============= Registration ==============*/
    $scope.Send = function () {
        deferred.notify();
        $http({
            method: 'POST',
            url: '/Login/SendMailToResetPassword',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ResetPassword
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
                                $window.location.href = '/Login';
                            }
                        }
                    }
                });
            } else {
                ErrorMessage(response.data.message);
            }
        }, function (response) {
            ErrorMessage(response.data);
            console.log(response.data);
            return deferred.reject(response.data);
        });
        return deferred.promise;
    }


    /*========== RESET PASSWORD ===========*/
    $scope.ResetPassword = function () {
        deferred.notify();
        $http({
            method: 'POST',
            url: '/ForgotPassword/ResetPassword',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ResetPassword
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'RESET_PASSWORD') {
                $.alert({
                    title: 'Message',
                    type: 'green',
                    content: response.data.message,
                    buttons: {
                        ok: {
                            btnClass: 'btn-green',
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
            ErrorMessage(response.data);
            console.log(response.data);
            return deferred.reject(response.data);
        });
        return deferred.promise;
    }

    /*========== INIT PAGE ===========*/
    $scope.initPage = function () {
        // TO DO SOMTHING HERE
    }

});