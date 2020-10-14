var myApp = angular.module("myModule", []);

myApp.controller("schoolController", schoolController);
myApp.directive("teleworldShopDirective", teleworldShopDirective);
myApp.service("Validator", Validator);
schoolController.$inject = ["$scope", "Validator"];
function schoolController($scope, Validator) {
    $scope.checkNumber = function () {
        $scope.message = Validator.checkNumber($scope.num);
    }
    $scope.num = 1;
}
function Validator($window) {
    return {
        checkNumber: checkNumber
    }
    function checkNumber(n) {
        if (n % 2 == 0) {
            return "This is even";
        }
        else {
            return "This is odd";
        }
    }
}
function teleworldShopDirective() {
    return {
        restrict:"A",
        templateUrl: "/Scripts/spa/teleworldShopDirective.html"
    };
}