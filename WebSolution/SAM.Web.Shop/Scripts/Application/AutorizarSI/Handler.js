function EventoEnterBuscarSI() {   
    $("#txtSI").keydown(function (e) {
        if (e.keyCode == 13) {
            if (($("#ProjectIdADD").val() == "0" || $("#ProjectIdADD").val() == 0) && $("#txtSI").val() == "") {
                if ($("html").prop("lang") == "en-US") {
                    MostrarError("Please Select A Project And Enter Sol. Inspect");
                } else {
                    MostrarError("Por Favor Seleccione un Proyecto E Ingrese Sol. Inspect");
                }
            } else {
                if ($("#ProjectIdADD").val() != "0" && $("#ProjectIdADD").val() != 0) {
                    if ($("#txtSI").val() != "") {
                        //Mostrar Spools          
                        BorrarSeccionError();
                        AjaxObtenerSpools();
                    } else {
                        //Falta SI
                        if ($("html").prop("lang") == "en-US") {
                            MostrarError("Please Enter Sol. Inspect");
                        } else {
                            MostrarError("Por Favor Ingrese Sol. Inspect");
                        }
                    }
                } else {
                    //Falta Proyecto
                    if ($("html").prop("lang") == "en-US") {
                        MostrarError("Please Select A Project.");
                    } else {
                        MostrarError("Por Favor Seleccione un Proyecto");
                    }
                }
            }
        }
    });
}

function EventoClickBuscarSI() {
    $("#BuscarSI").click(function (e) {
        if (($("#ProjectIdADD").val() == "0" || $("#ProjectIdADD").val() == 0) && $("#txtSI").val() == "") {
            if ($("html").prop("lang") == "en-US") {
                MostrarError("Please Select A Project And Enter Sol. Inspect");
            } else {
                MostrarError("Por Favor Seleccione un Proyecto E Ingrese Sol. Inspect");
            }
        } else {
            if ($("#ProjectIdADD").val() != "0" && $("#ProjectIdADD").val() != 0) {
                if ($("#txtSI").val() != "") {
                    //Mostrar Spools          
                    BorrarSeccionError();
                    AjaxObtenerSpools();
                } else {
                    //Falta SI
                    if ($("html").prop("lang") == "en-US") {
                        MostrarError("Please Enter Sol. Inspect");
                    } else {
                        MostrarError("Por Favor Ingrese Sol. Inspect");
                    }
                }
            } else {
                //Falta Proyecto
                if ($("html").prop("lang") == "en-US") {
                    MostrarError("Please Select A Project.");
                } else {
                    MostrarError("Por Favor Seleccione un Proyecto");
                }
            }
        }
    });    
}

function EventoSelectProyecto() {
    $("#ProjectIdADD").on("change", function () {
        if ($(this).val() != 0) {
            ActualizarCacheProyecto($(this).val());
        }
    });
}