
var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvent();
    },
    registerEvent: function () {
        var teleworldHub = $.connection.teleworldHub;
        $('#frmPayment').validate({
            rules: {
                name: "required",
                address: "required",
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    number: true
                }
            },
            messages: {
                name: "Name is required",
                address: "Address is required",
                email: {
                    required: "Email is required",
                    email: "Invalid email address"
                },
                phone: {
                    required: "Phone is required",
                    number: "Invalid phone number"
                }
            },          
        });
        $('.btnDeleteItem').off('click').on('click', function (e) {           
            var result = confirm("Do you want to remove this item from cart?");
            if (result) {
                e.preventDefault();
                var productId = parseInt($(this).data('id'));
                cart.deleteItem(productId);
            }
        });
        $('.txtQuantity').off('change').on('change', function () {
            var quantity = parseInt($(this).val());
            var productid = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
          
            if (isNaN(quantity) == false) {
                var amount = quantity * price;
                $('#amount_' + productid).text(numeral(amount).format('0,0'));
            }
            else if (quantity === 0) {
                var result = confirm("Do you want to remove this item from cart?");
                if (result) {
                    cart.deleteItem(productId);
                }
            }
            else {
                $('#amount_' + productid).text(0);
            }

            $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
            cart.updateAll();
        });
        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });
        $('#btnDeleteAll').off('click').on('click', function (e) {
            var result = confirm("Are you sure that you want to remove all item from cart?");
            if (result) {
                e.preventDefault();
                cart.deleteAll();
            }           
        });
        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#divCheckout').show();
        });
        $('#chkUserLoginInfo').off('click').on('click', function () {
            if ($(this).prop('checked'))
                cart.getLoginUser();
            else {
                $('#txtName').val('');
                $('#txtAddress').val('');
                $('#txtEmail').val('');
                $('#txtPhone').val('');
            }
        });
        $.connection.hub.start().done(function () {
            console.log('SignalR connection started');
            $('#btnCreateOrder').off('click').on('click', function (e) {
                e.preventDefault();
                var isValid = $('#frmPayment').valid();
                if (isValid) {
                    cart.createOrder(teleworldHub);
                }
            });
        })

        $('input[name="paymentMethod"]').off('click').on('click', function () {
            if ($(this).val() == 'NL') {
                $('#bankContent').hide();
                $('#nganluongContent').show();
            }
            else if ($(this).val() == 'ATM_ONLINE' || $(this).val() == 'IB_ONLINE') {
                $('#nganluongContent').hide();
                $('#bankContent').show();
            }
            else {
                $('.boxContent').hide();
            }
        });
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                }
            }
        });
    },

    createOrder: function (teleworldHub) {
        var bank_code = null;
        if ($('input[name="paymentMethod"]:checked').val() == 'VISA') {
            bank_code = 'VISA';
        }
        else {
            bank_code = $('input[name="bankcode"]:checked').prop('id');
        }
        $('#btnCreateOrder').prop("disabled", true);
        var order = {
            CustomerName: $('#txtName').val(),
            CustomerAddress: $('#txtAddress').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            PaymentMethod: $('input[name="paymentMethod"]:checked').val(),
            BankCode: bank_code,
            Status: false
        }

        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    teleworldHub.server.updateDashBoard();
                    teleworldHub.server.updateOrderList();
                    if (response.urlCheckout != undefined && response.urlCheckout != '') {
                        window.location.href = response.urlCheckout;
                    }
                    else {
                        $('#divCheckout').hide();
                        cart.deleteAll();
                        setTimeout(function () {
                            $('#cartContent').html('Order successful. We will contact to you soon');
                        }, 2000);
                    }
                }
                else {
                    $('#divMessage').show();
                    $('#divMessage').text(response.message);
                }
            }
        });
    },
    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();

                }
            }
        });
    },
    updateAll: function () {
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                ProductId: $(item).data('id'),
                Quantity: $(item).val()
            });
        });
        $.ajax({
            url: '/ShoppingCart/Update',
            type: 'POST',
            data: {
                cartData: JSON.stringify(cartList)
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    console.log('Update ok');
                }
            }
        });
    },
    deleteItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tplCart').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: item.Product.PromotionPrice ? item.Product.PromotionPrice : item.Product.Price,
                            PriceF: numeral(item.Product.PromotionPrice ? item.Product.PromotionPrice : item.Product.Price).format('0,0'),
                            Quantity: item.Quantity,
                            Amount: numeral(item.Product.PromotionPrice ? item.Quantity * item.Product.PromotionPrice : item.Quantity * item.Product.Price)
                                .format('0,0')
                        });
                    });
                    var totalQuantity = data.reduce(function (total, currentValue) {
                        return total + currentValue.Quantity
                    }, 0);
                    $(".jJyMq").html(`${totalQuantity.toString()}`);
                    $('#cartBody').html(html);
                    if (html == '') {
                        $('#cartContent').html('Cart is currently empty.');
                    }
                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));
                    cart.registerEvent();
                }
            }
        })
    }
}
cart.init();