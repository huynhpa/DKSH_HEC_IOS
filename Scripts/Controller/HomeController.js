'use strict';
angular.module('DKSHApp', ['ui.bootstrap'])
    .controller('HomeController', function ($scope, $http, $location, $window, $log, $rootScope, $q) {
    var deferred = $q.defer();
    $scope.QuickCommentModel = { RequestId: '', IssueId: '', Title: '', Descriptions: '', IsDelete: false };
    $scope.TicketNoList = [];
    $scope.IssueNoList = [];
    /*============= SEND COMMENT =============*/
    $scope.SendComment = function () {
        deferred.notify();
        $http({
            method: 'POST',
            url: '/Home/SendComment',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.QuickCommentModel
        }).then(function (response) {
            deferred.resolve(response);
            if (response.data.code === 'INSERT') {                
                SuccessMessage(response.data.message);
                $scope.QuickCommentModel = {};
                $('#txtDescription').summernote('code', '');
            }
        }, function (response) {
            ErrorMessage('Unexpected error while save data!!');
            console.log(response.data);
            return deferred.reject(response.data);
        });
        return deferred.promise;
    }

    /*=========== GetIssueNo ===========*/
    $scope.GetIssueNo = function (obj) {
        deferred.notify();
        $scope.TicketNoDto = { TicketId: obj };
        $http({
            method: 'GET',
            url: '/QuickReport/GetIssueNo',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.TicketNoDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.IssueNoList = response.data;
        }, function (response) {
            ErrorMessage(response.data);
            console.log(response.data);
            return deferred.reject(response.data);
        });
        return deferred.promise;
    }

    /*============= SAVE =============*/
    $scope.Validation = function () {
        var description = $('#txtDescription').val();
        if (IsNullOrEmpty($scope.QuickCommentModel.RequestId)) {
            ErrorMessage('Ticket No is required !!');
            return false;
        }
        if (IsNullOrEmpty($scope.QuickCommentModel.Title)) {
            ErrorMessage('Title is required !!!');
            return false;
        }
        if (IsNullOrEmpty(description)) {
            ErrorMessage('Descriptions is required !!!');
            return false;
        }
        return true;
    }
    $scope.Save = function () {
        if ($scope.Validation()) {
            $.confirm({
                title: 'Confirm!',
                type: 'blue',
                content: 'Do you want to save ?',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            $scope.QuickCommentModel.Descriptions = $('#txtDescription').val();
                            $scope.SendComment();
                        }
                    }, cancel: function () { }
                }
            });
        }
    }
    
    /*================ INIT PAGE ===================*/
    $scope.initPage = function () {

    }
    
    $scope.initPage();
})