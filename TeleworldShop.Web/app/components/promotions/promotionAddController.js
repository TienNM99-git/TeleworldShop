(function (app) {
    app.controller('promotionAddController', promotionAddController);

    promotionAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function promotionAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.promotion = {
            Name: 'Promotions',
            Type: 1,
            PromotionPrice: null,
            Apply: 1,
            Categories: [],
            Tags: [],
            StartDate: new Date(Date.now()),
            ExpireDate: new Date(Date.now()),
            Status: true,
        };

        $scope.promotionType = [
            {
                Id: 1,
                Name: 'VND',
            },
            {
                Id: 2,
                Name: '%',
            },
        ];

        $scope.promotionApply = [
            {
                Id: 1,
                Name: 'Product category',
            },
            {
                Id: 2,
                Name: 'Product tag ',
            },
        ];

        $scope.onPromotionTypeChange = onPromotionTypeChange;

        $scope.createPromotion = function () {
            if ($scope.promotion.Categories.length > 0) {
                if ($scope.promotion.ExpireDate.getTime() > $scope.promotion.StartDate.getTime()) {
                    apiService.post('api/promotion/create', $scope.promotion,
                        function (result) {
                            notificationService.displaySuccess(result.data.Name + ' added !!!');
                            $state.go('promotions');
                        }, function (error) {
                            notificationService.displayError('Add new promotion failed !!!');
                        }
                    );
                } else {
                    notificationService.displayError('The expire day must be greater than the start day !!!');
                }
            } else {
                notificationService.displayError('Please select at least 1 category !');
            }
        }

        function onPromotionTypeChange() {
            $scope.promotion.Discount = '';
        };

        $scope.onDiscountVNDKeyUp = function () {
            $scope.promotion.PromotionPrice = event.target.value.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        };

        $scope.onDiscountPercentKeyUp = function () {
            const percent = event.target.value;
            if (percent > 100)
                return $scope.promotion.PromotionPrice = 100;
        };

        function loadCategory() {
            apiService.get('api/productcategory/getListAvailableCategory', null, function (result) {
                $scope.categories = result.data;
            }, function () {
                notificationService.displayError('Cannot get category list !!!');
            });
        }

        loadCategory();
    }
})(angular.module('teleworldshop.promotions'));