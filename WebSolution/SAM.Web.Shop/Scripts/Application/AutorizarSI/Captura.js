var SpoolIDGlobal = 0;
function ActivarEventos() {
    IniciaGrid();
    CargarGridPopUp(); 
    ObtenerCacheProyecto();    
    EventoClickBuscarSI();
    EventoEnterBuscarSI();
    EventoSelectProyecto();
    EventoVerificarSiEsMobil();
    EventoChangeGrid();
    EventoTipoIncidencia();
    EventoDetalleIncidencia();
    EventoMostrarListaErrores();
    CerrarVentanaModal();
    NombrarEtiquetas();
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
    $("#grid").kendoGrid({
        autoBind: true,
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {                     
                        SpoolID: { type: "int", editable: false },
                        OrdenTrabajoSpoolID: { type: "int", editable: false },
                        NumeroControl: { type: "string", editable: false },                        
                        CuadranteID: { type: "int", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        SI: { type: "string", editable: false },
                        SqCliente: { type: "string", editable: false },                        
                        Hold: { type: "boolean", editable: false },
                        OkPnd: { type: "boolean", editable: false },                                                                        
                        Autorizado: { type: "boolean", editable: true },
                        //NoAutorizado: { type: "boolean", editable: true },
                        Accion: { type: "int", editable: false },
                        Incidencias: { type: "int", editable: false },
                        HistorySI: { type: "string", editable: false }
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
            { field: "Hold", title: "Hold", template: "#=Hold ? 'Si' : 'No' #", width: "10px", attributes: {style: "text-align: center;"} },
            {
                field: "Autorizado", title: $("html").prop("lang") != "en-US" ? "Autorizado" : "Authorized", filterable: {
                    multi: true,
                    messages: {
                        isTrue: $("html").prop("lang") != "en-US" ? "V" : "T",
                        isFalse: "F",
                        style: "max-width:20px;"
                    },
                    dataSource: [{ Autorizado: true }, { Autorizado: false }]
                }, template: '<input class="chkbx" type="checkbox" name="Autorizado" #= Autorizado ? "checked=checked" : ""# ></input>', width: "20px", attributes: { style: "text-align:center;" }
            },
            //{
            //    field: "NoAutorizado", title: $("html").prop("lang") != "en-US" ? "No Autorizado" : "Not Authorized", filterable: {
            //        multi: true,
            //        messages: {
            //            isTrue: $("html").prop("lang") != "en-US" ? "V" : "T",
            //            isFalse: "F",
            //            style: "max-width:20px;"
            //        },
            //        dataSource: [{ NoAutorizado: true }, { NoAutorizado: false }]
            //    }, template: '<input class="chkbx" type="checkbox" name="NoAutorizado" #= !Autorizado ? "checked=checked" : ""# ></input>', width: "20px", attributes: { style: "text-align:center;" }
            //},
            {
                field: "Incidencias", title: $("html").prop("lang") != "en-US" ? "Num. Incidencias" : "Num. Incidents", width: "10px", attributes: {style: "text-align: center;"}
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
                        VentanaModal();
                        AjaxObtenerTipoIncidencias();
                        AjaxObtenerIncidencias(dataItem.SpoolID);
                    }                    
                },
                title: $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents",
                width: "10px",
                attributes: { style: "text-align: center; margin: 0 auto" }
            }//,
            //{
            //    field: "HistorySI", title: $("html").prop("lang") != "en-US" ? "Historial SI" : "History SI", width: "10px", attributes: { style: "text-align: center;" }
            //}            
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
                        //MaterialSpoolID: { type: "int", editable: false },
                        //MaterialSpool: { type: "string", editable: false },
                        //JuntaSpoolID: { type: "int", editable: false },
                        //JuntaSpool: { type: "string", editable: false },
                        ErrorID: { type: "int", editable: false },
                        Error: { type: "string", editable: false },
                        Observaciones: { type: "string", editable: false },
                        Usuario: { type: "string", editable: false },
                        FechaIncidencia: { type: "string", editable: false }
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
            //{ field: "JuntaSpool", title: "JuntaSpool", width: "20px" },
            { field: "Error", title: "Error", width: "30px"  },
            { field: "Observaciones", title: $("html").prop("lang") != "en-US" ? "Observaciones" : "Observations", width: "20px" },
            //{ field: "Usuario", title: $("html").prop("lang") != "en-US" ? "Usuario" : "User", width: "20px" },
            //{ field: "FechaIncidencia", title: $("html").prop("lang") != "en-US" ? "Fecha" : "Date", width: "20px" },
            {
                command: {
                    text: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
                    className: "k-button k-button-icontext k-grid-Cancelar ",
                    click: function (e) {
                        e.preventDefault();                                                                        
                        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));                        
                        var dataSource = this.dataSource;
                        if (dataSource.view().length > 1) {                                     
                                                        
                        } else {
                            MostrarErrorGrid($("html").prop("lang") != "en-US" ? "No Hay Incidencias Registradas" : "There are no Registered Incidents");
                            $("#ContenedorGridPopUp").css("display", "none");
                        }
                    }
                },
                title: $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents",
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
    window.data("kendoWindow").center().open();

};

function MostrarError(Error) {
    $("#seccionError").append("");
    $("#seccionError").html("");
    $("#seccionError").css("display", "block");
    $("#seccionError").append(Error);
}
function BorrarSeccionError(Error) {
    $("#seccionError").append("");
    $("#seccionError").html("");
    $("#seccionError").css("display", "none");
}
function MostrarErrorGrid(Error) {
    $("#seccionErrorGrid").append("");
    $("#seccionErrorGrid").html("");
    $("#seccionErrorGrid").css("display", "block");
    $("#seccionErrorGrid").append(Error);
}
function BorrarSeccionErrorGrid() {
    $("#seccionErrorGrid").append("");
    $("#seccionErrorGrid").html("");
    $("#seccionErrorGrid").css("display", "none");
}
function MostrarAvisoGrid(Error) {
    $("#seccionAvisoGrid").append("");
    $("#seccionAvisoGrid").html("");
    $("#seccionAvisoGrid").css("display", "block");
    $("#seccionAvisoGrid").append(Error);
}
function BorrarAvisoGrid() {
    $("#seccionAvisoGrid").append("");
    $("#seccionAvisoGrid").html("");
    $("#seccionAvisoGrid").css("display", "none");
}

function MostrarAviso(Aviso) {
    $("#seccionAviso").append("");
    $("#seccionAviso").html("");
    $("#seccionAviso").css("display", "block");
    $("#seccionAviso").append(Aviso);
}
function BorrarSeccionAviso() {
    $("#seccionAviso").append("");
    $("#seccionAviso").html("");
    $("#seccionAviso").css("display", "none");
}

function EventoVerificarSiEsMobil() {
    if (isMobile.any()) {
        MostrarAviso("Para Visualizar Correctamente El Contenido Por Favor Gira tu Dispositivo Movil (Tiene Que Estar Activa La Rotación)");     
    }
}
