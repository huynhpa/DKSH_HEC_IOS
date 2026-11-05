'use strict';
angular.module('DKSHApp', ['ui.bootstrap'])
.factory('CdiService', function ($http, $q) {
    return {
        insertCdi: function () {
            // the $http API is based on the deferred/promise APIs exposed by the $q service
            // so it returns a promise for us by default
            return $http.post('')
                .then(function (response) {

                }, function (response) {
                    // something went wrong
                    return $q.reject(response.data);
                });
        }
    };
});