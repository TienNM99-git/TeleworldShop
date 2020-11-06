
/// <reference path="../../../assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('teleworldshop.product_categories', ['teleworldshop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        var states = [
            {
                name: 'product_categories',
                url: '/product_categories',
                templateUrl: '/app/components/product_categories/productCategoryListView.html',
                parent:'base',
                controller: 'productCategoryListController'
            },
            {
                name: 'add_product_category',
                url: '/add_product_category',
                templateUrl: '/app/components/product_categories/productCategoryAddView.html',
                parent: 'base',
                controller: 'productCategoryAddController'
            },
            {
                name: 'edit_product_category',
                url: '/edit_product_category/:id',
                templateUrl: '/app/components/product_categories/productCategoryEditView.html',
                parent: 'base',
                controller: 'productCategoryEditController'
            }
        ];
        states.forEach((state) => $stateProvider.state(state));
    }
})();