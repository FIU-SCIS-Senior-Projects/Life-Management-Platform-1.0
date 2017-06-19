var CoachApp = angular.module('CoachApp', []);
CoachApp.controller('CoachController', function ($scope, CoachService) {
 
    getCoaches();
    function getCoaches() {
        CoachService.getCoaches()
            .then(function(coaches) {
                $scope.Coaches = coaches;
                console.log($scope.Coaches);
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