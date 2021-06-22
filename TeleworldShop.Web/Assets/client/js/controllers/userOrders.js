const userOrder = {
    init: function () {
        userOrder.loadData();
    },
    loadData: function () {
        $.ajax({
            url: '/User/GetUserOrders',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var dataSource = response.data;
                    $('#purchaseHistoryTable').DataTable({
                        searching: false,
                        paging: true,
                        "pagingType": "numbers",
                        info: false,
                        data: dataSource,
                        columns: [
                            {
                                data: 'Id',
                                render: function (data) {
                                    return `<a href='/orders/history/${data}.html'>${data}</a>`
                                }
                            },
                            {
                                data: 'CreatedDate',
                                render: function (data) {
                                    const newDate = new Date(parseInt(data.replace("/Date(", "").replace(")/", ""), 10));
                                    return `${moment(newDate).format('DD-MM-YYYY')}`
                                }
                            },
                            { data: 'ProductName' },
                            {
                                data: 'Total',
                                render: function (data) {
                                    const newTotal = data.toString().replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                    return `${newTotal}đ`
                                }
                            },
                            { data: 'OrderStatus' },
                        ],
                        pageLength: 6
                    });
                    $(".dataTables_length").remove();
                }
            }
        });
        
    }
};

userOrder.init();