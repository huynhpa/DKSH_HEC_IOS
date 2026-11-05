'use strict';
angular.module('DKSHApp', ['ui.bootstrap', 'angularSimplePagination', 'ngFileSaver', 'treeGrid'])
    .controller('LayoutController', function ($scope, $http, $location, $window, $log, $rootScope, $filter, $q, FileSaver, Blob) {
    var deferred = $q.defer();
    $scope.UserSession = { UserId: '', UserName: '' };

    /*===== INIT PAGE =====*/
    $scope.initPage = function () {
        $scope.UserSession.UserId = $('#UserId').val();
        $scope.UserSession.UserName = $('#UserName').val();
    }
    $scope.initPage();
    /*===== INIT PAGE =====*/

})