(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function productAddController(apiService, $scope, notificationService, $state) {
        $scope.product = {
            CreatedDate: new Date(),
            Status: true,
            Name: "Iphone 12 Mini",
            Alias: "iphone-12-mini",
            CategoryId: 11,
            Price: 500,
            Description: "Latest product from Apple.",
            DisplayOrder: 100,
            MetaKeyword: "",
            MetaDescription: ""
        }
        //$scope.GetSeoTitle = GetSeoTitle;
        //function GetSeoTitle() {
        //    $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        //}
        $scope.ckeditorOptions = {
            language: 'vi',
            height: '200px'
        }
        $scope.AddProduct = AddProduct;

        function AddProduct() {
            apiService.post('api/product/create', $scope.product,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' added !!');
                    $state.go('products');
                }, function (error) {
                    notificationService.displayError('Add new product failed !!');
                });
        }
        function loadProductCategories() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.productCategories = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.product.Image = fileUrl;
            }
            finder.popup();
        }
        loadProductCategories();
    }
})(angular.module('teleworldshop.products'));