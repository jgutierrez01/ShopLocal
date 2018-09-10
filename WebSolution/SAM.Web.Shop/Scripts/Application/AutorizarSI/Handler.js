﻿function EventoEnterBuscarSI() {
    $("#txtSI").keydown(function (e) {
        if (e.keyCode == 13) {
            if (($("#ProjectIdADD").val() == "0" || $("#ProjectIdADD").val() == 0) && $("#txtSI").val() == "") {                
                MostrarError($("html").prop("lang") == "en-US" ? "Please Select A Project And Enter Sol. Inspect" : "Por Favor Seleccione un Proyecto E Ingrese Sol. Inspect");                
            } else {
                if ($("#ProjectIdADD").val() != "0" && $("#ProjectIdADD").val() != 0) {
                    if ($("#txtSI").val() != "") {
                        $("#chkAutorizaTodo").prop("checked", false);
                        AjaxObtenerSpools();                        
                    } else {
                        //Falta SI
                        MostrarError($("html").prop("lang") == "en-US" ? "Please Enter Sol. Inspect" : "Por Favor Ingrese Sol. Inspect");
                        $("#grid").data("kendoGrid").dataSource.data([]);                
                        $("#contieneGrid").css("display", "none");
                    }
                } else {
                    //Falta Proyecto                    
                    MostrarError($("html").prop("lang") == "en-US" ? "Please Select A Project." : "Por Favor Seleccione un Proyecto");
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    $("#contieneGrid").css("display", "none");
                }
            }
        }
    });
}

function EventoClickBuscarSI() {
    $("#BuscarSI").click(function (e) {
        if (($("#ProjectIdADD").val() == "0" || $("#ProjectIdADD").val() == 0) && $("#txtSI").val() == "") {            
            MostrarError($("html").prop("lang") == "en-US" ? "Please Select A Project And Enter Sol. Inspect" : "Por Favor Seleccione un Proyecto E Ingrese Sol. Inspect" );            
        } else {
            if ($("#ProjectIdADD").val() != "0" && $("#ProjectIdADD").val() != 0) {
                if ($("#txtSI").val() != "") {
                    $("#chkAutorizaTodo").prop("checked", false);
                    AjaxObtenerSpools();
                } else {
                    //Falta SI                    
                    MostrarError($("html").prop("lang") == "en-US" ? "Please Enter Sol. Inspect" : "Por Favor Ingrese Sol. Inspect");
                    $("#grid").data("kendoGrid").dataSource.data([]);
                    $("#contieneGrid").css("display", "none");
                }
            } else {
                //Falta Proyecto                
                MostrarError($("html").prop("lang") == "en-US" ? "Please Select A Project." : "Por Favor Seleccione un Proyecto");
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#contieneGrid").css("display", "none");
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
                if (dataItem.Incidencias > 0) {
                    e.stopPropagation();                    
                    this.checked = false;
                    MostrarError($("html").prop("lang") != "en-US" ? "El Spool " + dataItem.NumeroControl + " Tiene  Incidencias" : "The Spool " + dataItem.NumeroControl + " Has Incidents");
                    grid.dataSource.sync();
                } else {
                    dataItem.set("Autorizado", this.checked);                    
                    grid.dataSource.sync();
                }                
                break;            
        }
    });
}

function EventoTipoIncidencia() {
    $("#cmbTipoIncidencia").kendoComboBox({
        dataTextField: "Incidencia",
        dataValueField: "TipoIncidenciaID",                
        suggest: true,
        index: 3,
        filter: "contains",
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);
            if (dataItem != null) {
                AjaxObtenerDetalleIncidencias(dataItem.TipoIncidenciaID);
                AjaxObtenerListaErrores(dataItem.TipoIncidenciaID);
            }
        }
    });
}


function EventoDetalleIncidencia() {
    $("#cmbDetalleIncidencia").kendoComboBox({
        dataTextField: "Etiqueta",
        dataValueField: "ID",
        suggest: true,
        index: 3,
        filter: "contains",
        change: function (e) {
            var dataItem = this.dataItem(e.sender.selectedIndex);            
        }
    });
}
function EventoMostrarListaErrores() {
    $("#cmbErrores").kendoComboBox({
        dataTextField: "Error",
        dataValueField: "ErrorID",
        suggest: true,
        index: 3,
        filter: "contains",
        change: function (e) { }
    });
}

function EventoFocusOutCombos() {
    $("#cmbTipoIncidencia").focusout(function () {
        var combo = $("#cmbTipoIncidencia").data("kendoComboBox");
        if (combo.selectedIndex < 0) {
            combo.text("");
        }
    });
    $("#cmbDetalleIncidencia").focusout(function () {
        var combo = $("#cmbDetalleIncidencia").data("kendoComboBox");
        if (combo.selectedIndex < 0) {
            combo.text("");
        }
    });
    $("#cmbErrores").focusout(function () {
        var combo = $("#cmbErrores").data("kendoComboBox");
        if (combo.selectedIndex < 0) {
            combo.text("");
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
        $("#cmbErrores").data("kendoComboBox").dataSource.data([]);
        $("#cmbErrores").data("kendoComboBox").text("");
        $("#txtObservacion").val("");
        $("#ContenedorGridPopUp").css("display", "none");
        $("#windowGrid").data("kendoWindow").close();
    });
}
function EventoGuardarIncidencia() {
    $("#btnGuardar").click(function (e) {
        if ($("#cmbTipoIncidencia").val() != 0 && $("#cmbTipoIncidencia").val() != undefined) {
            if (($("#cmbTipoIncidencia").data("kendoComboBox").text() == "Spool" && $("#cmbDetalleIncidencia").val() == 0)) {
                if ($("#cmbErrores").val() != 0 && $("#cmbErrores").val() != undefined) {
                    AjaxGuardarIncidencia();
                } else {
                    MostrarMensaje($("html").prop("lang") != "en-US" ? "Seleccione un Tipo de Error" : "Select Error Type", "seccionErrorGrid");
                }
            } else {
                if ($("#cmbDetalleIncidencia").val() != 0 && $("#cmbDetalleIncidencia").val() != undefined) {
                    if ($("#cmbErrores").val() != 0 && $("#cmbErrores").val() != undefined) {
                        AjaxGuardarIncidencia();
                    } else {
                        MostrarMensaje($("html").prop("lang") != "en-US" ? "Seleccione un Tipo de Error" : "Select Error Type", "seccionErrorGrid");
                    }
                } else {
                    MostrarMensaje($("html").prop("lang") != "en-US" ? "Seleccione Un Detalle de Incidencia" : "Select Incident Detail", "seccionErrorGrid");
                }
            }
        } else {
            MostrarMensaje($("html").prop("lang") != "en-US" ? "Seleccione un Tipo de Incidencia" : "Select Incident Type", "seccionErrorGrid");
        }
    });
}
function EventoGuardarAutorizacion() {
    $("#btnGuardarCaptura").click(function () {
        var grid = $("#grid").data("kendoGrid");
        var ds = grid.dataSource;
        if ($("#txtSI").val() != "") {
            if (ds.data().length > 0) {
                AjaxGuardarAutorizacion(ds.data());                
            } else {
                MostrarError($("html").prop("lang") != "en-US" ? "No Hay Datos Por Guardar" : "No Data To Save");
            }
        } else {
            MostrarError($("html").prop("lang") != "en-US" ? "Ingrese Sol. Inspect" : "Enter Sol. Inspect");
        }
        
    });
}

function EventoClickCheckAutorizarTodo() {
    $("#chkAutorizaTodo").change(function (e) {
        e.preventDefault();
        var valor = this.checked;
        var grid = $("#grid").data("kendoGrid").dataSource._data;
        for (var i = 0; i < grid.length; i++) {
            if (grid[i].Incidencias == 0) {
                grid[i].Autorizado = valor;
                $("#grid").data("kendoGrid").dataSource.sync();
            }
        }
    });
}

/*Nuevos Requerimientos Fase 1 Proy: 00056 Shop*/
function CerrarModalResolucion() {
    $("#CerrarResolucion").click(function (e) {
        e.preventDefault();
        $("#txtDetalleResolucion").val("");
        $("#VentanaResolucion").data("kendoWindow").close();
    });
}

function EventoChangeRadioResolverIncidencia() {
    $('input[type=radio][name=radioOpcionResolver]').change(function () {
        if (this.value == 'Otro') {
            $("#campoDetalleResolver").css("display", "block");
            $("#txtDetalleResolucion").val("");
        } else {
            $("#campoDetalleResolver").css("display", "none");
            $("#txtDetalleResolucion").val("");
        }
    });
}

function EventoGuardarResolucion() {
    $("#btnGuardarResolucion").click(function (e) {
        if ($("input[type=radio][value=Otro]").prop("checked")) {
            if ($("#txtDetalleResolucion").val() != "") {
                //(spoolID, incidenciaID, origen, resolucion, accion, esModal)
                AjaxResolucionIncidencias($("#TmpSpoolID").val(), $("#TmpIncidenciaID").val(), 'AutorizarSI', $("#txtDetalleResolucion").val(), 2, true);
            } else {
                ErrorResolucion($("html").prop("lang") != "en-US" ? "Ingrese Como Resolvió La Incidencia" : "Enter How the Incident Was Resolved");
                $("#txtDetalleResolucion").focus();
            }
        } else {
            AjaxResolucionIncidencias($("#TmpSpoolID").val(), $("#TmpIncidenciaID").val(), 'AutorizarSI', '', 1, true);
        }
    });
}