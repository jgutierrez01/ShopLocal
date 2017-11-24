namespace("sam.pages.home");

(function ($, undefinied) {

    "use strict";

    sam.pages.home.index = function ($container) {

        var $cnt = $container,
            $cnContainer,
            $spContainer;

        var init = function () {

            var $selectedRadio;

            $cnt.find("#YardId").change(function () {
                var item;

                if ($(this).val()) {
                    item = $(this).find("option:selected").data("item");                  
                    var projectId = $(this).val();                    
                    $.getJSON('/Controls/UpdateYardId', { ID: projectId }, function (data) {
                        $.each(data, function (i, item) {
                            console.log('value yard : ', item.result);
                        });
                    });
                }
            });

        };

        return {
            init: init
        };
    };

})(jQuery);