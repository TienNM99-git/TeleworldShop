(function (app) {
    app.controller('promotionListController', promotionListController);

    promotionListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function promotionListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.promotions = [];

        $scope.selectAll = selectAll;

        $scope.getPromotion = getPromotion;
        $scope.search = search;

        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.keyword = '';

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.promotions, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.promotions, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("promotions", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function search() {
            getPromotion();
        }

        $scope.currencyFormat = function (value) {
            return value.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        };

        function getPromotion(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 6
                }
            };

            apiService.get('api/promotion/getall', config, function (result) {
                if (result.data.TotalCount == 0)
                    return notificationService.displayWarning('No item found !');

                $scope.promotions = result.data.Items;
                console.log($scope.promotions[0].ExpireDate);
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                return notificationService.displayWarning('Load promotion list fail !');
            });
        }

        $scope.search();

    }
})(angular.module('teleworldshop.promotions'));