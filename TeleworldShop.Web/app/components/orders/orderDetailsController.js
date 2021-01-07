(function (app) {
    app.controller('orderDetailsController', orderDetailsController);

    orderDetailsController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function orderDetailsController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.order = {};

        $scope.updateOrder = updateOrder;

        function loadOrderDetail() {
            apiService.get('api/order/getbyid/' + $stateParams.id, null, function (result) {
                console.log(result.data);
                $scope.order = result.data;
                //$scope.moreImages = JSON.parse($scope.order.MoreImages);
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function updateOrder() {
            //$scope.order.MoreImages = JSON.stringify($scope.moreImages)
            apiService.put('api/order/update', $scope.order,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' updated !!!');
                    $state.go('orders');
                }, function (error) {
                    notificationService.displayError('Update order failed');
                });
        }
        loadOrderDetail();
    }

})(angular.module('teleworldshop.orders'));