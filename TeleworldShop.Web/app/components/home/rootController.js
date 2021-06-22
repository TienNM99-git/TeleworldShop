(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$state', 'authData', 'loginService', '$scope', 'authenticationService', '$rootScope'];

    function rootController($state, authData, loginService, $scope, authenticationService, $rootScope) {
        $scope.logOut = function () {
            loginService.logOut();
            $state.go('login');
        }
        $scope.authentication = authData.authenticationData;
        var teleworldHub = $.connection.teleworldHub;

        teleworldHub.client.updateDashBoard = function () {
            $rootScope.$broadcast('UpdateDashBoard', '');
        }

        $.connection.hub.start().done(function () {
            console.log('SignalR connection started');
        });
        //authenticationService.validateRequest();
    }
})(angular.module('teleworldshop'));