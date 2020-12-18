/// <reference path="../../../assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('teleworldshop.application_roles', ['teleworldshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        var states = [
            {
                name: 'application_roles',
                url: '/application_roles',
                parent: 'base',
                templateUrl: '/app/components/application_roles/applicationRoleListView.html',
                controller: 'applicationRoleListController'
            },
            {
                name: 'add_application_role',
                url: '/add_application_role',
                parent: 'base',
                templateUrl: '/app/components/application_roles/applicationRoleAddView.html',
                controller: 'applicationRoleAddController'
            },
            {
                name: 'edit_application_role',
                url: '/edit_application_role/:id',
                parent: 'base',
                templateUrl: '/app/components/application_roles/applicationRoleEditView.html',
                controller: 'applicationRoleEditController'
            }
        ];
        states.forEach((state) => $stateProvider.state(state));
    }
})();