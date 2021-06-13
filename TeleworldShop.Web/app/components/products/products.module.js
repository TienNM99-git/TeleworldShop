/// <reference path="../../../assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('teleworldshop.products', ['teleworldshop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        var states = [
            {
                name: 'products',
                url: '/products',
                parent: 'base',
                templateUrl: '/app/components/products/productListView.html',
                controller: 'productListController'
            },
            {
                name: 'add_product',
                url: '/add_product',
                parent: 'base',
                templateUrl: '/app/components/products/productAddView.html',
                controller: 'productAddController'
            },
            {
                name: 'product_import',
                url: '/product_import',
                parent: 'base',
                templateUrl: '/app/components/products/productImportView.html',
                controller: 'productImportController'
            },
            {
                name: 'edit_product',
                url: '/edit_product/:id',
                parent: 'base',
                templateUrl: '/app/components/products/productEditView.html',
                controller: 'productEditController'
            }
        ];
        states.forEach((state) => $stateProvider.state(state));
    }
})();