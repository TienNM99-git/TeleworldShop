/// <reference path="../../../assets/admin/libs/angular/angular.js" />

//(function () {
//    angular.module('teleworldshop.products', ['teleworldshop.common']).config(config);
//    config.$inject = ['$stateProvider', '$urlRouterProvider'];
//    function config($stateProvider, $urlRouterProvider) {
//        $stateProvider.state('products', {
//            url: "/products",
//            templateUrl: "/app/components/products/productListView.html",
//            controller: "productListController"
//        }).state('product_add', {
//            url: "/product_add",
//            templateUrl: "/app/components/products/productAddView.html",
//            controller: "productAddController"
//        });
//    }
//})();
(function () {
    angular.module('teleworldshop.products', ['teleworldshop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        var states = [
            {
                name: 'products',
                url: '/products',
                templateUrl: '/app/components/products/productListView.html',
                controller: 'productListController'
            },
            {
                name: 'product_add',
                url: '/product_add',
                templateUrl: '/app/components/products/productAddView.html',
                controller: 'productAddController'
            },
            {
                name: 'product_edit',
                url: '/product_edit',
                templateUrl: '/app/components/products/productEditView.html',
                controller: 'productEditController'
            }
        ];
        states.forEach((state) => $stateProvider.state(state));
        //$stateProvider.state('products', {
        //    url: "/products",
        //    templateUrl: "/app/components/products/productListView.html",
        //    controller: "productListController"
        //}).state('product_add', {
        //    url: "/product_add",
        //    templateUrl: "/app/components/products/productAddView.html",
        //    controller: "productAddController"
        //});
    }
})();