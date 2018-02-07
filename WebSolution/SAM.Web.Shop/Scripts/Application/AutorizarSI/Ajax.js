var SIGlobal = "";
function ActualizarCacheProyecto(ProyectoID) {
    $.ajax({
        type: 'GET',
        url: '/AutorizarSI/ActualizarCacheProyecto/',
        dataType: 'json',
        data: { ProyectoID: $("#ProjectIdADD").val() },
        success: function (data) {
            if (data[0].result != "OK") {
                console.log("Error al actualizar cache de proyecto");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Información: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}
function ObtenerCacheProyecto() {
    $.ajax({
        type: 'GET',
        url: '/AutorizarSI/ObtenerProyecto/',
        dataType: 'json',
        data: {},
        success: function (data) {
            if (data[0].result != 0) {
                $("#ProjectIdADD").val(data[0].result);
            } 
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Información: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxObtenerSpools() {
    loadingStart();
    $.ajax({
        type: 'GET',
        url: '/AutorizarSI/ObtenerSpools/',
        dataType: 'json',
        data: { SI: $("#txtSI").val(), ProyectoID: $("#ProjectIdADD").val() },
        success: function (data) {
            if (data[0].result == "NODATA") {
                loadingStop();               
                MostrarError($("html").prop("lang") == "en-US" ? "No Information Was Found With Sol. Inspect: " + $("#txtSI").val() : "No Se Encontró Información Con La Sol. Inspect: " + $("#txtSI").val());
                $("#grid").data("kendoGrid").dataSource.data([]);                
                $("#contieneGrid").css("display", "none");
            } else {
                if ($("#grid").hasClass("k-widget")) {
                    $("#grid").removeClass("k-widget");
                }
                loadingStop();
                SIGlobal = $("#txtSI").val();
                var result = JSON.parse(data[0].result);
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#grid").data("kendoGrid").dataSource.data(result);
                $("#contieneGrid").css("display", "block");
                $(".k-grid-pager").css("width", "100%");                
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Información: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxObtenerTipoIncidencias() {    
    $.ajax({
        type: 'GET',
        url: '/AutorizarSI/ObtenerTipoIncidencias/',
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
        url: '/AutorizarSI/ObtenerDetalleIncidencias/',
        dataType: 'json',
        data: { TipoIncidenciaID: TipoIncidencia, SpoolID: SpoolIDGlobal },
        success: function (data) {
            if (data[0].result != "NODATA") {
                var result = JSON.parse(data[0].result);
                $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data([]);
                $("#cmbDetalleIncidencia").data("kendoComboBox").dataSource.data(result);
                if (result.length == 2) {
                    $("#cmbDetalleIncidencia").data("kendoComboBox").select(1);
                } else {
                    $("#cmbDetalleIncidencia").data("kendoComboBox").select(0);
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
        url: '/AutorizarSI/ObtenerListaErrores/',
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
        url: '/AutorizarSI/ObtenerIncidencias/',
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
function AjaxGuardarIncidencia(ds) {
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
            SI: SIGlobal
        };
    var json = JSON.stringify(lista);
    loadingStart();
    $.ajax({
        type: 'GET',        
        url: '/AutorizarSI/GuardarIncidencia/',
        dataType: 'json',
        data: lista,
        success: function (data) {
            if (data[0].result == "OK") {                
                BorrarCampos();
                MostrarAvisoGrid($("html").prop("lang") != "en-US" ? "Guardado Exitoso" : "Saved Successful");                
                AjaxObtenerIncidencias(SpoolIDGlobal);
                AjaxObtenerTipoIncidencias();
                AjaxObtenerSpools();
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

function AjaxGuardarAutorizacion(ds) {    
    var ListaDetalleCaptura = [];
    for (var x = 0; x < ds.length; x++) {
        ListaDetalleCaptura[x] = { SpoolID: 0, Autorizacion: false };
        ListaDetalleCaptura[x].SpoolID = ds[x].SpoolID;
        ListaDetalleCaptura[x].Autorizacion = ds[x].Autorizado;
    }    
    loadingStart();
    $.ajax({
        type: 'GET',
        url: '/AutorizarSI/GuardarAutorizacion/',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: { Captura: JSON.stringify(ListaDetalleCaptura) },        
        success: function (data) {
            if (data[0].result == "OK") {                
                MostrarSuccess($("html").prop("lang") != "en-US" ? "Guardado Correctamente" : "Saved Successfully");
                AjaxObtenerSpools();
                loadingStop();
            } else {
                loadingStop();                
                MostrarError($("html").prop("lang") != "en-US" ? "Ocurrio Un Error" : "An Error Ocurred");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Incidencias: " + "\n" + xhr.responseText + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxEliminarIncidencia(incidenciaID, SpoolID, pantalla) {
    $.ajax({
        type: 'GET',
        url: '/SQ/ResolverEliminarIncidencia/',
        dataType: 'json',
        data: { IncidenciaID: incidenciaID, Origen: pantalla, Accion: 1 },
        success: function (data) {
            if (data[0].result == "OK") {
                MostrarAvisoGrid($("html").prop("lang") != "en-US" ? "Incidencia Eliminada Correctamente" : "Incident Deleted Correctly");
                AjaxObtenerIncidencias(SpoolID);
                AjaxObtenerSpools();
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
        data: { IncidenciaID: incidenciaID, Origen: pantalla, Accion: 2 },
        success: function (data) {
            if (data[0].result == "OK") {
                MostrarAvisoGrid($("html").prop("lang") != "en-US" ? "Incidencia Resuelta Correctamente" : "Incident Resolved Correctly");
                AjaxObtenerIncidencias(SpoolID);
                AjaxObtenerSpools();
            } else {
                MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Error Al Resolver Incidencia" : "Error Resolving Incident");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Tipos de Incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
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
                AjaxObtenerSpools(($("#ProjectIdADD").val() == undefined || $("#ProjectIdADD").val() == "" ? 0 : $("#ProjectIdADD").val()), ($("#QuadrantIdCADD").val() == undefined || $("#QuadrantIdCADD").val() == "" ? 0 : $("#QuadrantIdCADD").val()));
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