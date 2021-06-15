(function (app) {
    app.controller('applicationUserImportController', applicationUserImportController);

    applicationUserImportController.$inject = ['apiService', '$http', 'authenticationService', '$scope', 'notificationService', '$state', 'commonService'];

    function applicationUserImportController(apiService, $http, authenticationService, $scope, notificationService, $state, commonService) {

        $scope.files = [];
        $scope.ImportUser = ImportUser;
        $scope.flatFolders = [];
        //listen for the file selected event
        $scope.$on("fileSelected", function (event, args) {
            $scope.$apply(function () {
                //add the file object to the scope's files collection
                $scope.files.push(args.file);
            });
        });

        function ImportUser() {
            authenticationService.setHeader();
            $http({
                method: 'POST',
                url: "/api/applicationUser/Import",
                headers: { 'Content-Type': undefined },
                //This method will allow us to change how the data is sent up to the server
                // for which we'll need to encapsulate the model data in 'FormData'
                transformRequest: function (data) {
                    var formData = new FormData();
                    for (var i = 0; i < data.files.length; i++) {
                        //add each file to the form data and iteratively name them
                        formData.append("file" + i, data.files[i]);
                    }
                    return formData;
                },
                //Create an object that contains the model and files which will be transformed
                // in the above transformRequest method
                data: { files: $scope.files }
            }).then(function (result, status, headers, config) {
                notificationService.displaySuccess(result.data);
                $state.go('application_users');
            },
            function (data, status, headers, config) {
                notificationService.displayError(data);
            });
        }
    }

})(angular.module('teleworldshop.application_users'));