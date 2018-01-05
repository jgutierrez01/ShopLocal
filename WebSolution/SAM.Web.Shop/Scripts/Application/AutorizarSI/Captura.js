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
                        NumeroControl: { type: "string", editable: false },
                        SpoolID: { type: "int", editable: false },
                        CuadranteID: { type: "int", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        Accion: { type: "int", editable: false },
                        OrdenTrabajoSpoolID: { type: "int", editable: false },
                        SqCliente: { type: "string", editable: false },
                        SQ: { type: "string", editable: false },
                        TieneHoldIngenieria: { type: "boolean", editable: false },
                        OkPnd: { type: "boolean", editable: false }
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
            { field: "TieneHoldIngenieria", title: "Hold", template: "#=TieneHoldIngenieria ? 'Si' : 'No' #", width: "10px" },

            {
                command: {
                    text: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
                    className: "k-button k-button-icontext k-grid-Cancel ",
                    click: function (e) {
                        e.preventDefault();
                        var grid = $("#grid").data("kendoGrid");
                        var ds = grid.dataSource;
                        var dataItem = grid.dataItem($(e.target).closest("tr"));
                        ds.remove(dataItem);
                        ds.sync();
                    }
                },
                title: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
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
