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
                        alert('Add product to cart successfully.');
                        common.getCart();
                    }
                    else {
                        alert(response.message);
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