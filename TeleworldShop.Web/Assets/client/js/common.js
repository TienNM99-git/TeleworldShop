function notificationService() {
    toastr.options = {
        "debug": false,
        "positionClass": "toast-top-right",
        "onclick": null,
        "fadeIn": 300,
        "fadeOut": 1000,
        "timeOut": 3000,
        "extendedTimeOut": 1000
    };

    function displaySuccess(message) {
        toastr.success(message);
    }

    function displayWarning(message) {
        return toastr.warning(message);
    }

    function displayError(error) {
        if (Array.isArray(error)) {
            error.each(function (err) {
                toastr.error(err);
            });
        }
        else {
            toastr.error(error);
        }
    }

    return {
        displaySuccess: displaySuccess,
        displayError: displayError,
        displayWarning: displayWarning,
    }
}

var common = {
    init: function () {
        common.getCart();
        common.registerEvents();
    },
    registerEvents: function () {
        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/GetListProductByName",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.Name);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.Name);
                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            const promotionPrice = item.PromotionPrice ?
                item.PromotionPrice.toString().replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "₫" : null;
            const price = item.Price.toString().replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "₫";

            const promotionPriceDiv = promotionPrice ? `<div class="box-p"> 
                    <p class="price-old black">${price}</p>
                </div>` : `<div></div>`

            const itemImg = `<div class="item-img">
                    <img src="${item.Image}" />
                 </div>`;
            const itemInfo = `<div class="item-info">
                    <h3>${item.Name}</h3>
                    <strong class="price">${promotionPrice ? promotionPrice : price}</strong>
                    ${promotionPriceDiv}
                </div>`;

            const productSuggest = `<a>
                    ${itemImg}
                    ${itemInfo}
                </a>`;

            return $(`<li class="product-suggest"></li>`)
                .append(productSuggest)
                .appendTo(ul);
        };

        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            $.ajax({
                url: '/ShoppingCart/Add',
                data: {
                    productId: productId
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        notificationService().displaySuccess("Add product to cart successfully.");
                        common.getCart();
                    }
                    else {
                        notificationService().displayError("Cannot add product to cart !!!.");
                    }
                }
            });
        });

        $('#btnLogout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#frmLogout').submit();
        });
    },
    getCart: function () {
        $.ajax({
            url: '/ShoppingCart/GetCart',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var cart = response.cart;
                    var totalQuantity = cart.reduce(function (total, currentValue) {
                        return total + currentValue.Quantity
                    }, 0);
                    $(".jJyMq").html(`${totalQuantity.toString()}`);
                } else {
                    $(".jJyMq").html(`0`);
                }
            }
        });
    }
}
common.init();