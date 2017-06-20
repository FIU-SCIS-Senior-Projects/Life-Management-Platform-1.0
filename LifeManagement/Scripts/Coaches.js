var CoachApp = angular.module('CoachApp', []);
CoachApp.filter('range', function () {
    return function(input, total) {
        total = parseInt(total);

        for (var i=0; i<total; i++) {
            input.push(i);
        }

        return input;
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