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

        $scope.deleteMultiple = deleteMultiple;
        $scope.deletePromotion = deletePromotion;

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

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.Id);
            });

            $ngBootbox.confirm('Are you sure that you want to delete these record?').then(function () {
                var config = {
                    params: {
                        checkedPromotions: JSON.stringify(listId)
                    }
                }
                apiService.del('api/promotion/deletemulti', config, function (result) {
                    notificationService.displaySuccess('Successful delete ' + result.data + ' records !!!');
                    search();
                }, function (error) {
                    notificationService.displayError('Delete failed');
                });
            });
        }

        function deletePromotion(id) {
            $ngBootbox.confirm('Are you sure that you want to delete?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/promotion/delete', config,
                    function (result) {
                        notificationService.displaySuccess(result.data.Name + ' deleted !!!');
                        search();
                    }, function (error) {
                        notificationService.displayError('Delete promotion failed');
                    });
            });
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
                    pageSize: 10
                }
            };

            apiService.get('api/promotion/getall', config, function (result) {
                if (result.data.TotalCount == 0)
                    return notificationService.displayWarning('No item found !');

                $scope.promotions = result.data.Items;
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