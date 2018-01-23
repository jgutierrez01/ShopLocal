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
        $("#seccionErrorGrid").css("display", "none");
        $("#seccionAvisoGrid").css("display", "none");
        $("#seccionErrorAdd").css("display", "none");
        $("#seccionErrorEdit").css("display", "none");
        $("#windowGrid").data("kendoWindow").close();
        if (VerificarIncidencias(2, NumeroControlGlobal) > 0) {
            AjaxInactivarSpoolSI(NumeroControlGlobal);
        }
    });
}


function EventoGuardarIncidencia() {
    $("#btnGuardar").click(function (e) {
        if ($("#cmbTipoIncidencia").val() != 0 && $("#cmbTipoIncidencia").val() != undefined) {                       
            if ($("#cmbDetalleIncidencia").val() != 0 && $("#cmbDetalleIncidencia").val() != undefined) {
                if ($("#cmbErrores").val() != 0 && $("#cmbErrores").val() != undefined) {
                    AjaxGuardarIncidencia();
                } else {
                    MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Seleccione un Tipo de Error" : "Select Error Type");
                }
            } else {
                MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Seleccione Un Detalle de Incidencia" : "Select Incident Detail");
            }
        } else {
            MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Seleccione un Tipo de Incidencia" : "Select Incident Type");
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
                AjaxResolucionIncidencias($("#TmpSpoolID").val(), $("#TmpIncidenciaID").val(), $("#OrigenIncidencia").val() == 1 ? "Sol.Inspect Nuevo" : "Sol.Inspect Editar", $("#txtDetalleResolucion").val(), 2, true);
            } else {
                ErrorResolucion($("html").prop("lang") != "en-US" ? "Ingrese Como Resolvió La Incidencia" : "Enter How the Incident Was Resolved");
                $("#txtDetalleResolucion").focus();
            }
        } else {
            AjaxResolucionIncidencias($("#TmpSpoolID").val(), $("#TmpIncidenciaID").val(), $("#OrigenIncidencia").val() == 1 ? "Sol.Inspect Nuevo" : "Sol.Inspect Editar", '', 1, true);
        }
    });
}