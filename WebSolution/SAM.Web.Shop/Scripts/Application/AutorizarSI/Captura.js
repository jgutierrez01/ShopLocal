var SpoolIDGlobal = 0;
function ActivarEventos() {
    IniciaGrid();
    ObtenerCacheProyecto();    
    EventoClickBuscarSI();
    EventoEnterBuscarSI();
    EventoSelectProyecto();
    EventoVerificarSiEsMobil();
    EventoChangeGrid();
    EventoTipoIncidencia();
    EventoDetalleIncidencia();
    CerrarVentanaModal();
    
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
            {
                field: "NoAutorizado", title: $("html").prop("lang") != "en-US" ? "No Autorizado" : "Not Authorized", filterable: {
                    multi: true,
                    messages: {
                        isTrue: $("html").prop("lang") != "en-US" ? "V" : "T",
                        isFalse: "F",
                        style: "max-width:20px;"
                    },
                    dataSource: [{ NoAutorizado: true }, { NoAutorizado: false }]
                }, template: '<input class="chkbx" type="checkbox" name="NoAutorizado" #= !Autorizado ? "checked=checked" : ""# ></input>', width: "20px", attributes: { style: "text-align:center;" }
            },
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
                    }                    
                },
                title: $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents",
                width: "10px",
                attributes: { style: "text-align: center; margin: 0 auto" }
            },
            {
                field: "HistorySI", title: $("html").prop("lang") != "en-US" ? "Historial SI" : "History SI", width: "10px", attributes: { style: "text-align: center;" }
            }            
        ]
    });
}


//function CargarGridPopUp() {
//    $("#gridPopUp").kendoGrid({
//        dataSource: {
//            data: [],
//            schema: {
//                model: {
//                    fields: {
//                        JuntaSpoolID: { type: "int", editable: false },
//                        Etiqueta: { type: "string", editable: false },
//                        Cedula: { type: "string", editable: false },
//                        Codigo: { type: "string", editable: false },
//                        Diametro: { type: "number", editable: false },
//                        Espesor: { type: "number", editable: false },
//                        Nombre: { type: "string", editable: false },
//                        TipoPrueba: { type: "string", editable: false },
//                        NumeroRequisicion: { type: "string", editable: false }
//                    }
//                }
//            },
//            pageSize: 10,
//            serverPaging: false,
//            serverFiltering: false,
//            serverSorting: false
//        },
//        pageable: {
//            refresh: false,
//            pageSizes: [10, 25, 50, 100],
//            info: false,
//            input: false,
//            numeric: true,
//        },
//        selectable: true,
//        filterable: getGridFilterableMaftec(),

//        columns: [
//            { field: "Etiqueta", title: _dictionary.columnJunta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "80px", attributes: { style: "text-align:right;" } },
//            { field: "Codigo", title: _dictionary.columnTipoJta[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "112px" },
//            { field: "Cedula", title: _dictionary.columnCedula[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "105px" },
//            { field: "Diametro", title: _dictionary.columnDiametro[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "94px", attributes: { style: "text-align:right;" } },
//            { field: "Espesor", title: _dictionary.columnEspesor[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellNumberMaftec(), width: "112px", attributes: { style: "text-align:right;" } },
//            { field: "Nombre", title: _dictionary.columnClasificacion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "135px" },
//            { field: "TipoPrueba", title: _dictionary.columnTipoPrueba[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "135px" },
//            { field: "NumeroRequisicion", title: _dictionary.columnRequisicion[$("#language").data("kendoDropDownList").value()], filterable: getGridFilterableCellMaftec(), width: "135px" },
//        ],
//        editable: false,
//        navigatable: true
//    });    
//}


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
