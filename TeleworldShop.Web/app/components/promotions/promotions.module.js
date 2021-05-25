
/// <reference path="../../../assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('teleworldshop.promotions', ['teleworldshop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        var states = [
            {
                name: 'promotions',
                url: '/promotions',
                templateUrl: '/app/components/promotions/promotionListView.html',
                parent: 'base',
                controller: 'promotionListController'
            },
            {
                name: 'promotions_add',
                url: '/promotions_add',
                templateUrl: '/app/components/promotions/promotionAddView.html',
                parent: 'base',
                controller: 'promotionAddController'
            },
            {
                name: 'promotions_edit',
                url: '/promotions_edit/:id',
                templateUrl: '/app/components/promotions/promotionEditView.html',
                parent: 'base',
                controller: 'promotionEditController'
            }
        ];
        states.forEach((state) => $stateProvider.state(state));
    }
})();