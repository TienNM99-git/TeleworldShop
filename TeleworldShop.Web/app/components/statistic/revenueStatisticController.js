(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', '$http', 'apiService', 'notificationService', '$filter', '$rootScope'];

    function revenueStatisticController($scope, $http, apiService, notificationService, $filter, $rootScope) {

        $scope.colors = ['#45b7cd', '#ff6384', '#ff8e72'];
        $scope.tabledata = []; //for revenues statistic
        $scope.labels = [];
        $scope.series = ['Revenues', 'Benefit'];
        $scope.userCount = 0;
        $scope.chartdata = [];
        $scope.revenueToday = revenueToday;
        $scope.revenueLastWeek = revenueLastWeek;
        $scope.revenueThisWeek = revenueThisWeek;
        $scope.revenueLastMonth = revenueLastMonth;
        $scope.revenueThisMonth = revenueThisMonth;
        $scope.revenueThisYear = revenueThisYear;

        $scope.tablechartData = []; //for order statistic
        $scope.linechartData = [];
        $scope.linechartLabels = [];
        $scope.linechartSerie = 'Total Order';
        $scope.orderCount = 0;
        $scope.orderToday = orderToday;
        $scope.orderLastWeek = orderLastWeek;
        $scope.orderThisWeek = orderThisWeek;
        $scope.orderLastMonth = orderLastMonth;
        $scope.orderThisMonth = orderThisMonth;
        $scope.orderThisYear = orderThisYear;

        $scope.sellData = [];      //for sell statistic
        $scope.sellchartData = [];
        $scope.sellchartLabels = [];
        $scope.sellSeries = 'Total Sold';

        $scope.inventoryData = [];      //for inventory statistic
        $scope.inventorychartData = [];
        $scope.inventorychartLabels = [];
        $scope.inventorySeries = 'Remain Quantity';
        $scope.$on('UpdateDashBoard', function () {
            notificationService.displaySuccess('Load data successfully !');
            getNumberOfOrder();
            revenueThisYear();
            orderThisYear();
            getInventoryStatistic();
            getSellStatistic();
            $rootScope.$apply();
        })

        // Revenue statistic
        function revenueToday() {
            var date = new Date();
            const dateRange = {
                fromDate: moment(date).format("MM/DD/YYYY"),
                toDate: moment(date.setDate(date.getDate() + 1)).format("MM/DD/YYYY")
            }
            getRevenueStatistic(dateRange)
        }

        function revenueLastWeek() {
            const dateRange = {
                fromDate: moment().startOf('week').subtract(7, 'days').format("MM/DD/YYYY"),
                toDate: moment().endOf('week').subtract(7, 'days').format("MM/DD/YYYY"),
            }
            getRevenueStatistic(dateRange)
        }

        function revenueThisWeek() {
            const dateRange = {
                fromDate: moment().startOf('week').format("MM/DD/YYYY"),
                toDate: moment().endOf('week').format("MM/DD/YYYY"),
            }
            getRevenueStatistic(dateRange)
        }

        function revenueLastMonth() {
            const dateRange = {
                fromDate: moment().startOf('month').subtract(1, 'months').format("MM/DD/YYYY"),
                toDate: moment().endOf('month').subtract(1, 'months').format("MM/DD/YYYY"),
            }
            getRevenueStatistic(dateRange)
        }

        function revenueThisMonth() {
            const dateRange = {
                fromDate: moment().startOf('month').format("MM/DD/YYYY"),
                toDate: moment().endOf('month').format("MM/DD/YYYY"),
            }
            getRevenueStatistic(dateRange)
        }

        function revenueThisYear() {
            const dateRange = {
                fromDate: moment().startOf('year').format("MM/DD/YYYY"),
                toDate: moment().endOf('year').format("MM/DD/YYYY"),
            }
            getRevenueStatistic(dateRange)
        }

        function getRevenueStatistic(dateRange) {
            apiService.get('api/statistic/getrevenue?fromDate=' + dateRange.fromDate
                + "&toDate=" + dateRange.toDate,
                null,
                function (response) {
                    $scope.tabledata = response.data;
                    var labels = [];
                    var chartData = [];
                    var revenues = [];
                    var benefits = [];
                    $.each(response.data, function (i, item) {
                        labels.push(item.Date);
                        revenues.push(item.Revenues);
                        benefits.push(item.Benefit);
                    });
                    chartData.push(revenues);
                    chartData.push(benefits);

                    $scope.chartdata = chartData;
                    $scope.labels = labels;
                }, function (response) {
                    notificationService.displaySuccess('Can not load data');
                });
        }

        // Order statistic
        function getNumberOfOrder() {
            const date = new Date();
            const dateRange = {
                fromDate: moment(date).format("MM/DD/YYYY"),
                toDate: moment(date.setDate(date.getDate() + 1)).format("MM/DD/YYYY")
            }
            apiService.get('api/statistic/getorderstatistics?fromDate=' + dateRange.fromDate
                + "&toDate=" + dateRange.toDate, null,
                function (response) {
                    var count = 0;
                    $.each(response.data, function (i, item) {
                        count += item.OrderCount;
                    });
                    $scope.orderCount = count;
                },
                function () {
                    notificationService.displayError('Can not load data');
                }
            );
        }

        function orderToday() {
            const date = new Date();
            const dateRange = {
                fromDate: moment(date).format("MM/DD/YYYY"),
                toDate: moment(date.setDate(date.getDate() + 1)).format("MM/DD/YYYY")
            }
            getOrderStatistic(dateRange)
        }

        function orderLastWeek() {
            const dateRange = {
                fromDate: moment().startOf('week').subtract(7, 'days').format("MM/DD/YYYY"),
                toDate: moment().endOf('week').subtract(7, 'days').format("MM/DD/YYYY"),
            }
            getOrderStatistic(dateRange)
        }

        function orderThisWeek() {
            const dateRange = {
                fromDate: moment().startOf('week').format("MM/DD/YYYY"),
                toDate: moment().endOf('week').format("MM/DD/YYYY"),
            }
            getOrderStatistic(dateRange)
        }

        function orderLastMonth() {
            const dateRange = {
                fromDate: moment().startOf('month').subtract(1, 'months').format("MM/DD/YYYY"),
                toDate: moment().endOf('month').subtract(1, 'months').format("MM/DD/YYYY"),
            }
            getOrderStatistic(dateRange)
        }

        function orderThisMonth() {
            const dateRange = {
                fromDate: moment().startOf('month').format("MM/DD/YYYY"),
                toDate: moment().endOf('month').format("MM/DD/YYYY"),
            }
            getOrderStatistic(dateRange)
        }

        function orderThisYear() {
            const dateRange = {
                fromDate: moment().startOf('year').format("MM/DD/YYYY"),
                toDate: moment().endOf('year').format("MM/DD/YYYY"),
            }
            getOrderStatistic(dateRange)
        }

        function getOrderStatistic(dateRange) {
            apiService.get('api/statistic/getorderstatistics?fromDate=' + dateRange.fromDate
                + "&toDate=" + dateRange.toDate,
                null,
                function (response) {
                    $scope.tablechartData = response.data;
                    var lineChartLabels = [];
                    var lineChartData = [];
                    var orderCount = [];
                    $.each(response.data, function (i, item) {
                        lineChartLabels.push(item.Date);
                        orderCount.push(item.OrderCount);
                    });
                    lineChartData.push(orderCount);

                    $scope.linechartData = lineChartData;
                    $scope.linechartLabels = lineChartLabels;
                }, function (response) {
                    notificationService.displayError('Can not load data');
                });
        }

        function getSellStatistic() {
            var config = {
                param: {
                    fromDate: '01/01/2021',
                    toDate: '01/01/2022'
                }
            }
            apiService.get('api/statistic/getsellstatistics?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.sellData = response.data;
                var sellChartLabels = [];
                var sellChartData = [];
                $.each(response.data, function (i, item) {
                    sellChartLabels.push(item.Name);
                    sellChartData.push(item.TotalSold);
                });
                $scope.sellchartData = sellChartData;
                $scope.sellchartLabels = sellChartLabels;
            }, function (response) {
                notificationService.displayError('Can not load data');
            });
        }
        function getInventoryStatistic() {
            var config = {
                param: {
                    fromDate: '01/01/2021',
                    toDate: '01/01/2022'
                }
            }
            apiService.get('api/statistic/getinventorystatistics?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.inventoryData = response.data;
                var inventoryChartLabels = [];
                var inventoryChartData = [];
                $.each(response.data, function (i, item) {
                    inventoryChartLabels.push(item.Name);
                    inventoryChartData.push(item.RemainQuantity);
                });
                $scope.inventorychartData = inventoryChartData;
                $scope.inventorychartLabels = inventoryChartLabels;
            }, function (response) {
                notificationService.displaySuccess('Can not load data');
            });
        }

        getNumberOfOrder();
        revenueThisYear();
        orderThisYear();
        getInventoryStatistic();
        getSellStatistic();
    }

})(angular.module('teleworldshop.statistics'));