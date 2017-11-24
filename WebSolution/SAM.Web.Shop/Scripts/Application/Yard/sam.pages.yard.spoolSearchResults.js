namespace("sam.pages.yard");

(function ($, router, undefinied) {

    "use strict";

    sam.pages.yard.spoolSearchResults = function ($container) {

        var $cnt = $container,
            pager,
            filters;

        var init = function () {
            var $pagerCnt = $cnt.find(".pager"),
                pagerOptions = { $container: $pagerCnt, onNextPage: gotoPage, onPreviousPage: gotoPage };

            filters = $cnt.data("search");
            pager = new sam.controls.pager(pagerOptions);
            pager.init();
        };

        var gotoPage = function (pageNumber, pageSize) {
            var url = router.action("Yard", "Spools", { spoolName: filters.Spool, pageNumber: pageNumber, pageSize: pageSize });

            sam.utils.showPleaseWait();

            $.ajax({
                url: url,
                cache: false,
                success: function (data) {
                    $cnt.find("#rows").html(data);
                },
                complete: function () {
                    sam.utils.hidePleaseWait();
                }
            });
        };

        return {
            init: init
        };
    };

})(jQuery);