'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'lr.upload'])
.controller('ProjectController', function ($scope, $http, $location, $window, $log, $rootScope, $q, upload, FileSaver, Blob) {
    var deferred = $q.defer();
    $scope.ListProject = [];
    $scope.AllProject = '';
    $scope.ProjectFile = [];
    $scope.isUpdate = false;
    $scope.ProjectDto = {
        ID: '' 
        , ProjectName: ''
        , IsActive: true
        , IsDeleted: false
        , CreatedBy: '' 
        , CreatedDate: '' 
        , UpdatedBy: '' 
        , UpdatedDate: ''
        , Header: ''
        , Footer: ''
    };
    $scope.ShowTextArea = false;

    $scope.SearchProject = function () {
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/Project/SearchProject',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ProjectDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ListProject = response.data;
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

    /*============ Project detail =============*/
    $scope.GetProjectDetail = function (item) {
        $('#txtProjectName').focus();
        $scope.ShowTextArea = true;
        $scope.isUpdate = true;
        $scope.ProjectDto = item;
        angular.element('#Loading').LoadingOverlay("show");
        deferred.notify();
        $http({
            method: 'GET',
            url: '/Project/GetProjectDetail',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ProjectDto
        }).then(function (response) {
            deferred.resolve(response);
            $scope.ProjectDto = response.data;
            $('#txtHeader').summernote('code', response.data.Header);
            $('#txtFooter').summernote('code', response.data.Footer);
            angular.element('#Loading').LoadingOverlay("hide");
        }, function (response) {
            ErrorMessage(response.data);
            angular.element('#Loading').LoadingOverlay("hide");
            return deferred.reject(response.data);
        });
        return deferred.promise;
    },

    $scope.Validate = function () {
        if (IsNullOrEmpty($scope.ProjectDto.ProjectName)) {
            $('#txtProjectName').focus();
            ErrorMessage('Project Name is required');
            return false;
        } if (IsNullOrEmpty($('#txtHeader').val())) {
            ErrorMessage('Project Header is required');
            return false;
        } if (IsNullOrEmpty($('#txtFooter').val())) {
            ErrorMessage('Project Footer is required');
            return false;
        }
        return true;
    },

    // SAVE PROJECT INFORMARION
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
                            $scope.ProjectDto.Header = $('#txtHeader').summernote('code');
                            $scope.ProjectDto.Footer = $('#txtFooter').summernote('code');
                            if ($scope.isUpdate) {
                                $scope.UpdateProject();
                            } else {
                                $scope.InsertProject();
                            }
                        }
                    }, cancel: { btnClass: 'btn-pinterest', action: function () { } }
                }
            });
        }
    },

    $scope.InsertProject = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'POST',
            url: '/Project/InsertProject',
            headers: [{ 'Content-Type': 'application/json' }],
            data: $scope.ProjectDto
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
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
                                $scope.Clear();
                                $scope.SearchProject();
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
    },

    $scope.UpdateProject = function () {
        angular.element('#Loading').LoadingOverlay("show");
        $http({
            method: 'POST',
            url: '/Project/UpdateProject',
            headers: [{ 'Content-Type': 'application/json' }],
            params: $scope.ProjectDto
        }).then(function (response) {
            angular.element('#Loading').LoadingOverlay("hide");
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
                                $scope.Clear();
                                $scope.SearchProject();
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
    },

    $scope.DeleteProject = function (item) {
        $scope.ProjectDto = item;
        $.confirm({
            title: 'Confirm!',
            type: 'blue',
            content: 'Do you want to delete <font class="text-blue">' + item.ProjectName + '</font>?',
            buttons: {
                submit: {
                    btnClass: 'btn-blue',
                    action: function () {
                        angular.element('#Loading').LoadingOverlay("show");
                        $http({
                            method: 'POST',
                            url: '/Project/DeleteProject',
                            headers: [{ 'Content-Type': 'application/json' }],
                            params: $scope.ProjectDto
                        }).then(function (response) {
                            angular.element('#Loading').LoadingOverlay("hide");
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
                                                $scope.Clear();
                                                $scope.SearchProject();
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
    },

    $scope.NewProject = function () {
        $scope.isUpdate = false;
        $scope.ShowTextArea = true;
        $('#txtProjectName').focus();
        $scope.ProjectDto = {
            ID: ''
            , ProjectName: ''
            , IsActive: true
            , IsDeleted: false
            , CreatedBy: ''
            , CreatedDate: ''
            , UpdatedBy: ''
            , UpdatedDate: ''
            , Header: ''
            , Footer: ''
        };
    },

    $scope.Clear = function () {
        $('#txtProjectName').focus();
        $scope.isUpdate = false;
        $scope.ShowTextArea = false;
        $scope.ProjectDto = {
            ID: ''
            , ProjectName: ''
            , IsActive: true
            , IsDeleted: false
            , CreatedBy: ''
            , CreatedDate: ''
            , UpdatedBy: ''
            , UpdatedDate: ''
            , Header: ''
            , Footer: ''
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
    }
    /*=========== PAGINATION=========== */
    /*====== INIT PAGE ======*/
    $scope.initPage = function () {
        $scope.SearchProject();
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