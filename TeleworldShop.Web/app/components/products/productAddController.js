(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function productAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.product = {
            CreatedDate: new Date(),
            Status: true,
            Name: "Iphone 12 Mini",
            Alias: "iphone-12-mini",
            CategoryId: 11,
            Price: 500,
            Description: "Latest product from Apple.",
            MetaKeyword: "",
            MetaDescription: "",
            HomeFlag: true,
            HotFlag: true
        }

        $scope.ckeditorOptions = {
            language: 'vi',
            height: '200px'
        }
        $scope.AddProduct = AddProduct;
        $scope.GetSeoTitle = GetSeoTitle;
        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }
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