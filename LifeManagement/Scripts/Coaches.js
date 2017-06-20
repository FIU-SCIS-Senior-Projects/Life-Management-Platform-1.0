var CoachApp = angular.module('CoachApp', []);
CoachApp.filter('array', function () {
    return function (items,selector) {
        var filtered = [];
        angular.forEach(items, function (item) {
            if(item.FirstName.includes(selector))
                filtered.push(item);
        });
        return filtered;
    };
});
CoachApp.controller('CoachController', function ($scope, CoachService) {
   
   
    getCoaches();
    function getCoaches() {
        CoachService.getCoaches()
            .then(function(coaches) {
                $scope.Coaches = coaches.data;
              
            });
    }
});
 
CoachApp.factory('CoachService', ['$http', function ($http) {
 
    var CoachService = {};
    CoachService.getCoaches = function () {
        return $http.get('/Coaches/GetCoaches');
    };
    return CoachService;
 
}]);