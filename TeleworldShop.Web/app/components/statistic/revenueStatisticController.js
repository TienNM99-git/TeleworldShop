(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter', '$rootScope'];

    function revenueStatisticController($scope, apiService, notificationService, $filter, $rootScope) {
        $scope.colors = ['#45b7cd', '#ff6384', '#ff8e72'];
        $scope.tabledata = []; //for revenues statistic
        $scope.labels = [];
        $scope.series = ['Revenues', 'Benefit'];
        $scope.userCount = 0;
        $scope.chartdata = [];

        $scope.tablechartData = []; //for order statistic
        $scope.linechartData = [];
        $scope.linechartLabels = [];
        $scope.linechartSerie = 'Total Order';

        $scope.sellData = [];      //for sell statistic
        $scope.sellchartData = [];
        $scope.sellchartLabels = [];
        $scope.sellSeries = 'Total Sold';

        $scope.inventoryData = [];      //for inventory statistic
        $scope.inventorychartData = [];
        $scope.inventorychartLabels = [];
        $scope.inventorySeries = 'Remain Quantity';
        $scope.$on('UpdateDashBoard', function () {
            notificationService.displaySuccess('Can not load data');
            getInventoryStatistic();
            getSellStatistic();
            getOrderStatistic();
            getStatistic();
            $rootScope.$apply();
        })
        //$scope.datasetOverride = [];
        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: '01/01/2021',
                    toDate: '01/01/2022'
                }
            }
            apiService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.tabledata = response.data;
                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(response.data, function (i, item) {
                    labels.push($filter('date')(item.Date, 'MM/yyyy'));
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = labels;
            }, function (response) {
                notificationService.displayError('Can not load data');
            });
        }
        function getOrderStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: '01/01/2021',
                    toDate: '01/01/2022'
                }
            }
            apiService.get('api/statistic/getorderstatistics?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
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
        //function getUserStatistic() {
        //    var config = {
        //        param: {
        //            //mm/dd/yyyy
        //            fromDate: '01/01/2021',
        //            toDate: '01/01/2022'
        //        }
        //    }
        //    apiService.get('api/statistic/getuserstatistics?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
        //        $scope.orderCount = response.data;
        //    }, function (response) {
        //        notificationService.displayError('Can not load data');
        //    });
        //}
        //getUserStatistic();
        function getSellStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
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
                    //mm/dd/yyyy
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
                    notificationService.displaySuccess('Load data successfully !');
            });
        }
        getInventoryStatistic();
        getSellStatistic();
        getOrderStatistic();
        getStatistic();
    }

})(angular.module('teleworldshop.statistics'));