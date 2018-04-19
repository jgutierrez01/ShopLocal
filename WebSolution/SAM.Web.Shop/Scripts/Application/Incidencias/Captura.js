var SpoolIDGlobal = 0;
var timeOut;
var NumeroControlGlobal = "";
function ActivarEventos() {
    document.title = $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidens";
    AjaxObtenerSpoolsResueltos();
    IniciaGrid();
    CargarGridPopUp();
    IniciaGridResueltos();
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
    CerrarModalResolucion();
    EventoChangeRadioResolverIncidencia();
    EventoGuardarResolucion();
    EventoGenerarSI();
}

function NombrarEtiquetas() {
    $("#lblTipoIncidencia").text($("html").prop("lang") != "en-US" ? "Tipo Incidencia" : "Incident Type");
    $("#lblDetalleIncidencia").text($("html").prop("lang") != "en-US" ? "Detalle Incidencia" : "Incident Detail");
    $("#lblErrores").text("Error");
    $("#lblObservacion").text($("html").prop("lang") != "en-US" ? "Observaciones" : "Observations");
    $("#txtObservacion").css("resize", "none");
    /*Nuevos requerimientos Shop*/
    $("#txtTituloOpcion1").text($("html").prop("lang") != "en-US" ? "Error de Captura" : "Capture Error");
    $("#txtTituloOpcion2").text($("html").prop("lang") != "en-US" ? "Otro" : "Other");
    $("#lblDetalleResolucion").text($("html").prop("lang") != "en-US" ? "Indique Como Se Resolvió La Incidencia" : "Indicate How the Incident Was Resolved");
    $("#txtDetalleResolucion").css("resize", "none");
    $("#lblSpoolResuelto").text($("html").prop("lang") != "en-US" ? "Generar Sol. Inspect" : "Generate Inspect. Req");
}
function IniciaGrid() {
    $("#grid").kendoGrid({
        autoBind: false,
        autoSync: false,
        edit: function (e) {
            if (e.model.Incidencias > 0) {
                this.closeCell();
                MostrarMensaje($("html").prop("lang") != "en-US" ? "El Spool " + e.model.NumeroControl + "  Tiene  Incidencias" : "The Spool " + e.model.NumeroControl + " Has Incidents", "seccionError");
            }
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        SpoolID: { type: "int", editable: false },
                        ProyectoID: { type: "int", editable: false },
                        CuadranteID: { type: "int", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        Hold: { type: "boolean", editable: false },
                        Incidencias: { type: "number", editable: false },
                        Granel: { type: "boolean", editable: false }
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
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
        filterable: filtroGeneral(),
        columns: [
            {
                field: "NumeroControl", title: $("html").prop("lang") != "en-US" ? "Numero de Control" : "Control Number", filterable: filtroTexto(),
                template: function (dataItem) {
                    return !dataItem.Granel ? '<a href="/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=' + dataItem.NumeroControl + '&ProyectoID=' + dataItem.ProyectoID + '" target="_blank" > ' + dataItem.NumeroControl + ' </a>' : "<label>" + dataItem.NumeroControl + "</label>"
                },
                width: "20px"
                //template: '#=!Granel ? <a href="/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=#=NumeroControl#&ProyectoID=#=ProyectoID#" target="_blank" > #=NumeroControl# </a> # : #=NumeroControl #', width: "20px"
            },
            { field: "Cuadrante", title: $("html").prop("lang") != "en-US" ? "Cuadrante" : "Quadrant", filterable: filtroTexto(), width: "20px" },
            { field: "Hold", title: "Hold", template: "#=Hold ? 'Si' : 'No' #", width: "10px", filterable: filtroSI_NO(), attributes: { style: "text-align: center;" }, },
            { field: "Incidencias", title: $("html").prop("lang") != "en-US" ? "Num. Incidencias" : "Num. Incidents", width: "20px", attributes: { style: "text-align: center;" }, filterable: filtroNumero() },
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
                        AjaxObtenerTipoIncidencias();
                        AjaxObtenerIncidencias(dataItem.SpoolID);
                        VentanaModal();
                        $("#txtNumeroControl").text("");
                        AjaxVerificarEsGranel(function (data) {
                            if (data != "GRANEL") {
                                $("#txtNumeroControl").append("<a href='/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=" + NumeroControlGlobal + "&ProyectoID=" + $("#ProjectIdADD").val() + "' target='_blank'>" + NumeroControlGlobal + "</a> ");
                            } else {
                                $("#txtNumeroControl").append("<label>" + NumeroControlGlobal + "</label>");
                            }
                        });
                    }
                },
                title: $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents",
                width: "10px",
                attributes: { style: "text-align: center; margin: 0 auto" },
                filterable: false
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
            //{
            //    command: {
            //        text: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
            //        className: "k-button k-button-icontext k-grid-Cancelar ",
            //        click: function (e) {
            //            var grid = $("#gridPopUp").data("kendoGrid");
            //            var ds = grid.dataSource;
            //            var dataItem = grid.dataItem($(e.target).closest("tr"));
            //            if (confirm($("html").prop("lang") != "en-US" ? "Realmente Desea Eliminar Esta Incidencia?" : "Do You Really Want To Eliminate This Incidence?")) {
            //                AjaxEliminarIncidencia(dataItem.IncidenciaID, dataItem.SpoolID, 'Incidencias');
            //            } else {
            //                e.preventDefault();
            //            }
            //        }
            //    },
            //    title: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
            //    width: "10px",
            //    attributes: { style: "text-align: center; margin: 0 auto" }
            //},
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

                        AbrirVentanaResolucion();
                        //if (confirm($("html").prop("lang") != "en-US" ? "Confirma Resolver Esta Incidencia?" : "Confirm Resolve This Incidence?")) {
                        //    AjaxResolverIncidencia(dataItem.IncidenciaID, dataItem.SpoolID, 'Incidencias');
                        //} else {
                        //    e.preventDefault();
                        //}
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
            top: "0px",
            left: "0px"
        },
        close: function onClose(e) {
            $("#cmbTipoIncidencia").data("kendoComboBox").dataSource.data([]);
            $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data([]);
            $("#cmbErrores").data("kendoComboBox").dataSource.data([]);
        }
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").open();
    window.data("kendoWindow").center();
    window.parent().find(".k-window-action").css("visibility", "hidden");

};

function EventoVerificarSiEsMobil() {
    if (isMobile.any()) {
        MostrarMensaje("Para Visualizar Correctamente El Contenido Por Favor Gira tu Dispositivo Movil (Tiene Que Estar Activa La Rotación)", "seccionAviso");
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


/*Nuevos Requerimientos Fase 1 Proy: 00056 Shop*/
function AbrirVentanaResolucion() {
    $("input[type=radio][value=Otro]").prop("checked", true).trigger("change"); //Por Default tiene que capturar la resolucion de la incidencia
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
            top: "0px",
            left: "0px"
        },
        actions: [
            "Close"
        ],
        close: function onClose(e) {
            $("#txtDetalleResolucion").val("");
        }
    }).data("kendoWindow");
    window.data("kendoWindow").title(modalTitle);
    window.data("kendoWindow").open();
    window.data("kendoWindow").center();
    window.parent().find(".k-window-action").css("visibility", "hidden");
}

/*Grid Spools resueltos*/
function IniciaGridResueltos() {
    $("#gridResueltos").kendoGrid({
        autoBind: false,
        autoSync: false,
        edit: function (e) {
            //if (e.model.Incidencias > 0) {
            //    this.closeCell();
            //    MostrarError($("html").prop("lang") != "en-US" ? "El Spool " + e.model.NumeroControl + "  Tiene  Incidencias" : "The Spool " + e.model.NumeroControl + " Has Incidents");
            //}
        },
        dataSource: {
            data: [],
            schema: {
                model: {
                    fields: {
                        SpoolID: { type: "int", editable: false },
                        ProyectoID: { type: "int", editable: false },
                        CuadranteID: { type: "int", editable: false },
                        Cuadrante: { type: "string", editable: false },
                        NumeroControl: { type: "string", editable: false },
                        SqCliente: { type: "string", editable: false },
                        SI: { type: "string", editable: false },
                        Hold: { type: "boolean", editable: false },
                        Granel: { type: "booelan", editable: false }
                    }
                }
            },
            pageSize: 10,
            serverPaging: false,
            serverFiltering: false,
            serverSorting: false
        },
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
        filterable: filtroGeneral(),
        columns: [
            {
                field: "NumeroControl", title: $("html").prop("lang") != "en-US" ? "Numero de Control" : "Control Number", filterable: filtroTexto(),
                template: function (dataItem) {
                    return !dataItem.Granel ? '<a href="/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=' + dataItem.NumeroControl + '&ProyectoID=' + dataItem.ProyectoID + '" target="_blank" > ' + dataItem.NumeroControl + ' </a>' : "<label>" + dataItem.NumeroControl + "</label>"
                },
                width: "20px"
                //template: '<a href="/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=#=NumeroControl#&ProyectoID=#=ProyectoID#" target="_blank" > #=NumeroControl# </a>', width: "20px"
            },
            { field: "Cuadrante", title: $("html").prop("lang") != "en-US" ? "Cuadrante" : "Quadrant", filterable: filtroTexto(), width: "20px" },
            { field: "Hold", title: "Hold", template: "#=Hold == 1 ? 'Si' : 'No' #", width: "10px", filterable: filtroSI_NO(), attributes: { style: "text-align: center;" } },
            //{ field: "Incidencias", title: $("html").prop("lang") != "en-US" ? "Num. Incidencias" : "Num. Incidents", width: "20px", attributes: { style: "text-align: center;" }, filterable: filtroNumero() },
            {
                command: {
                    text: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
                    className: "k-button k-button-icontext k-grid-Cancelar ",
                    click: function (e) {
                        var grid = $("#gridResueltos").data("kendoGrid");
                        var ds = grid.dataSource;
                        var dataItem = grid.dataItem($(e.target).closest("tr"));
                        if (confirm($("html").prop("lang") != "en-US" ?
                                "El Spool " + dataItem.NumeroControl + " Ya No Estará Disponible En Esta Pantalla, ¿Desea Continuar La Eliminación? " :
                                "The Spool " + dataItem.NumeroControl + " Will Not Be Available On This Screen. ¿Do You Wish To Continue?")) {
                            AjaxEliminarSpoolsDeSession(dataItem.SpoolID);
                            ds.remove(dataItem);
                            ds.sync();
                        } else {
                            e.preventDefault();
                        }
                    }
                },
                title: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
                width: "10px",
                attributes: { style: "text-align: center; margin: 0 auto" }
            },
        ]
    });
}

//var SpoolIDGlobal = 0;
//var timeOut;
//var NumeroControlGlobal = "";
//function ActivarEventos() {    
//    document.title = $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidens";
//    AjaxObtenerSpoolsResueltos();
//    IniciaGrid();
//    CargarGridPopUp();
//    IniciaGridResueltos();
//    ObtenerCacheProyecto();    
//    EventoClickBuscarSpools();
//    NombrarEtiquetas();    
//    EventoSelectProyecto();    
//    EventoChangeGrid();
//    EventoTipoIncidencia();
//    EventoDetalleIncidencia();
//    EventoMostrarListaErrores();
//    EventoFocusOutCombos();
//    CerrarVentanaModal();
//    EventoGuardarIncidencia();
//    CerrarModalResolucion();
//    EventoChangeRadioResolverIncidencia();
//    EventoGuardarResolucion();
//    EventoGenerarSI();
//}

//function NombrarEtiquetas() {
//    $("#lblTipoIncidencia").text($("html").prop("lang") != "en-US" ? "Tipo Incidencia" : "Incident Type");
//    $("#lblDetalleIncidencia").text($("html").prop("lang") != "en-US" ? "Detalle Incidencia" : "Incident Detail");
//    $("#lblErrores").text("Error");
//    $("#lblObservacion").text($("html").prop("lang") != "en-US" ? "Observaciones" : "Observations");    
//    $("#txtObservacion").css("resize", "none");
//    /*Nuevos requerimientos Shop*/
//    $("#txtTituloOpcion1").text($("html").prop("lang") != "en-US" ? "Error de Captura" : "Capture Error");
//    $("#txtTituloOpcion2").text($("html").prop("lang") != "en-US" ? "Otro" : "Other");
//    $("#lblDetalleResolucion").text($("html").prop("lang") != "en-US" ? "Indique Como Se Resolvió La Incidencia" : "Indicate How the Incident Was Resolved");
//    $("#txtDetalleResolucion").css("resize", "none");
//    $("#lblSpoolResuelto").text($("html").prop("lang") != "en-US" ? "Generar Sol. Inspect" : "Generate Inspect. Req");    
//}
//function IniciaGrid() {  
//    $("#grid").kendoGrid({
//        autoBind: false,
//        autoSync: false,
//        edit: function (e) {
//            if (e.model.Incidencias > 0) {
//                this.closeCell();
//                MostrarMensaje($("html").prop("lang") != "en-US" ? "El Spool " + e.model.NumeroControl + "  Tiene  Incidencias" : "The Spool " + e.model.NumeroControl + " Has Incidents", "seccionError");
//            }
//        },
//        dataSource: {
//            data: [],
//            schema: {
//                model: {
//                    fields: {
//                        SpoolID: { type: "int", editable: false },
//                        ProyectoID: {type: "int", editable: false},
//                        CuadranteID: { type: "int", editable: false },
//                        Cuadrante: { type: "string", editable: false },
//                        NumeroControl: { type: "string", editable: false },
//                        Hold: { type: "boolean", editable: false },
//                        Incidencias: { type: "number", editable: false },
//                        Granel: { type: "boolean", editable: false }
//                    }
//                }
//            },
//            pageSize: 10,
//            serverPaging: false,
//            serverFiltering: false,
//            serverSorting: false
//        },       
//        navigatable: true,
//        editable: true,
//        //autoWidth: true,
//        autoHeigth: true,
//        sortable: true,
//        scrollable: false,
//        pageable: {
//            refresh: false,
//            pageSizes: [10, 25, 50, 100],
//            info: false,
//            input: false,
//            numeric: true,
//        },        
//        filterable: filtroGeneral(),
//        columns: [           
//            {
//                field: "NumeroControl", title: $("html").prop("lang") != "en-US" ? "Numero de Control" : "Control Number", filterable: filtroTexto(),
//                template: function (dataItem) {
//                    return !dataItem.Granel ? '<a href="/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=' + dataItem.NumeroControl + '&ProyectoID=' + dataItem.ProyectoID + '" target="_blank" > ' + dataItem.NumeroControl + ' </a>' : "<label>" + dataItem.NumeroControl + "</label>"
//                },
//                width: "20px"
//                //template: '#=!Granel ? <a href="/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=#=NumeroControl#&ProyectoID=#=ProyectoID#" target="_blank" > #=NumeroControl# </a> # : #=NumeroControl #', width: "20px"
//            },
//            { field: "Cuadrante", title: $("html").prop("lang") != "en-US" ? "Cuadrante" : "Quadrant", filterable: filtroTexto(), width: "20px" },
//            { field: "Hold", title: "Hold", template: "#=Hold ? 'Si' : 'No' #", width: "10px", filterable: filtroSI_NO(), attributes: { style: "text-align: center;" }, },
//            { field: "Incidencias", title: $("html").prop("lang") != "en-US" ? "Num. Incidencias" : "Num. Incidents", width: "20px", attributes: { style: "text-align: center;" }, filterable: filtroNumero() },
//            {
//                command: {
//                    text: $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents",
//                    className: "k-button k-button-icontext k-grid-Incidencias ",
//                    click: function (e) {
//                        e.preventDefault();
//                        var grid = $("#grid").data("kendoGrid");
//                        var ds = grid.dataSource;
//                        var dataItem = grid.dataItem($(e.target).closest("tr"));
//                        SpoolIDGlobal = dataItem.SpoolID;
//                        NumeroControlGlobal = dataItem.NumeroControl;
//                         AjaxObtenerTipoIncidencias();
//                        AjaxObtenerIncidencias(dataItem.SpoolID);
//                        VentanaModal();
//                        $("#txtNumeroControl").text("");
//                        AjaxVerificarEsGranel(function (data) {
//                            if (data != "GRANEL") {
//                                $("#txtNumeroControl").append("<a href='/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=" + NumeroControlGlobal + "&ProyectoID=" + $("#ProjectIdADD").val() + "' target='_blank'>" + NumeroControlGlobal + "</a> ");
//                            } else {
//                                $("#txtNumeroControl").append("<label>" + NumeroControlGlobal + "</label>");
//                            }
//                        });                        
//                    }
//                },
//                title: $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents",
//                width: "10px",
//                attributes: { style: "text-align: center; margin: 0 auto" },
//                filterable: false
//            }
//        ]      
//    });    
//}


//function CargarGridPopUp() {
//    $("#gridPopUp").kendoGrid({
//        autoBind: true,
//        dataSource: {
//            data: [],
//            schema: {
//                model: {
//                    fields: {
//                        Accion: { type: "int", editable: false },
//                        IncidenciaID: { type: "int", editable: false },
//                        SpoolID: { type: "int", editable: false },
//                        NumeroControl: { type: "string", editable: false },
//                        Incidencia: { type: "string", editable: false },
//                        MaterialJunta: { type: "string", editable: false },
//                        ErrorID: { type: "int", editable: false },
//                        Error: { type: "string", editable: false },
//                        Observaciones: { type: "string", editable: false },
//                        SI: { type: "string", editable: false }
//                    }
//                }
//            },
//            pageSize: 10,
//            serverPaging: false,
//            serverFiltering: false,
//            serverSorting: false
//        },
//        filterable: false,
//        navigatable: true,
//        editable: true,
//        //autoWidth: true,
//        autoHeigth: true,
//        sortable: true,
//        scrollable: false,
//        pageable: {
//            refresh: false,
//            pageSizes: [10, 25, 50, 100],
//            info: false,
//            input: false,
//            numeric: true,
//        },
//        columns: [
//            { field: "Incidencia", title: $("html").prop("lang") != "en-US" ? "Incidencia" : "Incident", width: "20px" },
//            { field: "MaterialJunta", title: "Material/Junta", width: "20px" },
//            { field: "Error", title: "Error", width: "30px" },
//            { field: "Observaciones", title: $("html").prop("lang") != "en-US" ? "Observaciones" : "Observations", width: "20px" },
//            { field: "SI", title: "SI", width: "20px" },
//            //{
//            //    command: {
//            //        text: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
//            //        className: "k-button k-button-icontext k-grid-Cancelar ",
//            //        click: function (e) {
//            //            var grid = $("#gridPopUp").data("kendoGrid");
//            //            var ds = grid.dataSource;
//            //            var dataItem = grid.dataItem($(e.target).closest("tr"));
//            //            if (confirm($("html").prop("lang") != "en-US" ? "Realmente Desea Eliminar Esta Incidencia?" : "Do You Really Want To Eliminate This Incidence?")) {
//            //                AjaxEliminarIncidencia(dataItem.IncidenciaID, dataItem.SpoolID, 'Incidencias');
//            //            } else {
//            //                e.preventDefault();
//            //            }
//            //        }
//            //    },
//            //    title: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
//            //    width: "10px",
//            //    attributes: { style: "text-align: center; margin: 0 auto" }
//            //},
//            {
//                command: {
//                    text: $("html").prop("lang") != "en-US" ? "Resolver" : "Solve",
//                    className: "k-button k-button-icontext k-grid-Resolver ",
//                    click: function (e) {                        
//                        var grid = $("#gridPopUp").data("kendoGrid");
//                        var ds = grid.dataSource;
//                        var dataItem = grid.dataItem($(e.target).closest("tr"));                                                
//                        $("#txtInfoIncidencia").text(
//                            $("html").prop("lang") != "en-US" ? "Incidencia: " + dataItem.Incidencia + "\n Material/Junta: " + dataItem.MaterialJunta :
//                            "Incidence: " + dataItem.Incidencia + "\n Material/Joint: " + dataItem.MaterialJunta
//                        );
//                        /*Guardo valores para poder identificar que incidencia fue y que spool en la ventana modal*/
//                        $("#TmpSpoolID").val(dataItem.SpoolID);
//                        $("#TmpIncidenciaID").val(dataItem.IncidenciaID);
//                        $("#txtNumeroControlResolucion").text("");
//                        AjaxVerificarEsGranel(function (data) {
//                            if (data != "GRANEL") {                                
//                                $("#txtNumeroControlResolucion").append("<a href='/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=" + NumeroControlGlobal + "&ProyectoID=" + $("#ProjectIdADD").val() + "' target='_blank'>" + NumeroControlGlobal + "</a> ");
//                            } else {
//                                $("#txtNumeroControlResolucion").append("<label>" + NumeroControlGlobal + "</label>");
//                            }
//                        });
                        
//                        AbrirVentanaResolucion();
//                        //if (confirm($("html").prop("lang") != "en-US" ? "Confirma Resolver Esta Incidencia?" : "Confirm Resolve This Incidence?")) {
//                        //    AjaxResolverIncidencia(dataItem.IncidenciaID, dataItem.SpoolID, 'Incidencias');
//                        //} else {
//                        //    e.preventDefault();
//                        //}
//                    }
//                },
//                title: $("html").prop("lang") != "en-US" ? "Resolver" : "Solve",
//                width: "10px",
//                attributes: { style: "text-align: center; margin: 0 auto" }
//            },
//        ]
//    });
//}

//function VentanaModal() {
//    var modalTitle = "";
//    modalTitle = $("html").prop("lang") != "en-US" ? "Incidencias" : "Incidents";
//    var window = $("#windowGrid");
//    var win = window.kendoWindow({
//        modal: true,
//        title: modalTitle,
//        resizable: false,
//        visible: true,
//        width: "95%",
//        minWidth: 30,
//        position: {
//            top: "0px",
//            left: "0px"
//        },        
//        close: function onClose(e) {
//            $("#cmbTipoIncidencia").data("kendoComboBox").dataSource.data([]);
//            $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data([]);
//            $("#cmbErrores").data("kendoComboBox").dataSource.data([]);
//        }
//    }).data("kendoWindow");
//    window.data("kendoWindow").title(modalTitle);
//    window.data("kendoWindow").open();
//    window.data("kendoWindow").center();
//    window.parent().find(".k-window-action").css("visibility", "hidden");

//};

//function EventoVerificarSiEsMobil() {
//    if (isMobile.any()) {
//        MostrarMensaje("Para Visualizar Correctamente El Contenido Por Favor Gira tu Dispositivo Movil (Tiene Que Estar Activa La Rotación)", "seccionAviso");
//    }
//}

//function BorrarCampos() {
//    $("#cmbTipoIncidencia").data("kendoComboBox").dataSource.data([]);
//    $("#cmbTipoIncidencia").data("kendoComboBox").text("");
//    $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data([]);
//    $("#cmbDetalleIncidencia").data("kendoComboBox").text("");
//    $("#cmbErrores").data("kendoComboBox").dataSource.data([]);
//    $("#cmbErrores").data("kendoComboBox").text("");
//    $("#txtObservacion").val("");
//}


///*Nuevos Requerimientos Fase 1 Proy: 00056 Shop*/
//function AbrirVentanaResolucion() {
//    $("input[type=radio][value=Otro]").prop("checked", true).trigger("change"); //Por Default tiene que capturar la resolucion de la incidencia
//    var modalTitle = "";
//    modalTitle = $("html").prop("lang") != "en-US" ? "Resolver Incidencia" : "Resolve Incidence";
//    var window = $("#VentanaResolucion");
//    var win = window.kendoWindow({
//        modal: true,
//        title: modalTitle,
//        resizable: false,
//        visible: true,
//        width: "70%",
//        minWidth: 30,
//        position: {
//            top: "0px",
//            left: "0px"
//        },
//        actions: [
//            "Close"
//        ],
//        close: function onClose(e) {            
//            $("#txtDetalleResolucion").val("");
//        }
//    }).data("kendoWindow");
//    window.data("kendoWindow").title(modalTitle);
//    window.data("kendoWindow").open();
//    window.data("kendoWindow").center();
//    window.parent().find(".k-window-action").css("visibility", "hidden");
//}

///*Grid Spools resueltos*/
//function IniciaGridResueltos() {  
//    $("#gridResueltos").kendoGrid({
//        autoBind: false,
//        autoSync: false,
//        edit: function (e) {
//            //if (e.model.Incidencias > 0) {
//            //    this.closeCell();
//            //    MostrarError($("html").prop("lang") != "en-US" ? "El Spool " + e.model.NumeroControl + "  Tiene  Incidencias" : "The Spool " + e.model.NumeroControl + " Has Incidents");
//            //}
//        },
//        dataSource: {
//            data: [],
//            schema: {
//                model: {
//                    fields: {                                                        
//                        SpoolID: { type: "int", editable: false },
//                        ProyectoID: {type: "int", editable: false},
//                        CuadranteID: { type: "int", editable: false },
//                        Cuadrante: { type: "string", editable: false },
//                        NumeroControl: { type: "string", editable: false },
//                        SqCliente: { type: "string", editable: false },
//                        SI: { type: "string", editable: false },        
//                        Hold: { type: "boolean", editable: false },
//                        Granel: { type: "booelan", editable: false }
//                    }
//                }
//            },
//            pageSize: 10,
//            serverPaging: false,
//            serverFiltering: false,
//            serverSorting: false
//        },       
//        navigatable: true,
//        editable: true,
//        //autoWidth: true,
//        autoHeigth: true,
//        sortable: true,
//        scrollable: false,
//        pageable: {
//            refresh: false,
//            pageSizes: [10, 25, 50, 100],
//            info: false,
//            input: false,
//            numeric: true,
//        },        
//        filterable: filtroGeneral(),
//        columns: [
//            {
//                field: "NumeroControl", title: $("html").prop("lang") != "en-US" ? "Numero de Control" : "Control Number", filterable: filtroTexto(),
//                template: function (dataItem) {
//                    return !dataItem.Granel ? '<a href="/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=' + dataItem.NumeroControl + '&ProyectoID=' + dataItem.ProyectoID + '" target="_blank" > ' + dataItem.NumeroControl + ' </a>' : "<label>" + dataItem.NumeroControl + "</label>"
//                },
//                width: "20px"
//                //template: '<a href="/LinkTraveler/ObtenerPDFTraveler/?NumeroControl=#=NumeroControl#&ProyectoID=#=ProyectoID#" target="_blank" > #=NumeroControl# </a>', width: "20px"
//            },
//            { field: "Cuadrante", title: $("html").prop("lang") != "en-US" ? "Cuadrante" : "Quadrant", filterable: filtroTexto(), width: "20px" },
//            { field: "Hold", title: "Hold", template: "#=Hold == 1 ? 'Si' : 'No' #", width: "10px", filterable: filtroSI_NO(), attributes: { style: "text-align: center;" } },
//            //{ field: "Incidencias", title: $("html").prop("lang") != "en-US" ? "Num. Incidencias" : "Num. Incidents", width: "20px", attributes: { style: "text-align: center;" }, filterable: filtroNumero() },
//            {
//                command: {
//                    text: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
//                    className: "k-button k-button-icontext k-grid-Cancelar ",
//                    click: function (e) {
//                        var grid = $("#gridResueltos").data("kendoGrid");
//                        var ds = grid.dataSource;
//                        var dataItem = grid.dataItem($(e.target).closest("tr"));
//                        if (confirm($("html").prop("lang") != "en-US" ?
//                                "El Spool " + dataItem.NumeroControl + " Ya No Estará Disponible En Esta Pantalla, ¿Desea Continuar La Eliminación? " :
//                                "The Spool " + dataItem.NumeroControl + " Will Not Be Available On This Screen. ¿Do You Wish To Continue?")) {
//                            AjaxEliminarSpoolsDeSession(dataItem.SpoolID);
//                            ds.remove(dataItem);
//                            ds.sync();
//                        } else {
//                            e.preventDefault();
//                        }
//                    }
//                },
//                title: $("html").prop("lang") != "en-US" ? "Eliminar" : "Delete",
//                width: "10px",
//                attributes: { style: "text-align: center; margin: 0 auto" }
//            },
//        ]      
//    });    
//}