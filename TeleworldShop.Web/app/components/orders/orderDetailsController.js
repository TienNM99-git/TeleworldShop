(function (app) {
    app.controller('orderDetailsController', orderDetailsController);

    orderDetailsController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function orderDetailsController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.order = {};

        $scope.orderedProduct = {};

        $scope.updateOrder = updateOrder;

        $scope.markOrderAsPaid = markOrderAsPaid;

        function loadOrderDetail() {
            apiService.get('api/order/getbyid/' + $stateParams.id, null, function (result) {
                $scope.order = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function loadOrderedItem() {
            apiService.get('api/order/getorderedproduct/' + $stateParams.id, null, function (result) {
                $scope.orderedProduct = result.data;
                var template = $('#tplOrderDetail').html();
                var html = '';
                $.each($scope.orderedProduct, function (i, item) {
                    html += Mustache.render(template, {
                        ProductId: item.Id,
                        ProductName: item.Name,
                        Image: item.Image,
                        Price: item.Price,
                        PriceF: numeral(item.Price).format('0,0'),
                        Quantity: item.Quantity,
                        Amount: numeral(item.Quantity * item.Price).format('0,0')
                    });
                });
                $('#orderDetailBody').html(html);
                if (html == '') {
                    $('#orderDetailContent').html('No item found');
                }
                $('#lblTotalCost').text(numeral(getTotalCost()).format('0,0'));
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function getTotalCost() {
            var listTextBox = $('.txtOrderedQuantity');
            var total = 0;
            $.each(listTextBox, function (i, item) {
                total += parseInt($(item).val()) * parseFloat($(item).data('price'));
            });
            return total;
        }
        function updateOrder() {
            //$scope.order.MoreImages = JSON.stringify($scope.moreImages)
            apiService.put('api/order/update', $scope.order,
                function (result) {
                    notificationService.displaySuccess('Order Id: '+ result.data.Id + ' verified !!!');
                    $state.go('orders');
                }, function (error) {
                    notificationService.displayError('Verified order failed');
                });
        }
        function markOrderAsPaid() {
            //$scope.order.MoreImages = JSON.stringify($scope.moreImages)
            apiService.put('api/order/markaspaid', $scope.order,
                function (result) {
                    notificationService.displaySuccess('Order Id: ' + result.data.Id + ' marked as paid !!!');
                    $state.go('orders');
                }, function (error) {
                    notificationService.displayError('Mark order as paid failed');
                });
        }
        loadOrderDetail();
        loadOrderedItem();
    }

})(angular.module('teleworldshop.orders'));