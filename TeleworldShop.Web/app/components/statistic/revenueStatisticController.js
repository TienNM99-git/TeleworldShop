(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function revenueStatisticController($scope, apiService, notificationService, $filter) {
        $scope.colors = ['#45b7cd', '#ff6384', '#ff8e72'];
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Revenues', 'Benefit'];

        $scope.chartdata = [];
        //$scope.datasetOverride = [];
        function getStatistic() {
            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: '01/12/2020',
                    toDate: '01/01/2021'
                }
            }
            apiService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.tabledata = response.data;
                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(response.data, function (i, item) {
                    labels.push($filter('date')(item.Date, 'dd/MM/yyyy'));
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = labels;
                //$scope.datasetOverride.push(
                //    {
                //        label: $scope.labels[0],
                //        borderWidth: 1,
                //        type: 'bar',
                //        data: chartData[0]
                //    },
                //    {
                //        label: $scope.labels[1],
                //        borderWidth: 3,
                //        hoverBackgroundColor: "rgba(255,99,132,0.4)",
                //        hoverBorderColor: "rgba(255,99,132,1)",
                //        type: 'line',
                //        data: chartData[1]
                //    });
            }, function (response) {
                notificationService.displayError('Can not load data');
            });
        }

        getStatistic();
    }

})(angular.module('teleworldshop.statistics'));