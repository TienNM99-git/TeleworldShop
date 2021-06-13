(function (app) {
    app.controller('loginController', ['$scope', 'loginService', '$injector', 'notificationService',
        function ($scope, loginService, $injector, notificationService) {

            $scope.loginData = {
                userName: "",
                password: ""
            };

            $scope.loginSubmit = function () {
                loginService.login($scope.loginData.userName, $scope.loginData.password).then(function (response) {
                    if (response != null && response.error != undefined) {
                        notificationService.displayError("Access denied. Please check your username or password");
                        stateService.go('home');
                    }
                    else {
                        var stateService = $injector.get('$state');
                        stateService.go('home');
                        notificationService.displaySuccess("Access granted. Welcome, " + $scope.loginData.userName);
                    }
                });
            }
        }]);
})(angular.module('teleworldshop'));