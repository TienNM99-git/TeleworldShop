(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function revenueStatisticController($scope, apiService, notificationService, $filter) {
        $scope.colors = ['#45b7cd', '#ff6384', '#ff8e72'];
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Revenues', 'Benefit'];
        $scope.userCount = 0;
        $scope.chartdata = [];
        $scope.tablechartData = [];
        $scope.linechartData = [];
        $scope.linechartLabels = [];
        $scope.linechartSerie = 'Total Order';

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
        function getUserStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: '01/01/2021',
                    toDate: '01/01/2022'
                }
            }
            apiService.get('api/statistic/getuserstatistics?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.orderCount = response.data;
            }, function (response) {
                notificationService.displayError('Can not load data');
            });
        }
        getUserStatistic();
        getOrderStatistic();
        getStatistic();
    }

})(angular.module('teleworldshop.statistics'));