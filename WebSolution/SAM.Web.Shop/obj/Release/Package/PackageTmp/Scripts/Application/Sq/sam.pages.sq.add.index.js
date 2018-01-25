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
                        $("#ProjectIdEditar").val(projectId);
                        $.getJSON('/Controls/LimpiarGridAdd', { ProyectoID: projectId }, function (data) {
                            $.each(data, function (i, item) {                                
                                window.location = "/SQ/";
                                $("#ProyectoAnteriorAdd").val(projectId);
                            });
                        });
                    } else {                                                
                        var projectId = ProyectoIDAddAnterior;
                        $("select#ProjectIdADD").val(projectId).prop("selected", true);
                        $cntWkS.find("input[id^='ProjectIdADD']").val(projectId);
                        $("#ProjectIdEditar").val(projectId);
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
                        $("#ProjectIdEditar").val(projectId);
                        $.getJSON('/Controls/UpdateProjetId', { ID: projectId }, function (data) {
                            $.each(data, function (i, item) {
                                console.log('value project : ', item.result);
                                $("#ProyectoAnteriorAdd").val(projectId);
                            });
                        });

                        //Verifico si el proyecto tiene inicializado algun consecutivo
                        if ($("#ProjectIdADD").val() != "" && $("#ProjectIdADD").val() != undefined) {
                            $.ajax({
                                type: 'GET',
                                url: '/SQ/VerificarConsecutivoProyecto/',
                                dataType: 'json',
                                data: { ProyectoID: $("#ProjectIdADD").val() },
                                success: function (data) {
                                    if (data[0].result == "0") {
                                        $("#errorClient").css("display", "block");
                                        $("#errorClient").html("");
                                        $("#errorClient").append("Este Proyecto No Tiene Inicializado Ningun Consecutivo <br>");
                                        $("#TieneConsecutivoAdd").val("0");
                                    } else {
                                        $("#TieneConsecutivoAdd").val("1");
                                        $("#errorClient").html("");
                                        $("#errorClient").append("");
                                        $("#errorClient").css("display", "none");

                                    }
                                },
                                error: function () {
                                    alert("Ocurrio un error");
                                }
                            });
                        }
                        
                    } else {
                        $cnt.find("#wo-addon-text").text("");
                    }
                }                                
            });
                  

            $("#resumenSpool").click(function (e) { //Boton Guardar
                var msg = "";
                var cont = 0;
                if ($("#TieneConsecutivoAdd").val() == "0") {
                    cont++;
                    msg += "El Proyecto Seleccionado No Tiene Inicializado El Campo Consecutivo <br>";
                }

                if ($("#gridAdd").find("table").find("tbody").length == 0) {
                    cont++;
                    msg += "No Hay Datos Por Guardar <br>";
                }

                if (cont > 0) {
                    $("#errorClient").css("display", "block");
                    $("#errorClient").html("");
                    $("#errorClient").append(msg);
                    e.preventDefault();
                } else {
                    $("#errorClient").css("display", "none");
                    $("#errorClient").append("");
                }
            });


            $("#resumenSpool1Add, #resumenSpool2Add").click(function (e) {
                var msg = "";
                var cont = 0;                
                if ($("input[name=SearchTypeADD]:checked").val() == "c") {
                    if ($("#TieneConsecutivoAdd").val() == "0") {
                        cont++;
                        msg += "El Proyecto Seleccionado No Tiene Inicializado El Campo Consecutivo <br>";
                    }

                    if ($("#ProjectIdADD").val() == 0) {
                        cont++;
                        msg += "Seleccione Proyecto <br>";
                    }

                    if ($("#QuadrantIdCADD").val() == 0) {
                        cont++;
                        msg += "Ingrese Cuadrante <br>";
                    }
                    
                    if (cont > 0) {
                        $("#errorClient").css("display", "block");
                        $("#errorClient").html("");
                        $("#errorClient").append("");
                        $("#errorClient").append(msg);
                        e.preventDefault();
                    } else {
                        $("#errorClient").css("display", "none");
                        $("#errorClient").append("");
                    }
                } else if ($("input[name=SearchTypeADD]:checked").val() == "nc") {
                    if ($("#TieneConsecutivoAdd").val() == "0") {
                        cont++;
                        msg += "El Proyecto Seleccionado No Tiene Inicializado El Campo Consecutivo <br>";
                    }
                    if ($("#ProjectIdADD").val() == 0) {
                        cont++;
                        msg += "Seleccione Proyecto <br>";
                    }
                    if ($("#WorkOrderNumberADD").val() == "") {
                        cont++;
                        msg += "Ingrese Orden de Trabajo <br>";
                    }
                    if ($("#ControlNumberADD").val() == "") {
                        cont++;
                        msg += "Ingrese Numero de Control <br>";
                    }

                    if ($("#QuadrantIdNCADD").val() == 0) {
                        cont++;
                        msg += "Ingrese Cuadrante <br>";
                    }

                    if (cont > 0) {
                        $("#errorClient").css("display", "block");
                        $("#errorClient").html("");
                        $("#errorClient").append("");
                        $("#errorClient").append(msg);
                        e.preventDefault();
                    } else {
                        $("#errorClient").css("display", "none");
                        $("#errorClient").append("");
                    }

                } else {
                    //Validacion para Spools Resueltos
                    if ($("#ProjectIdADD").val() == 0) {
                        cont++;
                        msg += "Seleccione Proyecto <br>";
                    }

                    if ($("#QuadrantIdCADD").val() == 0) {
                        cont++;
                        msg += "Ingrese Cuadrante <br>";
                    }

                    if (cont > 0) {
                        $("#errorClient").css("display", "block");
                        $("#errorClient").html("");
                        $("#errorClient").append("");
                        $("#errorClient").append(msg);
                        e.preventDefault();
                    } else {
                        $("#errorClient").css("display", "none");
                        $("#errorClient").append("");
                    }
                }
            });

            $("#QuadrantIdCADD").change(function () {
                if ($(this).val() != 0) {
                    var CuadranteID = $(this).val();
                    $.getJSON('/Controls/UpdateCuadranteID', { CuadranteID: CuadranteID }, function (data) {
                        $.each(data, function (i, item) {
                            if (item.result == "OK") {
                                console.log('cuadrante Actualizdao : ', item.result);
                                $("#QuadrantIdCADD").val(CuadranteID);
                                $("#QuadrantIdNCADD").val(CuadranteID);
                                $("#QuadrantIdNCEdit").val(CuadranteID);
                                $("#QuadrantIdCEdit").val(CuadranteID);
                            }
                        });
                    });

                }
            });
            $cnt.find("#QuadrantIdNCADD").change(function () {
                if ($(this).val() != 0) {
                    var CuadranteID = $(this).val();
                    $.getJSON('/Controls/UpdateCuadranteID', { CuadranteID: CuadranteID }, function (data) {
                        $.each(data, function (i, item) {
                            if (item.result == "OK") {
                                console.log('cuadrante Actualizdao : ', item.result);
                                $("#QuadrantIdCADD").val(CuadranteID);
                                $("#QuadrantIdNCADD").val(CuadranteID);
                                $("#QuadrantIdNCEdit").val(CuadranteID);
                                $("#QuadrantIdCEdit").val(CuadranteID);
                            }
                        });
                    });
                }
            });


            $cnt.find("input[id^='SearchType']").click(function () {
                $("#errorClient").css("display", "none");
                $("#errorClient").append("");
                switch ($(this).val()) {
                    case "c":
                        $cuadranteContainer.show();
                        $numeroControlContainer.hide();                                             
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
                        break;
                    case "s":
                        $cuadranteContainer.show();
                        $numeroControlContainer.hide();
                        break;
                    default:
                        throw new Error("Invalid option");
                }
            });

            $selectedRadio = $cnt.find("input[id^='SearchType']:checked");
            
            if ($selectedRadio.size() == 1) {
                $selectedRadio.trigger("click");
            } else {
                $cnt.find("input[id^='SearchType'][value='c']")
                    .prop("checked", true)
                    .trigger("click");
            }
        };       
        return {
            init: init
        };
    };

})(jQuery);
function abrirIncidencias(SpoolID, NumeroControl, origen) {
    SpoolIDGlobal = SpoolID;
    NumeroControlGlobal = NumeroControl;
    AjaxObtenerTipoIncidencias();    
    VentanaModal();
    $("#OrigenIncidencia").val("")
    $("#OrigenIncidencia").val(origen);
    AjaxObtenerIncidencias(SpoolID);
    
}
