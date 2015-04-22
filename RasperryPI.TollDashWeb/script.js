// script.js

// create the module and name it scotchApp
var scotchApp = angular.module('scotchApp', ['ngRoute', 'ngGrid']);

// configure our routes
scotchApp.config(function ($routeProvider) {
    $routeProvider
        // route for the about page
        .when('/toll', {
            templateUrl: 'pages/tolls.html',
            controller: 'tollController'
        });
});

scotchApp.controller('tollController', function ($scope, $http) {
    $scope.filterOptions = {
        filterText: ''
    };

    $scope.message = 'Search for Tolls.';
    $scope.searchtext = '';
    $scope.tolls = [];
    $scope.gridOptions = {
        data: 'tolls',
        columnDefs: [
            { field: 'TollId', displayName: 'Toll Id' },
            { field: 'VehicleModel', displayName: 'Vehicle Model' },
            { field: 'VehicleNo', displayName: 'Vehicle No' },
            { field: 'CitizenName', displayName: 'Citizen Name' },
            { field: 'TollLocation', displayName: 'Toll Location' },
            { field: 'TollImageUrl', displayName: 'Image Url', cellTemplate: '<div><a href={{row.entity[col.field]}}>click here</a></div>' }],

        jqueryUIDraggable: true,
        filterOptions: $scope.filterOptions,
        showFilter: true
    };

    $scope.filterNephi = function () {
        var filterText = 'CitizenName:Srini';
        if ($scope.filterOptions.filterText === '') {
            $scope.filterOptions.filterText = filterText;
        }
        else if ($scope.filterOptions.filterText === filterText) {
            $scope.filterOptions.filterText = '';
        }
    };

    // get data.
    $scope.search = function () {

        setTimeout(function() {
            jQuery.support.cors = true;
            $.ajax({
                url: 'http://tastoll.azurewebsites.net/tolls/' + "all",
                type: 'GET',
                dataType: 'json',
                crossDomain: true,
                success: function (data, textStatus, xhr) {
                    function compare(a, b) {
                        if (a.TollId < b.TollId)
                            return 1;
                        if (a.TollId > b.TollId)
                            return -1;
                        return 0;
                    }

                    data.sort(compare);

                    $scope.tolls = (data);
                    $scope.show = $scope.tolls.length > 0;
                    $scope.$apply();
                    // $scope.gridOptions = { data: 'tolls' };
                    $scope.search();
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert('something went wrong while loading data, try again');
                }
            });
        }, 1000);

    };

    $scope.show = $scope.tolls.length > 0;

    $scope.search();
    

});