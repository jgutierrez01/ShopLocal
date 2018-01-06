function ActivarEventos() {
    IniciaGrid();
    ObtenerCacheProyecto();
    EventoClickBuscarSI();
    EventoEnterBuscarSI();
    EventoSelectProyecto();
    EventoVerificarSiEsMobil();
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
                        NoAutorizado: { type: "boolean", editable: true },
                        Accion: { type: "int", editable: false }
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
            { field: "Hold", title: "Hold", template: "#=Hold ? 'Si' : 'No' #", width: "10px" },
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
                }, template: '<input class="chkbx" type="checkbox" name="NoAutorizado" #= NoAutorizado ? "checked=checked" : ""# ></input>', width: "20px", attributes: { style: "text-align:center;" }
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
                        alert("Numero de Control: " + dataItem.NumeroControl );
                    }                    
                },
                title: $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents",
                width: "10px",
                attributes: { style: "text-align: center; margin: 0 auto" }
            }
        ]
    });
}

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
