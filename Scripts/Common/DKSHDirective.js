'use strict';
var dkshApp = angular.module('DKSHApp', ['ui.bootstrap'])
dkshApp.directive(['onEnterKey', function onEnterKeyDirective() {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (e) {
            if (e.which === 13) {
                e.preventDefault();

                scope.$apply(function () {
                    scope.$eval(attrs.onEnterKey);
                });
            }
        });
    };
}]);