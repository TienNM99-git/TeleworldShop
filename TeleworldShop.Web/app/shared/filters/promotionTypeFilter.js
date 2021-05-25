(function (app) {
    app.filter('promotionTypeFilter', function () {
        return function (input) {
            if (input == 1)
                return 'VND';
            else
                return '%';
        }
    });
})(angular.module('teleworldshop.common'));