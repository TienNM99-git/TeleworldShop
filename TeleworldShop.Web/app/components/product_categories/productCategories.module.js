
/// <reference path="../../../assets/admin/libs/angular/angular.js" />
(function () {
    angular.module('teleworldshop.product_categories', ['teleworldshop.common']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        //var states = [
        //    {
        //        name: 'product_categories',
        //        url: '/product_categories',
        //        templateUrl: '/app/components/products/productCategoryListView.html',
        //        controller: 'productCategoryListController'
        //    }
        //    {
        //        name: 'product_add',
        //        url: '/product_add',
        //        templateUrl: '/app/components/products/productAddView.html',
        //        controller: 'productAddController'
        //    },
        //    {
        //        name: 'product_edit',
        //        url: '/product_edit',
        //        templateUrl: '/app/components/products/productEditView.html',
        //        controller: 'productEditController'
        //    }
        //];
        //states.forEach((state) => $stateProvider.state(state));
        $stateProvider.state('product_categories', {
            url: "/product_categories",
            templateUrl: "app/components/product_categories/productCategoryListView.html",
            controller: "productCategoryListController"
        });
        //}).state('product_add', {
        //    url: "/product_add",
        //    templateUrl: "/app/components/products/productAddView.html",
        //    controller: "productAddController"
        //});
    }
})();