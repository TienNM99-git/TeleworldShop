
var orderDetails = {
    init: function () {
        orderDetails.loadOrderData();
        orderDetails.registerEvent();
    },
    loadOrderData: function () {
        $.ajax({
            url: '/api/order/getall',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tplOrderDetail').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: item.Product.Price,
                            PriceF: numeral(item.Product.Price).format('0,0'),
                            Quantity: item.Quantity,
                            Amount: numeral(item.Quantity * item.Product.Price).format('0,0')
                        });
                    });
                    $('#orderDetailBody').html(html);
                    if (html == '') {
                        $('#orderDetailContent').html('No item found');
                    }
                    $('#lblTotalCost').text(numeral(cart.getTotalOrder()).format('0,0'));
                    cart.registerEvent();
                }
            }
        })
    }
}
cart.init();