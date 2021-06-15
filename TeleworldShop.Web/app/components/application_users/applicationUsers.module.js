/// <reference path="../../../assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('teleworldshop.application_users', ['teleworldshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('application_users', {
            url: "/application_users",
            templateUrl: "/app/components/application_users/applicationUserListView.html",
            parent: 'base',
            controller: "applicationUserListController"
        })
            .state('add_application_user', {
                url: "/add_application_user",
                parent: 'base',
                templateUrl: "/app/components/application_users/applicationUserAddView.html",
                controller: "applicationUserAddController"
            })
            .state('edit_application_user', {
                url: "/edit_application_user/:id",
                templateUrl: "/app/components/application_users/applicationUserEditView.html",
                controller: "applicationUserEditController",
                parent: 'base',
            })
            .state('import_application_user', {
                url: "/import_application_user",
                templateUrl: "/app/components/application_users/applicationUserImportView.html",
                controller: "applicationUserImportController",
                parent: 'base',
            });
    }
})();