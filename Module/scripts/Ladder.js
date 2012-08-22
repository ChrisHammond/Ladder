function LadderFile($, ko, settings) {
    var moduleId = settings.moduleId;
    var tabId = settings.tabId;
    var serviceFramework = settings.servicesFramework;
    var baseServicePath = serviceFramework.getServiceRoot('Ladder') + 'Ladder.ashx/';

    function game(g) {
        this.GameId = s.GameId;

    }

    var viewModel = {
        games: ko.observableArray([])
    };

    //get slides on initialization
    this.init = function(element) {

        var data = { };
        data.moduleId = moduleId;
        data.tabId = tabId;
        serviceFramework.getAntiForgeryProperty();
        $.ajax({
            type: "POST",
            cache: false,
            url: baseServicePath + 'ListOfGames',
            data: data
        }).done(function(data) {
            viewModel.slides = ko.utils.arrayMap(data, function(s) {
                return new slide(s);
            });
            ko.applyBindings(viewModel);
            $(element).jmpress();
        }).fail(function() {
            Console.Log('Sorry failed to load Games');
        });
    };
}