namespace("sam.pages.yard");

(function($, undefinied) {

    "use strict";

    sam.pages.yard.index = function ($container, $containerWkS) {
        
        var $cnt = $container,
            $cnContainer,
            $spContainer,
            $cntWkS = $cnt.find("#yard-form");

        var init = function () {

            var $selectedRadio;

            $cnContainer = $cnt.find("#cnContainer");
            $spContainer = $cnt.find("#spContainer");

            $cnt.find("#ProjectId").change(function () {
                var item;

                if ($(this).val()) {
                    item = $(this).find("option:selected").data("item");
                    $cnt.find("#wo-addon-text").text(item.WorkOrderPrefix);
                    var projectId = $(this).val();
                    $cntWkS.find("input[id^='projectId']").val(projectId);
                    $.getJSON('/Controls/UpdateProjetId', { ID: projectId }, function (data) {
                        $.each(data, function (i, item) {
                            console.log('value project : ', item.result);
                        });
                    });

                } else {
                    $cnt.find("#wo-addon-text").text("");
                }
            });

            $cnt.find("input[id^='SearchType']").click(function () {
                switch ($(this).val()) {
                    case "cn":
                        $cnContainer.show();
                        $spContainer.hide();
                        $cnt.find("input[id^='ControlNumber']").val('');
                        $cnt.find("input[id^='WorkOrderNumber']").val('');
                        $cnt.find("#ProjectId").trigger("change");
                        $cnt.find("input[id^='WorkOrderNumber']").focus();
                        break;
                    case "sp":
                        $cnContainer.hide();
                        $spContainer.show();
                        $cnt.find("input[id^='SpoolName']").val('');
                        $cnt.find("input[id^='SpoolName']").focus();
                        break;
                    default:
                        throw new Error("Invalid option");
                }
            });

            $('#resumenSpool').on('click', function (e) {
                $cntWkS.find("input[id^='typeSearch']").val('1');
            });

            $('#detailSpool').on('click', function (e) {
                $cntWkS.find("input[id^='typeSearch']").val('2');
            });

            $selectedRadio = $cnt.find("input[id^='SearchType']:checked");

            if ($selectedRadio.size() == 1) {
                $selectedRadio.trigger("click");
            } else {
                $cnt.find("input[id^='SearchType'][value='cn']")
                    .prop("checked", true)
                    .trigger("click");
            }
        };

        return {
            init: init
        };
    };

})(jQuery);