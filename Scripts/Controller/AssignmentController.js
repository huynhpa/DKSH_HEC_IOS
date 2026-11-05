'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'lr.upload'])
.controller('AssignmentController', function ($scope, $http, $location, $window, $log, $rootScope, $q, upload, FileSaver, Blob) {
    var deferred = $q.defer();
    $scope.ListAssignment = [];
    $scope.ServiceTypeList = [];
    $scope.AgentList = [];
    $scope.AssignmentDto = {
        ServiceTypeList: '',
        ServiceType: '',
        ServiceName: '',
        ProjectID: '',
        ProjectName: '',
        UserId: '',
        IsAssigned: false,
        TotalCustomer: '',
        ID: ''
    };
    $scope.ServiceTypeDto = { WindowUser: '' };

    $scope.ViewAssignment = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $scope.AssignmentDto.ProjectID = $scope.AssignmentDto.ProjectID.ID;
        deferred.notify();
        $http({
            method: 'GET',
            url: '/Assignment/ViewAssignment',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.AssignmentDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListAssignment = response.data;
            $scope.totalItems = response.data.length;
            $scope.SetPaging();
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    $scope.GetActiveServiceType = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/ServiceType/GetActiveServiceType',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ServiceTypeDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ServiceTypeList = response.data;
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    $scope.GetProjectByWindowUser = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/Assignment/GetProjectByWindowUser',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ServiceTypeDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ServiceTypeList = response.data;
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    /*============== get agent ==============*/
    $scope.GetAgents = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/Agent/SearchAgent',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.AssignmentDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.AgentList = response.data;
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },


    $scope.Validate = function () {
        if (IsNullOrEmpty($scope.AssignmentDto.ProjectID)) {
            ErrorMessage('Please select a project');
            return false;
        } 
        return true;
    },

    $scope.RemoveAssignment = function () {
        if ($scope.Validate()) {
            $scope.AssignmentDto.ProjectID = $scope.AssignmentDto.ProjectID.ID;
            $.confirm({
                title: 'Confirm!',
                type: 'blue',
                content: 'Do you want to remove Customer Code of <font class="text-blue">' + $('#ddlProject :selected').text() + '</font>?',
                buttons: {
                    submit: {
                        btnClass: 'btn-blue',
                        action: function () {
                            angular.element('#Loading').LoadingOverlay("show");
                            $http({
                                method: 'POST',
                                url: '/Assignment/RemoveAssignment',
                                headers: [{ 'Content-Type': 'application/json' }],
                                params: $scope.AssignmentDto
                            }).then(function (response) {
                                angular.element('#Loading').LoadingOverlay("hide");
                                deferred.resolve(response);
                                if (response.data.code === 'DELETE') {
                                    $.alert({
                                        title: 'Message',
                                        type: 'green',
                                        content: response.data.message,
                                        buttons: {
                                            ok: {
                                                btnClass: 'btn-green',
                                                action: function () {
                                                    $scope.Clear();
                                                    $scope.ViewAssignment();
                                                }
                                            }
                                        }
                                    });
                                } else if (response.data.code === 'ERROR') {
                                    ErrorMessage(response.data.message);
                                }
                            }, function (response) {
                                angular.element('#Loading').LoadingOverlay("hide");
                                ErrorMessage('Unexpected error while delete. Please contact admin!!');
                                return deferred.reject(response.data);
                            });
                        }
                    }, cancel: { btnClass: 'btn-pinterest' }
                }
            });
        }
    },

    $scope.Clear = function () {
        $scope.AssignmentDto = {
            ServiceType: '',
            ServiceName: '',
            ProjectID: '',
            ProjectName: '',
            UserId: '',
            IsAssigned: false,
            TotalCustomer: ''
        };
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
    /*====== INIT PAGE ======*/
    $scope.initPage = function () {
        $scope.GetProjectByWindowUser();
        $scope.GetAgents();
        $scope.ViewAssignment();        
    }
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