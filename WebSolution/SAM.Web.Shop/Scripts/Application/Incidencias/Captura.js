var SpoolIDGlobal = 0;
var timeOut;
var NumeroControlGlobal = "";
function ActivarEventos() {    
    document.title = $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidens";
    IniciaGrid();
    CargarGridPopUp();
    ObtenerCacheProyecto();    
    EventoClickBuscarSpools();
    NombrarEtiquetas();    
    EventoSelectProyecto();    
    EventoChangeGrid();
    EventoTipoIncidencia();
    EventoDetalleIncidencia();
    EventoMostrarListaErrores();
    EventoFocusOutCombos();
    CerrarVentanaModal();
    EventoGuardarIncidencia();
}

function NombrarEtiquetas() {
    $("#lblTipoIncidencia").text($("html").prop("lang") != "en-US" ? "Tipo Incidencia" : "Incident Type");
    $("#lblDetalleIncidencia").text($("html").prop("lang") != "en-US" ? "Detalle Incidencia" : "Incident Detail");
    $("#lblErrores").text("Error");
    $("#lblObservacion").text($("html").prop("lang") != "en-US" ? "Observaciones" : "Observations");    
    $("#txtObservacion").css("resize", "none");
}
function IniciaGrid() {
    kendo.ui.Grid.fn.editCell = (function (editCell) {
        return function (cell) {
            cell = $(cell);

            var that = this,
                column = that.columns[that.cellIndex(cell)],
                model = that._modelForContainer(cell),
                event = {
                    container: cell,
                    model: model,
                    preventDefault: function () {
                        this.isDefaultPrevented = true;
                    }
                };

            if (model && typeof this.options.beforeEdit === "function") {
                this.options.beforeEdit.call(this, event);
                if (event.isDefaultPrevented) return;
            }

            editCell.call(this, cell);
        };
    })(kendo.ui.Grid.fn.editCell);
    $("#grid").kendoGrid({
        autoBind: true,
        edit: function (e) {
            if (e.model.Incidencias > 0) {
                this.closeCell();
                MostrarError($("html").prop("lang") != "en-US" ? "El Spool " + e.model.NumeroControl + "  Tiene  Incidencias" : "The Spool " + e.model.NumeroControl + " Has Incidents");
            }
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        SpoolID: { type: "int", editable: false },
                        CuadranteID: { type: "int", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        Hold: { type: "boolean", editable: false },
                        Incidencias: { type: "int", editable: false }
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        filterable: false,
        navigatable: true,
        editable: true,
        //autoWidth: true,
        autoHeigth: true,
        sortable: true,
        scrollable: false,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        columns: [
            { field: "NumeroControl", title: $("html").prop("lang") != "en-US" ? "Numero de Control" : "Control Number", width: "20px" },
            { field: "Cuadrante", title: $("html").prop("lang") != "en-US" ? "Cuadrante" : "Quadrant", width: "20px" },
            { field: "Hold", title: "Hold", template: "#=Hold ? 'Si' : 'No' #", width: "10px", attributes: { style: "text-align: center;" } },            
            {
                field: "Incidencias", title: $("html").prop("lang") != "en-US" ? "Num. Incidencias" : "Num. Incidents", width: "10px", attributes: { style: "text-align: center;" }
            },
            {
                command: {
                    text: $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents",
                    className: "k-button k-button-icontext k-grid-Incidencias ",
                    click: function (e) {
                        e.preventDefault();
                        var grid = $("#grid").data("kendoGrid");
                        var ds = grid.dataSource;
                        var dataItem = grid.dataItem($(e.target).closest("tr"));
                        SpoolIDGlobal = dataItem.SpoolID;
                        NumeroControlGlobal = dataItem.NumeroControl;
                        VentanaModal();
                        $("#txtNumeroControl").text(NumeroControlGlobal);
                        AjaxObtenerTipoIncidencias();
                        AjaxObtenerIncidencias(dataItem.SpoolID);
                    }
                },
                title: $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents",
                width: "10px",
                attributes: { style: "text-align: center; margin: 0 auto" }
            }
        ]       
    });
}


function CargarGridPopUp() {
    $("#gridPopUp").kendoGrid({
        autoBind: true,
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        Accion: { type: "int", editable: false },
                        IncidenciaID: { type: "int", editable: false },
                        SpoolID: { type: "int", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        Incidencia: { type: "string", editable: false },
                        MaterialJunta: { type: "string", editable: false },
                        ErrorID: { type: "int", editable: false },
                        Error: { type: "string", editable: false },
                        Observaciones: { type: "string", editable: false },
                        SI: { type: "string", editable: false }
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
        filterable: false,
        navigatable: true,
        editable: true,
        //autoWidth: true,
        autoHeigth: true,
        sortable: true,
        scrollable: false,
        pageable: {
            refresh: false,
            pageSizes: [10, 25, 50, 100],
            info: false,
            input: false,
            numeric: true,
        },
        columns: [
            { field: "Incidencia", title: $("html").prop("lang") != "en-US" ? "Incidencia" : "Incident", width: "20px" },
            { field: "MaterialJunta", title: "Material/Junta", width: "20px" },
            { field: "Error", title: "Error", width: "30px" },
            { field: "Observaciones", title: $("html").prop("lang") != "en-US" ? "Observaciones" : "Observations", width: "20px" },
            { field: "SI", title: "SI", width: "20px" },
            {
                command: {
                    text: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
                    className: "k-button k-button-icontext k-grid-Cancelar ",
                    click: function (e) {
                        var grid = $("#gridPopUp").data("kendoGrid");
                        var ds = grid.dataSource;
                        var dataItem = grid.dataItem($(e.target).closest("tr"));
                        if (confirm($("html").prop("lang") != "en-US" ? "Realmente Desea Eliminar Esta Incidencia?" : "Do You Really Want To Eliminate This Incidence?")) {
                            AjaxEliminarIncidencia(dataItem.IncidenciaID, dataItem.SpoolID, 'Incidencias');
                        } else {
                            e.preventDefault();
                        }
                    }
                },
                title: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
                width: "10px",
                attributes: { style: "text-align: center; margin: 0 auto" }
            },
            {
                command: {
                    text: $("html").prop("lang") != "en-US" ? "Resolver" : "Solve",
                    className: "k-button k-button-icontext k-grid-Resolver ",
                    click: function (e) {
                        var grid = $("#gridPopUp").data("kendoGrid");
                        var ds = grid.dataSource;
                        var dataItem = grid.dataItem($(e.target).closest("tr"));
                        if (confirm($("html").prop("lang") != "en-US" ? "Confirma Resolver Esta Incidencia?" : "Confirm Resolve This Incidence?")) {
                            AjaxResolverIncidencia(dataItem.IncidenciaID, dataItem.SpoolID, 'Incidencias');
                        } else {
                            e.preventDefault();
                        }
                    }
                },
                title: $("html").prop("lang") != "en-US" ? "Resolver" : "Solve",
                width: "10px",
                attributes: { style: "text-align: center; margin: 0 auto" }
            },
        ]
    });
}


function VentanaModal() {
    var modalTitle = "";
    modalTitle = $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents";
    var window = $("#windowGrid");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "95%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },        
        close: function onClose(e) {
            $("#cmbTipoIncidencia").data("kendoComboBox").dataSource.data([]);
            $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data([]);
            $("#cmbErrores").data("kendoComboBox").dataSource.data([]);
        }
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();
    window.parent().find(".k-window-action").css("visibility", "hidden");

};

function MostrarError(Error) {
    $("#seccionError").append("");
    $("#seccionError").html("");
    $("#seccionError").css("display", "block");
    $("#seccionError").append(Error);
    BorrarSeccionError();
}
function BorrarSeccionError() {    
    clearTimeout(timeOut);
    if (!$("#seccionError").is("visible")) {
        timeOut = setTimeout(function () {
            $("#seccionError").fadeTo(1000, 500).slideUp(500, function () {
                $("#seccionError").css("display", "none");
                $("#seccionError").slideUp(500);
                $("#seccionError").append("");
                $("#seccionError").html("");                
            });
        }, 2000);
    }
}
function MostrarSuccess(Error) {    
    $("#seccionSuccess").append("");
    $("#seccionSuccess").html("");
    $("#seccionSuccess").css("display", "block");
    $("#seccionSuccess").append(Error);
    BorrarSuccess();
}
function BorrarSuccess() {    
    clearTimeout(timeOut);
    if (!$("#seccionSuccess").is("visible")) {
        timeOut = setTimeout(function () {
            $("#seccionSuccess").fadeTo(1000, 500).slideUp(500, function () {
                $("#seccionSuccess").slideUp(500);
                $("#seccionSuccess").append("");
                $("#seccionSuccess").html("");
                $("#seccionSuccess").css("display", "none");
            });
        }, 2000);
    }
}

function MostrarErrorGrid(Error) {    
    $("#seccionErrorGrid").append("");
    $("#seccionErrorGrid").html("");
    $("#seccionErrorGrid").css("display", "block");
    $("#seccionErrorGrid").append(Error);
    BorrarSeccionErrorGrid();

}
function BorrarSeccionErrorGrid() {    
    clearTimeout(timeOut);        
    if (!$("#seccionErrorGrid").is("visible")) {
        timeOut = setTimeout(function () {
            $("#seccionErrorGrid").fadeTo(1000, 500).slideUp(500, function () {
                $("#seccionErrorGrid").slideUp(500);
                $("#seccionErrorGrid").append("");
                $("#seccionErrorGrid").html("");
                $("#seccionErrorGrid").css("display", "none");
            });
        }, 2000);
    }       
}
function MostrarAvisoGrid(Error) {    
    $("#seccionAvisoGrid").append("");
    $("#seccionAvisoGrid").html("");
    $("#seccionAvisoGrid").css("display", "block");
    $("#seccionAvisoGrid").append(Error);
    BorrarAvisoGrid();
}
function BorrarAvisoGrid() {
    clearTimeout(timeOut);
    if (!$("#seccionAvisoGrid").is("visible")) {
        timeOut = setTimeout(function () {
            $("#seccionAvisoGrid").fadeTo(1000, 500).slideUp(500, function () {
                $("#seccionAvisoGrid").slideUp(500);
                $("#seccionAvisoGrid").append("");
                $("#seccionAvisoGrid").html("");
                $("#seccionAvisoGrid").css("display", "none");
            });
        }, 2000);
    }
}

function MostrarAviso(Aviso) {    
    $("#seccionAviso").append("");
    $("#seccionAviso").html("");
    $("#seccionAviso").css("display", "block");
    $("#seccionAviso").append(Aviso);
    BorrarSeccionAviso();
}
function BorrarSeccionAviso() {
    clearTimeout(timeOut);
    if (!$("#seccionAviso").is("visible")) {
        timeOut = setTimeout(function () {
            $("#seccionAviso").fadeTo(1000, 500).slideUp(500, function () {
                $("#seccionAviso").slideUp(500);
                $("#seccionAviso").append("");
                $("#seccionAviso").html("");
                $("#seccionAviso").css("display", "none");
            });
        }, 2000);
    }
}

function EventoVerificarSiEsMobil() {
    if (isMobile.any()) {
        MostrarAviso("Para Visualizar Correctamente El Contenido Por Favor Gira tu Dispositivo Movil (Tiene Que Estar Activa La Rotación)");
    }
}

function BorrarCampos() {
    $("#cmbTipoIncidencia").data("kendoComboBox").dataSource.data([]);
    $("#cmbTipoIncidencia").data("kendoComboBox").text("");
    $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data([]);
    $("#cmbDetalleIncidencia").data("kendoComboBox").text("");
    $("#cmbErrores").data("kendoComboBox").dataSource.data([]);
    $("#cmbErrores").data("kendoComboBox").text("");
    $("#txtObservacion").val("");
}
