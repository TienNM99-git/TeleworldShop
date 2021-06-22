(function (app) {
    app.controller('orderListController', orderListController);

    orderListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function orderListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.orders = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.keyword = '';

        $scope.getOrders = getOrders;
        $scope.search = search;

        $scope.deleteOrder = deleteOrder;
        $scope.selectAll = selectAll;

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function (i, item) {
                listId.push(item.Id);
            });

            $ngBootbox.confirm('Are you sure that you want to delete these record?').then(function () {
                var config = {
                    params: {
                        checkedorders: JSON.stringify(listId)
                    }
                }
                apiService.del('api/order/deletemulti', config, function (result) {
                    notificationService.displaySuccess('Successful delete ' + result.data + ' records !!!');
                    search();
                }, function (error) {
                    notificationService.displayError('Delete failed');
                });
            });
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.orders, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.orders, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("orders", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteOrder(id) {
            $ngBootbox.confirm('Are you sure that you want to delete?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                //apiService.put('api/order/delete', config, function () {
                //    notificationService.displaySuccess('Delete successful !!!');
                //    search();
                //}, function () {
                //    notificationService.displayError('Delete failed !!!');
                //})
                apiService.del('api/order/delete', config,
                    function (result) {
                        notificationService.displaySuccess(result.data.Name + ' deleted !!!');
                        search();
                    }, function (error) {
                        notificationService.displayError('Delete order failed');
                    });
            });
        }
        function search() {
            $scope.getOrders();
        }
        function getOrders(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 6
                }
            }
            apiService.get('/api/order/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('No record found');
                }
                //else {
                //    notificationService.displaySuccess('Found ' + result.data.TotalCount + (result.data.TotalCount == 1 ? ' record!!!':' records!!!'));
                //}
                $scope.orders = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load order list failed!!!');
            });
        }

        $scope.getOrders();
    }
})(angular.module('teleworldshop.orders'));