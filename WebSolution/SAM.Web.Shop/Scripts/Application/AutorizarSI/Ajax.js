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
            } else {
                if ($("#grid").hasClass("k-widget")) {
                    $("#grid").removeClass("k-widget");
                }
                loadingStop();
                var result = JSON.parse(data[0].result);
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#grid").data("kendoGrid").dataSource.data(result);
                $("#contieneGrid").css("display", "block");
                $(".k-grid-pager").css("width", "100%");
                //$("#grid").data("kendoGrid").dataSource.sync();
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
                $("#cmbDetalleIncidencia").data("kendoComboBox").select(0);
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
                BorrarSeccionErrorGrid();
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
function AjaxGuardarIncidencia(Spool) {    
    var data = { SpoolID: Spool, TipoIncidenciaID: $("#cmbTipoIncidencia").val(), DetalleIncidenciaID: $("#cmbDetalleIncidencia").val(), ErrorID: $("#cmbErrores").val(), Observaciones: $("#txtObservacion").val() };
    var json = JSON.stringify(data);
    loadingStart();
    $.ajax({
        type: 'GET',
        url: '/AutorizarSI/GuardarIncidencia/',
        dataType: 'json',
        data: json,
        success: function (data) {
            if (data[0].result == "OK") {
                BorrarSeccionErrorGrid();
                BorrarAvisoGrid();
                MostrarAvisoGrid($("html").prop("lang") != "en-US" ? "Guardado Existoso" : "Saved Successful");
                $("#ContenedorGridPopUp").css("display", "none");                
                loadingStop();
            } else {
                loadingStop();
                BorrarAvisoGrid();
                MostrarErrorGrid($("html").prop("lang") != "en-US" ? "Ocurrio Un Error" : "An Error Ocurred");                
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}