(function (app) {
    app.filter('statusFilter', function () {
        return function (input) {
            if (input == true)
                return 'Available';
            else
                return 'Unavailable';
        }
    });
})(angular.module('teleworldshop.common'));