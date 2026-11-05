'use strict';
angular.module('DKSHApp', ['ui.bootstrap'])
.controller('RegisterController', function ($scope, $http, $location, $window, $log, $rootScope, $q) {
    var deferred = $q.defer();

    $scope.RegisterModel = {
        UserId: '',
        UserName: '',
        FullName: '',
        Email: '',
        Team: '',
        JobTitle: '',
        Pwd: '',
        JoinDate: ''
    };

    /*============== VALIDATION ===============*/
    $scope.Validate = function () {
        if (IsNullOrEmpty($scope.RegisterModel.UserId)) {
            ErrorMessage('User Id is required !!');
            return false;
        }
        if (IsNullOrEmpty($scope.RegisterModel.UserName)) {
            ErrorMessage('User name is required !!');
            return false;
        }
        if (IsNullOrEmpty($scope.RegisterModel.FullName)) {
            ErrorMessage('Full name is required !!');
            return false;
        }
        if (IsNullOrEmpty($scope.RegisterModel.Email)) {
            ErrorMessage('Email is required !!');
            return false;
        }
        if (IsNullOrEmpty($scope.RegisterModel.Pwd)) {
            ErrorMessage('Password is required !!');
            return false;
        }
        if (IsNullOrEmpty($scope.RegisterModel.Team)) {
            ErrorMessage('Team is required !!');
            return false;
        }
        if (IsNullOrEmpty($scope.RegisterModel.JobTitle)) {
            ErrorMessage('Job title is required !!');
            return false;
        }
        if (IsNullOrEmpty($scope.RegisterModel.JoinDate)) {
            ErrorMessage('Join date is required !!');
            return false;
        }
        return true;
    }

    /*============= Registration ==============*/
    $scope.Registration = function () {
        deferred.notify();
        $http({
            method: 'POST',
            url: '/Register/Registration',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.RegisterModel
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'INSERT') {
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
            } else if (response.data.code === 'EXISTED') {
                ErrorMessage(response.data.message);
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

    /*============== SAVE ===============*/
    $scope.Save = function () {
        if ($scope.Validate()) {
            $.confirm({
                title: 'Confirm!',
                type: 'blue',
                content: 'Do you want to become a membership ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            $scope.Registration();
                        }
                    }, cancel: function () { }
                }
            });
        }
    }


    /*========== INIT PAGE ===========*/
    $scope.initPage = function () {
        // TO DO SOMTHING HERE
    }

});