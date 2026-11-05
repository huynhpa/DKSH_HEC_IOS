'use strict';
angular.module('DKSHApp', ['ui.bootstrap'])
.controller('SignInController', function ($scope, $http, $location, $window, $log, $rootScope, $q) {
    var deferred = $q.defer();
    // User to search
    $scope.LoginModel = {
        UserId: '',
        UserName: '',
        Email: '',
        Password: '',
        RememberMe: false
    }

    /* SIGN IN */
    $scope.SignIn = function () {
        $http({
            method: 'POST',
            url: '/Login/SignIn',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.LoginModel
        }).then(function (response) {
            deferred.resolve(response);
            var url = '/Home/Index';
            $location.path(url);
        }, function (response) {
            $.alert({
                title: 'Alert!',
                type: 'red',
                content: 'Sign In Failed',
            });
            return deferred.reject(response.data);
        });
    }


    /* ===== INIT PAGE ==== */
    $scope.initPage = function () {

    }
});