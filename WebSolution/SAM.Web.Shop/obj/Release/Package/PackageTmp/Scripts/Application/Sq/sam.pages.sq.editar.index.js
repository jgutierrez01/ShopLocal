namespace("sam.pages.sq.editar");

(function ($, undefinied) {

    "use strict";

    sam.pages.sq.editar.index = function ($container, $containerWkS) {

        var $cnt = $container,
            $cuadranteContainer,
            $numeroControlContainer,
            $cntWkS = $cnt.find("#sq-form");
        var $ejecutar = true;

        var init = function () {

            var $selectedRadio;
            var ProyectoIDEditarAnterior = ($("#ProjectIdEditar").val() == null || $("#ProjectIdEditar").val() == undefined) ? 0 : $("#ProjectIdEditar").val();
            
            $cuadranteContainer = $cnt.find("#cuadranteEditarContainer");
            $numeroControlContainer = $cnt.find("#numeroControlEditarContainer");           

            $("#SQ").on("focusout", function () {                
                $.getJSON('/Controls/UpdateSQ', { SI: $("#SQ").val() }, function (data) {
                    $.each(data, function (i, item) {
                        if (item.result == "OK") {
                            console.log("SI Actualizado");
                        }
                    });
                });               
            });
            
            $("#BuscarSQ").click(function (e) {
                var cont = 0, msg = "";
                if ($("#SQ").val() == "") {
                    cont++;
                    msg += "Ingrese Sol. Inspect <br>";
                }
                if ($("#TieneConsecutivoEdit").val() == "0") {
                    cont++;
                    msg += "El Proyecto Seleccionado No Tiene Inicializado El Campo Consecutivo <br>";
                }
                if (cont > 0) {
                    $("#errorClientEdit").css("display", "block");
                    $("#errorClientEdit").html("");
                    $("#errorClientEdit").append("");
                    $("#errorClientEdit").append(msg);
                    e.preventDefault();

                } else {
                    $("#errorClientEdit").css("display", "none");
                    $("#errorClientEdit").append("");
                }
            });

            $("#resumenSpool1Edit, #resumenSpool2Edit").click(function (e) {
                var msg = "";
                var cont = 0;
                //input[id ^= 'ProjectIdEditar']
                //if ($("input[name^='SearchTypeEdit']:checked").val() == "c") {
                if ($("input[name='SearchTypeEdit']:checked").val() == "c") {
                    if ($("#TieneConsecutivoEdit").val() == "0") {
                        cont++;
                        msg += "El Proyecto Seleccionado No Tiene Inicializado El Campo Consecutivo <br>";
                    }
                    if ($("#ProjectIdEditar").val() == 0) {
                        cont++;
                        msg += "Seleccione Proyecto <br>";
                    }

                    if ($("#QuadrantIdCEdit").val() == 0) {
                        cont++;
                        msg += "Ingrese Cuadrante <br>";
                    }

                    //if ($("#SQ").val() == "") {
                    //    cont++;
                    //    msg += "Por favor Ingrese Sol. Inspect  <br>";
                    //}

                    //if ($("#SQ").val() != "" && $(".grid-mvc").find("table").find('tr').length == 0) {
                    //    cont++;
                    //    msg += "Porfavor Realize La Busqueda Del SQ Antes De Agregar Nuevos Spools <br>";
                    //}

                    if (cont > 0) {
                        $("#errorClientEdit").append("");
                        $("#errorClientEdit").css("display", "none");
                        $("#errorClientEdit").css("display", "block");                        
                        $("#errorClientEdit").append(msg);
                        e.preventDefault();
                    } else {
                        $("#errorClientEdit").css("display", "none");
                        $("#errorClientEdit").append("");
                    }
                } else if($("input[name='SearchTypeEdit']:checked").val() == "nc") {
                    if ($("#TieneConsecutivoEdit").val() == "0") {
                        cont++;
                        msg += "El Proyecto Seleccionado No Tiene Inicializado El Campo Consecutivo <br>";
                    }

                    if ($("#ProjectIdEditar").val() == 0) {
                        cont++;
                        msg += "Seleccione Proyecto <br>";
                    }                   
                    //if ($("#SQ").val() == "" ) {
                    //    cont++;
                    //    msg += "Porfavor Ingrese Sol. Inspect <br>";
                    //}
                    
                    //if ($("#SQ").val() != "" && $(".grid-mvc").find("table").find('tr').length == 0) {
                    //    cont++;
                    //    msg += "Porfavor Realize La Busqueda Del SQ Antes De Agregar Nuevos Spools <br>";
                    //}                    
                    if ($("#WorkOrderNumberEdit").val() == "") {
                        cont++;
                        msg += "Ingrese Orden de Trabajo <br>";
                    }
                    if ($("#ControlNumberEDIT").val() == "") {
                        cont++;
                        msg += "Ingrese Numero de Control <br>";
                    }

                    if ($("#QuadrantIdNCEdit").val() == 0) {
                        cont++;
                        msg += "Ingrese Cuadrante <br>";
                    }

                    if (cont > 0) {
                        $("#errorClientEdit").css("display", "block");
                        $("#errorClientEdit").html("");
                        $("#errorClientEdit").append("");
                        $("#errorClientEdit").append(msg);
                        e.preventDefault();
                    } else {
                        $("#errorClientEdit").css("display", "none");
                        $("#errorClientEdit").append("");
                    }
                } else {
                    
                    //Validacion para Spools Resueltos
                    if ($("#ProjectIdEditar").val() == 0) {
                        cont++;
                        msg += "Seleccione Proyecto <br>";
                    }

                    if ($("#QuadrantIdCEdit").val() == 0) {
                        cont++;
                        msg += "Ingrese Cuadrante <br>";
                    }

                    if (cont > 0) {
                        $("#errorClientEdit").css("display", "block");
                        $("#errorClientEdit").html("");
                        $("#errorClientEdit").append("");
                        $("#errorClientEdit").append(msg);
                        e.preventDefault();
                    } else {
                        $("#errorClientEdit").css("display", "none");
                        $("#errorClientEdit").append("");
                    }
                }
            });

            $("#GuardarEdicion").click(function (e) {
                var msg = "";
                var cont = 0;
                if ($("#TieneConsecutivoEdit").val() == "0") {
                    cont++;
                    msg += "El Proyecto Seleccionado No Tiene Inicializado El Campo Consecutivo <br>";
                }

                if ($("#SQ").val() == "") {
                    cont++;
                    msg += "Por Favor Ingrese Sol. Inspect";
                }                               

                if ($("#gridEditar").find("table").find("tbody").length == 0 ){                    
                    cont++;
                    msg += "No Hay Datos Por Guardar <br>";
                }

                                                            
                if (cont > 0) {
                    $("#errorClientEdit").css("display", "block");
                    $("#errorClientEdit").html("");                    
                    $("#errorClientEdit").append(msg);
                    e.preventDefault();
                } else {
                    $("#errorClientEdit").css("display", "none");
                    $("#errorClientEdit").append("");
                }
            });

            $("#QuadrantIdCEdit").change(function () {                
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
            $cnt.find("#QuadrantIdNCEdit").change(function () {
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



            $cnt.find("#ProjectIdEditar").change(function () {
                var item;
                if ($("#TieneDatosGrid").prop("checked")) {
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
                        $("#ProjectIdADD").val(projectId);
                        $cntWkS.find("input[id^='ProjectIdEditar']").val(projectId);
                        $.getJSON('/Controls/LimpiarGrid', { ProyectoID: projectId }, function (data) {
                            $.each(data, function (i, item) {
                                window.location = "/SQ/EditarNC?ProjectIdEditar=" + projectId;
                                $("#ProyectoAnterior").val(projectId);
                            });
                        });
                    } else {                        
                        var projectId = ProyectoIDEditarAnterior;
                        $("select#ProjectIdEditar").val(projectId).prop("selected", true);
                        $("#ProjectIdADD").val(projectId);
                        $cntWkS.find("input[id^='ProjectIdEditar']").val(projectId);
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
                        $("#ProjectIdADD").val(projectId);
                        $cntWkS.find("input[id^='ProjectIdEditar']").val(projectId);
                        $.getJSON('/Controls/UpdateProjetId', { ID: projectId }, function (data) {
                            $.each(data, function (i, item) {
                                console.log('value project : ', item.result);
                                $("#ProyectoAnterior").val(projectId);
                            });
                        });

                    } else {
                        $cnt.find("#wo-addon-text").text("");
                    }
                }

                //Verifico si el proyecto tiene inicializado algun consecutivo
                if ($("#ProjectIdEditar").val() != "" && $("#ProjectIdEditar").val() != undefined) {
                    $.ajax({
                        type: 'GET',
                        url: '/SQ/VerificarConsecutivoProyecto/',
                        dataType: 'json',
                        data: { ProyectoID: $("#ProjectIdEditar").val() },
                        success: function (data) {
                            if (data[0].result == "0") {
                                $("#errorClientEdit").css("display", "block");
                                $("#errorClientEdit").html("");
                                $("#errorClientEdit").append("Este Proyecto No Tiene Inicializado Ningun Consecutivo <br>");
                                $("#TieneConsecutivoEdit").val("0");
                            } else {
                                $("#TieneConsecutivoEdit").val("1");
                                $("#errorClientEdit").html("");
                                $("#errorClientEdit").append("");
                                $("#errorClientEdit").css("display", "none");

                            }
                        },
                        error: function () {
                            alert("Ocurrio un error");
                        }
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
                        //$cnt.find("input[id^='ControlNumber']").val('');
                        //$cnt.find("input[id^='WorkOrderNumber']").val('');
                        //$cnt.find("#ProjectId").trigger("change");
                        //$cnt.find("input[id^='WorkOrderNumber']").focus();                       
                        break;
                    case "nc":
                        $cuadranteContainer.hide();
                        $numeroControlContainer.show();
                        var item = $("#ProjectIdEditar").find("option:selected").data("item");
                        if (item != null) {
                            $cnt.find("#wo-addon-text").text(item.WorkOrderPrefix);
                        } else {
                            $cnt.find("#wo-addon-text").text("");
                        }
                        //$cnt.find("input[id^='SpoolName']").val('');
                        //$cnt.find("input[id^='SpoolName']").focus();
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