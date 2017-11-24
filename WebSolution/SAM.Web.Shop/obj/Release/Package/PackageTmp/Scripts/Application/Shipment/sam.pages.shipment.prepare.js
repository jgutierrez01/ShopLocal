namespace("sam.pages.shipment");


(function ($, undefinied) {


    "use strict";


    sam.pages.shipment.prepare = function ($container, $containerWkS) {


        var $cnt = $container,
            $cnContainer,
            $spContainer,
            $cntWkS = $containerWkS;


        var init = function () {


            var $selectedRadio;
            var $selectedProjectId;


            $cnContainer = $cnt.find("#cnContainer");
            $spContainer = $cnt.find("#spContainer");

            var date = $cnt.find("input[id^='datepicker']").val();


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


            $selectedRadio = $cnt.find("input[id^='SearchType']:checked");


            if ($selectedRadio.size() == 1) {
                $selectedRadio.trigger("click");
            } else {
                $cnt.find("input[id^='SearchType'][value='cn']")
                    .prop("checked", true)
                    .trigger("click");
            }


            $cnt.find("input[id^='NumberProcess']").change(function () {


                var ns = $cnt.find("input[id^='NumberProcess']").val();
                $cntWkS.find("input[id^='ns']").val(ns);
                console.log('value ns : ', $cntWkS.find("input[id^='ns']").val());
            });


            $cnt.find("input[id^='NumberProcess']").focus(function () {
                $(this).select();
            });


            $cnt.find("#TypeReportId").change(function () {
                var tr = $cnt.find("select[id^='TypeReportId']").val();
                $cnt.find("input[id^='TypeReportIDH']").val(tr);
                $cntWkS.find("input[id^='tr']").val(tr);
            });


            $cnt.find("input[id^='datepicker']").change(function () {
                var date = $cnt.find("input[id^='datepicker']").val();
                $cntWkS.find("input[id^='datepicker']").val(date);
                console.log('value dateProcess : ', $cntWkS.find("input[id^='datepicker']").val());
            });


            $cnt.find("#QuadrantId").change(function () {

                if ($(this).val()) {
                    var qdt = $cnt.find("select[id^='QuadrantId']").val();
                    $cnt.find("input[id^='QuadrantIDH']").val(qdt);
                    $cntWkS.find("input[id^='qdt']").val(qdt);

                }
            });



            $("a.delete").on('click', function (s) {

                var currentCulture = $("html").prop("lang");
                var answer = '';
                if (currentCulture == "en-US") {
                    answer = confirm('Confirm delete of this control number?');
                }
                else {
                    answer = confirm('¿Confirma la eliminación del Número de Control?');
                }

                if (answer) {

                    var dateProcess = $cnt.find("input[id^='datepicker']").val();
                    var numberProcess = $cnt.find("input[id^='NumberProcess']").val();
                    var typeReport = $cnt.find("select[id^='TypeReportId']").val();
                    var quadrant = $cnt.find("select[id^='QuadrantId']").val();


                    if (dateProcess) {
                        var date = new Date(dateProcess);
                        this.href = this.href.replace("DP", dateProcess);
                    }
                    else {
                        var date = new Date();
                        this.href = this.href.replace("DP", dateProcess);
                    }


                    if (numberProcess) {
                        this.href = this.href.replace("NP", numberProcess);
                    }
                    else {
                        this.href = this.href.replace("NP", "");
                    }


                    if (typeReport) {
                        this.href = this.href.replace("TR", typeReport);
                    }
                    else {
                        this.href = this.href.replace("TR", "0");
                    }


                    if (quadrant) {
                        this.href = this.href.replace("QTE", quadrant);
                    }
                    else {
                        this.href = this.href.replace("QTE", "0");
                    }
                }


                return answer;


            });
        };


        return {
            init: init
        };
    };




})(jQuery);
