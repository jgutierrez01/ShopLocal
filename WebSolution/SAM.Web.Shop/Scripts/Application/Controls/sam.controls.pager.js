namespace("sam.controls");

(function ($, undefinied) {

    "use strict";

    sam.controls.pager = function (options) {

        var opts = options,
            $cnt,
            pagerModel;

        var init = function () {
            $cnt = opts.$container;
            pagerModel = $cnt.data("pager");

            $cnt.find(".previous a").click(onPreviousPage);
            $cnt.find(".next a").click(onNextPage);

            setPrevNextState();
        };

        var onNextPage = function () {

            if ($(this).parent().hasClass("disabled")) {
                return;
            }

            pagerModel.PageNumber++;

            if ($.isFunction(opts.onNextPage)) {
                opts.onNextPage.call(this, pagerModel.PageNumber, pagerModel.PageSize);
            }

            setPrevNextState();
        };

        var onPreviousPage = function () {

            if ($(this).parent().hasClass("disabled")) {
                return;
            }

            pagerModel.PageNumber--;

            if ($.isFunction(opts.onPreviousPage)) {
                opts.onPreviousPage.call(this, pagerModel.PageNumber, pagerModel.PageSize);
            }

            setPrevNextState();
        };

        var setPrevNextState = function() {
            if (pagerModel.PageNumber === 1) {
                $cnt.find("li.previous").addClass("disabled");
            } else {
                $cnt.find("li.previous").removeClass("disabled");
            }

            if (pagerModel.PageNumber === pagerModel.TotalPages) {
                $cnt.find("li.next").addClass("disabled");
            } else {
                $cnt.find("li.next").removeClass("disabled");
            }
        };
        
        return {
            init: init
        };
    };

})(jQuery);