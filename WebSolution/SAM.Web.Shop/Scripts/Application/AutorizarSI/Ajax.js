﻿function ActualizarCacheProyecto(ProyectoID) {
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
            } else {
                console.log("no hay cache de proyecto");
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
                if ($("html").prop("lang") == "en-US") {
                    MostrarError("No Information Was Found With Sol. Inspect: " + $("#txtSI").val());
                } else {
                    MostrarError("No Se Encontró Información Con La Sol. Inspect: " + $("#txtSI").val());
                }
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