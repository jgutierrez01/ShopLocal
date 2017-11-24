namespace("sam.pages.sq.add");

(function ($, undefinied) {

    "use strict";

    sam.pages.sq.add.index = function ($container, $containerWkS) {

        var $cnt = $container,
            $cuadranteContainer,
            $numeroControlContainer,
            $cntWkS = $cnt.find("#sq-form");
        var $ejecutar = true;

        var init = function () {

            var $selectedRadio;
           
            $cuadranteContainer = $cnt.find("#cuadranteContainer");
            $numeroControlContainer = $cnt.find("#numeroControlContainer");
            var ProyectoIDAddAnterior = ($("#ProyectoAnteriorAdd").val() == null || $("#ProyectoAnteriorAdd").val() == undefined) ? 0 : $("#ProyectoAnteriorAdd").val();
            $cnt.find("#ProjectIdADD").change(function (e) {
                var item;

                if ($("#TieneDatosGridAdd").prop("checked")) {

                    var currentCulture = $("html").prop("lang");
                    var answer = '';
                    if (currentCulture == "en-US") {
                        answer = confirm('Changes Will not Be Saved, Do You Want to Continue?');
                    }
                    else {
                        answer = confirm('Los Cambios No Serán Guardados, ¿Desea Continuar?');
                    }

                    if (answer) {
                        item = $(this).find("option:selected").data("item");
                        $cnt.find("#wo-addon-text").text(item.WorkOrderPrefix);
                        var projectId = $(this).val();
                        $cntWkS.find("input[id^='ProjectIdADD']").val(projectId);
                        $.getJSON('/Controls/LimpiarGridAdd', { ProyectoID: projectId }, function (data) {
                            $.each(data, function (i, item) {
                                //window.location = "/SQ/AddNC?ProjectId=" + projectId;
                                window.location = "/SQ/";
                                $("#ProyectoAnteriorAdd").val(projectId);
                            });
                        });
                    } else {                                                
                        var projectId = ProyectoIDAddAnterior;
                        $("select#ProjectIdADD").val(projectId).prop("selected", true);
                        $cntWkS.find("input[id^='ProjectIdADD']").val(projectId);
                        item = $(this).find("option:selected").data("item");
                        $cnt.find("#wo-addon-text").text(item.WorkOrderPrefix);
                        $.getJSON('/Controls/UpdateProjetId', { ID: projectId }, function (data) {
                            $.each(data, function (i, item) {
                                console.log('value project : ', item.result);                                
                            });
                        });
                    }



                } else {
                    if ($(this).val()) {
                        item = $(this).find("option:selected").data("item");
                        $cnt.find("#wo-addon-text").text(item.WorkOrderPrefix);
                        var projectId = $(this).val();
                        $cntWkS.find("input[id^='ProjectIdADD']").val(projectId);
                        $.getJSON('/Controls/UpdateProjetId', { ID: projectId }, function (data) {
                            $.each(data, function (i, item) {
                                console.log('value project : ', item.result);
                                $("#ProyectoAnteriorAdd").val(projectId);
                            });
                        });

                    } else {
                        $cnt.find("#wo-addon-text").text("");
                    }
                }                                
            });
                  

            $cnt.find("input[id^='SearchType']").click(function () {
                               
                switch ($(this).val()) {
                    case "c":
                        $cuadranteContainer.show();
                        $numeroControlContainer.hide();                        
                        //$cnt.find("input[id^='ControlNumber']").val('');
                        //$cnt.find("input[id^='WorkOrderNumber']").val('');
                        //$cnt.find("#ProjectId").trigger("change");
                        //$cnt.find("input[id^='WorkOrderNumber']").focus();
                        break;
                    case "nc":
                        $cuadranteContainer.hide();
                        $numeroControlContainer.show();
                        var item = $("#ProjectIdADD").find("option:selected").data("item");
                        if (item != null) {
                            $cnt.find("#wo-addon-text").text(item.WorkOrderPrefix);
                        } else {
                            $cnt.find("#wo-addon-text").text("");
                        }
                        //$cnt.find("input[id^='SpoolName']").val('');
                        //$cnt.find("input[id^='SpoolName']").focus();
                        break;
                    default:
                        throw new Error("Invalid option");
                }
            });

            //$('#resumenSpool').on('click', function (e) {
            //    $cntWkS.find("input[id^='typeSearch']").val('1');
            //});

            //$('#detailSpool').on('click', function (e) {
            //    $cntWkS.find("input[id^='typeSearch']").val('2');
            //});

            $selectedRadio = $cnt.find("input[id^='SearchType']:checked");
            
            if ($selectedRadio.size() == 1) {
                $selectedRadio.trigger("click");
            } else {
                $cnt.find("input[id^='SearchType'][value='c']")
                    .prop("checked", true)
                    .trigger("click");
            }



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

                    //var dateProcess = $cnt.find("input[id^='datepicker']").val();
                    //var numberProcess = $cnt.find("input[id^='NumberProcess']").val();
                    //var typeReport = $cnt.find("select[id^='TypeReportId']").val();
                    //var quadrant = $cnt.find("select[id^='QuadrantId']").val();


                    //if (dateProcess) {
                    //    var date = new Date(dateProcess);
                    //    this.href = this.href.replace("DP", dateProcess);
                    //}
                    //else {
                    //    var date = new Date();
                    //    this.href = this.href.replace("DP", dateProcess);
                    //}

                    //if (numberProcess) {
                    //    this.href = this.href.replace("NP", numberProcess);
                    //}
                    //else {
                    //    this.href = this.href.replace("NP", "");
                    //}

                    //if (typeReport) {
                    //    this.href = this.href.replace("TR", typeReport);
                    //}
                    //else {
                    //    this.href = this.href.replace("TR", "0");
                    //}

                    //if (quadrant) {
                    //    this.href = this.href.replace("QTE", quadrant);
                    //}
                    //else {
                    //    this.href = this.href.replace("QTE", "0");
                    //}
                }

                $ejecutar = false;

                return answer;

            });
            
        };

        return {
            init: init
        };
    };

})(jQuery);