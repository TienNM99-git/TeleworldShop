/// <reference path="../../../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('teleworldshop.application_groups', ['teleworldshop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        var states = [
            {
                name: 'application_groups',
                url: '/application_groups',
                parent: 'base',
                templateUrl: '/app/components/application_groups/applicationGroupListView.html',
                controller: 'applicationGroupListController'
            },
            {
                name: 'add_application_group',
                url: '/add_application_group',
                parent: 'base',
                templateUrl: '/app/components/application_groups/applicationGroupAddView.html',
                controller: 'applicationGroupAddController'
            },
            {
                name: 'edit_application_group',
                url: '/edit_application_group/:id',
                parent: 'base',
                templateUrl: '/app/components/application_groups/applicationGroupEditView.html',
                controller: 'applicationGroupEditController'
            }
        ];
        states.forEach((state) => $stateProvider.state(state));
    }
})();
