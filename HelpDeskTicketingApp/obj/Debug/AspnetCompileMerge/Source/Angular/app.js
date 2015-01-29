var app = angular.module('helpDeskApp', [
    'ngSanitize'
]);



app.controller('MainController', ['$scope', 'ApiDataService', function ($scope, ApiDataService) {
    //getAlbums
    var mvm = this;

    mvm.isSearching = false;
    mvm.isASearching = false;
    mvm.questionCount = 0;
    mvm.foundedQuestions = [];
    mvm.selectQuestion = {};


    mvm.answerCount = 0;
    mvm.foundedAnswers = [];

    //ApiDataService.getAlbums().then(function (data) {
    //    $scope.albums = data;
    //});

    mvm.searchOnSOF = function (search) {
        mvm.isSearching = true;
        ApiDataService.searchQuestions(search).then(function (data) {
            mvm.foundedQuestions = data;
            mvm.questionCount = mvm.foundedQuestions.items.length;
            mvm.isSearching = false;
        });
    };

    mvm.searchOnSOFinTitle = function (search) {
        mvm.isSearching = true;
        ApiDataService.searchQuestionsIntitles(search).then(function (data) {
            mvm.foundedQuestions = data;
            mvm.questionCount = mvm.foundedQuestions.items.length;
            mvm.isSearching = false;
        });
    };


    mvm.getAnswerbyID = function (questionId) {
        mvm.isASearching = true;
        ApiDataService.searchAsnwers(questionId).then(function (data) {
            mvm.foundedAnswers = data.items;
            mvm.answerCount = mvm.foundedAnswers.length;
            mvm.isASearching = false;
        });
    };

    mvm.getQuestion = function (question) {
        console.log(question);
        angular.element('#questionModal').modal('show');
        mvm.selectQuestion = question;
    };


}]);





app.factory('ApiDataService', ['$http', '$q', function ($http, $q) {
   
    var mainSearchUrl = "https://api.stackexchange.com/2.2/search/advanced?filter=!9YdnSJ*_S&fromdate=1341100800&todate=1412121600&";
    mainSearchUrl += "order=desc&pagesize=100&sort=activity&site=stackoverflow&q=";

    var inTitleUrl = "https://api.stackexchange.com/2.2/search?order=desc&sort=activity&";
    inTitleUrl += "fromdate=1341100800&todate=1412121600&order=desc&pagesize=100&site=stackoverflow&intitle=";


    var answerUrl = "https://api.stackexchange.com/2.2/questions/";
   var answerUrl2 = "/answers?order=desc&sort=activity&site=stackoverflow&filter=!9YdnSM68i";

    var searchQuestions = function (search) {
        var questionsDeferred = $q.defer();

        $http({

            url: mainSearchUrl + search,
            method: 'GET',
        }).success(function (data) {

            questionsDeferred.resolve(data);
        }).error(function () {

            questionsDeferred.reject();
        });

        return questionsDeferred.promise;
    }


    var searchQuestionsIntitles = function (search) {
        var questionsDeferred = $q.defer();

        $http({

            url: inTitleUrl + search,
            method: 'GET',
        }).success(function (data) {

            questionsDeferred.resolve(data);
        }).error(function () {

            questionsDeferred.reject();
        });

        return questionsDeferred.promise;
    }

    var searchAsnwers = function (id) {
        var answerDeferred = $q.defer();

        $http({

            url: answerUrl +id+answerUrl2 ,
            method: 'GET',
        }).success(function (data) {

            answerDeferred.resolve(data);
        }).error(function () {

            answerDeferred.reject();
        });

        return answerDeferred.promise;
    }
  


    return {

        searchQuestions: searchQuestions,
        searchAsnwers: searchAsnwers,
        searchQuestionsIntitles: searchQuestionsIntitles
    };

}]);
