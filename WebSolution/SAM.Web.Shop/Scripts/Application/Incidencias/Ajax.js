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

function AjaxObtenerSpoolPorCuadrante(proyecto, cuadranteID) {
    loadingStart();
    $.ajax({
        type: 'GET',
        url: '/Incidencias/ObtenerSpoolsPorCuadrante/',
        dataType: 'json',
        data: { ProyectoID: proyecto, CuadranteID: cuadranteID },
        success: function (data) {
            if (data[0].result == "NODATA") {
                loadingStop();
                if (proyecto != 0 && cuadranteID != 0) {
                    MostrarMensaje($("html").prop("lang") == "en-US" ? "No Information Was Found With this Project And Quadrant " : "No Se Encontró Información Con Este Proyecto y Cuadrante ", "seccionError");
                } else if (proyecto != 0 && cuadranteID == 0) {
                    MostrarMensaje($("html").prop("lang") == "en-US" ? "No Information Was Found With this Project " : "No Se Encontró Información Con Este Proyecto ", "seccionError");
                } else {
                    MostrarMensaje($("html").prop("lang") == "en-US" ? "No Information Was Found With this Quadrant " : "No Se Encontró Información Con Este Cuadrante ", "seccionError");
                }
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#contieneGrid").css("display", "none");
            } else if (data[0].result.indexOf("ERROR:") !== -1) {
                MostrarMensaje(data[0].result, "seccionError");
                loadingStop();
                ("#grid").data("kendoGrid").dataSource.data([]);
                $("#contieneGrid").css("display", "none");
            } else {
                if ($("#grid").hasClass("k-widget")) {
                    $("#grid").removeClass("k-widget");
                }                
                var result = JSON.parse(data[0].result);

                /*AGREGO SOLO LOS QUE NO TIENEN HOLD*/
                var listaSpool = [];               
                for (var i = 0; i < result.length; i++) {
                    if (result[i].Hold)
                        listaSpool.push({ NumeroControl: result[i].NumeroControl });
                }
                var newData = result.filter(a => !a.Hold);
                $("#grid").data("kendoGrid").dataSource.data([]);
                //$("#grid").data("kendoGrid").dataSource.data(result);
                $("#grid").data("kendoGrid").dataSource.data(newData);
                $("#contieneGrid").css("display", "block");
                $(".k-grid-pager").css("width", "100%");
                $("#WorkOrderNumberADD").val("");
                $("#ControlNumberADD").val("");

                if (listaSpool.length > 0) {
                    var txt = "";
                    for (var i = 0; i < listaSpool.length; i++) {
                        txt += listaSpool[i].NumeroControl + " <br />";
                    }
                    MostrarMensajeConTiempo(txt + " No se agregan porque tienen Hold", "seccionError", 3500);
                }

                loadingStop();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Información: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxObtenerSpoolPorNumeroControl(proyecto, ot, consecutivo, spoolid) {
    loadingStart();
    if (spoolid == null || spoolid == undefined)
        spoolid = 0;
    $.ajax({
        type: 'GET',
        url: '/Incidencias/ObtenerSpoolsPorNumeroControl/',
        dataType: 'json',
        data: { ProyectoID: proyecto, OT: ot, Consecutivo: consecutivo, SpoolID: spoolid },        
        success: function (data) {
            if (data[0].result == "NODATA") {
                loadingStop();                
                MostrarMensaje($("html").prop("lang") == "en-US" ? "Spool not found " : "No se encontró Spool ", "seccionError");                                
            } else if (data[0].result.indexOf("ERROR:") !== -1) {                
                MostrarMensaje(data[0].result, "seccionError");
                loadingStop();
                ("#grid").data("kendoGrid").dataSource.data([]);
                $("#contieneGrid").css("display", "none");
            } else {
                if ($("#grid").hasClass("k-widget")) {
                    $("#grid").removeClass("k-widget");
                }
                var grid = $("#grid").data("kendoGrid");
                var result = JSON.parse(data[0].result);
                if (result[0] != undefined && result[0] != null && result[0].Hold) {
                    MostrarMensajeConTiempo("El Spool: " + result[0].NumeroControl + " tienen Hold", "seccionError", 3500);
                } else {
                    if (!SpoolRepetido(result[0].NumeroControl)) {
                        grid.dataSource.insert(0, result[0]);
                    } else {
                        if (spoolid > 0) {
                            var data = $("#grid").data("kendoGrid").dataSource._data;
                            for (var i = 0; i < data.length; i++) {
                                if (data[i].SpoolID == spoolid) {
                                    $("#grid").data("kendoGrid").dataSource.remove(data[i]);
                                }
                            }
                            grid.dataSource.insert(0, result[0]);
                            grid.dataSource.sync();
                        } else {
                            MostrarMensaje($("html").prop("lang") == "en-US" ? "Spool already exists " : "El Spool ya se encuentra en la captura ", "seccionError");
                        }
                    }
                    grid.dataSource.sync();
                    $("#contieneGrid").css("display", "block");
                    $(".k-grid-pager").css("width", "100%");
                    $("#WorkOrderNumberADD").val("");
                    $("#WorkOrderNumberADD").focus();
                    $("#ControlNumberADD").val("");
                }                
                loadingStop();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Información: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function SpoolRepetido(numeroControl) {
    var data = $("#grid").data("kendoGrid").dataSource._data;
    for (var i = 0; i < data.length; i++) {
        if (data[i].NumeroControl.toString().toLowerCase() == numeroControl.toString().toLowerCase())
            return true;
    }
    return false;
}


function AjaxObtenerSpools(proyecto) {
    loadingStart();    
    $.ajax({
        type: 'GET',
        url: '/Incidencias/ObtenerSpools/',
        dataType: 'json',
        data: { ProyectoID: proyecto },
        success: function (data) {
            if (data[0].result == "NODATA") {
                loadingStop();
                //if(proyecto != 0 && cuadranteID != 0){
                //    MostrarMensaje($("html").prop("lang") == "en-US" ? "No Information Was Found With this Project And Quadrant " : "No Se Encontró Información Con Este Proyecto y Cuadrante ", "seccionError");
                //} else if(proyecto != 0 && cuadranteID == 0) {
                //    MostrarMensaje($("html").prop("lang") == "en-US" ? "No Information Was Found With this Project " : "No Se Encontró Información Con Este Proyecto ", "seccionError");
                //} else {
                //    MostrarMensaje($("html").prop("lang") == "en-US" ? "No Information Was Found With this Quadrant " : "No Se Encontró Información Con Este Cuadrante ", "seccionError");
                //}                
                MostrarMensaje($("html").prop("lang") == "en-US" ? "No Information Was Found With this Project " : "No Se Encontró Información Con Este Proyecto ", "seccionError");
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#contieneGrid").css("display", "none");
            } else {
                if ($("#grid").hasClass("k-widget")) {
                    $("#grid").removeClass("k-widget");
                }                
                var result = JSON.parse(data[0].result);
                $("#grid").data("kendoGrid").dataSource.data([]);
                $("#grid").data("kendoGrid").dataSource.data(result);
                $("#contieneGrid").css("display", "block");
                $(".k-grid-pager").css("width", "100%");
                loadingStop();
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
            $("#gridPopUp").data("kendoGrid").dataSource.data([]);
            if (data[0].result != "NODATA") {
                $("#ContenedorGridPopUp").css("display", "none");
                if ($("#gridPopUp").hasClass("k-widget")) {
                    $("#gridPopUp").removeClass("k-widget");
                }                
                var result = JSON.parse(data[0].result);                
                $("#gridPopUp").data("kendoGrid").dataSource.data(result);
                $("#ContenedorGridPopUp").css("display", "block");
                $(".k-grid-pager").css("width", "100%");
                loadingStop();
            } else {
                MostrarMensaje($("html").prop("lang") != "en-US" ? "No Hay Incidencias Registradas" : "There are no Registered Incidents", "seccionErrorGrid");
                //$("#ContenedorGridPopUp").css("display", "none");
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
            SI: ""
        };
    var json = JSON.stringify(lista);
    loadingStart();
    $.ajax({
        type: 'GET',
        url: '/Incidencias/GuardarIncidencia/',
        dataType: 'json',
        data: lista,
        success: function (data) {
            if (data[0].result == "OK") {
                BorrarCampos();
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Guardado Exitoso" : "Saved Successful", "seccionAvisoGrid");
                AjaxObtenerIncidencias(SpoolIDGlobal);
                AjaxObtenerTipoIncidencias();
                var proyecto = $("#ProjectIdADD").val();
                if (proyecto != null && proyecto != undefined && proyecto != "" && proyecto > 0) {
                    AjaxObtenerSpoolPorNumeroControl(proyecto, "", "", SpoolIDGlobal);
                }                
                //AjaxObtenerSpools(($("#ProjectIdADD").val() == undefined || $("#ProjectIdADD").val() == "" ? 0 : $("#ProjectIdADD").val()), ($("#QuadrantIdCADD").val() == undefined || $("#QuadrantIdCADD").val() == "" ? 0 : $("#QuadrantIdCADD").val()));                
                loadingStop();
            } else {
                loadingStop();
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Ocurrio Un Error" : "An Error Ocurred", "seccionErrorGrid");
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
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Guardado Correctamente" : "Saved Successfully", "seccionSuccess");
                AjaxObtenerSpools(($("#ProjectIdADD").val() == undefined || $("#ProjectIdADD").val() == "" ? 0 : $("#ProjectIdADD").val()), ($("#QuadrantIdCADD").val() == undefined || $("#QuadrantIdCADD").val() == "" ? 0 : $("#QuadrantIdCADD").val()));
                loadingStop();
            } else {
                loadingStop();
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Ocurrio Un Error" : "An Error Ocurred", "seccionError");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Incidencias: " + "\n" + xhr.responseText + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxEliminarIncidencia(incidenciaID, SpoolID, pantalla, esModal) {
    $.ajax({
        type: 'GET',
        url: '/SQ/ResolverEliminarIncidencia/',
        dataType: 'json',
        data: { IncidenciaID: incidenciaID, Origen: pantalla, Accion: 1 },
        success: function (data) {
            if (data[0].result == "OK") {
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Incidencia Eliminada Correctamente" : "Incident Deleted Correctly", "seccionAvisoGrid");
                AjaxObtenerIncidencias(SpoolID);
                AjaxObtenerSpools(($("#ProjectIdADD").val() == undefined || $("#ProjectIdADD").val() == "" ? 0 : $("#ProjectIdADD").val()), ($("#QuadrantIdCADD").val() == undefined || $("#QuadrantIdCADD").val() == "" ? 0 : $("#QuadrantIdCADD").val()));
                if (esModal) {
                    $("#CerrarResolucion").trigger("click");
                }
            } else {
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Error Al Eliminar Incidencia" : "Error Deleting Incident", "seccionErrorGrid");
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
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Incidencia Resuelta Correctamente" : "Incident Resolved Correctly", "seccionAvisoGrid");
                AjaxObtenerIncidencias(SpoolID);
                AjaxObtenerSpools(($("#ProjectIdADD").val() == undefined || $("#ProjectIdADD").val() == "" ? 0 : $("#ProjectIdADD").val()), ($("#QuadrantIdCADD").val() == undefined || $("#QuadrantIdCADD").val() == "" ? 0 : $("#QuadrantIdCADD").val()));
            } else {
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Error Al Resolver Incidencia" : "Error Resolving Incident", "seccionErrorGrid");
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
                var proyecto = $("#ProjectIdADD").val();
                if (proyecto != null && proyecto != undefined && proyecto != "" && proyecto > 0) {
                    AjaxObtenerSpoolPorNumeroControl(proyecto, "", "", spoolID);
                }
                //AjaxObtenerSpools(($("#ProjectIdADD").val() == undefined || $("#ProjectIdADD").val() == "" ? 0 : $("#ProjectIdADD").val()), ($("#QuadrantIdCADD").val() == undefined || $("#QuadrantIdCADD").val() == "" ? 0 : $("#QuadrantIdCADD").val()));
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Incidencia Resuelta Correctamente" : "Incident Resolved Correctly", "seccionAvisoGrid");
                $("#CerrarResolucion").trigger("click");
            } else {
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Error Al Resolver Incidencia" : "Error Resolving Incident", "seccionErrorGrid");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Resolviendo incidencias: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

//Funcion que retorna valor mediante otra function
function AjaxObtenerNumeroIncidencias(getNumIncidencia) {    
    $.ajax({
        async: false,
        type: 'GET',
        url: '/SQ/ObtenerNumeroIncidencias/',
        dataType: 'json',
        data: { SpoolID: SpoolIDGlobal },
        success: function (data) {            
            getNumIncidencia(data[0].result);
        }
    });   
}


function AjaxObtenerSpoolAgregarSI(Spool) {
    loadingStart();
    $.ajax({
        type: 'GET',
        url: '/Incidencias/ObtenerSpoolAgregarSI/',
        dataType: 'json',
        data: { SpoolID: Spool },
        success: function (data) {
            if (data[0].result != "NODATA") {                
                if ($("#gridResueltos").hasClass("k-widget")) {
                    $("#gridResueltos").removeClass("k-widget");
                }
                loadingStop();
                var result = JSON.parse(data[0].result);
                var grid = $("#gridResueltos").data("kendoGrid");
                var ds = grid.dataSource;
                ds.add(result[0]);                
                //$("#gridResueltos").data("kendoGrid").dataSource.add(result);
                $("#contenedorSpoolsResueltos").css("display", "block");
                $(".k-grid-pager").css("width", "100%");
            } else {
                MostrarMensaje($("html").prop("lang") != "en-US" ? "No Se Encontró Información" : "Not Data Found", "seccionError");
                if ($("#gridResueltos").data("kendoGrid").dataSource.data().length == 0) {
                    $("#contenedorSpoolsResueltos").css("display", "none");
                }
                loadingStop();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Informacion de Spool: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxObtenerSpoolsResueltos() {
    $.ajax({
        type: 'GET',
        url: '/Incidencias/ObtenerSpoolsResueltos/',
        dataType: 'json',
        data: {},
        success: function (data) {
            if (data[0].result != "NODATA") {                
                if ($("#gridResueltos").hasClass("k-widget")) {
                    $("#gridResueltos").removeClass("k-widget");
                }
                loadingStop();
                var result = JSON.parse(data[0].result);
                $("#gridResueltos").data("kendoGrid").dataSource.data([]);
                $("#gridResueltos").data("kendoGrid").dataSource.data(result);
                if ($("#gridResueltos").data("kendoGrid").dataSource.data().length > 0) {
                    $("#contenedorSpoolsResueltos").css("display", "block");
                    $(".k-grid-pager").css("width", "100%");
                } else {
                    $("#contenedorSpoolsResueltos").css("display", "none");
                }
                
                
            } else {
                $("#contenedorSpoolsResueltos").css("display", "none");
                $("#gridResueltos").data("kendoGrid").dataSource.data([]);                
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Obteniendo Spools Resueltos: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}
function AjaxEliminarSpoolsDeSession(Spool) {
    loadingStart();
    $.ajax({
        type: 'GET',
        url: '/Incidencias/EliminarSpoolDeSession/',
        dataType: 'json',
        data: {SpoolID: Spool },
        success: function (data) {
            if (data[0].result == "OK") {
                loadingStop();
                MostrarMensaje($("html").prop("lang") != "en-US" ? "Spool Eliminado Correctamente" : "Spoold Deleted Successfully", "seccionSuccess");
            } 
        },
        error: function (xhr, textStatus, errorThrown) {
            loadingStop();
            alert("Error Eliminando Spools de Session: " + "\n" + xhr + "\n" + textStatus + "\n" + errorThrown);
        }
    });
}

function AjaxGenerarSI(ds) {   
    var data = ds.data();
    if ($("#gridResueltos").data("kendoGrid").dataSource.data().length > 0) {
        var msgSqcliente = "", msgSI = "", msgHold = "";
        for (var i = 0; i < data.length; i++) {
            if (data[i].SqCliente != "") {
                msgSqcliente += $("html").prop("lang") != "en-US" ? "El Spool " + data[i].NumeroControl + " Tiene SI Agregado" : "The Spool " + data[i].NumeroControl + " Has IR Added \n";
            }
            if (data[i].SI != "") {
                msgSI += $("html").prop("lang") != "en-US" ? "El Spool " + data[i].NumeroControl + " Se Encuentra En La SI: " + data[i].SI : "The Spool " + data[i].NumeroControl + " Is In IR:  " + data[i].SI + " \n";
            }
            if (data[i].Hold) {
                msgHold += $("html").prop("lang") != "en-US" ? "El Spool " + data[i].NumeroControl + " Tiene Hold." : "The Spool " + data[i].NumeroControl + " Has Hold \n";
            }
        }    

        var ListaDetalleCaptura = [];
        for (var x = 0; x < data.length; x++) {
            ListaDetalleCaptura[x] = { SpoolID: 0, ProyectoID: 0, CuadranteID: 0 };
            if (data[x].SqCliente == "" ) {
                if (data[x].SI == "" ) {
                    if (!data[x].Hold) {
                        ListaDetalleCaptura[x].SpoolID = data[x].SpoolID;
                        ListaDetalleCaptura[x].ProyectoID = data[x].ProyectoID;
                        ListaDetalleCaptura[x].CuadranteID = data[x].CuadranteID;
                    }
                }
            }        
        }
        if (msgSqcliente != "" || msgSI != "" || msgHold != "") {
            MostrarMensaje(msgSqcliente + "\n" + msgSI + "\n" + msgHold + "\n", "msgError");
        }
        if (ListaDetalleCaptura != null) {
            loadingStart();
            $.ajax({
                type: 'GET',
                url: '/Incidencias/GenerarSI/',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: { Data: JSON.stringify(ListaDetalleCaptura) },
                success: function (data) {
                    if (data[0].result != "") {
                        //Correcto
                        loadingStop();
                        MostrarMensajeConTiempo(data[0].result, "msgExito", 10000);
                        $("#gridResueltos").data("kendoGrid").dataSource.data([]);
                        $("#contenedorSpoolsResueltos").css("display", "none");
                    } else {
                        //Error
                        loadingStop();
                        MostrarMensaje($("html").prop("lang") != "en-US" ? "Ocurrio Un Error: " + data[0].result : "An Error Ocurred: " + data[0].result, "msgError");
                    }                
                },
                error: function (xhr, textStatus, errorThrown) {
                    loadingStop();
                    MostrarMensaje("Ocurrió un Error: ");
                }
            });
        } 
    } else {
        MostrarMensaje($("html").prop("lang") != "en-US" ? "No Hay Spools Para Generar SI" : "Not Spools To Generate IR" , "msgError");
    }    
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