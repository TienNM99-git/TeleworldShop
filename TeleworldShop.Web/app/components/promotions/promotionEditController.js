(function (app) {
    app.controller('promotionEditController', promotionAddController);

    promotionAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function promotionAddController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.promotion = {};

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

        function loadDetail() {
            apiService.get('/api/promotion/detail/' + $stateParams.id, null,
                function (result) {
                    $scope.promotion = result.data;
                    $scope.promotion.Categories.forEach(x => $scope.categories.unshift(x));
                    $scope.promotion.PromotionPrice = result.data.PromotionPrice;
                    $scope.promotion.StartDate = new Date($scope.promotion.StartDate);
                    $scope.promotion.ExpireDate = new Date($scope.promotion.ExpireDate);
                },
                function (result) {
                    notificationService.displayError(result.data);
                });
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

        $scope.updatePromotion = function () {
            if ($scope.promotion.Categories.length > 0) {
                if ($scope.promotion.ExpireDate.getTime() > $scope.promotion.StartDate.getTime()) {
                    apiService.put('api/promotion/update', $scope.promotion,
                        function (result) {
                            notificationService.displaySuccess(result.data.Name + ' updated !!!');
                        }, function (error) {
                            notificationService.displayError('Failed to update !!!');
                        }
                    );
                } else {
                    notificationService.displayError('The expire day must be greater than the start day !!!');
                }
            } else {
                notificationService.displayError('Please select at least 1 category !');
            }
        }

        function loadCategory(callback) {
            apiService.get('api/productcategory/getListAvailableCategory', null, function (result) {
                $scope.categories = result.data;
                callback();
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        loadCategory(loadDetail);
        
    }
})(angular.module('teleworldshop.promotions'));