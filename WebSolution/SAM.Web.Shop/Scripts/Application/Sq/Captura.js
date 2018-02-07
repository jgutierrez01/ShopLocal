﻿var SpoolIDGlobal = 0;
var timeOut;
var NumeroControlGlobal = "";

function ActivarEventos(){
    document.title = "Sol. Inspect";
    CargarGridPopUp();
    EventoTipoIncidencia();
    EventoDetalleIncidencia();
    EventoMostrarListaErrores();
    EventoFocusOutCombos();
    CerrarVentanaModal();
    NombrarEtiquetas();
    EventoGuardarIncidencia();
    CerrarModalResolucion();
    EventoChangeRadioResolverIncidencia();
    EventoGuardarResolucion();
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
                        SI: { type: "string", editable: false },
                        Resolver: { type: "boolean", editable: false }
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
            {
                command: {
                    text: $("html").prop("lang") != "en-US" ? "Resolver" : "Solve",
                    className: "k-button k-button-icontext k-grid-Resolver ",
                    click: function (e) {
                        var grid = $("#gridPopUp").data("kendoGrid");
                        var ds = grid.dataSource;
                        var dataItem = grid.dataItem($(e.target).closest("tr"));                                                
                        $("#txtInfoIncidencia").text(
                            $("html").prop("lang") != "en-US" ? "Incidencia: " + dataItem.Incidencia + "\n Material/Junta: " + dataItem.MaterialJunta :
                            "Incidence: " + dataItem.Incidencia + "\n Material/Joint: " + dataItem.MaterialJunta
                        );
                        /*Guardo valores para poder identificar que incidencia fue y que spool en la ventana modal*/
                        $("#TmpSpoolID").val(dataItem.SpoolID);
                        $("#TmpIncidenciaID").val(dataItem.IncidenciaID);
                        $("#txtNumeroControlResolucion").text("");
                        AjaxVerificarEsGranel(function (data) {
                            if (data != "GRANEL") {
                                $("#txtNumeroControlResolucion").append("<a href='/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=" + NumeroControlGlobal + "&ProyectoID=" + $("#ProjectIdADD").val() + "' target='_blank'>" + NumeroControlGlobal + "</a> ");
                            } else {
                                $("#txtNumeroControlResolucion").append("<label>" + NumeroControlGlobal + "</label>");
                            }
                        });
                        //$("#txtNumeroControlResolucion").append("<a href='/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=" + NumeroControlGlobal + "&ProyectoID=" + $("#ProjectIdADD").val() + "' target='_blank'>" + NumeroControlGlobal + "</a> ");
                        AbrirVentanaResolucion();                                                
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
    $("#txtNumeroControl").text("");
    AjaxVerificarEsGranel(function (data) {
        if (data != "GRANEL") {
            $("#txtNumeroControl").append("<a href='/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=" + NumeroControlGlobal + "&ProyectoID=" + $("#ProjectIdADD").val() + "' target='_blank'>" + NumeroControlGlobal + "</a> ");
        } else {
            $("#txtNumeroControl").append("<label>" + NumeroControlGlobal + "</label>");
        }
    });    
    //$("#txtNumeroControl").append("<a href='/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=" + NumeroControlGlobal + "&ProyectoID=" + $("#ProjectIdADD").val() + "' target='_blank'>" + NumeroControlGlobal + "</a> ");
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
        actions: [
            "Close"
        ],
        close: function onClose(e) {
            $("#cmbTipoIncidencia").data("kendoComboBox").dataSource.data([]);
            $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data([]);
            $("#cmbErrores").data("kendoComboBox").dataSource.data([]);
        }
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();//.element.closest(".k-window").css({ top: 15, left: 15 });
    window.parent().find(".k-window-action").css("visibility", "hidden");

}
function NombrarEtiquetas() {
    $("#lblTipoIncidencia").text($("html").prop("lang") != "en-US" ? "Tipo Incidencia" : "Incident Type");
    $("#lblDetalleIncidencia").text($("html").prop("lang") != "en-US" ? "Detalle Incidencia" : "Incident Detail");
    $("#lblErrores").text("Error");
    $("#lblObservacion").text($("html").prop("lang") != "en-US" ? "Observaciones" : "Observations");
    $("#lblAutorizaTodo").text($("html").prop("lang") != "en-US" ? "Autorizar Todos" : "Authorize All");
    $("#txtObservacion").css("resize", "none");
    /*Nuevos requerimientos Shop*/
    $("#txtTituloOpcion1").text($("html").prop("lang") != "en-US" ? "Error de Captura" : "Capture Error");
    $("#txtTituloOpcion2").text($("html").prop("lang") != "en-US" ? "Otro" : "Other");
    $("#lblDetalleResolucion").text($("html").prop("lang") != "en-US" ? "Indique Como Se Resolvió La Incidencia" : "Indicate How the Incident Was Resolved");
    $("#txtDetalleResolucion").css("resize", "none");
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

function MostrarErrorAdd(Aviso) {    
    $("#seccionErrorAdd").append("");
    $("#seccionErrorAdd").html("");
    $("#seccionErrorAdd").css("display", "block");
    $("#seccionErrorAdd").append(Aviso);
    BorrarErrorAdd();
}
function BorrarErrorAdd() {
    clearTimeout(timeOut);
    if (!$("#seccionErrorAdd").is("visible")) {
        timeOut = setTimeout(function () {
            $("#seccionErrorAdd").fadeTo(1000, 500).slideUp(500, function () {
                $("#seccionErrorAdd").slideUp(500);
                $("#seccionErrorAdd").append("");
                $("#seccionErrorAdd").html("");
                $("#seccionErrorAdd").css("display", "none");
            });
        }, 2000);
    }
}


function MostrarErrorEdit(Aviso) {    
    $("#seccionErrorEdit").append("");
    $("#seccionErrorEdit").html("");
    $("#seccionErrorEdit").css("display", "block");
    $("#seccionErrorEdit").append(Aviso);
    BorrarErrorEdit();
}
function BorrarErrorEdit() {
    clearTimeout(timeOut);
    if (!$("#seccionErrorEdit").is("visible")) {
        timeOut = setTimeout(function () {
            $("#seccionErrorEdit").fadeTo(1000, 500).slideUp(500, function () {
                $("#seccionErrorEdit").slideUp(500);
                $("#seccionErrorEdit").append("");
                $("#seccionErrorEdit").html("");
                $("#seccionErrorEdit").css("display", "none");
            });
        }, 2000);
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
function EliminarSpoolDeTabla(NumeroControl) { //Solo Aplica para editar ya que ahi tiene SI
    //Eliminar la fila con javascript                                                        
    $("#gridEditar table").find('tr').filter(function () {
        return $(this).html().indexOf(NumeroControl) != -1;
    }).remove();
}



/*Nuevos Requerimientos Fase 1 Proy: 00056 Shop*/
function AbrirVentanaResolucion() {
    $('input[type=radio][value=Otro]').prop("checked", true).trigger("change"); //Por Default es Error de Captura
    var modalTitle = "";
    modalTitle = $("html").prop("lang") != "en-US" ? "Resolver Incidencia" : "Resolve Incidence";
    var window = $("#VentanaResolucion");
    var win = window.kendoWindow({
        modal: true,
        title: modalTitle,
        resizable: false,
        visible: true,
        width: "70%",
        minWidth: 30,
        position: {
            top: "10px",
            left: "10px"
        },
        actions: [
            "Close"
        ],
        close: function onClose(e) {
            $("#txtDetalleResolucion").val("");
        }
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").center().open();//.element.closest(".k-window").css({ top: 15, left: 15 });
    window.parent().find(".k-window-action").css("visibility", "hidden");
}


function ErrorResolucion(Error) {
    $("#ErrorResolucion").append("");
    $("#ErrorResolucion").html("");
    $("#ErrorResolucion").css("display", "block");
    $("#ErrorResolucion").append(Error);
    BorrarErrorResolucion();
}
function BorrarErrorResolucion() {
    clearTimeout(timeOut);
    if (!$("#ErrorResolucion").is("visible")) {
        timeOut = setTimeout(function () {
            $("#ErrorResolucion").fadeTo(1000, 500).slideUp(500, function () {
                $("#ErrorResolucion").slideUp(500);
                $("#ErrorResolucion").append("");
                $("#ErrorResolucion").html("");
                $("#ErrorResolucion").css("display", "none");
            });
        }, 2000);
    }
}