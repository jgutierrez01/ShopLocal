function AjaxObtenerTipoIncidencias() {    
    $.ajax({
        type: 'GET',
        url: '/SQ/ObtenerTipoIncidencias/',
        dataType: 'json',
        data: {},
        success: function (data) {
            if (data[0].result != "NODATA") {
                var result = JSON.parse(data[0].result);
                $("#cmbTipoIncidencia").data("kendoComboBox").dataSource.data([]);
                $("#cmbTipoIncidencia").data("kendoComboBox").dataSource.data(result);
                $("#cmbTipoIncidencia").data("kendoComboBox").select(0);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop
            alert("Error Obteniendo Tipos de Incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}


function AjaxObtenerDetalleIncidencias(TipoIncidencia) {    
    $.ajax({
        type: 'GET',
        url: '/SQ/ObtenerDetalleIncidencias/',
        dataType: 'json',
        data: { TipoIncidenciaID: TipoIncidencia, SpoolID: SpoolIDGlobal },
        success: function (data) {
            if (data[0].result != "NODATA") {
                var result = JSON.parse(data[0].result);
                $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data([]);
                $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data(result);
                $("#cmbDetalleIncidencia").data("kendoComboBox").select(0);
                if (result.length == 2) {                                        
                    $("#cmbDetalleIncidencia").data("kendoComboBox").select(1);
                }                
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Tipos de Incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}
function AjaxObtenerListaErrores(TipoIncidencia) {
    $.ajax({
        type: 'GET',
        url: '/SQ/ObtenerListaErrores/',
        dataType: 'json',
        data: { TipoIncidenciaID: TipoIncidencia },
        success: function (data) {
            if (data[0].result != "NODATA") {
                var result = JSON.parse(data[0].result);
                $("#cmbErrores").data("kendoComboBox").dataSource.data([]);
                $("#cmbErrores").data("kendoComboBox").dataSource.data(result);
                $("#cmbErrores").data("kendoComboBox").select(0);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Tipos de Incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}


function AjaxObtenerIncidencias(Spool) {
    $.ajax({
        type: 'GET',
        url: '/SQ/ObtenerIncidencias/',
        dataType: 'json',
        data: { SpoolID: Spool },
        success: function (data) {
            if (data[0].result != "NODATA") {                
                $("#ContenedorGridPopUp").css("display", "none");
                if ($("#gridPopUp").hasClass("k-widget")) {
                    $("#gridPopUp").removeClass("k-widget");
                }
                loadingStop();
                var result = JSON.parse(data[0].result);                
                $("#gridPopUp").data("kendoGrid").dataSource.data([]);
                $("#gridPopUp").data("kendoGrid").dataSource.data(result);
                $("#ContenedorGridPopUp").css("display", "block");
                $(".k-grid-pager").css("width", "100%");                
            } else {
                MostrarErrorGrid($("html").prop("lang") != "en-US" ? "No Hay Incidencias Registradas" : "There are no Registered Incidents");
                $("#ContenedorGridPopUp").css("display", "none");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Tipos de Incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxGuardarIncidencia() {
    var lista =
        {
            SpoolID: SpoolIDGlobal == null ? 0 : SpoolIDGlobal,
            TipoIncidenciaID: parseInt($("#cmbTipoIncidencia").val()),
            //MaterialSpoolID: $("#cmbTipoIncidencia").data("kendoComboBox").text() == "Materiales" ? parseInt($("#cmbDetalleIncidencia").val()) : 0,
            //JuntaSpoolID: $("#cmbTipoIncidencia").data("kendoComboBox").text() == "Juntas" ? parseInt($("#cmbDetalleIncidencia").val()) : 0,
            MaterialSpoolID: $("#cmbDetalleIncidencia").val(),
            JuntaSpoolID: $("#cmbDetalleIncidencia").val(),
            ErrorIncidenciaID: parseInt($("#cmbErrores").val()),
            Observacion: $("#txtObservacion").val(),
            SI: $("#OrigenIncidencia").val() == 1 ? "" : $("#SQ").val()
        };
    var json = JSON.stringify(lista);
    loadingStart();
    $.ajax({
        type: 'GET',
        url: '/SQ/GuardarIncidencia/',
        dataType: 'json',
        data: lista,
        success: function (data) {
            if (data[0].result == "OK") {
                BorrarCampos();
                MostrarAvisoGrid($("html").prop("lang") != "en-US" ? "Guardado Correctamente" : "Saved Successful");
                AjaxObtenerIncidencias(SpoolIDGlobal);
                AjaxObtenerTipoIncidencias();
                AjaxObtenerNumeroIncidencias(SpoolIDGlobal);
                //AjaxObtenerSpools();
                loadingStop();
            } else {
                loadingStop();
                MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Ocurrio Un Error" : "An Error Ocurred");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Incidencias: " + "\n" + xhr.responseText + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxObtenerNumeroIncidencias(Spool) {
    $.ajax({
        type: 'GET',
        url: '/SQ/ObtenerNumeroIncidencias/',
        dataType: 'json',
        data: { SpoolID: Spool },
        success: function (data) {
            if (data[0].result >= 0) {
                CambiarNumeroIncidencia(NumeroControlGlobal, data[0].result);
            } else {
                loadingStop();
                MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Ocurrio Un Error" : "An Error Ocurred");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Incidencias: " + "\n" + xhr.responseText + "\n" + textStatus + "\n" + errorThrown);
        }
    });

}

function CambiarNumeroIncidencia(NumeroControl, NumeroIncidencia) {
    var column = 1; //Columna donde se encuentra el NumeroControl
    var table;
    var colIncidencia;
    var origen = $("#OrigenIncidencia").val();
    if (origen == 1) { //Nuevo       
        table = $('#gridAdd table'); 
        colIncidencia = $('#gridAdd tr th').filter(function () { return $(this).text() == 'Num. Incidencias'; }).index(); //Columna donde se modificará el numero de incidencia   
    } else {//Editar
        table = $('#gridEditar table'); 
        colIncidencia = $('#gridEditar tr th').filter(function () { return $(this).text() == 'Num. Incidencias'; }).index(); //Columna donde se modificará el numero de incidencia   
    }         
    var tr = $(table).find('tr'); //Filas       
    for (var i = 0; i < tr.length; i++) {
        var td = $(tr[i]).find("td"); //Columnas        
        for (var j = 0; j < td.length; j++) {            
            if (j == column && td[j].innerHTML.indexOf(NumeroControl) > -1) {
                td[colIncidencia].innerHTML = NumeroIncidencia;                                
            }
        }
    }
}
function VerificarIncidencias(Origen, NumeroControl) {    
    var column = 1; //Columna donde se encuentra el NumeroControl
    var table;
    var colIncidencia;
    //var origen = $("#OrigenIncidencia").val();    
    if (Origen == 1) { //Nuevo       
        table = $('#gridAdd table');
        colIncidencia = $('#gridAdd tr th').filter(function () { return $(this).text() == 'Num. Incidencias'; }).index(); //Columna donde se modificará el numero de incidencia   
    } else {//Editar
        table = $('#gridEditar table');
        colIncidencia = $('#gridEditar tr th').filter(function () { return $(this).text() == 'Num. Incidencias'; }).index(); //Columna donde se modificará el numero de incidencia   
    }
    var tr = $(table).find('tr'); //Filas       
    for (var i = 0; i < tr.length; i++) {
        var td = $(tr[i]).find("td"); //Columnas        
        for (var j = 0; j < td.length; j++) {
            if (j == column && td[j].innerHTML.indexOf(NumeroControl) > -1) {
                return td[colIncidencia].innerHTML;
            }
        }
    }    
}

function AjaxEliminarIncidencia(incidenciaID, SpoolID, pantalla) {
    $.ajax({
        type: 'GET',
        url: '/SQ/ResolverEliminarIncidencia/',
        dataType: 'json',
        data: { IncidenciaID: incidenciaID, Origen: pantalla, Accion: 1 },
        success: function (data) {
            if (data[0].result == "OK") {
                MostrarAvisoGrid($("html").prop("lang") != "en-US" ? "Incidencia Eliminada Correctamente" : "Successfully Eliminated Incidence");
                AjaxObtenerIncidencias(SpoolID);                
                AjaxObtenerNumeroIncidencias(SpoolID);
            } else {
                MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Error Al Eliminar Incidencia" : "Error Deleting Incident");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Tipos de Incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}


function AjaxResolverIncidencia(incidenciaID, SpoolID, pantalla) {
    $.ajax({
        type: 'GET',
        url: '/SQ/ResolverEliminarIncidencia/',
        dataType: 'json',
        data: { IncidenciaID: incidenciaID, Origen: pantalla, Accion: 2  },
        success: function (data) {
            if (data[0].result == "OK") {
                MostrarAvisoGrid($("html").prop("lang") != "en-US" ? "Incidencia Resuelta Correctamente" : "Incident Resolved Correctly");
                AjaxObtenerIncidencias(SpoolID);                
                AjaxObtenerNumeroIncidencias(SpoolID);                
            } else {
                MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Error Al Eliminar Incidencia" : "Error Deleting Incident");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Tipos de Incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxInactivarSpoolSI(numeroControl) {
    loadingStart();
    $.ajax({
        type: 'GET',
        url: '/SQ/InactivarSpoolSI/',
        dataType: 'json',
        data: { NumeroControl: numeroControl, ProyectoID: ($("#ProjectIdADD").val() != undefined && $("#ProjectIdADD").val() != 0) ? $("#ProjectIdADD").val() : 0 },
        success: function (data) {
            if (data[0].result == "OK") {                
                EliminarSpoolDeTabla(numeroControl);
                MostrarErrorEdit($("html").prop("lang") != "en-US" ? "El Spool: " + numeroControl + " Fue Eliminado De La Sol. Inspect: " + $("#SQ").val() + " Porque Tiene Incidencias" : "The Spool: " + numeroControl + " Was Eliminated OF The Sol. Inspect : " + $("#SQ").val() + " Because Has Incidents");
            } else {
                MostrarErrorEdit($("html").prop("lang") != "en-US" ? "Error Al Inactivar SI" : "Error To Inactivate SI");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Inactivando Spool De SI: " + "\n" + xhr.responseText + "\n" + textStatus + "\n" + errorThrown);
        }
    });
    loadingStop();
}


/*Nuevos Requerimientos Fase 1 Proy: 00056 Shop*/
function AjaxResolucionIncidencias(spoolID, incidenciaID, origen, resolucion, accion, esModal) {
    $.ajax({
        type: 'GET',
        url: '/SQ/ResolucionIncidencias/',
        dataType: 'json',
        data: { SpoolID: spoolID, IncidenciaID: incidenciaID, Resolucion: resolucion, Origen: origen, Accion: accion },
        success: function (data) {
            if (data[0].result == "OK") {
                AjaxObtenerIncidencias(spoolID);
                AjaxObtenerNumeroIncidencias(spoolID);                
                MostrarAvisoGrid($("html").prop("lang") != "en-US" ? "Incidencia Resuelta Correctamente" : "Incident Resolved Correctly");
                $("#CerrarResolucion").trigger("click");
            } else {
                MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Error Al Resolver Incidencia" : "Error Resolving Incident");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Resolviendo incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxVerificarEsGranel(getGranel) {
    $.ajax({
        async: false,
        type: 'GET',
        url: '/LinkTraveler/ValidaGranel/',
        dataType: 'json',
        data: { NumeroControl: NumeroControlGlobal, ProyectoID: $("#ProjectIdADD").val() },
        success: function (data) {
            getGranel(data[0].result);
        }
    });
}