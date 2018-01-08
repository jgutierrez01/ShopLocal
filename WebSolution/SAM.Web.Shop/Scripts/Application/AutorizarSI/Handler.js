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

function EventoChangeGrid() {
    $("#grid").on("change", ":checkbox", function (e) {        
        var grid = $("#grid").data("kendoGrid");
        var dataItem = grid.dataItem($(e.target).closest("tr"));
        switch (this.name) {
            case "Autorizado":
                dataItem.set("Autorizado", this.checked);
                dataItem.set("NoAutorizado", false);
                grid.dataSource.sync();
                break;
            case "NoAutorizado":
                dataItem.set("Autorizado", false);
                dataItem.set("NoAutorizado", this.checked);
                grid.dataSource.sync();
                break;
        }
    });
}

function EventoTipoIncidencia() {
    $("#cmbTipoIncidencia").kendoComboBox({
        dataTextField: "Incidencia",
        dataValueField: "TipoIncidenciaID",
        suggest: true,
        delay: 10,
        //filter: "endswith",
        //index: 3,+
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != null) {
                AjaxObtenerDetalleIncidencias(dataItem.TipoIncidenciaID);
            }
            //dataItem = this.dataItem(e.sender.selectedIndex);
            //if (dataItem != undefined) {
            //    if ($("#InputID").val().length == 1) {
            //        $("#InputID").data("kendoComboBox").value(("00" + $("#InputID").val()).slice(-3));
            //    }
            //    if ($("#InputID").val() != '' && $("#InputOrdenTrabajo").val() != '') {
            //        Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
            //        $("#LabelProyecto").text(dataItem.Proyecto);
            //        //if ($('input:radio[name=TipoAgregado]:checked').val() != "Reporte") {
            //        //    AjaxJunta($("#InputID").val());
            //        //}                    
            //    }
            //}
        }
    });
}


function EventoDetalleIncidencia() {
    $("#cmbDetalleIncidencia").kendoComboBox({
        dataTextField: "Etiqueta",
        dataValueField: "ID",
        suggest: true,
        delay: 10,
        //filter: "endswith",
        //index: 3,+
        change: function (e) {
            //dataItem = this.dataItem(e.sender.selectedIndex);
            //if (dataItem != undefined) {
            //    if ($("#InputID").val().length == 1) {
            //        $("#InputID").data("kendoComboBox").value(("00" + $("#InputID").val()).slice(-3));
            //    }
            //    if ($("#InputID").val() != '' && $("#InputOrdenTrabajo").val() != '') {
            //        Cookies.set("Proyecto", dataItem.ProyectoID + '°' + dataItem.Proyecto);
            //        $("#LabelProyecto").text(dataItem.Proyecto);
            //        //if ($('input:radio[name=TipoAgregado]:checked').val() != "Reporte") {
            //        //    AjaxJunta($("#InputID").val());
            //        //}                    
            //    }
            //}
        }
    });
}
function CerrarVentanaModal() {
    $("#CerrarModal").click(function (e) {
        e.preventDefault();
        $("#cmbTipoIncidencia").data("kendoComboBox").dataSource.data([]);
        $("#cmbTipoIncidencia").data("kendoComboBox").text("");
        $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data([]);
        $("#cmbDetalleIncidencia").data("kendoComboBox").text("");
        $("#windowGrid").data("kendoWindow").close();
    });
}
