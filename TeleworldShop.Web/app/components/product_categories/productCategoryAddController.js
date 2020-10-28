(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);

    productCategoryAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function productCategoryAddController(apiService, $scope, notificationService, $state) {
        $scope.productCategory = {
            CreatedDate: new Date(),
            Status: true,
            Name: "Refridgerator",
            Alias: "refridgerator",
            Description: "Refridgerator from Mitsubishi, Sharp, ...",
            DisplayOrder: 100,
            MetaKeyword: "",
            MetaDescription: ""
        }
        //$scope.GetSeoTitle = GetSeoTitle;
        //function GetSeoTitle() {
        //    $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        //}

        $scope.AddProductCategory = AddProductCategory;

        function AddProductCategory() {
            apiService.post('api/productcategory/create', $scope.productCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' added !!');
                    $state.go('product_categories');
                }, function (error) {
                    notificationService.displayError('Add new category failed !!');
                });
        }
        function loadParentCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.parentCategories = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }
        loadParentCategory();
    }
})(angular.module('teleworldshop.product_categories'));