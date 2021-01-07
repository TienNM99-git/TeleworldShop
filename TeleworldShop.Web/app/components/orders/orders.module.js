/// <reference path="../../../assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('teleworldshop.orders', ['teleworldshop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        var states = [         
            {
                name: 'orders',
                url: '/orders',
                parent:'base',
                templateUrl: '/app/components/orders/orderListView.html',
                controller: 'orderListController'
            },
            {
                name: 'order_details',
                url: '/order_details/:id',
                parent: 'base',
                templateUrl: '/app/components/orders/orderDetailsView.html',
                controller: 'orderDetailsController'
            }
        ];
        states.forEach((state) => $stateProvider.state(state));
    }
})();