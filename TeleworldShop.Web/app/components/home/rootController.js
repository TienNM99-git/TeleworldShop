(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$scope','$state'];

    function rootController($scope, $state) {
        $scope.logOut = function () {
            $state.go('login');
        }
    }
})(angular.module('teleworldshop'));