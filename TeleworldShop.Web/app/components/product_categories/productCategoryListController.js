
(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope', 'apiService'];

    function productCategoryListController($scope, apiService) {
        $scope.productCategories = [];

        $scope.getProductCagories = getProductCagories;

        function getProductCagories() {
            apiService.get('/api/productcategory/getallparents', null, function (result) {
                $scope.productCategories = result.data;
            }, function () {
                console.log('Load Product Categories failed!');
            });
        }

        $scope.getProductCagories();
    }
})(angular.module('teleworldshop.product_categories'));