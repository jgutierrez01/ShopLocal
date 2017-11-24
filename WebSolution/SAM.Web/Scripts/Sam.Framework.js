//namespace global
if (!Sam) {
    var Sam = {};
}

//namespaces por area
Sam.Utilerias = {};
Sam.Materiales = {};
Sam.Ingenieria = {};
Sam.Catalogos = {};
Sam.Proyectos = {};
Sam.Produccion = {};
Sam.Administracion = {};
Sam.Workstatus = {};
Sam.Usuarios = {};
Sam.Calidad = {};
Sam.WebService = {};
Sam.Filtro = {};
Sam.Destajo = {};
Sam.Reportes = {};
Sam.Seguimientos = {};
Sam.Programa = {};

//////////////////////////////////////////////
//
//           PROYECTOS
//
/////////////////////////////////////////////

Sam.Proyectos.Validaciones = {};

Sam.Proyectos.Validaciones.AlMenosUnEquivalente = function (sender, args) {
    args.IsValid = false;
    var idGrid = $('#hdnGrdClientID').val();
    var grid = $find(idGrid);
    var elementos = grid.get_masterTableView().get_dataItems();

    if (elementos && elementos !== null && elementos.length > 0) {
        args.IsValid = true;
    }
}

Sam.Proyectos.Validaciones.ItemCodePadreSeleccionado = function (sender, args) {
    //Truco para disparar las validaciones de otro grupo bajo una condición muy curiosa
    var validadores = Page_Validators;
    $.each(validadores, function (i, validador) {
        if (validador.validationGroup === "valGroupGuardar" && validador.clientvalidationfunction !== "Sam.Proyectos.Validaciones.AlMenosUnEquivalente") {
            ValidatorValidate(validador);
        }
    });
    args.IsValid = true;
}

//////////////////////////////////////////////
//
//           ADMINISTRACION
//
/////////////////////////////////////////////


Sam.Administracion.ValidaArchivoCSV = function (sender, args) {
    args.IsValid = false;

    var upload = $find($("[id*=rdArchivo]").attr("ID"));
    var inputs = upload.getFileInputs();

    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].value != "" && upload.isExtensionValid(inputs[i].value)) {
            args.IsValid = true;
            break;
        }
    }
}

Sam.Administracion.ToggleLista = function (radio) {
    var rdId = radio.id;
    var lstRadios = $("#" + rdId).parent().parent();
    $(".lista input", lstRadios).each(function (i, item) {
        item.checked = radio.checked;
    });
}

Sam.Administracion.AbrePopupEstimacionJunta = function (idGrid) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    //funciona para la página de cruce
    var grid = $find(idGrid);
    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var idArr = new Array();

    $.each(selected, function (key, item) {
        idArr.push(item.getDataKeyValue("JuntaWorkstatusId"));
    });

    if (idArr.length > 0) {
        var csv = idArr.join(',');
        var proyID = $("[id*=hdnProyectoID]").val() || $("[id*=ddlProyecto]").val();
        oWnd.setUrl("/Administracion/PopupEstimacionJunta.aspx?JWIDS=" + escape(csv) + "&PID=" + proyID);
        Sam.Utilerias.SetSize(oWnd, 850, 500);
        //oWnd.setSize(850, 500);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    }
    else {
        Sam.Alerta(10);
    }
}

Sam.Administracion.AbrePopupEstimacionSpool = function (idGrid) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    //funciona para la página de cruce
    var grid = $find(idGrid);
    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var idArr = new Array();

    $.each(selected, function (key, item) {
        idArr.push(item.getDataKeyValue("WorkstatusSpoolID"));
    });

    if (idArr.length > 0) {
        var csv = idArr.join(',');
        var proyID = $("[id*=hdnProyectoID]").val() || $("[id*=ddlProyecto]").val();
        oWnd.setUrl("/Administracion/PopupEstimacionSpool.aspx?JWIDS=" + escape(csv) + "&PID=" + proyID);
        Sam.Utilerias.SetSize(oWnd, 850, 500);
        //oWnd.setSize(850, 500);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    }
    else {
        Sam.Alerta(7);
    }
}


Sam.Produccion.ToggleLista = function (radio) {
    var checked = radio.checked;
    var rdId = radio.id;

    var lstRadios = $("#" + rdId).parent();
    $(lstRadios).children(':input').each(function (i, item) {
        item.checked = checked;
    });
}

Sam.Produccion.AbrePopUpNuevoPendiente = function () {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Administracion/PopUpNuevoPendiente.aspx");
    Sam.Utilerias.SetSize(oWnd, 790, 515);
    //oWnd.setSize(790, 515);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Produccion.AbrePopUpEdicionPendiente = function (pendienteID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Administracion/PopUpEdicionPendiente.aspx?ID=" + escape(pendienteID));
    Sam.Utilerias.SetSize(oWnd, 800, 550);
    //oWnd.setSize(800, 550);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

//////////////////////////////////////////////
//
//           PRODUCCION
//
/////////////////////////////////////////////

Sam.Produccion.Validaciones = {};
Sam.Produccion.Combos = {};

Sam.Produccion.AbrePopUpTaller = function (idGrid, cultureInfo, idWindow, proyectoID) {
    //debugger;
    //var ventana = $find(idWindow);
    var grid = $find(idGrid);
    var selectedRows = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var arregloIDs = new Array();
    $.each(selectedRows, function (key, item) {
        arregloIDs.push(item.getDataKeyValue("SpoolID"));
    });

    if (arregloIDs.length > 0) {
        var lista = arregloIDs.join(',');
        var windowManager = GetRadWindowManager();
        var ventana = windowManager.getWindowByName("genericWindow");
        ventana.setUrl("/Produccion/PopupTaller.aspx?SPIDS=" + escape(lista) + "&PID=" + proyectoID);
        ventana.set_behaviors(Telerik.Web.UI.WindowBehaviors.None);
        Sam.Utilerias.SetSize(ventana, 550, 400);
        ventana.set_modal(true);
        ventana.center();
        ventana.show();
    }
    else {
        if (cultureInfo == 'en-US') {
            alert("You must select at least one Item");
        }
        else {
            alert("Debe seleccionar al menos un elemento de la lista");
        }
    }

}

Sam.Produccion.ActualizaGridRevisionesEspeciales = function () {
    var boton = $find("cphBody_cphBodyInner_btnQuitaSeleccionados");
    boton.click();
}

Sam.Produccion.AbrePopupCruce = function (idGrid, idDropDown, sortOrder) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    //funciona para la página de cruce
    var grid = $find(idGrid);
    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var idArr = new Array();

    $.each(selected, function (key, item) {
        idArr.push(item.getDataKeyValue("SpoolID"));
    });

    if (idArr.length > 0) {
        var csv = idArr.join(',');
        oWnd.setUrl("/Produccion/PopupCruceOdt.aspx?SPIDS=" + escape(csv) + "&PID=" + $('#' + idDropDown).val() + "&SortOrder=" + escape(sortOrder));
        Sam.Utilerias.SetSize(oWnd, 550, 350);
        //oWnd.setSize(550, 350);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    }
    else {
        Sam.Alerta(7);
    }
}

Sam.Produccion.FilaSeleccionada = function (sender, eventArgs) {
    var grid = sender;
    var MasterTable = grid.get_masterTableView();
    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var spools = document.getElementById("spSpoolsSeleccionados");
    var juntas = $("#spJuntasSeleccionados")[0];
    var accesorio = $("#spAccesoriosSeleccionados")[0];
    var tubo = $("#spTubosSeleccionados")[0];
    var longitud = $("#spLongitudSeleccionados")[0];
    var pdis = $("#spPdiSeleccionados")[0];
    var kgs = $("#spKgsSeleccionados")[0];
    var area = $("#spAreaSeleccionados")[0];
    var peqs = $("#spPeqsSeleccionados")[0];
    var sumaPdi = 0;
    var sumaJuntas = 0;
    var sumaAccesorio = 0;
    var sumaTubo = 0;
    var sumaLongitud = 0;
    var sumaKgs = 0;
    var sumaArea = 0;
    var sumaPeqs = 0;

    juntas.innerHTML = 0;
    accesorio.innerHTML = 0;
    tubo.innerHTM = 0;
    longitud.innerHTML = 0;
    pdis.innerHTML = 0;
    kgs.innerHTML = 0;
    area.innerHTML = 0;
    peqs.innerHTML = 0;
    Sam.Produccion.FilaSeleccionada
    $.each(selected, function (key, row) {
        var juntasCell = MasterTable.getCellByColumnUniqueName(row, "Juntas");
        var accesorioCell = MasterTable.getCellByColumnUniqueName(row, "TotalAccesorio");
        var tuboCell = MasterTable.getCellByColumnUniqueName(row, "TotalTubo");
        var longCell = MasterTable.getCellByColumnUniqueName(row, "LongitudTubo");
        var pdisCell = MasterTable.getCellByColumnUniqueName(row, "Pdis");
        var kgsCell = MasterTable.getCellByColumnUniqueName(row, "Peso");
        var areaCell = MasterTable.getCellByColumnUniqueName(row, "Area");
        var peqsCell = MasterTable.getCellByColumnUniqueName(row, "TotalPeqs");

        sumaJuntas += parseInt($(juntasCell)[0].innerHTML);
        sumaAccesorio += parseInt($(accesorioCell)[0].innerHTML);
        sumaTubo += parseInt($(tuboCell)[0].innerHTML);
        sumaLongitud += parseInt($(longCell)[0].innerHTML);
        sumaPdi += parseFloat($(pdisCell)[0].innerHTML);
        sumaKgs += parseFloat($(kgsCell)[0].innerHTML);
        sumaArea += parseFloat($(areaCell)[0].innerHTML);
        sumaPeqs += parseFloat($(peqsCell)[0].innerHTML);
    });

    //Se asignan los valores
    spools.innerHTML = selected.length;
    juntas.innerHTML = sumaJuntas;
    accesorio.innerHTML = sumaAccesorio;
    tubo.innerHTML = sumaTubo;
    longitud.innerHTML = sumaLongitud;
    $("#spPdiSeleccionados").html(sumaPdi.toFixed(2));
    $("#spKgsSeleccionados").html(sumaKgs.toFixed(2));
    $("#spAreaSeleccionados").html(sumaArea.toFixed(2));
    $("#spPeqsSeleccionados").html(sumaPeqs.toFixed(2));
}

Sam.Produccion.AbrePopupFijarPrioridad = function (wndID) {
    var wnd = $find(wndID);

    Sam.Utilerias.SetSize(wnd, 530, 300);
    //wnd.setSize(530, 300);
    wnd.show();
}

Sam.Produccion.AbrePopupFijarPrioridadSeleccionados = function (idGrid, proyID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    //funciona para la página de FijarPrioridad
    var grid = $find(idGrid);

    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    if (selected.length > 0) {

        $.each(selected, function (key, item) {
            idArr.push(item.getDataKeyValue("SpoolID"));
        });

        if (idArr.length > 0) {
            var csv = idArr.join(',');
            oWnd.setUrl("/Produccion/PopUpFijarPrioridadSeleccionados.aspx?IDs=" + escape(csv) + "&PID=" + proyID);
            Sam.Utilerias.SetSize(oWnd, 570, 370);
            //oWnd.setSize(850, 500);
            oWnd.set_modal(true);
            oWnd.center();
            oWnd.show();
        }
    }
    else {
        Sam.Alerta(07);
    }
}



Sam.Produccion.AbrePopupSpoolRO = function (idSpool) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Produccion/PopupSpoolRO.aspx?ID=" + escape(idSpool));
    Sam.Utilerias.SetSize(oWnd, 790, 515);
    //oWnd.setSize(790, 515);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Produccion.AbrePopupSpoolRevision = function (idSpool) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Produccion/PopupSpoolRO.aspx?ID=" + escape(idSpool) + "&SPREV=" + true);
    Sam.Utilerias.SetSize(oWnd, 790, 515);
    //oWnd.setSize(790, 515);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Produccion.AbrePopupSpoolOdtRO = function (idSpool) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Produccion/PopupSpoolOdtRO.aspx?ID=" + escape(idSpool));
    Sam.Utilerias.SetSize(oWnd, 940, 580);
    //oWnd.setSize(790, 515);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Produccion.OrdenDeTrabajoGenerada = function () {
    $('#btnWrapper > input')[0].click();
}

Sam.Produccion.Validaciones.PatioProyectoRequerido = function (sender, args) {
    var patio = $.trim($("[id*=ddlPatio]").val());
    //telerik lo mete en un div por eso hay que buscarlo también por tag name
    var proyecto = $.trim($("[id*=ddlProyecto] > select").val());
    args.IsValid = patio !== "" || proyecto !== "";
}

Sam.Produccion.AbrePopupImpresionOdt = function (odtId) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Produccion/PopupImpresionOdt.aspx?ID=" + escape(odtId));
    Sam.Utilerias.SetSize(oWnd, 605, 605);
    //nd.setSize(540, 320);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Produccion.AbrePopupHistoricoOdt = function (odtId) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Produccion/PopupHistoricoOdt.aspx?ID=" + escape(odtId));
    Sam.Utilerias.SetSize(oWnd, 840, 320);
    //nd.setSize(540, 320);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Produccion.MovInventario_NumUnicoRequesting = function (sender, args) {
    Sam.WebService.AgregaProyectoID(sender, args);
}

Sam.Produccion.MovInventario_ddlProyecto_SelectedIndexChanged = function (sender) {
    Sam.WebService.LimpiaItemsRadCombo("ddlNumeroUnico");
    Sam.Produccion.LimpiaItems();
}

Sam.Produccion.LimpiaItems = function (sender, args) {
    $(".ToClear").each(function (i, item) {
        item.innerHTML = "";
    });
}

Sam.Produccion.AbrePopupCongeladoNumeroUnico = function (idGrid, proyID, numunic, codigo) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(idGrid);
    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var idArr = new Array();
    var iSpool = new Array();
    var iMatSpool = new Array();

    $.each(selected, function (key, item) {
        iSpool.push(item.getDataKeyValue("SpoolID"));
    });

    $.each(selected, function (key, item) {
        idArr.push(item.getDataKeyValue("CantCong"));
    });

    $.each(selected, function (key, item) {
        iMatSpool.push(item.getDataKeyValue("MaterialSpoolID"));
    });

    if (idArr.length > 0) {
        var csv = idArr.join(',');
        var spool = iSpool.join(',');
        var MatSpool = iMatSpool.join(',');
        oWnd.setUrl("/Produccion/PopupCongeladoNumeroUnico.aspx?IDs=" + escape(spool) + "&cantcong=" + escape(csv) + "&MatSpool=" + escape(MatSpool) + "&PID=" + proyID + "&NUM=" + numunic + "&CODIGO=" + codigo);
        Sam.Utilerias.SetSize(oWnd, 750, 480);
        oWnd.center();
        oWnd.show();
    }
    else {
        Sam.Alerta(07);
    }
}

Sam.Produccion.AbrePopupCongeladoOrdenTrabajo = function (spool, MatSpool, cantcong, proyID, numunic, codigo) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Produccion/PopupCongeladoNumeroUnico.aspx?IDs=" + escape(spool) + "&cantcong=" + escape(cantcong) + "&MatSpool=" + escape(MatSpool) + "&PID=" + proyID + "&NUM=" + numunic + "&CODIGO=" + codigo);
    Sam.Utilerias.SetSize(oWnd, 750, 480);
    oWnd.center();
    oWnd.show();
}

Sam.Produccion.AbrePopupJuntasCampo = function (juntaSpoolId) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    if (oWnd.get_events() !== null && oWnd.get_events()._list !== null && oWnd.get_events()._list.beforeClose !== null) {
        oWnd.add_beforeClose(Sam.Produccion.ActualizaGridJuntasCampo);
    }
    oWnd.setUrl("/Produccion/PopupJuntasCampo.aspx?JuntaSpoolID=" + escape(juntaSpoolId));
    Sam.Utilerias.SetSize(oWnd, 800, 510);
    oWnd.center();
    oWnd.show();
}

Sam.Produccion.ActualizaGridJuntasCampo = function (sender, args) {
    var idGrid = $("[id$=grdSpools]").attr("ID");
    var grid = $find(idGrid);

    if (grid && grid !== null) {
        var masterTable = grid.get_masterTableView();
        masterTable.rebind();
    }
}

Sam.Produccion.ValidaNuevasFechas = function (proceso, mdpFechaProcesoActual, fechasProcesoAnterior1, fechasProcesoAnterior2, fechasProcesoAnterior3) {
    var fechaProcesoAnterior = $("[id*=ctl00_cphBody_rdwCambiarFechaProcesoAnterior_C_mdpFechaProcesoAnterior_dateInput_text]").val();
    var fechaReporteProcesoAnterior = $("[id*=ctl00_cphBody_rdwCambiarFechaProcesoAnterior_C_mdpFechaReporteProcesoAnterior_dateInput_text]").val();

    var fechaProcesoActual = $("[id*=" + mdpFechaProcesoActual + "_dateInput_text]").val();
    var fechaReporteProcesoActual = $("[id*=ctl00_cphBody_mdpFechaReporte_dateInput_text]").val();

    if (fechaProcesoAnterior != "" && fechaReporteProcesoAnterior != "") {
        if ($("[id*=hdnCambiaFechas]").val() == 1) {
            var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

            if (valorCookie == "en-US") {
                var dateProcesoAnterior = new Date(fechaProcesoAnterior);
                var dateReporteProcesoAnterior = new Date(fechaReporteProcesoAnterior);

                var dateProcesoActual = new Date(fechaProcesoActual);
                var dateReporteProcesoActual = new Date(fechaReporteProcesoActual);
            }
            else {
                var partesFechaProcesoAnterior = fechaProcesoAnterior.split("/");
                var dateProcesoAnterior = new Date(partesFechaProcesoAnterior[2], (partesFechaProcesoAnterior[1] - 1), partesFechaProcesoAnterior[0]);

                if (fechaReporteProcesoAnterior != null) {
                    var partesFechaReporteProcesoAnterior = fechaReporteProcesoAnterior.split("/")
                    var dateReporteProcesoAnterior = new Date(partesFechaReporteProcesoAnterior[2], (partesFechaReporteProcesoAnterior[1] - 1), partesFechaReporteProcesoAnterior[0]);

                    if (proceso != 3) {
                        if (dateReporteProcesoAnterior < dateProcesoAnterior) {
                            alert(Sam.MensajesUI(41));
                            return false;
                        }
                    }
                }

                var partesFechaProcesoActual = fechaProcesoActual.split("/");
                var dateProcesoActual = new Date(partesFechaProcesoActual[2], (partesFechaProcesoActual[1] - 1), partesFechaProcesoActual[0]);

                if (fechaReporteProcesoActual != null) {
                    var partesFechaReporteProcesoActual = fechaReporteProcesoActual.split("/")
                    var dateReporteProcesoActual = new Date(partesFechaReporteProcesoActual[2], (partesFechaReporteProcesoActual[1] - 1), partesFechaReporteProcesoActual[0]);
                }
            }

            if (proceso != 3) {
                if (dateProcesoActual < dateProcesoAnterior || dateReporteProcesoActual < dateReporteProcesoAnterior) {
                    alert(Sam.MensajesUI(49));
                    return false;
                }
            }

            if (fechasProcesoAnterior1 != null) {
                if (!Sam.Produccion.ValidaConProcesoAnterior(proceso, fechaProcesoAnterior, fechaReporteProcesoAnterior, fechasProcesoAnterior1, 1)) {
                    return false;
                }
            }

        }
        else if ($("[id*=hdnCambiaFechas]").val() == 2) {
            if (fechasProcesoAnterior2 != null) {
                Sam.Produccion.ValidaConProcesoAnterior(proceso, fechaProcesoAnterior, fechaReporteProcesoAnterior, fechasProcesoAnterior2, 2);
                return false;
            }
        }
        else if ($("[id*=hdnCambiaFechas]").val() == 3) {
            if (fechasProcesoAnterior3 != null) {
                Sam.Produccion.ValidaConProcesoAnterior(proceso, fechaProcesoAnterior, fechaReporteProcesoAnterior, fechasProcesoAnterior3, 3);
                return false;
            }
        }
    }
    else {
        alert(Sam.MensajesUI(42));
        return false;
    }
}

Sam.Produccion.ValidaConProcesoAnterior = function (proceso, fechaProceso, fechaReporteProceso, fechas, contador) {
    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var dateProcesoAnterior = new Date(fechaProceso);
        var dateReporteProcesoAnterior = new Date(fechaReporteProceso);
        if (proceso == 1) {
            var msg = Sam.MensajesUI(32);
            var titulo = Sam.MensajesUI(33);
            var lblEncabezadoFechaProcesoAnterior = "Assembly date:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Report date:";
            var lblNuevaFecha = "New Assembly date:";
            var lblNuevaFechaReporte = "New Report date:";
        }
        else if (proceso == 2 && contador == 1) {
            var msg = Sam.MensajesUI(44);
            var titulo = Sam.MensajesUI(43);
            var lblEncabezadoFechaProcesoAnterior = "Welding date:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Report date:";
            var lblNuevaFecha = "New Welding date:";
            var lblNuevaFechaReporte = "New Report date:";
        }
        else if (proceso == 2 && contador == 2) {
            var msg = Sam.MensajesUI(32);
            var titulo = Sam.MensajesUI(33);
            var lblEncabezadoFechaProcesoAnterior = "Assembly date:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Report date:";
            var lblNuevaFecha = "New Assembly date:";
            var lblNuevaFechaReporte = "New Report date:";
        }
        else if (proceso == 3 && contador == 1) {

            var msg = Sam.MensajesUI(45);
            var titulo = Sam.MensajesUI(46);
            var lblEncabezadoFechaProcesoAnterior = "Visual Inspection date:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Report date:";
            var lblNuevaFecha = "New Visual Inspection date:";
            var lblNuevaFechaReporte = "New Report date:";
        }
        else if (proceso == 3 && contador == 2) {
            var msg = Sam.MensajesUI(44);
            var titulo = Sam.MensajesUI(43);
            var lblEncabezadoFechaProcesoAnterior = "Welding date:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Report date:";
            var lblNuevaFecha = "New Welding date:";
            var lblNuevaFechaReporte = "New Report date:";
        }
        else if (proceso == 3 && contador == 3) {
            var msg = Sam.MensajesUI(32);
            var titulo = Sam.MensajesUI(33);
            var lblEncabezadoFechaProcesoAnterior = "Assembly date:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Report date:";
            var lblNuevaFecha = "New Assembly date:";
            var lblNuevaFechaReporte = "New Report date:";
        }
    }
    else {

        if (proceso == 3 && contador == 1) {
            var partesFechaProceso = fechaProceso.split("/");
            var dateProceso = new Date(partesFechaProceso[2], (partesFechaProceso[1] - 1), partesFechaProceso[0]);
        }
        else {
            var partesFechaProceso = fechaProceso.split("/");
            var dateProceso = new Date(partesFechaProceso[2], (partesFechaProceso[1] - 1), partesFechaProceso[0]);
            var partesFechaReporteProceso = fechaReporteProceso.split("/");
            var dateReporteProceso = new Date(partesFechaReporteProceso[2], (partesFechaReporteProceso[1] - 1), partesFechaReporteProceso[0]);
        }
        if (proceso == 1) {
            var msg = Sam.MensajesUI(32);
            var titulo = Sam.MensajesUI(33);
            var lblEncabezadoFechaProcesoAnterior = "Fecha de Armado:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Fecha del Reporte:";
            var lblNuevaFecha = "Nueva Fecha de Armado:";
            var lblNuevaFechaReporte = "Nueva Fecha del Reporte:";
        }
        else if (proceso == 2 && contador == 1) {
            var msg = Sam.MensajesUI(44);
            var titulo = Sam.MensajesUI(43);
            var lblEncabezadoFechaProcesoAnterior = "Fecha de soldadura:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Fecha del Reporte:";
            var lblNuevaFecha = "Nueva fecha de soldadura:";
            var lblNuevaFechaReporte = "Nueva Fecha del Reporte:";
        }
        else if (proceso == 2 && contador == 2) {
            var msg = Sam.MensajesUI(32);
            var titulo = Sam.MensajesUI(33);
            var lblEncabezadoFechaProcesoAnterior = "Fecha de Armado:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Fecha de Reporte:";
            var lblNuevaFecha = "Nueva Fecha de Armado:";
            var lblNuevaFechaReporte = "Nueva Fecha de Reporte:";
        }
        else if (proceso == 3 && contador == 1) {
            var msg = Sam.MensajesUI(45);
            var titulo = Sam.MensajesUI(46);
            var lblEncabezadoFechaProcesoAnterior = "Fecha de Inspección Visual:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Fecha del Reporte:";
            var lblNuevaFecha = "Nueva fecha de Inspección Visual:";
            var lblNuevaFechaReporte = "Nueva Fecha del Reporte:";
        }
        else if (proceso == 3 && contador == 2) {

            var msg = Sam.MensajesUI(44);
            var titulo = Sam.MensajesUI(43);
            var lblEncabezadoFechaProcesoAnterior = "Fecha de soldadura:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Fecha del Reporte:";
            var lblNuevaFecha = "Nueva fecha de soldadura:";
            var lblNuevaFechaReporte = "Nueva Fecha del Reporte:";
        }
        else if (proceso == 3 && contador == 3) {
            var msg = Sam.MensajesUI(32);
            var titulo = Sam.MensajesUI(33);
            var lblEncabezadoFechaProcesoAnterior = "Fecha de Armado:";
            var lblEncabezadoFechaReporteProcesoAnterior = "Fecha de Reporte:";
            var lblNuevaFecha = "Nueva Fecha de Armado:";
            var lblNuevaFechaReporte = "Nueva Fecha de Reporte:";
        }
    }

    if (dateReporteProceso < dateProceso) {
        alert(Sam.MensajesUI(41));
        return false;
    }

    var arrayfechasProcesoAnterior = fechas.split(',');

    var datefechasProcesoAnterior = new Date(arrayfechasProcesoAnterior[0]);
    var datefechasReporteProcesoAnterior = new Date(arrayfechasProcesoAnterior[1]);

    if (dateProceso < datefechasProcesoAnterior || dateReporteProceso < datefechasReporteProcesoAnterior) {
        var answer = confirm(msg);
        if (answer) {
            var wnd = $find("ctl00_cphBody_rdwCambiarFechaProcesoAnterior");
            $("#ctl00_cphBody_rdwCambiarFechaProcesoAnterior_C_Reportes").show();
            wnd.set_title(titulo);

            contador++;
            $("[id*=hdnCambiaFechas]").val(contador);
            $("[id*=hdnProcesoAnterior" + contador + "]").val(fechaProceso);
            $("[id*=hdnProcesoReporteAnterior" + contador + "]").val(fechaReporteProceso);

            var parteArrayProcesoAnterior = arrayfechasProcesoAnterior[0].split("/");

            $("[id*=lblFechaProcesoAnterior]").text(parteArrayProcesoAnterior[1] + "/" + (parteArrayProcesoAnterior[0]) + "/" + parteArrayProcesoAnterior[2]);

            var parteArrayProcesoAnterior = arrayfechasProcesoAnterior[1].split("/");

            $("[id*=lblFechaReporteProcesoAnterior]").text(parteArrayProcesoAnterior[1] + "/" + (parteArrayProcesoAnterior[0]) + "/" + parteArrayProcesoAnterior[2]);

            $("[id*=lblEncabezadoFechaProcesoAnterior]").text(lblEncabezadoFechaProcesoAnterior);
            $("[id*=lblEncabezadoFechaReporteProcesoAnterior]").text(lblEncabezadoFechaReporteProcesoAnterior);
            $("[id*=lblNuevaFecha]").text(lblNuevaFecha);
            $("[id*=lblNuevaFechaReporte]").text(lblNuevaFechaReporte);

            return false;
        }
        else {
            $("[id*=hdnCambiaFechas]").val(contador);
            return false;
        }
    }
    else {
        return true;
    }

}



Sam.Produccion.CambioFechasRequisiciones = function (idVentana, fechaProcesoAnterior) {
    $("[id*=hdnCambiaFechas]").val("0");
    var fechaProcesoActual = $("[id*=ctl00_cphBody_mdpFechaRequisicion_dateInput_text]").val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var dateProceso = new Date(fechaProcesoActual);
    }
    else {
        var partesFechaProceso = fechaProcesoActual.split("/");
        var dateProceso = new Date(partesFechaProceso[2], (partesFechaProceso[1] - 1), partesFechaProceso[0]);
    }

    var fechasArrayProcesoAnterior = fechaProcesoAnterior.split(',');

    var dateProcesoAnterior = new Date(fechasArrayProcesoAnterior[0]);

    var dateReporteProcesoAnterior = new Date(fechasArrayProcesoAnterior[1]);

    if (dateProcesoAnterior < dateReporteProcesoAnterior) {
        dateProcesoAnterior = dateReporteProcesoAnterior
    }

    if (dateProcesoAnterior > dateProceso) {

        var msg = Sam.MensajesUI(45);

        var answer = confirm(msg);
        if (answer) {
            var wnd = $find(idVentana);

            wnd.set_title(Sam.MensajesUI(46));

            $("[id*=hdnCambiaFechas]").val("1");

            var parteArrayProcesoAnterior = fechasArrayProcesoAnterior[0].split("/");

            $("[id*=lblFechaProcesoAnterior]").text(parteArrayProcesoAnterior[1] + "/" + (parteArrayProcesoAnterior[0]) + "/" + parteArrayProcesoAnterior[2]);

            var parteArrayProcesoAnterior = fechasArrayProcesoAnterior[1].split("/");

            $("[id*=lblFechaReporteProcesoAnterior]").text(parteArrayProcesoAnterior[1] + "/" + (parteArrayProcesoAnterior[0]) + "/" + parteArrayProcesoAnterior[2]);

            wnd.setSize(690, 195);
            wnd.moveTo(Sam.Workstatus.X + 100, Sam.Workstatus.Y + 100);
            wnd.show();
        }
        else {
            return false;
        }
    }
    else {
        //Dar click en el botón del popUp para hacer el proceso del save
        $("[id*=hdnCambiaFechas]").val("0");
        $("[id*=btnGuardarPopUp]").click();
    }
    return false;
}

Sam.Produccion.CambioFechasPruebas = function (idVentana, fechaProcesoAnterior) {
    $("[id*=hdnCambiaFechas]").val("0");
    $("#ctl00_cphBody_rdwCambiarFechaProcesoAnterior_C_Reportes").hide();
    var fechaProcesoActual = $("[id*=ctl00_cphBody_mdpFechaPND_dateInput_text]").val();
    var fechaReporteProcesoActual = $("[id*=ctl00_cphBody_mdpFechaReporte_dateInput_text]").val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var dateProceso = new Date(fechaProcesoActual);
        var dateReporteProceso = new Date(fechaReporteProcesoActual);
    }
    else {
        var partesFechaProceso = fechaProcesoActual.split("/");
        var dateProceso = new Date(partesFechaProceso[2], (partesFechaProceso[1] - 1), partesFechaProceso[0]);

        var partesFechaReporteProceso = fechaReporteProcesoActual.split("/");
        var dateReporteProceso = new Date(partesFechaReporteProceso[2], (partesFechaReporteProceso[1] - 1), partesFechaReporteProceso[0]);
    }

    if ((fechaReporteProcesoActual < fechaProcesoActual)) {
        alert(Sam.MensajesUI(41));
        return false;
    }

    var dateProcesoAnterior = new Date(fechaProcesoAnterior);

    if (dateProceso < dateReporteProceso) {
        dateProceso = dateReporteProceso;
    }

    if (dateProcesoAnterior > dateProceso) {

        var msg = Sam.MensajesUI(48);

        var answer = confirm(msg);
        if (answer) {
            var wnd = $find(idVentana);

            wnd.set_title(Sam.MensajesUI(47));

            $("[id*=hdnCambiaFechas]").val("1");

            var parteArrayProcesoAnterior = fechaProcesoAnterior.split("/");

            $("[id*=lblFechaProcesoAnterior]").text(parteArrayProcesoAnterior[1] + "/" + (parteArrayProcesoAnterior[0]) + "/" + parteArrayProcesoAnterior[2]);

            wnd.setSize(690, 195);
            wnd.moveTo(Sam.Workstatus.X + 100, Sam.Workstatus.Y + 100);
            wnd.show();
        }
        else {
            return false;
        }
    }
    else {
        //Dar click en el botón del popUp para hacer el proceso del save
        $("[id*=hdnCambiaFechas]").val("0");
        $("[id*=btnGuardarPopUp]").click();
    }
    return false;
}

//////////////////////////////////////////////
//
//           WORKSTATUS
//
/////////////////////////////////////////////

Sam.Workstatus.Validaciones = {};

Sam.Workstatus.ActualizaGridDespacho = function () {
    Sam.Popup.VentanaPadre().Sam.Workstatus.ActualizaGrid();
}

Sam.Workstatus.AbrePopupDespacho = function (odtMatId) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    oWnd.setUrl("/WorkStatus/PopupDespacho.aspx?ID=" + escape(odtMatId));
    Sam.Utilerias.SetSize(oWnd, 820, 500);
    //oWnd.setSize(820, 450);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.AbrePopupEdicionEspecialDespacho = function (despachoID) {
    ////debugger;
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/WorkStatus/PopUpEdicionEspecialDespacho.aspx?ID=" + escape(despachoID));
    Sam.Utilerias.SetSize(oWnd, 800, 575);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.EdicionEspecialDespachoTerminada = function () {
    ////debugger;
    $('#btnWrapper > input')[0].click();
}

Sam.Workstatus.AbrePopupEdicionEspecialArmado = function (juntaSpoolID, juntaArmadoID) {
    ////debugger;
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/WorkStatus/PopupArmado.aspx?ID=" + escape(juntaSpoolID) + "&RO=" + false + "&IDJA=" + escape(juntaArmadoID) + "&EDES=" + true);
    Sam.Utilerias.SetSize(oWnd, 800, 575);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.AbrePopUpTranferenciaCorte = function (proyID, grdID, ordenTrabajoID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("NumeroUnicoSegmentoID")); });
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpTransferenciaCorte.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv) + "&OT=" + escape(ordenTrabajoID));
        Sam.Utilerias.SetSize(oWnd, 500, 297);
        //oWnd.setSize(500, 297);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(9);
    }
}

Sam.Workstatus.SoldaduraConsumibles = function (sender, args) {
    var PatioID = $("[id*=hdnPatioID]").val();
    args._context["PatioID"] = PatioID;
}

Sam.Workstatus.AbrePopupDetalleCorte = function (idCorte) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Workstatus/PopUpDetalleCorte.aspx?ID=" + escape(idCorte));
    Sam.Utilerias.SetSize(oWnd, 650, 500);
    //oWnd.setSize(650, 500);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.AbrePopupDetalleDespacho = function (idDespacho) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Workstatus/PopUpDetalleDespacho.aspx?ID=" + escape(idDespacho));
    Sam.Utilerias.SetSize(oWnd, 650, 400);
    //oWnd.setSize(650, 400);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.AbrePopupArmado = function (juntaSpoolID, readOnly) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Workstatus/PopupArmado.aspx?ID=" + escape(juntaSpoolID) + "&RO=" + escape(readOnly));
    Sam.Utilerias.SetSize(oWnd, 800, 575);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}


Sam.Workstatus.AbrePopupSoldadura = function (idJuntaWorkstatusID, readOnly) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Workstatus/PopupSoldadura.aspx?ID=" + escape(idJuntaWorkstatusID) + "&RO=" + escape(readOnly));
    Sam.Utilerias.SetSize(oWnd, 790, 560);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.AbrePopUpSoldaduraEdicion = function (idJuntaWsID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Workstatus/PopupSoldadura.aspx?ID=" + escape(idJuntaWsID) + "&RO=" + false + "&EDES=" + true);
    Sam.Utilerias.SetSize(oWnd, 790, 560);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.AbrePopUpEdicionReporte = function (idReporte, tipoPrueba) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Workstatus/PopUPEdicionEspecialReporte.aspx?ID=" + escape(idReporte) + "&TP=" + escape(tipoPrueba));
    Sam.Utilerias.SetSize(oWnd, 550, 350);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}


Sam.Workstatus.AbrePopupEspecificarSistema = function (idReqPinturaDetalleID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(idReqPinturaDetalleID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("SpoolID")); });
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopupEspecificarSistema.aspx?RPID=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 500, 320);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }

}

Sam.Workstatus.AbrePopupGenerarRequisicion = function (proyID, idReqPinturaDetalle) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    var grid = $find(idReqPinturaDetalle)
    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var idArr = new Array();
    var idArrOtsIDs = new Array();
    $.each(selected, function (key, item) {
        idArr.push(item.getDataKeyValue("WorkstatusSpoolID"));
        idArrOtsIDs.push(item.getDataKeyValue("OrdenTrabajoSpoolID"));
    });
    if (idArrOtsIDs.length > 0) {
        var csvOtsIDs = idArrOtsIDs.join(',');
    }

    if (idArr.length > 0 || idArrOtsIDs > 0) {
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopupGenerarRequisicion.aspx?ID=" + escape(proyID) + "&RPID=" + escape(csv) + "&OTSIDs=" + escape(csvOtsIDs));
        Sam.Utilerias.SetSize(oWnd, 500, 320);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    }
    else {
        Sam.Alerta(7);
    }
}

Sam.Workstatus.AbrePopUpReporteInspeccionVisual = function (proyID, grdID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("JuntaWorkstatusID")); });
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpInspeccionVisual.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 800, 550);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(10);
    }
}

Sam.Workstatus.AbrePopupReporteLiberacionVisualPatio = function (idGrid) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    //funciona para la página de cruce
    var grid = $find(idGrid);
    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var idArr = new Array();

    $.each(selected, function (key, item) {
        idArr.push(item.getDataKeyValue("InspeccionVisualPatioID"));
    });

    if (idArr.length > 0) {
        var csv = idArr.join(',');
        var proyID = $("[id*=hdnProyectoID]").val() || $("[id*=ddlProyecto]").val();
        oWnd.setUrl("/WorkStatus/PopupReporteLiberacionVisualPatio.aspx?JWIDS=" + escape(csv) + "&PID=" + proyID);
        Sam.Utilerias.SetSize(oWnd, 850, 350);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    }
    else {
        Sam.Alerta(7);
    }
}

Sam.Workstatus.AbrePopupReporteLiberacionDimencionalPatio = function (idGrid) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    //funciona para la página de cruce
    var grid = $find(idGrid);
    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var idArr = new Array();

    $.each(selected, function (key, item) {
        idArr.push(item.getDataKeyValue("InspeccionDimansionalPatioID"));
    });

    if (idArr.length > 0) {
        var csv = idArr.join(',');
        var proyID = $("[id*=hdnProyectoID]").val() || $("[id*=ddlProyecto]").val();
        oWnd.setUrl("/WorkStatus/PopupReporteLiberacionDimensionalPatio.aspx?SPIDS=" + escape(csv) + "&PID=" + proyID);
        Sam.Utilerias.SetSize(oWnd, 850, 350);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    }
    else {
        Sam.Alerta(7);
    }
}

Sam.Workstatus.AbrePopUpReporteInspeccionDimensional = function (proyID, grdID, TRID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idSpoolArr = new Array();
        $.each(selectedRows, function (key, item) { idSpoolArr.push(item.getDataKeyValue("SpoolID")); });
        var csSpoolv = idSpoolArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpInspeccionDimensional.aspx?ID=" + escape(proyID) + "&SIDs=" + escape(csSpoolv) + "&TR=" + escape(TRID));
        Sam.Utilerias.SetSize(oWnd, 800, 450);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}

Sam.Workstatus.AbrePopUpRequisicionSpoolPruebas = function (proyID, grdID, tipoPruebaID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        var idArrOts = new Array();

        $.each(selectedRows, function (key, item) {
            idArr.push(item.getDataKeyValue("WorkstatusSpoolID"));
            idArrOts.push(item.getDataKeyValue("OrdenTrabajoSpoolID"));
        });

        var csSpoolv = idArr.join(',');
        var csOts = idArrOts.join(',');

        oWnd.setUrl("/Workstatus/PopUpRequisicionSpoolPruebas.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csSpoolv) + "&TP=" + escape(tipoPruebaID) + "&OTSIDs=" + escape(csOts));
        Sam.Utilerias.SetSize(oWnd, 800, 450);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}


Sam.Workstatus.AbrePopUpRequisicionPruebas = function (proyID, grdID, tipoPruebaID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("JuntaWorkstatusID")); });
        var csSpoolv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpRequisicionPruebas.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csSpoolv) + "&TP=" + escape(tipoPruebaID));
        Sam.Utilerias.SetSize(oWnd, 800, 450);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(10);
    }
}

Sam.Workstatus.AbrePopUpPruebasSpoolPND = function (proyID, grdID, tipoPruebaID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("WorkstatusSpoolID")); });
        var csv = idArr.join(',');
        var idReqArr = new Array();
        $.each(selectedRows, function (key, item) { idReqArr.push(item.getDataKeyValue("RequisicionSpoolID")); });
        var csReqv = idReqArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpReporteSpoolPND.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv) + "&TP=" + escape(tipoPruebaID) + "&RID=" + escape(csReqv));
        Sam.Utilerias.SetSize(oWnd, 700, 580);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}

Sam.Workstatus.AbrePopUpPruebasPND = function (proyID, grdID, tipoPruebaID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();

        $.each(selectedRows, function (key, item) {
            idArr.push(item.getDataKeyValue("JuntaWorkstatusID"));
        });
        var csv = idArr.join(',');
        var idReqArr = new Array();

        $.each(selectedRows, function (key, item) {
            idReqArr.push(item.getDataKeyValue("RequisicionID"));
        });

        var csReqv = idReqArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpReportePND.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv) + "&TP=" + escape(tipoPruebaID) + "&RID=" + escape(csReqv));
        Sam.Utilerias.SetSize(oWnd, 800, 650);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(10);
    }
}

Sam.Workstatus.AbrePopUpPruebasTT = function (proyID, grdID, tipoPruebaID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("JuntaWorkstatusID")); });
        var csv = idArr.join(',');
        var idReqArr = new Array();
        $.each(selectedRows, function (key, item) { idReqArr.push(item.getDataKeyValue("RequisicionID")); });
        var csReqv = idReqArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpReporteTT.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv) + "&TP=" + escape(tipoPruebaID) + "&RID=" + escape(csReqv));
        Sam.Utilerias.SetSize(oWnd, 800, 480);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(10);
    }
}

Sam.Workstatus.AbrePopUpPintura = function (proyID, grdID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {

        var idArr = new Array();
        var idArrOTS = new Array();
        var idReqArr = new Array();

        $.each(selectedRows, function (key, item) {
            idArr.push(item.getDataKeyValue("WorkstatusSpoolID"));
            idArrOTS.push(item.getDataKeyValue("OrdenTrabajoSpoolID"));
            idReqArr.push(item.getDataKeyValue("RequisicionPinturaDetalleID"));
        });


        var csv = idArr.join(',');
        var csReqv = idReqArr.join(',');
        var csvOts = idArrOTS.join(',');

        oWnd.setUrl("/Workstatus/PopUpPintura.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv) + "&RID=" + escape(csReqv) + "&OTSIDs=" + escape(csvOts));
        Sam.Utilerias.SetSize(oWnd, 800, 520);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}

Sam.Workstatus.AbrePopUpImprimeEtiquetas = function (proyID, grdID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("SpoolID")); });
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpImprimirEtiqueta.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 600, 365);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}

Sam.Workstatus.AbrePopUpEtiquetar = function (proyID, grdID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("SpoolID")); });
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpEtiquetar.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 600, 300);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}


Sam.Workstatus.AbrePopUpPreparar = function (proyID, grdID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("WorkstatusSpoolID")); });
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpPreparar.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 600, 300);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}



Sam.Workstatus.AbrePopUpEmbarcar = function (proyID, grdID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("WorkstatusSpoolID")); });
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpEmbarcar.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 600, 629);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}

Sam.Workstatus.AbrePopUpTransferenciaSpool = function (proyID, grdID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("TransferenciaSpoolID")); });
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopUpTransferenciaSpool.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 600, 629);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}



Sam.Workstatus.AbrePopUpPrepararSpoolTransferencia = function (proyID, grdID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows, function (key, item) { idArr.push(item.getDataKeyValue("SpoolID")); });
        var csv = idArr.join(',');
        oWnd.setUrl("/Workstatus/PopupPrepararSpoolTransferencia.aspx?ID=" + escape(proyID) + "&IDs=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 600, 300);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(7);
    }
}


Sam.Workstatus.ActualizaGridPersonalizado = function () {
    ////debugger;
    var boton = $("[id*=cphBody_cphBodyInner_btnQuitaSeleccionados]");
    boton.click();
}

Sam.Workstatus.CierraPopUpGenerico = function () {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.close();
}

Sam.Workstatus.ActualizaGrid = function () {
    $('#btnWrapper > input')[0].click();
}

Sam.Workstatus.ActualizayCierra = function () {
    $('#btnWrapper > input')[0].click();
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.close();
}

Sam.Workstatus.AbrePopupReportePintura = function (workstatusID, ordenTrabajoSpoolId) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    oWnd.setUrl("/Workstatus/PopUpReportePintura.aspx?ID=" + escape(workstatusID) + "&otsID=" + escape(ordenTrabajoSpoolId));

    Sam.Utilerias.SetSize(oWnd, 780, 520);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.AbrePopupReporteEmbarque = function (spoolID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Workstatus/PopUpReporteEmbarque.aspx?ID=" + escape(spoolID));
    Sam.Utilerias.SetSize(oWnd, 780, 520);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.AbrePopupReporteTransferencia = function (spoolID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Workstatus/PopUpReporteTransferencia.aspx?ID=" + escape(spoolID));
    Sam.Utilerias.SetSize(oWnd, 780, 520);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Workstatus.CVFechaReporteIVContraSoldadura = function (sender, args) {
    args.IsValid = Sam.Workstatus.ValidaFechaReporteSoldaduraContraIV(args.Value);
}

Sam.Workstatus.ValidaFechaReporteSoldaduraContraIV = function (fechaReporteIV) {

    var esValido = true;

    var fechaReporteSoldadura = $("[id*=ctl00_cphBody_ctrlInfo_mdpFechaReporte_dateInput_text]").val();
    var fechaProcSoldadura = $("#ctl00_cphBody_ctrlInfo_mdpFechaSoldadura_dateInput_text").val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var datefechaReporteSoldadura = new Date(fechaReporteSoldadura);
        var datefechaProcSoldadura = new Date(fechaProcSoldadura);

        var datefechaReporteIV = new Date(fechaReporteIV);
    }
    else {
        var partesfechaReporteSoldadura = fechaReporteSoldadura.split("/");
        var datefechaReporteSoldadura = new Date(partesfechaReporteSoldadura[2], (partesfechaReporteSoldadura[1] - 1), partesfechaReporteSoldadura[0]);

        var partesfechaProcSoldadura = fechaProcSoldadura.split("/");
        var datefechaProcSoldadura = new Date(partesfechaProcSoldadura[2], (partesfechaProcSoldadura[1] - 1), partesfechaProcSoldadura[0]);

        var partesfechaReporteIV = fechaReporteIV.split("/");
        var datefechaReporteIV = new Date(partesfechaReporteIV[2], (partesfechaReporteIV[1] - 1), partesfechaReporteIV[0]);
    }

    if ((fechaReporteIV != "") && (datefechaReporteIV < datefechaReporteSoldadura)) {
        esValido = confirm(Sam.MensajesUI(57));
    }

    return esValido;
}


Sam.Workstatus.CVFechaReporteSoldadura = function (sender, args) {
    //debugger;
    args.IsValid = Sam.Workstatus.ValidaFechaReporteSoldadura(args.Value);
}

Sam.Workstatus.ValidaFechaReporteSoldadura = function (fechaReporteArmado) {
    //debugger;
    var esValido = true;

    var fechaReporteSoldadura = $("#ctl00_cphBody_ctrlInfo_mdpFechaReporte").val();
    var fechaProcSoldadura = $("#ctl00_cphBody_ctrlInfo_mdpFechaSoldadura").val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var datefechaReporteSoldadura = new Date(fechaReporteSoldadura);
        var datefechaProcSoldadura = new Date(fechaProcSoldadura);

        var datefechaReporteArmado = new Date(fechaReporteArmado);
    }
    else {

        //var partesfechaReporteSoldadura = fechaReporteSoldadura.split("-");

        var datefechaReporteSoldadura = new Date(fechaReporteSoldadura);

        //var partesfechaProcSoldadura = fechaProcSoldadura.split("-");
        //var datefechaProcSoldadura = new Date(partesfechaProcSoldadura[1], (partesfechaProcSoldadura[2] - 1), partesfechaProcSoldadura[0]);
        var datefechaProcSoldadura = new Date(fechaProcSoldadura);

        if (fechaReporteArmado != null) {
            var partesfechaReporteArmado = fechaReporteArmado.split("/");
            var datefechaReporteArmado = new Date(partesfechaReporteArmado[2], (partesfechaReporteArmado[1] - 1), partesfechaReporteArmado[0]);
        }
    }

    if (datefechaReporteSoldadura < datefechaProcSoldadura) {
        alert(Sam.MensajesUI(41));

        return false;
    }

    if ((fechaReporteArmado != "") && (datefechaReporteArmado > datefechaReporteSoldadura)) {
        esValido = confirm(Sam.MensajesUI(52));
    }

    return esValido;
}

Sam.Workstatus.ValidaFechasReporteSoldadura = function (fechaSoldadura, fechaReporte) {
    var fechaReporteSoldadura = fechaReporte.value;
    var fechaProcSoldadura = fechaSoldadura.value;

    var partesfechaReporteSoldadura;

    if (fechaReporteSoldadura.indexOf("/") == -1) {
        partesfechaReporteSoldadura = fechaReporteSoldadura.split("-");
    }
    else {
        partesfechaReporteSoldadura = fechaReporteSoldadura.split("/");
    }

    var datefechaReporteSoldadura = new Date(partesfechaReporteSoldadura[0], (partesfechaReporteSoldadura[1] - 1), partesfechaReporteSoldadura[2]);

    var partesfechaProcSoldadura;

    if (fechaProcSoldadura.indexOf("/") == -1) {
        partesfechaProcSoldadura = fechaProcSoldadura.split("-");
    }
    else {
        partesfechaProcSoldadura = fechaProcSoldadura.split("/");
    }

    var datefechaProcSoldadura = new Date(partesfechaProcSoldadura[0], (partesfechaProcSoldadura[1] - 1), partesfechaProcSoldadura[2]);


    if (datefechaReporteSoldadura < datefechaProcSoldadura) {
        alert(Sam.MensajesUI(41));

        return false;
    }
    else {
        return true;
    }

}

Sam.Workstatus.ValidaFechaReporteArmado = function (fechaRepSoldadura, fechaArmado, fechaReporte) {

    var esValido = true;

    var fechaReporteArmado = fechaReporte.value;
    var fechaProcArmado = fechaArmado.value;

    var partesfechaReporteArmado;

    if (fechaReporteArmado.indexOf("/") == -1) {
        partesfechaReporteArmado = fechaReporteArmado.split("-");
    }
    else {
        partesfechaReporteArmado = fechaReporteArmado.split("/");
    }

    var datefechaReporteArmado = new Date(partesfechaReporteArmado[0], (partesfechaReporteArmado[1] - 1), partesfechaReporteArmado[2]);

    var partesfechaProcArmado;

    if (fechaProcArmado.indexOf("/") == -1) {
        partesfechaProcArmado = fechaProcArmado.split("-");
    }
    else {
        partesfechaProcArmado = fechaProcArmado.split("/");
    }

    var datefechaProcArmado = new Date(partesfechaProcArmado[0], (partesfechaProcArmado[1] - 1), partesfechaProcArmado[2]);

    var partesfechaReporteSoldadura;
    var datefechaReporteSoldadura;

    if (fechaRepSoldadura != "1") {
        if (fechaRepSoldadura.indexOf("/") == -1) {
            partesfechaReporteSoldadura = fechaRepSoldadura.split("-");
        }
        else {
            partesfechaReporteSoldadura = fechaRepSoldadura.split("/");
        }

        datefechaReporteSoldadura = new Date(partesfechaReporteSoldadura[0], (partesfechaReporteSoldadura[1] - 1), partesfechaReporteSoldadura[2]);
    }

    if (datefechaReporteArmado < datefechaProcArmado) {
        alert(Sam.MensajesUI(41));

        return false;
    }

    if ((fechaRepSoldadura != "1") && (datefechaReporteSoldadura < datefechaReporteArmado)) {
        esValido = confirm(Sam.MensajesUI(51));
    }

    return esValido;
}

Sam.Workstatus.ValidaFechaArmado = function (fechaSoldadura) {

    var fechaArmado = $("[id*=ctl00_cphBody_mdpFechaArmado_dateInput_text]").val();
    var fechaReporteArmado = $("[id*=ctl00_cphBody_mdpFechaReporte_dateInput_text]").val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var datefechaArmado = new Date(fechaArmado);
        var datefechaReporteArmado = new Date(fechaReporteArmado);
    }
    else {
        var partesfechaArmado = fechaArmado.split("/");
        var partesfechaReporteArmado = fechaReporteArmado.split("/");

        var datefechaArmado = new Date(partesfechaArmado[2], (partesfechaArmado[1] - 1), partesfechaArmado[0]);
        var datefechaReporteArmado = new Date(partesfechaReporteArmado[2], (partesfechaReporteArmado[1] - 1), partesfechaReporteArmado[0]);
    }

    if (datefechaReporteArmado < datefechaArmado) {
        alert(Sam.MensajesUI(41));
        return false;
    }
    if (fechaSoldadura != "") {
        var fechaSoldaduraArray = fechaSoldadura.split(',');
        var dateSoldadura = new Date(fechaSoldaduraArray[0]);
        var dateReporteSoldadura = new Date(fechaSoldaduraArray[1]);

        if (dateSoldadura < datefechaArmado || dateReporteSoldadura < datefechaReporteArmado) {
            alert(Sam.MensajesUI(40));
            return false;
        }

    }
}

Sam.Workstatus.CambioFechas = function (idVentana, fechaArmado, proceso) {
    $("[id*=hdnCambiaFechas]").val("0");

    if (proceso == 0) {
        var fechaProceso = $("[id*=ctl00_cphBody_ctrlInfo_mdpFechaSoldadura_dateInput_text]").val();
        var fechaReporte = $("[id*=ctl00_cphBody_ctrlInfo_mdpFechaReporte_dateInput_text]").val();
    }
    else if (proceso == 1) {
        var fechaProceso = $("[id*=ctl00_cphBody_mdpFechaSoldadura_dateInput_text]").val();
        var fechaReporte = $("[id*=ctl00_cphBody_mdpFechaReporte_dateInput_text]").val();
    }
    else if (proceso == 2) {
        var fechaProceso = $("[id*=ctl00_cphBody_mdpFechaInspeccionVisual_dateInput_text]").val();
        var fechaReporte = $("[id*=ctl00_cphBody_mdpFechaReporte_dateInput_text]").val();
    }
    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var dateProceso = new Date(fechaProceso);
        var dateReporteProceso = new Date(fechaReporte);
    }
    else {
        var partesFechaProceso = fechaProceso.split("/");
        var dateProceso = new Date(partesFechaProceso[2], (partesFechaProceso[1] - 1), partesFechaProceso[0]);

        var partesFechaReporte = fechaReporte.split("/");
        var dateReporteProceso = new Date(partesFechaReporte[2], (partesFechaReporte[1] - 1), partesFechaReporte[0]);
    }

    if ((dateReporteProceso < dateProceso)) {
        alert(Sam.MensajesUI(41));
        return false;
    }

    var fechasArrayArmado = fechaArmado.split(',');

    var dateArmado = new Date(fechasArrayArmado[0]);

    var dateReporteArmado = new Date(fechasArrayArmado[1]);

    if ((dateArmado > dateProceso) || (dateReporteArmado > dateReporteProceso)) {
        if (proceso == 2) {
            var msg = Sam.MensajesUI(44);
        }
        else {
            var msg = Sam.MensajesUI(32);
        }
        var answer = confirm(msg);
        if (answer) {
            var wnd = $find(idVentana);
            if (proceso == 2) {
                wnd.set_title(Sam.MensajesUI(43));
            }
            else {
                wnd.set_title(Sam.MensajesUI(33));
            }
            $("[id*=hdnCambiaFechas]").val("1");

            var parteArrayArmado = fechasArrayArmado[0].split("/");

            $("[id*=lblFechaProcesoAnterior]").text(parteArrayArmado[1] + "/" + (parteArrayArmado[0]) + "/" + parteArrayArmado[2]);

            var parteArrayArmado = fechasArrayArmado[1].split("/");

            $("[id*=lblFechaReporteProcesoAnterior]").text(parteArrayArmado[1] + "/" + (parteArrayArmado[0]) + "/" + parteArrayArmado[2]);

            wnd.setSize(690, 195);
            wnd.moveTo(Sam.Workstatus.X + 100, Sam.Workstatus.Y + 100);
            wnd.show();
        }
        else {
            return false;
        }
    }
    else {
        //Dar click en el botón del popUp para hacer el proceso del save
        $("[id*=hdnCambiaFechas]").val("0");
        $("[id*=btnGuardarPopUp]").click();
    }
    return false;
}

Sam.Workstatus.ValidaNuevasFechas = function () {

    var fechaProcesoAnterior = $("[id*=ctl00_cphBody_rdwCambiarFechaArmado_C_mdpFechaArmado_dateInput_text]").val();
    var fechaReporteProcesoAnterior = $("[id*=ctl00_cphBody_rdwCambiarFechaArmado_C_mdpFechaReporteArmado_dateInput_text]").val();

    var fechaProcesoActual = $("[id*=ctl00_cphBody_ctrlInfo_mdpFechaSoldadura_dateInput_text]").val();
    var fechaReporteProcesoActual = $("[id*=ctl00_cphBody_ctrlInfo_mdpFechaReporte_dateInput_text]").val();

    if (fechaProcesoAnterior != "" && fechaReporteProcesoAnterior != "" && fechaProcesoActual != "" && fechaReporteProcesoActual != "") {
        if ($("[id*=hdnCambiaFechas]").val() == 1) {
            var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

            if (valorCookie == "en-US") {
                var dateProcesoAnterior = new Date(fechaProcesoAnterior);
                var dateReporteProcesoAnterior = new Date(fechaReporteProcesoAnterior);

                var dateProcesoActual = new Date(fechaProcesoActual);
                var dateReporteProcesoActual = new Date(fechaReporteProcesoActual);
            }
            else {
                var partesFechaProcesoAnterior = fechaProcesoAnterior.split("/");
                var partesFechaReporteProcesoAnterior = fechaReporteProcesoAnterior.split("/")

                var dateProcesoAnterior = new Date(partesFechaProcesoAnterior[2], (partesFechaProcesoAnterior[1] - 1), partesFechaProcesoAnterior[0]);
                var dateReporteProcesoAnterior = new Date(partesFechaReporteProcesoAnterior[2], (partesFechaReporteProcesoAnterior[1] - 1), partesFechaReporteProcesoAnterior[0]);

                var partesFechaProcesoActual = fechaProcesoActual.split("/");
                var partesFechaReporteProcesoActual = fechaReporteProcesoActual.split("/")

                var dateProcesoActual = new Date(partesFechaProcesoActual[2], (partesFechaProcesoActual[1] - 1), partesFechaProcesoActual[0]);
                var dateReporteProcesoActual = new Date(partesFechaReporteProcesoActual[2], (partesFechaReporteProcesoActual[1] - 1), partesFechaReporteProcesoActual[0]);
            }

            if (dateReporteProcesoAnterior < dateProcesoAnterior) {
                alert(Sam.MensajesUI(41));
                return false;
            }

            if (dateProcesoAnterior > dateProcesoActual || dateReporteProcesoAnterior > dateReporteProcesoActual) {
                alert(Sam.MensajesUI(37));
                return false;
            }
        }
    }
    else {
        alert(Sam.MensajesUI(42));
        return false;
    }
}

Sam.Workstatus.ValidacionFechasLiberacionVisualPatio = function (juntas, fechasReporte) {

    // fecha de reporte nuevo
    var inputfecha = $("[id=ctl00_cphBody_dtpFechaReporte]").val();

    // fecha de reporte existente
    if (!inputfecha) {
        inputfecha = $("[id=cphBody_hfdtpFechaReporte]").val();
    }

    if (!inputfecha) {
        return true;
    }

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    var partesFecha = (inputfecha.indexOf("/") == -1) ? inputfecha.split("-") : inputfecha.split("/");
    var fechaReporteIV;
    if (partesFecha[0].length > 2) {

        fechaReporteIV = new Date(partesFecha[0], (partesFecha[1] - 1), partesFecha[2]);
    }
    else {
        fechaReporteIV = new Date(partesFecha[2], (partesFecha[1] - 1), partesFecha[0]);
    }

    var fechasReportesArray = fechasReporte.split(",");
    var juntasArray = juntas.split(".");
    var juntasReporte = "";

    var countReport = 0;

    for (var i = 0; i < fechasReportesArray.length; i++) {
        var fechaReporteIndice = fechasReportesArray[i];
        //fechaReporteIndice = fechaReporteIndice.slice(0, fechaReporteIndice.length - 1);

        var fechaReporteSoldadura;

        if (valorCookie == "en-US") {
            var fechaReporteSoldadura = new Date(fechaReporteIndice);
        }
        else {
            var partesFechaReporteSoldadura = (fechaReporteIndice.indexOf("/") == -1) ? fechaReporteIndice.split("-") : fechaReporteIndice.split("/");
            if (partesFechaReporteSoldadura[0].length > 2) {

                fechaReporteSoldadura = new Date(partesFechaReporteSoldadura[0], (partesFechaReporteSoldadura[1] - 1), partesFechaReporteSoldadura[2]);
            }
            else {
                fechaReporteSoldadura = new Date(partesFechaReporteSoldadura[2], (partesFechaReporteSoldadura[1] - 1), partesFechaReporteSoldadura[0]);
            }
        }

        if (fechaReporteIV < fechaReporteSoldadura) {
            juntasReporte += "\n" + juntasArray[i] + ".";
            countReport++;
        }
    }

    if (countReport > 0) {
        return confirm(Sam.MensajesUI(54, juntasReporte));
    }
    else {
        return true;
    }
}

Sam.Workstatus.ValidaFechasLiberacionDimensionalPatio = function (fechaReporteLD, fechasReportes) {

    if (fechasReportes) {
        var inputFechaReporte = "" + fechaReporteLD.value;
        var partesFechaReporteLD;

        if (inputFechaReporte.indexOf("/") == -1) {
            partesFechaReporte = inputFechaReporte.split("-");
        }
        else {
            partesFechaReporte = inputFechaReporte.split("/");
        }

        var fechaReporteLDFormated = new Date(partesFechaReporte[0], (partesFechaReporte[1] - 1), partesFechaReporte[2]);

        var countReport = 0;

        for (var i = 0; i < fechasReportes.length; i++) {
            var fechaReporteIndice = fechasReportes[i];

            var partesFechaReporteSoldadura;

            if (fechaReporteIndice.indexOf("/") == -1) {
                partesFechaReporteSoldadura = fechaReporteIndice.split("-");
            }
            else {
                partesFechaReporteSoldadura = fechaReporteIndice.split("/");
            }

            var fechaReporteSoldaduraFormated = new Date(partesFechaReporteSoldadura[0], (partesFechaReporteSoldadura[1] - 1), partesFechaReporteSoldadura[2]);

            if (fechaReporteLDFormated < fechaReporteSoldaduraFormated) {
                countReport++;
            }
        }

        if (countReport > 0) {
            return confirm(Sam.MensajesUI(58));
        }
        else {
            return true;
        }
    }
    else {
        // si no hay registros de reporte no valida fechas
        return true;
    }
}

Sam.Workstatus.ValidaFechasInspeccionDimensional = function (numerosControl, fechaProceso, fechaReporte, fechas, fechasReportes) {

    var inputFecha = fechaProceso.value;
    var inputFechaReporte = fechaReporte.value;
    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");
    var fechaLD;
    var fechaReporteLD;

    var partesFecha = (inputFecha.indexOf("/") == -1) ? inputFecha.split("-") : inputFecha.split("/");

    fechaLD = new Date(partesFecha[0], (partesFecha[1] - 1), partesFecha[2]);

    var partesFechaReporte = (inputFechaReporte.indexOf("/") == -1) ? inputFechaReporte.split("-") : inputFechaReporte.split("/");

    fechaReporteLD = new Date(partesFechaReporte[0], (partesFechaReporte[1] - 1), partesFechaReporte[2]);

    if (fechaReporteLD < fechaLD) {
        alert(Sam.MensajesUI(41));
        return false;
    }

    var fechasArray = fechas.split(",");
    var fechasReportesArray = fechasReportes.split(",");
    var ncArray = numerosControl.split(",");

    var countProcess = 0;
    var countReport = 0;

    var juntasProceso = "";
    var juntasReporte = "";

    for (var i = 0; i < fechasArray.length; i++) {
        var fechaIndice = fechasArray[i];
        var fechaReporteIndice = fechasReportesArray[i];

        if (fechaIndice === "-1" || fechaReporteIndice === "-1") {
            continue;
        }

        var fechaProcSoldadura;
        var fechaReporteSoldadura;

        if (valorCookie == "en-US") {
            fechaProcSoldadura = new Date(fechaIndice);
            fechaReporteSoldadura = new Date(fechaReporteIndice);
        }
        else {
            var partesFechaSoldadura = (fechaIndice.indexOf("/") == -1) ? fechaIndice.split("-") : fechaIndice.split("/");
            fechaProcSoldadura = new Date(partesFechaSoldadura[2], (partesFechaSoldadura[1] - 1), partesFechaSoldadura[0]);

            var partesFechaReporteSoldadura = (fechaReporteIndice.indexOf("/") == -1) ? fechaReporteIndice.split("-") : fechaReporteIndice.split("/");
            fechaReporteSoldadura = new Date(partesFechaReporteSoldadura[2], (partesFechaReporteSoldadura[1] - 1), partesFechaReporteSoldadura[0]);
        }

        if (fechaLD < fechaProcSoldadura) {
            juntasProceso += "\n" + ncArray[i];
            countProcess++;
        }

        if (fechaReporteLD < fechaReporteSoldadura) {
            juntasReporte += "\n" + ncArray[i] + " " + fechaReporteSoldadura.toLocaleDateString();
            countReport++;
        }
    }
    if (countProcess > 0) {
        alert(Sam.MensajesUI(55, juntasProceso));
        return false;
    }

    if (countReport > 0) {
        return confirm(Sam.MensajesUI(56, juntasReporte));
    }
    else {
        return true;
    }
}

Sam.Workstatus.ValidacionFechasRequisicionSpool = function (numerosControl, fechas) {

    var inputfecha = $("[id*=ctl00_cphBody_rdpFechaRequisicion_dateInput_text]").val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");
    if (valorCookie == "en-US") {
        var fechaRequisicion = new Date(inputfecha);
    }
    else {
        var partesFecha = inputfecha.split("/");
        var fechaRequisicion = new Date(partesFecha[2], (partesFecha[0] - 1), partesFecha[1]);
    }

    var fechasArray = fechas.split(",");
    var numerosControlArray = numerosControl.split(",");

    var countProcess = 0;
    var numerosControlProceso = "";

    for (var i = 0; i < fechasArray.length; i++) {
        var fechaIndice = fechasArray[i];

        if (valorCookie == "en-US") {
            var fechaLD = new Date(fechaIndice);
        }
        else {
            var partesFechaLD = fechaIndice.split("/");
            var fechaLD = new Date(partesFechaLD[2], (partesFechaLD[1] - 1), partesFechaLD[0]);
        }

        if (fechaRequisicion < fechaLD) {
            numerosControlProceso += "\n" + numerosControlArray[i];
            countProcess++;
        }
    }

    if (countProcess > 0) {
        alert(Sam.MensajesUI(59, numerosControlProceso));
        return false;
    }

    return true;
}


Sam.Workstatus.ValidacionFechasRequisicion = function (juntas, fechas) {

    var inputfecha = $("[id*=ctl00_cphBody_rdpFechaRequisicion_dateInput_text]").val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var fechaRequisicion = new Date(inputfecha);
    }
    else {
        var partesFecha = inputfecha.split("/");
        var fechaRequisicion = new Date(partesFecha[2], (partesFecha[1] - 1), partesFecha[0]);
    }

    var fechasArray = fechas.split(",");
    var juntasArray = juntas.split(".");

    var countProcess = 0;
    var juntasProceso = "";

    for (var i = 0; i < fechasArray.length; i++) {
        var fechaIndice = fechasArray[i];

        var fechaLenght = fechaIndice.length;
        fechaIndice = fechaIndice.slice(0, fechaIndice.length - 1);

        if (valorCookie == "en-US") {
            var fechaSoldadura = new Date(fechaIndice);
        }
        else {
            var partesFechaSoldadura = fechaIndice.split("/");
            var fechaSoldadura = new Date(partesFechaSoldadura[2], (partesFechaSoldadura[1] - 1), partesFechaSoldadura[0]);
        }

        if (fechaRequisicion < fechaSoldadura) {
            juntasProceso += "\n" + juntasArray[i];
            countProcess++;
        }
    }

    if (countProcess > 0) {
        alert(Sam.MensajesUI(53, juntasProceso));
        return false;
    }

    return true;
}

Sam.Workstatus.ValidaFechasInspeccionVisual = function (numerosControl, juntas, fechas, fechasReportes, fechaInspeccionId, fechaReporteId) {
    //debugger;

    var inputFechaInspeccion = fechaInspeccionId.value;
    var inputFechaReporte = fechaReporteId.value;

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    var partesFechaInspeccion = (inputFechaInspeccion.indexOf("/") == -1) ? inputFechaInspeccion.split("-") : inputFechaInspeccion.split("/");
    var fechaInspeccionFormated = new Date(partesFechaInspeccion[0], (partesFechaInspeccion[1] - 1), partesFechaInspeccion[2]);
    //var fechaInspeccionFormated = new Date(inputFechaInspeccion);

    var partesFechaReporte = (inputFechaReporte.indexOf("/") == -1) ? inputFechaReporte.split("-") : inputFechaReporte.split("/");
    var fechaReporteFormated = new Date(partesFechaReporte[0], (partesFechaReporte[1] - 1), partesFechaReporte[2]);

    //var fechaReporteFormated = new Date(inputFechaReporte);

    if (fechaReporteFormated < fechaInspeccionFormated) {
        alert(Sam.MensajesUI(41));
        return false;
    }

    var countProcess = 0;
    var countReport = 0;

    var juntasProceso = "";
    var juntasReporte = "";

    if (fechas) {
        for (var i = 0; i < fechas.length; i++) {
            if (fechas[i].length == 10) {
                var fechaSoldaduraIndice = fechas[i];
                var fechaReporteSoldaduraIndice = fechasReportes[i];

                var partesFechaSoldadura = (fechaSoldaduraIndice.indexOf("/") == -1) ? fechaSoldaduraIndice.split("-") : fechaSoldaduraIndice.split("/");
                var fechaSoldaduraFormated = new Date(partesFechaSoldadura[2], (partesFechaSoldadura[0] - 1), partesFechaSoldadura[1]);

                var partesFechaReporteSoldadura = (fechaReporteSoldaduraIndice.indexOf("/") == -1) ? fechaReporteSoldaduraIndice.split("-") : fechaReporteSoldaduraIndice.split("/");

                var fechaReporteSoldaduraFormated;

                if (valorCookie == "en-US") {
                    fechaReporteSoldaduraFormated = new Date(partesFechaReporteSoldadura[2], (partesFechaReporteSoldadura[0] - 1), partesFechaReporteSoldadura[1]);
                }
                else {
                    fechaReporteSoldaduraFormated = new Date(partesFechaReporteSoldadura[2], (partesFechaReporteSoldadura[1] - 1), partesFechaReporteSoldadura[0]);
                }

                if (fechaInspeccionFormated < fechaSoldaduraFormated) {
                    juntasProceso += "\n" + numerosControl[i] + ", " + juntas[i];
                    countProcess++;
                }

                if (fechaReporteFormated < fechaReporteSoldaduraFormated) {
                    juntasReporte += "\n" + numerosControl[i] + ", " + juntas[i];
                    countReport++;
                }
            }
        }
    }

    if (countProcess > 0) {
        alert(Sam.MensajesUI(53, juntasProceso));
        return false;
    }

    if (countReport > 0) {
        return confirm(Sam.MensajesUI(54, juntasReporte));
    }
    else {
        return true;
    }
}

Sam.Workstatus.ValidaFechasInspeccionVisualSinSoldadura = function (fechaInspeccionId, fechaReporteId) {
    //debugger;
    var inputFechaInspeccion = fechaInspeccionId.value;
    var inputFechaReporte = fechaReporteId.value;

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    var partesFechaInspeccion;

    if (inputFechaInspeccion.indexOf("/") == -1) {
        partesFechaInspeccion = inputFechaInspeccion.split("-");
    }
    else {
        partesFechaInspeccion = inputFechaInspeccion.split("/");
    }

    var fechaInspeccionFormated = new Date()
    fechaInspeccionFormated.setFullYear(partesFechaInspeccion[0], (partesFechaInspeccion[1] - 1), partesFechaInspeccion[2]);

    var partesFechaReporte;

    if (inputFechaReporte.indexOf("/") == -1) {
        partesFechaReporte = inputFechaReporte.split("-");
    }
    else {
        partesFechaReporte = inputFechaReporte.split("/");
    }

    var fechaReporteFormated = new Date();
    fechaReporteFormated.setFullYear(partesFechaReporte[0], (partesFechaReporte[1] - 1), partesFechaReporte[2]);

    if (fechaReporteFormated < fechaInspeccionFormated) {
        alert(Sam.MensajesUI(41));
        return false;
    }
    else {
        return true;
    }
}

Sam.Workstatus.ValidacionFechasReqReporteSpool = function (numerosControl, fechas) {
    var inputfechaProceso = $("[id*=ctl00_cphBody_rdpFechaPrueba_dateInput_text]").val();
    var inputfechaReporte = $("[id*=ctl00_cphBody_rdpFechaReporte_dateInput_text]").val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");
    if (valorCookie == "en-US") {
        var fechaProceso = new Date(inputfechaProceso);
        var fechaReporte = new Date(inputfechaReporte);
    }
    else {
        var partesFecha = inputfechaProceso.split("/");
        var fechaProceso = new Date(partesFecha[2], (partesFecha[1] - 1), partesFecha[0]);

        var partesFechaReporte = inputfechaReporte.split("/");
        var fechaReporte = new Date(partesFechaReporte[2], (partesFechaReporte[1] - 1), partesFechaReporte[0]);
    }

    if (fechaReporte < fechaProceso) {
        alert(Sam.MensajesUI(41));
        return false;
    }

    var fechasReqArray = fechas.split(",");
    var ncArray = numerosControl.split(",");
    ////debugger;;
    var count = 0;

    var numerosControl = "";
    for (var i = 0; i < fechasReqArray.length; i++) {
        var fechaReq = new Date(fechasReqArray[i]);

        if (fechaProceso < fechaReq) {
            numerosControl += "\n" + ncArray[i];
            count++;
        }
    }

    if (count > 0) {
        alert(Sam.MensajesUI(60, numerosControl));
        return false;
    }

    return true;
}

Sam.Workstatus.ValidacionFechas = function (numerosControl, juntas, fechas, fechasReporte, proceso, procesoAnterior) {
    if (proceso == 1) {
        var inputfecha = $("[id*=ctl00_cphBody_rdpFechaInspeccion_dateInput_text]").val();
    }
    else if (proceso == 2) {
        var inputfecha = $("[id*=ctl00_cphBody_rdpFechaLiberacion_dateInput_text]").val();
    }
    else if (proceso == 3) {
        var inputfecha = $("[id*=ctl00_cphBody_rdpFechaPrueba_dateInput_text]").val();
    }

    var inputfechaReporte = $("[id*=ctl00_cphBody_rdpFechaReporte_dateInput_text]").val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var fechaIV = new Date(inputfecha);
        var fechaReporteIV = new Date(inputfechaReporte);
    }
    else {
        var partesFecha = inputfecha.split("/");
        var fechaIV = new Date(partesFecha[2], (partesFecha[1] - 1), partesFecha[0]);

        var partesFechaReporte = inputfechaReporte.split("/");
        var fechaReporteIV = new Date(partesFechaReporte[2], (partesFechaReporte[1] - 1), partesFechaReporte[0]);
    }

    if (fechaReporteIV < fechaIV) {
        alert(Sam.MensajesUI(41));
        return false;
    }

    var fechasArray = fechas.split(",");
    var fechasReportesArray = fechasReporte.split(",");
    var juntasArray = juntas.split(",");
    var ncArray = numerosControl.split(",");

    var count = 0;

    var juntas = "";
    for (var i = 0; i < fechasArray.length; i++) {
        var fechaIndice = fechasArray[i];
        var fechaReporteIndice = fechasReportesArray[i];
        if (proceso == 1) {
            var fechaLenght = fechaIndice.length;
            var tieneProcesoPosterior = fechaIndice.slice(fechaIndice.length - 1, fechaIndice.length);
            fechaIndice = fechaIndice.slice(0, fechaIndice.length - 1);
            fechaReporteIndice = fechaReporteIndice.slice(0, fechaReporteIndice.length - 1);
        }
        var fecha = new Date(fechaIndice);
        var fechaReportes = new Date(fechaReporteIndice);
        if (tieneProcesoPosterior == 0) {
            if (fecha < fechaIV || fechaReportes < fechaReporteIV) {
                if (proceso == 2) {
                    juntas += juntasArray[i] + ",";
                }
                else {
                    juntas += "\n" + ncArray[i] + ", " + juntasArray[i];
                }
                count++;
            }
        }
        else {
            if (fechaIV < fecha || fechaReporteIV < fechaReportes) {
                if (proceso == 2) {
                    juntas += juntasArray[i] + ",";
                }
                else {
                    juntas += "\n" + ncArray[i] + ", " + juntasArray[i];
                }
                count++;
            }
        }
    }

    if (count == fechasArray.length) {
        alert(Sam.MensajesUI(36, procesoAnterior));
        return false;
    }
    else if (juntas != "") {
        if (proceso == 1 || proceso == 3) {
            return Sam.Confirma(34, juntas);
        }
        else if (proceso == 2) {
            var strLenght = juntas.length;
            juntas = juntas.slice(0, strLenght - 1);
            return Sam.Confirma(35, juntas);
        }
    }
}


Sam.Workstatus.ValidacionFechasEmbarque = function (juntas, fechasDimension, fechasPreparacion, fechaProceso) {

    var proceso = 'Preparacion';
    var inputFecha = $('#ctl00_cphBody_mdpFechaEmbarque_dateInput').val();

    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");
    var fechaEmbarque;

    if (valorCookie == "en-US") {
        fechaEmbarque = new Date(inputFecha);
        proceso = 'Preparation';
    }
    else {
        var partesFecha = inputFecha.split("/");
        fechaEmbarque = new Date(partesFecha[2], (partesFecha[1] - 1), partesFecha[0]);
    }

    var fechasDimensionArray = fechasDimension.split(",");
    var fechasPreparacionArray = fechasPreparacion.split(",");
    var juntasArray = juntas.split(",");
    var countD = 0;
    var countP = 0;
    var juntas = "";

    for (var i = 0; i < fechasDimensionArray.length; i++) {
        var fechaIndice = fechasDimensionArray[i];
        var fecha = new Date(fechaIndice);

        if (fechaEmbarque < fecha) {
            juntas += "\n" + juntasArray[i] + ", ";
            countD++;
        }
    }

    for (var i = 0; i < fechasPreparacionArray.length; i++) {
        var fechaIndice = fechasPreparacionArray[i];
        var fecha = new Date(fechaIndice);

        if (fechaEmbarque < fecha) {
            juntas += "\n" + juntasArray[i] + ", ";
            countP++;
        }
    }

    if (countP > 0) {
        alert(Sam.MensajesUI(55, proceso));
        return false;
    } else if (countD > 0) {
        alert(Sam.MensajesUI(55, procesoAnterior));
        return false;
    }
    else {
        return true;
    }
}

Sam.Workstatus.ValidacionFechasPruebas = function (juntas, fechas, proceso, procesoAnterior) {
    ////debugger;;
    //var inputfecha = $("[id*=ctl00_cphBody_mdpFechaEmbarque_dateInput_text]").val();
    //var temp2 = $('#ctl00_cphBody_mdpFechaEmbarque_dateInput');
    ////debugger;;

    if (proceso == 1) {
        var inputfecha = $("[id*=ctl00_cphBody_rdpFechaRequisicion_dateInput_text]").val();
    }
    else if (proceso == 2) {
        var inputfecha = $("[id*=ctl00_cphBody_mdpFechaReq_dateInput_text]").val();
    }
    else if (proceso == 3) {
        //var inputfecha = $("[id*=ctl00_cphBody_mdpFechaEmbarque_dateInput_text]").val();
        var inputfecha = $('#ctl00_cphBody_mdpFechaEmbarque_dateInput').val();
        ////debugger;;
    }
    else if (proceso == 4) {
        var inputfecha = $("[id*=ctl00_cphBody_dtpFechaReporte_dateInput_text]").val();
    }
    else if (proceso == 5) {
        var inputfecha = $("[id*=ctl00_cphBody_dtpFechaReporte_dateInput_text]").val();
    }
    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var fecharequisicion = new Date(inputfecha);
    }
    else {
        var partesFecha = inputfecha.split("/");
        var fecharequisicion = new Date(partesFecha[2], (partesFecha[1] - 1), partesFecha[0]);
    }
    var fechasArray = fechas.split(",");
    if (proceso == 4 || proceso == 1) {
        var juntasArray = juntas.split(".");
    }
    else {
        var juntasArray = juntas.split(",");
    }

    var count = 0;

    var juntas = "";
    for (var i = 0; i < fechasArray.length; i++) {
        var fechaIndice = fechasArray[i];
        if (proceso == 4) {
            var fechaLenght = fechaIndice.length;
            var tieneProcesoPosterior = fechaIndice.slice(fechaIndice.length - 1, fechaIndice.length);
            fechaIndice = fechaIndice.slice(0, fechaIndice.length - 1);
        }
        var fecha = new Date(fechaIndice);
        if (tieneProcesoPosterior == 0) {
            if (fecha < fecharequisicion) {
                juntas += "\n" + juntasArray[i] + ", ";
                count++;
            }
        }
        else {
            if (fecharequisicion < fecha) {
                juntas += "\n" + juntasArray[i] + ", ";
                count++;
            }
        }

    }

    if (count == fechasArray.length) {
        alert(Sam.MensajesUI(36, procesoAnterior));
        return false;
    }
    else if (juntas != "") {
        if (proceso == 4) {
            return Sam.Confirma(39, juntas)
        }
        var strLenght = juntas.length;
        juntas = juntas.slice(0, strLenght - 2);
        if (proceso == 1) {
            return Sam.Confirma(34, juntas);
        }
        else if (proceso == 2 || proceso == 3 || proceso == 5) {
            return Sam.Confirma(35, juntas);
        }
    }
}

Sam.Workstatus.ValidacionFechasPintura = function (fecha) {
    if (Sam.Workstatus.CompraraFechasPintura(fecha, "ctl00_cphBody_rdpFechaSandBlast_dateInput_text")) {
        var texto = $("[id*=ctl00_cphBody_rdpFechaSandBlast_dateInput_text]").parents("#SandBlast").find("h3").html();
        alert(Sam.MensajesUI(38, texto));
        return false;
    }

    if (Sam.Workstatus.CompraraFechasPintura(fecha, "ctl00_cphBody_rdpFechaPrimario_dateInput_text")) {
        var texto = $("[id*=ctl00_cphBody_rdpFechaSandBlast_dateInput_text]").parents("#Primario").find("h3").html();
        alert(Sam.MensajesUI(38, texto));
        return false;
    }

    if (Sam.Workstatus.CompraraFechasPintura(fecha, "ctl00_cphBody_rdpFechaIntermedio_dateInput_text")) {
        var texto = $("[id*=ctl00_cphBody_rdpFechaSandBlast_dateInput_text]").parents("#Intermedio").find("h3").html();
        alert(Sam.MensajesUI(38, texto));
        return false;
    }

    if (Sam.Workstatus.CompraraFechasPintura(fecha, "ctl00_cphBody_rdpAcabadoVisual_dateInput_text")) {
        var texto = $("[id*=ctl00_cphBody_rdpFechaSandBlast_dateInput_text]").parents("#Acabado").find("h3").html();
        alert(Sam.MensajesUI(38, texto));
        return false;
    }

    if (Sam.Workstatus.CompraraFechasPintura(fecha, "ctl00_cphBody_rdpFechaAdherencia_dateInput_text")) {
        var texto = $("[id*=ctl00_cphBody_rdpFechaSandBlast_dateInput_text]").parents("#Adherencia").find("h3").html();
        alert(Sam.MensajesUI(38, texto));
        return false;
    }

    if (Sam.Workstatus.CompraraFechasPintura(fecha, "ctl00_cphBody_rdpPullOff_dateInput_text")) {
        var texto = $("[id*=ctl00_cphBody_rdpFechaSandBlast_dateInput_text]").parents("#Pull").find("h3").html();
        alert(Sam.MensajesUI(38, texto));
        return false;
    }

}

Sam.Workstatus.CompraraFechasPintura = function (fechaReq, rdpID) {
    var inputfecha = $("[id*=" + rdpID + "]").val();
    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (valorCookie == "en-US") {
        var fecha = new Date(inputfecha);
    }
    else {
        var partesfecha = inputfecha.split("/");
        var fechardp = new Date(partesfecha[2], (partesfecha[1] - 1), partesfecha[0]);

        var fecha = new Date(fechaReq);
    }
    return fechardp < fecha;
}

Sam.Workstatus.ConfirmaBorrarSistema = function () {
    return Sam.Confirma(63);
}


//////////////////////////////////////////////
//
//           USUARIOS
//
/////////////////////////////////////////////
Sam.Usuarios.AbreContextMenu = function (e, menu) {
    var contextMenu = $find(menu);
    if ((!e.relatedTarget) || (!$telerik.isDescendantOrSelf(contextMenu.get_element(), e.relatedTarget))) {
        contextMenu.show(e);
    }
    $telerik.cancelRawEvent(e);
}

Sam.Usuarios.AbrePopupCambiarPassword = function () {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Usuarios/CambiaPassword.aspx");
    Sam.Utilerias.SetSize(oWnd, 550, 380);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Usuarios.AbrePopupCambiarPregunta = function () {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Usuarios/CambiaPregunta.aspx");
    Sam.Utilerias.SetSize(oWnd, 630, 360);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Usuarios.Logout = function () {
    var linkLogout = $("#perfil_hlSalir");
    window.location = linkLogout.attr("href");
}


//////////////////////////////////////////////
//
//           UTILERIAS
//
/////////////////////////////////////////////
Sam.Utilerias.OnGridHeaderMenuShowing = function (sender, args) {
    var menu = args.get_menu();
    var item = menu.findItemByValue("Freeze");
    menu.trackChanges();
    item._attributes.setAttribute("ColumnName", args.get_gridColumn()._data.UniqueName);
    menu.commitChanges();
}


//Mantiene viva la sesión de la página
Sam.Utilerias.PingSession = function () {
    var randomnumber = Math.floor(Math.random() * 1000000);
    $.post("/Administracion/KeepAlive.aspx?Rnd=" + randomnumber);
}

Sam.Utilerias.Resize = function () {
    var nw = $(window).width() - 90;
    nw = nw < 910 ? 910 : nw;
    $("#autoWrapper").width(nw);
    $("#autoWrapper").css("overflow-x", "hidden");
    document.cookie = "Sam.Width=" + nw + ";path=/";
}

Sam.Utilerias.ColorSeleccionado = function (sender, eventArgs) {
    var color = sender.get_selectedColorTitle();
    $('#lblNombreColor').text(color);
    $('#txtColorSeleccionado').val(color);
}

Sam.Utilerias.ValidaColorRequerido = function (sender, args) {
    args.IsValid = $('#txtColorSeleccionado').val() !== "";
}

Sam.Utilerias.ObtenCookie = function (nombre) {
    var valor = "";
    var buscar = nombre + "=";
    if (document.cookie.length > 0) {
        offset = document.cookie.indexOf(buscar);
        if (offset != -1) {
            offset += buscar.length;
            end = document.cookie.indexOf(";", offset);
            if (end == -1) end = document.cookie.length;
            valor = unescape(document.cookie.substring(offset, end))
        }
    }
    return valor;
}

Sam.Utilerias.ValidacionComboTelerikTieneValor = function (sender, args) {
    args.IsValid = Sam.Utilerias.ComboTelerikTieneValorEntero(sender);
}

Sam.Utilerias.ValidacionComboTelerikTieneValorCustom = function (sender, args) {
    args.IsValid = Sam.Utilerias.ComboTelerikTieneValorCustom(sender);
}

Sam.Utilerias.ComboTelerikTieneValorEntero = function (sender) {
    var cmb = $find(sender.controltovalidate);
    var valor = cmb.get_value();
    return valor !== null && valor != "" && !isNaN(valor) && parseInt(valor, 10) > 0;
}

Sam.Utilerias.ComboTelerikTieneValorCustom = function (sender) {
    var cmb = $find(sender.controltovalidate);
    var valor = cmb.get_value();
    return valor !== null && valor != "";
}

Sam.Utilerias.MuestraOverlay = function () {
    $('#mainOverlay').fadeIn('fast', function () {
        $('#msgProcesando').fadeIn('fast');
    });
}

Sam.Utilerias.PostbackProcesando = function (causaValidacion, grupo) {
    var todoValido = true;
    if (causaValidacion === true) {
        if (grupo !== "") {
            todoValido = Page_ClientValidate(grupo);
        } else {
            todoValido = Page_ClientValidate();
        }
    }

    if (todoValido === true) {
        Sam.Utilerias.MuestraOverlay();
    }
}

Sam.Utilerias.SelectedItemsReales = function (selectedItems) {
    var newArray = new Array();
    if (selectedItems && selectedItems.length > 0) {
        $.each(selectedItems, function (i, item) {
            var input = $("[id*=SelectCheckBox]", $(item._element));
            if (input.length > 0) {
                newArray.push(item);
            }
        });
    }
    return newArray;
}


//////////////////////////////////////////////
//
//           INGENIERIA
//
/////////////////////////////////////////////

Sam.Ingenieria = {
    Validaciones: {}
};


Sam.Ingenieria.AbrePopupJuntaAdicional = function (materialSpoolID, readOnly) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Ingenieria/PopUpJuntaAdicional.aspx?ID=" + escape(materialSpoolID) + "&RO=" + escape(readOnly));
    Sam.Utilerias.SetSize(oWnd, 780, 645);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Ingenieria.Validaciones.CargaArchivos = function (sender, args) {
    var invalido = false;
    $(".ruFileInput").each(function (i, item) {
        $(".ruFileInput:not(#" + $(item).attr("id") + ")").each(function (e, item1) {
            if ($(item).val() == $(item1).val() || $(item).val() == "") {
                invalido = true;
                return;
            }
        });
        if (invalido) {
            return;
        }
    });
    args.IsValid = !invalido;
}

Sam.Ingenieria.Validaciones.ExtensionArchivos = function (sender, args) {
    var invalido = false;
    $(".ruFileInput").each(function (i, item) {
        $(".ruFileInput:not(#" + $(item).attr("id") + ")").each(function (e, item1) {
            if (!$(item).val().toLowerCase().endsWith(".csv")) {
                invalido = true;
                return;
            }
        });
        if (invalido) {
            return;
        }
    });
    args.IsValid = !invalido;
}

Sam.Ingenieria.Validaciones.Proyecto = function (sender, args) {
    args.IsValid = $("[id*=ddlProyecto]").val() != -1;
}

Sam.Ingenieria.Validaciones.Checks = function (sender, args) {
    var todos = true;
    $(".rpTemplate input[type=checkbox]").each(function (i, item) {
        if (!item.checked) {
            todos = false;
        }
    });
    args.IsValid = todos;
}

Sam.Ingenieria.Validaciones.CheckAll = function (idGrid) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    //funciona para eliminar Spool
    var grid = $find(idGrid);
    var masterTable = grid.get_masterTableView();

    var x = masterTable.get_element().getElementsByTagName("INPUT");

    return x[0].checked;
}

Sam.Ingenieria.Validaciones.FamiliasAceros = function (sender, args) {
    var todos = true;
    $("[id*=ddlFamilia]").each(function (i, item) {
        if ($(item).val() == -1 || $(item).val() == "") {
            todos = false;
        }
    });
    args.IsValid = todos;
}

Sam.Ingenieria.Validaciones.Homologacion = function (sender, args) {
    //si la mitad de los radios existentes no estan con check significa que no esta todo aun correcto
    args.IsValid = $(":input[type=radio][checked][id*=repHomologacion]").length * 2 == $(":input[type=radio][id*=repHomologacion]").length;
}

Sam.Ingenieria.Validaciones.CheckItemsSelected = function (idGrid) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var result = false;
    //funciona para eliminar Spool
    var grid = $find(idGrid);

    var selected = grid.get_masterTableView().get_selectedItems();

    if (selected.length > 0) {
        result = Sam.Confirma(61);
    }
    else {
        Sam.Alerta(07);
    }

    return result;
}

Sam.Ingenieria.HomologacionValidaOperacion = function (operacion) {



    var spoolId = $("#hidSpoolId").val();
    var spoolPendienteId = $("#hidSpoolPendienteId").val();

    var seleccion1Correcta = $("#bdMat .homologacionSelected :input:first")[0] != undefined;
    var seleccion2Correcta = $("#archivoMat .homologacionSelected :input:first")[0] != undefined;

    var etiqueta1 = "";
    var etiqueta2 = "";

    if (seleccion1Correcta) {
        etiqueta1 = $("#bdMat .homologacionSelected :input:first").val();
    }

    if (seleccion2Correcta) {
        etiqueta2 = $("#archivoMat .homologacionSelected :input:first").val();
    }


    switch (operacion) {
        case 0: //iguales
            if (!seleccion1Correcta || !seleccion2Correcta) {
                alert(Sam.MensajesUI(26));
                return false;
            }
            operacion = "iguales";
            break;
        case 1: //nuevo
            if ((seleccion1Correcta && seleccion2Correcta) || (!seleccion1Correcta && !seleccion2Correcta)) {
                alert(Sam.MensajesUI(25));
                return false;
            }
            if (seleccion1Correcta) {
                alert(Sam.MensajesUI(27));
                return false;
            }
            operacion = "nuevo";
            break;
        case 2: //eliminar
            if ((seleccion1Correcta && seleccion2Correcta) || (!seleccion1Correcta && !seleccion2Correcta)) {
                alert(Sam.MensajesUI(25));
                return false;
            }
            if (seleccion2Correcta) {
                alert(Sam.MensajesUI(28));
                return false;
            }
            operacion = "eliminar";
            break;
        case 3: //similares
            if (!seleccion1Correcta || !seleccion2Correcta) {
                alert(Sam.MensajesUI(26));
                return false;
            }
            operacion = "similares";
            break;
    }


    $("#hidMaterialSpoolPendienteId")[0].value = etiqueta2;
    $("#hidMaterialSpoolId")[0].value = etiqueta1;
    $("#hidAccion")[0].value = operacion;

    Sam.Utilerias.PostbackProcesando(false, '');
    return true;
}

Sam.Ingenieria.ValidaHomologacionCompleta = function () {

    var valido = $("#bdMat tr.simplehighlight").length == 0 && $("#archivoMat tr.simplehighlight").length == 0;
    if (valido) {
        Sam.Utilerias.PostbackProcesando(false, '');
    }
    return valido;
}


Sam.Ingenieria.AbrePopupDetalleHold = function (idSpool, tipoHold) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Ingenieria/PopUpDetHold.aspx?ID=" + escape(idSpool) + "&TH=" + escape(tipoHold));
    Sam.Utilerias.SetSize(oWnd, 700, 450);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Ingenieria.AbrePopupFijarPrioridad = function (wndID, idGrid) {
    var grid = $find(idGrid);
    var selected = grid.get_masterTableView().get_selectedItems();

    if (selected.length > 0) {
        var wnd = $find(wndID);

        Sam.Utilerias.SetSize(wnd, 530, 300);
        //wnd.setSize(530, 300);
        wnd.show();
    }
    else {
        Sam.Alerta(07);
    }
}


Sam.Ingenieria.AbrePopupFijarPrioridadSeleccionados = function (spoolsIdsSelected, proyID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    var selected = spoolsIdsSelected;

    if (selected.length > 0) {


        var csv = selected.join(',');
        oWnd.setUrl("/Produccion/PopUpFijarPrioridadSeleccionados.aspx?IDs=" + escape(csv) + "&PID=" + proyID);
        Sam.Utilerias.SetSize(oWnd, 570, 370);
        //oWnd.setSize(850, 500);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();

    }
    else {
        Sam.Alerta(07);
    }
}

Sam.Ingenieria.AbrePopupAprobadoParaCruce = function (wndID) {
    var wnd = $find(wndID);
    Sam.Utilerias.SetSize(wnd, 530, 300);
    //wnd.setSize(530, 300);
    wnd.show();
}


Sam.Ingenieria.AbrePopupDocumentoAprobado = function (spoolsIdsSelected, proyID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");

    var selected = spoolsIdsSelected;

    if (selected.length > 0) {
        var csv = spoolsIdsSelected;// idArr.join(',');
        oWnd.setUrl("/Ingenieria/PopUpFijarRevIngSeleccionados.aspx?IDs=" + spoolsIdsSelected + "&PID=" + proyID);
        Sam.Utilerias.SetSize(oWnd, 530, 370);
        //oWnd.setSize(850, 500);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    }
    else {
        Sam.Alerta(07);
    }
    //var wnd = $find(wndID);
    //Sam.Utilerias.SetSize(wnd, 530, 300);
    //wnd.setSize(530, 300);
    //wnd.show();
}



Sam.Ingenieria.AbrePopupAjustarLongitud = function (matID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Ingenieria/PopUpAjustarLongitud.aspx?ID=" + matID);
    Sam.Utilerias.SetSize(oWnd, 650, 450);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Ingenieria.HoldIngenieria = function () {
    $('#bntActualiza > input')[0].click();
}

Sam.Ingenieria.EstilizaPanelBar = function (sender, args) {
    //los elementos LI de segundo nivel excluyenedo el LI que contiene la palabra confirme y el checkbox
    $(".rpSlide .rpItem:not(.rpLast):odd").addClass("rpItemAlt");
    //los elementos LI que contienen la palabra confirme y el checkbox
    $(".rpSlide .rpLast .rpTemplate").addClass("ancho100");
    //los elementos LI de primer nivel
    $(".rpRootGroup > li").children("a").children(".rpOut").addClass("repSam repEncabezado");

}

Sam.Ingenieria.AbrePopupHomologacion = function (spoolID, proyectoID, soloLectura) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Ingenieria/PopUpHomologacion.aspx?ID=" + escape(spoolID) + "&PID=" + escape(proyectoID) + "&RO=" + soloLectura);
    Sam.Utilerias.SetSize(oWnd, 980, 580);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();

}

Sam.Ingenieria.HomologacionAcepta = function (spoolID, aceptar) {
    if (Sam.Popup) {
        Sam.Popup.VentanaPadre().Sam.Ingenieria.HomologacionAcepta(spoolID, aceptar);
        return;
    }
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.Close();
    if (aceptar) {
        $("[name$=rb" + spoolID + "][value$=Aceptar]").attr("checked", true);
    } else {
        $("[name$=rb" + spoolID + "][value$=" + spoolID + "]").attr("checked", true);
    }
}

Sam.Ingenieria.AbrePopUpDatosNoEncontrados = function () {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Ingenieria/PopUpDatosNoEncontrados.aspx");
    Sam.Utilerias.SetSize(oWnd, 790, 515);
    //oWnd.setSize(790, 515);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
    oWnd.show();
}

Sam.Ingenieria.ActualizayCierra = function () {
    $('#btnWrapper > input')[0].click();
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.close();
}

Sam.Ingenieria.Cierra = function () {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.close();
}



Sam.Ingenieria.FilaSeleccionada = function (sender, eventArgs) {
    var grid = sender;
    var MasterTable = grid.get_masterTableView();
    var selected = Sam.Utilerias.SelectedItemsReales(grid.get_masterTableView().get_selectedItems());
    var spools = document.getElementById("spSpoolsSeleccionados");
    if (spools) {
        var kgs = $("#spKgsSeleccionados")[0];
        var area = $("#spAreaSeleccionados")[0];
        var peqs = $("#spPeqsSeleccionados")[0];

        var sumaKgs = 0;
        var sumaArea = 0;
        var sumaPeqs = 0;

        kgs.innerHTML = 0;
        area.innerHTML = 0;
        peqs.innerHTML = 0;

        $.each(selected, function (key, row) {
            var kgsCell = MasterTable.getCellByColumnUniqueName(row, "Peso");
            var areaCell = MasterTable.getCellByColumnUniqueName(row, "Area");
            var peqsCell = MasterTable.getCellByColumnUniqueName(row, "Peqs");

            sumaKgs += parseFloat($(kgsCell)[0].innerHTML);
            sumaArea += parseFloat($(areaCell)[0].innerHTML);
            sumaPeqs += parseFloat($(peqsCell)[0].innerHTML);
            sumaArea.toFixed(2);
        });

        //Se asignan los valores
        spools.innerHTML = selected.length;

        sumaKgs = Sam.Ingenieria.AgregarComas(sumaKgs.toFixed(2));
        sumaArea = Sam.Ingenieria.AgregarComas(sumaArea.toFixed(2));
        sumaPeqs = Sam.Ingenieria.AgregarComas(sumaPeqs.toFixed(2));

        $("#spKgsSeleccionados").html(sumaKgs);
        $("#spAreaSeleccionados").html(sumaArea);
        $("#spPeqsSeleccionados").html(sumaPeqs);
    }
}

Sam.Ingenieria.AgregarComas = function (str) {
    var parts = (str + "").split("."),
        main = parts[0],
        len = main.length,
        output = "",
        i = len - 1;

    while (i >= 0) {
        output = main.charAt(i) + output;
        if ((len - i) % 3 === 0 && i > 0) {
            output = "," + output;
        }
        --i;
    }

    // put decimal part back
    if (parts.length > 1) {
        output += "." + parts[1];
    }

    return output;
}

Sam.Ingenieria.PeqsActualizados = function (cant) {
    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");

    if (cant > 0) {

        if (valorCookie == "en-US") {
            alert('Sucessfull Upgrade Peqs ');
        }
        else {
            alert('Los Peqs se actualizaron con éxito.');
        }
    }
    else {
        if (valorCookie == "en-US") {
            alert('No PEQs found to update.');
        }
        else {
            alert('No se encontraron Peqs para actualizar.');
        }
    }
}

//////////////////////////////////////////////
//
//           FILTRO GENERICO
//
/////////////////////////////////////////////

Sam.Filtro.DdlProyectoOnClientSelectedIndexChanged = function (sender, args) {
    Sam.WebService.LimpiaItemsRadCombo("radCmbNumeroControl");
    Sam.WebService.LimpiaItemsRadCombo("radCmbNumeroUnico");
    Sam.WebService.LimpiaItemsRadCombo("radCmbOrdenTrabajo");
}

Sam.Filtro.NumControlOnClientItemsRequestingEventHandler = function (sender, args) {
    Sam.WebService.AgregaProyectoID(sender, args);
    ////debugger;;
    var combo = $find($("div[id$=radCmbOrdenTrabajo]").attr("ID"));
    if (combo) {
        args._context.OrdenTrabajoID = combo._value;
    }
}

Sam.Filtro.CuadranteOnclientItemRequesting = function (sender, args) {
    Sam.WebService.AgregaProyectoID(sender, args);
    ////debugger;;
    var combo = $find($("div[id$=radComboCuadrante]").attr("ID"));
    if (combo) {
        args._context.CuadranteID = combo._value;
    }
}

Sam.Filtro.EmbarqueOnclientItemRequesting = function (sender, args) {
    Sam.WebService.AgregaProyectoID(sender, args);
    ////debugger;;
    var combo = $find($("div[id$=radCmbEmbarque]").attr("ID"));
    if (combo) {
        args._context.EmbarqueID = combo._value;
    }
}

Sam.Filtro.TallerOnClientItemRequesting = function (sender, args) {
    Sam.WebService.AgregaProyectoID(sender, args);
}

Sam.Filtro.OrdenTrabajoOnClientSelectedIndexChanged = function (sender, args) {
    Sam.WebService.LimpiaItemsRadCombo("radCmbNumeroControl");

}

Sam.Filtro.EmbarqueOnClientSelectedIndexChanged = function (sender, args) {
    Sam.WebService.LimpiaItemsRadCombo("radCmbEmbarque");
}


//////////////////////////////////////////////
//
//           CALIDAD
//
/////////////////////////////////////////////

Sam.Calidad.GridCreated = function () {
    if ($.browser.msie && $.browser.version.indexOf("7.0") != -1) {
        $('.RadGrid td[colspan]').attr('colspan', '1');
    }
}

Sam.Calidad.AttachHandlers = function () {
    $("#tblHeaders, #tblBody").scrollsync({ targetSelector: "#tblBody", axis: "x" });
    $("#tblCongelados, #tblBody").scrollsync({ targetSelector: "#tblBody", axis: "y" });
    $(window).resize(Sam.Calidad.AjustaAncho);
    $("[class=spHeader]").mousedown(function (e) {
        if (e.button === 2) {
            Sam.Calidad.AbreMenu(this);
        }
    });
}

Sam.Calidad.MuestraGrid = function () {
    var dvOculto = $("#ocultoInicio");
    //hack - mostrar temporalmente para poder medir
    $(dvOculto).css({ position: "absolute", visibility: "hidden", display: "block" });
    Sam.Calidad.AjustaAncho(true);
    //regresar a estado original
    $(dvOculto).css({ position: "", visibility: "", display: "block" });
}

Sam.Calidad.AjustaAncho = function () {
    var anchoCol = $("#topLeft").width();
    var anchoWrapper = $("[class=phc]").width();
    var anchoAjustado = anchoWrapper - anchoCol;
    $("#tblHeaders").width(anchoAjustado);
    $("#tblBody").width(anchoAjustado);
}

Sam.Calidad.Certificacion_DescargarReportes = function (grdID, pd) {

    var oManager = GetRadWindowManager();
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows,
            function (key, item) {
                idArr.push(item.getDataKeyValue("SpoolID"));
            });
        var csv = idArr.join(',');
        var hRefBackUp = $("#lnkImgDescargar").attr("href");
        $("#lnkDescargar,#lnkImgDescargar").attr("href", "/Calidad/LstCertificacion.aspx?ID=" + $("[id*=ddlProyecto]").val() + "&IDS=" + escape(csv) + "&PD=" + pd);
        window.location = $("#lnkImgDescargar").attr("href");
        $("#lnkDescargar,#lnkImgDescargar").attr("href", hRefBackUp);
    } else {
        Sam.Alerta(9);
    }


}

Sam.Calidad.AbrePopUpDetalleSeguimientoSpool = function (id) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Calidad/PopUpSegSpool.aspx?ID=" + id);
    Sam.Utilerias.SetSize(oWnd, 960, 590);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Calidad.AbrePopUpDetalleSeguimientoJunta = function (pid, jsid, juntaCampo) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Calidad/PopUpSegJunta.aspx?PID=" + pid + "&JSID=" + jsid + "&EsJuntaCampo=" + juntaCampo);
    Sam.Utilerias.SetSize(oWnd, 960, 590);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Calidad.ToggleLista = function (radio) {
    var rdId = radio.id;
    var lstRadios = $("#" + rdId).parent().parent();
    $(".lista input", lstRadios).each(function (i, item) {
        item.checked = radio.checked;
    });
}

Sam.Calidad.AjaxRequestEnd = function () {
    Sam.Calidad.AttachHandlers();
}

Sam.Calidad.AbreMenu = function (cntSpan) {
    var columna = $(cntSpan).attr("data");
    var clientIDMenu = $("#hdnMenuColumnasID").val();
    $("#hdnColumnaSeleccionada").val(columna);
    Sam.Calidad.AbreMenuColumna(event, clientIDMenu);
}

Sam.Calidad.AbreMenuColumna = function (e, menu) {
    var contextMenu = $find(menu);
    if ((!e.relatedTarget) || (!$telerik.isDescendantOrSelf(contextMenu.get_element(), e.relatedTarget))) {
        contextMenu.show(e);
    }
    $telerik.cancelRawEvent(e);
}

Sam.Calidad.ItemMenuClicked = function (sender, args) {
    Sam.Utilerias.MuestraOverlay();
}

//////////////////////////////////////////////
//
//           MATERIALES
//
/////////////////////////////////////////////

Sam.Materiales.AbreVentanaNumUnicos = function (idProyecto, idIC) {
    var direccion = '/Materiales/LstNumeroUnico.aspx?ProyID=' + idProyecto + '&IcID=' + idIC;

    myWindow = window.open(direccion, '_blank');
    myWindow.focus();
}

Sam.Materiales.AbrePopupAgregaNumUnicos = function (idRecepcion) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Materiales/PopupAgregaNumeroUnico.aspx?ID=" + idRecepcion);
    Sam.Utilerias.SetSize(oWnd, 600, 280);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();

}

Sam.Materiales.AbrePopupAgregaItemCodes = function (idProyecto) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Materiales/PopupAgregaItemCode.aspx?PID=" + idProyecto);
    Sam.Utilerias.SetSize(oWnd, 600, 400);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
    return false;

}

Sam.Materiales.AbrePopupAgregaColada = function (idProyecto) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Materiales/PopupAgregaColada.aspx?PID=" + idProyecto);
    Sam.Utilerias.SetSize(oWnd, 600, 400);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
    return false;
}

Sam.Materiales.AltaNumerosUnicos = function (numUnicoID, cantidad) {
    $("#hdnNumeroUnicoID").val(numUnicoID);
    $("#hdnCantidadNumUnicos").val(cantidad);
    $('#btnWrapper > input')[0].click();
}

Sam.Materiales.AbrePopUpEdicionNumUnico = function (idNumUnico) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Materiales/PopUpEdicionNumeroUnico.aspx?ID=" + idNumUnico);
    Sam.Utilerias.SetSize(oWnd, 910, 655);
    //Sam.Utilerias.SetSize(oWnd,910, 600);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Materiales.AbrePopUpEdicionNumUnicoConProyecto = function (idNumUnico, idProyecto, idTransportista) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Materiales/PopUpEdicionNumeroUnico.aspx?ID=" + idNumUnico + "&PID=" + idProyecto + "&TID=" + idTransportista);
    Sam.Utilerias.SetSize(oWnd, 910, 655);
    //Sam.Utilerias.SetSize(oWnd,910, 600);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Materiales.NumeroUnicoEditado = function (transportistaid) {
    $("select[id$='ddlTransportista']").val(transportistaid);
    $('#bntActualiza > input')[0].click();
}

Sam.Materiales.AbrePopUpInventarios = function (idNumUnico) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Materiales/PopUpMovimientoInventarios.aspx?ID=" + idNumUnico);
    Sam.Utilerias.SetSize(oWnd, 800, 600);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Materiales.AbrePopupOdtReqMaterial = function (idNumUnico, matspool) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Materiales/PopUpOdtReqMaterial.aspx?NU=" + idNumUnico + "&MS=" + escape(matspool));
    //oWnd.setUrl("/Materiales/PopUpOdtReqMaterial.aspx?NU=" + idNumUnico );
    Sam.Utilerias.SetSize(oWnd, 950, 480);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}



Sam.Materiales.AbrePopUpRequisicion = function (grdID, proyID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows,
            function (key, item) {
                idArr.push(item.getDataKeyValue("NumeroUnicoID"));
            });
        var csv = idArr.join(',');
        oWnd.setUrl("/Materiales/PopUpReqPinturaNumUnico.aspx?proyID=" + escape(proyID) + "&IDs=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 600, 400);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(9);
    }

}

Sam.Materiales.ActualizaReqPinturaNumUnico = function () {
    $('#bntActualiza > input')[0].click();
}

Sam.Materiales.ActualizaPinturaNumUnico = function () {
    $('#bntActualiza > input')[0].click();
}


Sam.Materiales.AbrePopUpRequisitarPintura = function (grdID, proyID) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    var grid = $find(grdID);
    var MasterTable = grid.get_masterTableView();
    var selectedRows = Sam.Utilerias.SelectedItemsReales(MasterTable.get_selectedItems());

    if (selectedRows.length >= 1) {
        var idArr = new Array();
        $.each(selectedRows,
            function (key, item) {
                idArr.push(item.getDataKeyValue("NumeroUnicoID"));
            });
        var csv = idArr.join(',');
        oWnd.setUrl("/Materiales/PopUpPinturaNumeroUnico.aspx?proyID=" + escape(proyID) + "&IDs=" + escape(csv));
        Sam.Utilerias.SetSize(oWnd, 750, 450);
        oWnd.set_modal(true);
        oWnd.center();
        oWnd.show();
    } else {
        Sam.Alerta(9);
    }

}

Sam.Materiales.LimpiaItems = function (sender, args) {
    $(".ToClear").each(function (i, item) {
        item.innerHTML = "";
    });
}

Sam.Materiales.AbrePopUpConfinarSpool = function (idSpool, tipoConfinado) {
    var oManager = GetRadWindowManager();
    var oWnd = oManager.getWindowByName("genericWindow");
    oWnd.setUrl("/Materiales/PopUpConfinarSpool.aspx?ID=" + escape(idSpool) + "&TH=" + escape(tipoConfinado));
    Sam.Utilerias.SetSize(oWnd, 800, 450);
    oWnd.set_modal(true);
    oWnd.center();
    oWnd.show();
}

Sam.Materiales.CambioEstatusNumeroUnico = function () {
    var puedeTransferir = $("[id*=hdnPuedeTransferir]").val();
    var estatusOriginal = $("[id*=hdnEstatusOriginal]").val();
    var valorComboBox = $("[id*=ctrlGeneral_ddlEstatus]").val()
    if (valorComboBox != "A" && valorComboBox != estatusOriginal) {
        if (puedeTransferir == "True") {
            return Sam.Confirma(31);
        }
    }
}


//////////////////////////////////////////////
//
//           PROTOTYPE FUNCTIONS
//
/////////////////////////////////////////////

String.prototype.beginsWith = function (t, i) {
    if (i == false) {
        return
        (t == this.substring(0, t.length));
    } else {
        return (t.toLowerCase()
== this.substring(0, t.length).toLowerCase());
    }
}

String.prototype.endsWith = function (t, i) {
    if (i == false) {
        return (t
== this.substring(this.length - t.length));
    } else {
        return
        (t.toLowerCase() == this.substring(this.length -
t.length).toLowerCase());
    }
}

String.prototype.format = function () {
    var s = this;
    for (var i = 0; i < arguments.length; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i]);
    }
    return s;
}

String.prototype.pad = function (char, length) {
    var str = '' + this;
    while (str.length < length) {
        str = char + str;
    }
    return str;
}

//////////////////////////////////////////////
//
//           ERRORES Y CONFIRMACIONES
//
//////////////////////////////////////////////

Sam.Confirma = function (numMensaje, params) {
    var msg = Sam.MensajesUI(numMensaje, params);
    return confirm(msg);
}

Sam.Alerta = function (numMensaje, params) {
    var msg = Sam.MensajesUI(numMensaje, params);
    alert(msg);
}

//////////////////////////////////////////////
//
//           RAD COMBO WEBSERVICES
//
//////////////////////////////////////////////

//Limpia los items del combo que se envia como argumento.
Sam.WebService.LimpiaItemsRadCombo = function (radCmbId) {
    var combo = $find($("div[id$=" + radCmbId + "]").attr("ID"));
    if (combo) {
        combo.clearSelection();
        combo.clearItems();
    }
}

//Agrega el ID del proyecto al contexto ya sea proveniente de un combo o de un hidden
//es importante que el combo sea llamado ddlProyecto y el hidden hdnProyectoID
Sam.WebService.AgregaProyectoID = function (sender, args) {
    var proyID = $("[id*=ddlProyecto]").val() || $("[id*=hdnProyectoID]").val();
    if (proyID) {
        args._context.ProyectoID = proyID;
    } else {
        args._context.ProyectoID = -1;
    }
}

Sam.WebService.AgregaCorteSpoolID = function (sender, args) {
    //debugger;
    var corteSpoolId = $("[id*=hdnCorteID]").val();
    if (corteSpoolId) {
        args._context.CorteSpoolID = corteSpoolId;
    }
    else {
        args._context.CorteSpoolID = -1;
    }
}

Sam.WebService.DespachoAgregaProyectoID = function (sender, args) {
    var proyID = $("[id*=hdnProyectoID]").val();
    args._context.ProyectoID = proyID;
}


Sam.WebService.AgregaDatosParaSeguimientoJuntasEnRT = function (sender, args) {
    var proyID = $("[id*=ddlProyecto]").val() || $("[id*=hdnProyectoID]").val();
    var JuntaWsIds = $("[id*=hdnJuntaWorkstatusIDs]").val();
    if (proyID) {
        args._context.ProyectoID = proyID;
    } else {
        args._context.ProyectoID = -1;
    }
    if (JuntaWsIds) {
        args._context.JuntaWorkstatusIDs = JuntaWsIds
    }
    else {
        args._context.JuntaWorkstatusIDs = -1;
    }
}


Sam.WebService.AgregaDatosParaArmadoCampo = function (sender, args) {
    Sam.WebService.AgregaProyectoID(sender, args);

    var spool = $("[id*=hdnSpool1]").val();
    if (spool) {
        args._context.Spool1 = spool;
    } else {
        args._context.Spool1 = -1;
    }

    var etiquetaMaterial1 = $("[id*=hdnEtiquetaMaterial1]").val();
    if (etiquetaMaterial1) {
        args._context.EtiquetaMaterial1 = etiquetaMaterial1;
    } else {
        args._context.EtiquetaMaterial1 = "";
    }

    var etiquetaMaterial2 = $("[id*=hdnEtiquetaMaterial2]").val();
    if (etiquetaMaterial2) {
        args._context.EtiquetaMaterial2 = etiquetaMaterial2;
    } else {
        args._context.EtiquetaMaterial2 = "";
    }
}

//Ingresa al contexto proyecto y orden de trabajo (odt en caso de existir en un combo)
Sam.WebService.NumControlOnClientItemsRequestingEventHandler = function (sender, args) {
    Sam.WebService.AgregaProyectoID(sender, args);
    //debugger;;
    var cboNumUnico = $find($("div[id$=radNumUnico]").attr("ID"));
    if (cboNumUnico) {
        args._context.NumeroUnicoID = cboNumUnico._value;
        var texto = cboNumUnico.get_text();

        if (texto.length > 0) {
            args._context.Segmento = texto.substring(texto.length - 1);
        }
    }

    var combo = $find($("div[id$=radCmbOrdenTrabajo]").attr("ID"));
    if (combo) {
        args._context.OrdenTrabajoID = combo._value;
    }

}


//Crea por cada número único recibido un renglón del tipo template y lo llena con los datos obtenidos.
Sam.WebService.NumeroUnicoTablaDataBound = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.CodigoSegmento);
    item.set_value(dataItem.NumeroUnicoID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateNumeroUnico"));
    template.instantiateIn(item.get_element(), dataItem);
}


//Este método siempre deberá utilizarse en los radCombos con tablas.
//En el evento OnSelectedIndexChanged.
Sam.WebService.RadComboOnSelectedIndexChanged = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    sender.set_text(item.get_attributes().getAttribute("text"));
    sender.set_value(item.get_value());
}

Sam.WebService.RadCortadorSelectedIndexChanged = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var attr = item.get_attributes();
    var texto = attr.getAttribute("nombre");
    var valor = item.get_value();
    if (valor != null) {
        sender.set_text(texto);
        sender.set_value(valor);
        $("#hdnTextCortador").val(texto);
        $("#hdnCortadorSelected").val(valor);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
        $("#hdnTextCortador").val("");
        $("#hdnCortadorSelected").val("");
    }
}

Sam.WebService.RadCortadorDropClosed = function (sender, args) {
    //debugger;
    var texto = $("[id*=hdnTextCortador]").val();
    var valor = $("[id*=hdnCortadorSelected]").val();
    if (valor != null) {
        sender.set_text(texto);
        sender.set_value(valor);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
    }
}



//Agrega el materialSpoolID al contexto proveniente de un hidden llamado hdnMatSpoolID
Sam.WebService.ComboOnNumeroUnicoParaAsignacionRequested = function (sender, args) {
    var tableCell = $('#' + sender.get_tableElement().parentNode.id).parent();
    args._context.MaterialSpoolID = tableCell.children("input[id*='hdnMaterialSpoolID']").val();
    args._context.Cantidad = tableCell.children("input[id*='hdnCantidad']").val();
}

//Funcion que cambia el elemento seleccionado en el combo de numeros unicos
//utilizado en asignacion.
Sam.WebService.ComboOnNumeroUnicoParaAsignacionIndexChanged = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var attr = item.get_attributes();
    var valorCookie = Sam.Utilerias.ObtenCookie("Culture");
    var si = valorCookie == "en-US" ? "Yes" : "No";
    var esEquiv = attr.getAttribute("equiv") ? si : "No";

    sender.set_value(item.get_value());
}

//Funcion que llena los registros de una tabla utilizada en un combo
//Utilizada para mostrar numeros unicos para asignar
Sam.WebService.ComboOnNumeroUnicoParaAsignacionItemDataBound = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    var attributes = item.get_attributes();
    attributes.setAttribute("text", dataItem.CodigoNumeroUnico);
    attributes.setAttribute("cantidad", dataItem.InventarioBuenEstado);
    attributes.setAttribute("ic", dataItem.CodigoItemCode);
    attributes.setAttribute("desc", dataItem.DescripcionItemCode);

    item.set_value(dataItem.NumeroUnicoID + "-" + dataItem.Segmento);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateNumeroUnicoParaAsignacion"));
    template.instantiateIn(item.get_element(), dataItem);
}

//Agrega el materialSpoolID al contexto proveniente de un hidden llamado hdnMatSpoolID
Sam.WebService.ComboOnNumeroUnicoDespachoRequested = function (sender, args) {
    args._context.MaterialSpoolID = $("#hdnMatSpoolID").val();
}

//Agregamos el id de JuntaSpool para buscar numeros unicos desde armado
Sam.WebService.ComboOnNumeroUnicoDesdeArmadoRequested = function (sender, args) {
    //debugger;
    args._context.juntaSpoolID = $("#hdnJuntaSpoolID").val();
}

Sam.WebService.RadDespachadorSelectedIndexChanged = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var attr = item.get_attributes();

    var texto = attr.getAttribute("nombre");
    var valor = item.get_value();

    if (valor != null) {
        sender.set_text(texto);
        sender.set_value(valor);
        $("#hdnTextDespachador").val(texto);
        $("#hdnDespachadorSelected").val(valor);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
    }
}

Sam.WebService.RadDespachadorDropClosed = function (sender, args) {
    //debugger;
    var texto = $("[id*=hdnTextDespachador]").val();
    var valor = $("[id*=hdnDespachadorSelected]").val();

    if (valor != null) {
        sender.set_text(texto);
        sender.set_value(valor);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
    }
}

Sam.WebService.ComboOnNumeroUnicoDespachoAccesorioDropClosed = function (sender, args) {
    //debugger;
    var texto = $("[id*=hdnTextoSeleccionado]").val();
    var valor = $("[id*=hdnNuSelected]").val();

    if (valor != null) {
        sender.set_text(texto);
        sender.set_value(valor);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
    }
}

//Funcion que cambia el elemento seleccionado en el combo de numeros unicos
//utilizado en despachos.
Sam.WebService.ComboOnNumeroUnicoDespachoIndexChanged = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var attr = item.get_attributes();


    var value = item.get_value();
    var text = attr.getAttribute("text");

    if (value != null) {
        var valorCookie = Sam.Utilerias.ObtenCookie("Culture");
        var si = valorCookie == "en-US" ? "Yes" : "No";
        var esEquiv = attr.getAttribute("equiv") ? si : "No";

        sender.set_text(text);
        sender.set_value(value);
        $("#lblCantidad").text(attr.getAttribute("cantidad"));
        $("#lblIc").text(attr.getAttribute("ic"));
        $("#lblIcDesc").text(attr.getAttribute("desc"));
        $("#lblEquiv").text(esEquiv);

        var valorHidden = attr.getAttribute("cantidad") + "|" + attr.getAttribute("ic") + "|" + attr.getAttribute("desc") + "|" + esEquiv;

        $("#hdnNuSeleccionado").val(valorHidden);
        $("#hdnTextoSeleccionado").val(text);
        $("#hdnNuSelected").val(value);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
    }
}

Sam.WebService.ComboOnNumeroUnico1ArmadoIndexChanged = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var attr = item.get_attributes();

    var texto = attr.getAttribute("text");
    var valor = item.get_value();

    if (valor != null) {
        sender.set_text(texto);
        sender.set_value(valor);
        $("#hdnTextNU1Selected").val(texto);
        $("#hdnNU1Selected").val(valor);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
        $("#hdnTextNU1Selected").val("");
        $("#hdnNU1Selected").val("");
    }
}

Sam.WebService.ComboOnNumeroUnico1DropClosed = function (sender, args) {
    //debugger;
    var texto = $("[id*=hdnTextNU1Selected]").val();
    var valor = $("[id*=hdnNU1Selected]").val();
    if (valor != null) {
        sender.set_text(texto);
        sender.set_value(valor);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
    }
}

Sam.WebService.ComboOnNumeroUnico2ArmadoIndexChanged = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var attr = item.get_attributes();

    var texto = attr.getAttribute("text");
    var valor = item.get_value();

    if (valor != null) {
        sender.set_text(texto);
        sender.set_value(valor);
        $("#hdnTextNU2Selected").val(texto);
        $("#hdnNU2Selected").val(valor);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
        $("#hdnTextNU2Selected").val("");
        $("#hdnNU2Selected").val("");
    }
}

Sam.WebService.ComboOnNumeroUnico2DropClosed = function (sender, args) {
    //debugger;
    var texto = $("[id*=hdnTextNU2Selected]").val();
    var valor = $("[id*=hdnNU2Selected]").val();
    if (valor != null) {
        sender.set_text(texto);
        sender.set_value(valor);
    }
    else {
        sender.set_text("");
        sender.set_value(null);
    }
}

//Funcion que llena los registros de una tabla utilizada en un combo
//Utilizada para mostrar numeros unicos para despachar
Sam.WebService.ComboOnNumeroUnicoDespachoAccesorioItemDataBound = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    var attributes = item.get_attributes();
    attributes.setAttribute("text", dataItem.CodigoNumeroUnico);
    attributes.setAttribute("cantidad", dataItem.InventarioBuenEstado);
    attributes.setAttribute("ic", dataItem.CodigoItemCode);
    attributes.setAttribute("desc", dataItem.DescripcionItemCode);
    attributes.setAttribute("equiv", dataItem.EsEquivalente);

    item.set_value(dataItem.NumeroUnicoID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateNumeroUnicoAccesorio"));
    template.instantiateIn(item.get_element(), dataItem);
}

//Agrega el número único id al contexto proveniente de un 
//HiddenField
Sam.WebService.AgregaNumeroUnicoIDhdn = function (sender, args) {

    var _NumeroUnicoID = $("[id*=hdnNumeroUnicoID]").val();
    var _CantCong = $("[id*=hdnCantidadCongelada]").val();

    if (_NumeroUnicoID) {
        args._context.NumeroUnicoID = _NumeroUnicoID;
        args._context.CantCong = _CantCong;
    }
}

//Agrega el numero unico id al contexto proveniente de un combo 
//llamada radNumUnico
Sam.WebService.AgregaNumeroUnicoID = function (sender, args) {
    var cboNumUnico = $find($("div[id$=radNumUnico]").attr("ID"));
    if (cboNumUnico) {
        args._context.NumeroUnicoID = cboNumUnico._value;
    }
}

//Funcion que ayuda para obtener los materiales candidatos a cortar 
//dependiendo del numero de control y el numero unico especificados.
Sam.WebService.MaterialesPorNumControlNumUnico = function (sender, args) {
    var cboNumUnico = $find($("div[id$=radNumUnico]").attr("ID"));
    if (cboNumUnico) {
        args._context.NumeroUnicoID = cboNumUnico._value;
        var texto = cboNumUnico.get_text();

        if (texto.length > 0) {
            args._context.Segmento = texto.substring(texto.length - 1);
        }
    }

    var cboNumeroControl = $find($("div[id$=radNumeroControl]").attr("ID"));
    if (cboNumeroControl) {
        args._context.OrdenTrabajoSpoolID = cboNumeroControl._value;
    }


    var grid = $find($("[id$=grdCorte]").attr("ID"));
    if (grid) {
        var dataItems = grid.get_masterTableView().get_dataItems();
        var _etiqueta = "";

        for (var i = 0; i < dataItems.length; i++) {
            _etiqueta += dataItems[i].get_cell("MaterialSpoolID").innerHTML + ";";
        }

        args._context.Etiqueta = _etiqueta;
    }
}

//Funcion de ayuda para generar el template de materiales candidatos a cortar
Sam.WebService.EtiquetaMaterialTablaDataBound = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    var attributes = item.get_attributes();
    attributes.setAttribute("text", dataItem.EtiquetaMaterial);
    attributes.setAttribute("ic", dataItem.ItemCode);
    attributes.setAttribute("desc", dataItem.Descripcion);
    attributes.setAttribute("equiv", dataItem.EsEquivalente);

    item.set_value(dataItem.MaterialSpoolID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateEtiquetaMaterial"));
    template.instantiateIn(item.get_element(), dataItem);
}

//Crea por cada número único recibido un renglón del tipo template y lo llena con los datos obtenidos.
Sam.WebService.ItemCodeTablaDataBound = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.Codigo);
    item.get_attributes().setAttribute("desc", dataItem.Descripcion);
    item.set_value(dataItem.ItemCodeID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateItemCode"));
    template.instantiateIn(item.get_element(), dataItem);
}

Sam.WebService.SpoolTablaDataBound = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.Spool);
    item.get_attributes().setAttribute("desc", dataItem.Etiqueta);
    item.set_value(dataItem.SpoolID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateSpool"));
    template.instantiateIn(item.get_element(), dataItem);
}

//Crea por cada colada recibido un renglón del tipo template y lo llena con los datos obtenidos.
Sam.WebService.ColadaTablaDataBound = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.NumeroColada);
    item.get_attributes().setAttribute("desc", dataItem.NumeroColada);
    item.set_value(dataItem.ColadaID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("coladaTemplate"));
    template.instantiateIn(item.get_element(), dataItem);
}

//Crea por cada tubero recibido un renglón del tipo template y lo llena con los datos obtenidos.
Sam.WebService.TuberoTablaDataBound = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.Codigo);
    item.get_attributes().setAttribute("nombre", dataItem.NombreCompleto);
    item.set_value(dataItem.TuberoID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateItemCode"));
    template.instantiateIn(item.get_element(), dataItem);
}

Sam.WebService.CortadorTablaDataBound = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.NumeroEmpleado);
    item.get_attributes().setAttribute("nombre", dataItem.NombreCompleto);
    item.set_value(dataItem.CortadorID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateCortador"));
    template.instantiateIn(item.get_element(), dataItem);
}

Sam.WebService.DespachadorTablaDataBound = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.NumeroEmpleado);
    item.get_attributes().setAttribute("nombre", dataItem.NombreCompleto);
    item.set_value(dataItem.DespachadorID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateDespachador"));
    template.instantiateIn(item.get_element(), dataItem);
}



//Crea por cada soldador recibido un renglón del tipo template y lo llena con los datos obtenidos.
Sam.WebService.SoldadorTablaDataBound = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.Codigo);
    item.get_attributes().setAttribute("nombre", dataItem.NombreCompleto);
    item.set_value(dataItem.SoldadorID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateItemCode"));
    template.instantiateIn(item.get_element(), dataItem);
}

//Crea por cada soldador recibido un renglón del tipo template y lo llena con los datos obtenidos.
Sam.WebService.InspectorTablaDataBound = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.NumeroEmpleado);
    item.get_attributes().setAttribute("nombre", dataItem.NombreCompleto);
    item.set_value(dataItem.InspectorID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateItemCode"));
    template.instantiateIn(item.get_element(), dataItem);
}


//Crea por cada despachador recibido un renglón del tipo template y lo llena con los datos obtenidos.
Sam.WebService.DespachadorTablaDataBound = function (sender, eventArgs) {
    //debugger;
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.NumeroEmpleado);
    item.get_attributes().setAttribute("nombre", dataItem.NombreCompleto);
    item.set_value(dataItem.DespachadorID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateDespachador"));
    template.instantiateIn(item.get_element(), dataItem);
}

//Crea por cada usuario recibido un renglón del tipo template y lo llena con los datos obtenidos.
Sam.WebService.UsuarioDataBound = function (sender, eventArgs) {
    var item = eventArgs.get_item();
    var dataItem = eventArgs.get_dataItem();
    item.get_attributes().setAttribute("text", dataItem.NombreCompleto);
    item.set_value(dataItem.UsuarioID);
    dataItem.Index = item.get_index();

    var template = new Sys.UI.Template($get("templateItemCode"));
    template.instantiateIn(item.get_element(), dataItem);
}

Sam.WebService.AgregaTallerID = function (sender, args) {
    var tallerID = $("[id*=ddlTaller]").val() || $("[id*=hdnTallerID]").val();
    if (tallerID) {
        args._context.TallerID = tallerID;
    }
    else {
        args._context.TallerID = 0;
    }
}

//////////////////////////////////////////////
//
//           DESTAJOS
//
/////////////////////////////////////////////

Sam.Destajo.X = 0;
Sam.Destajo.Y = 0;

Sam.Destajo.Inicializa = function () {
    $('.ajuste').each(function () {
        $(this).autoNumeric({ aSign: '$', aNeg: '-' });
        $(this).blur(Sam.Destajo.BlurMoneda);
    });
    $('.moneda').each(function () {
        $(this).autoNumeric({ aSign: '$' });
        $(this).blur(Sam.Destajo.BlurMoneda);
    });
    $('.entero').each(function () {
        $(this).autoNumeric({ mNum: 1, mDec: 0 });
        $(this).blur(Sam.Destajo.BlurEntero);
    });
    $(document).mousemove(function (e) {
        Sam.Destajo.X = e.pageX;
        Sam.Destajo.Y = e.pageY;
    });

    //Evitar que el botón cause el postback de .NET
    var btn = $('#btnCmts');
    if (btn.length > 0) {
        btn.attr("onclick", "");
        btn.unbind();
    }
}

Sam.Destajo.BlurMoneda = function () {
    var texto = $.trim($(this).val());
    if (texto === "") {
        $(this).val("$0.00");
    }
    Sam.Destajo.Recalcula();
}

Sam.Destajo.BlurEntero = function () {
    var texto = $.trim($(this).val());
    if (texto === "") {
        $(this).val("0");
    }
    Sam.Destajo.Recalcula();
}

Sam.Destajo.Recalcula = function () {
    var tipo = $('#hdnTipoDestajo').val();
    if (tipo === "T") {
        Sam.Destajo.RecalculaTubero();
    }
    else {
        Sam.Destajo.RecalculaSoldador();
    }
}

Sam.Destajo.RecalculaTubero = function () {
    var totalDestajo = parseFloat(0.0);
    var totalCuadro = parseFloat(0.0);
    var totalOtros = parseFloat(0.0);
    var totalDiasFestivos = parseFloat(0.0);
    var totalAjuste = parseFloat(0.0);
    var granTotal = parseFloat(0.0);
    var cntDiasF = parseInt($.fn.autoNumeric.Strip($(".diasF > input[id*=txtCantidadDiasF]")[0].id), 10);
    var costoDiaF = parseFloat($.fn.autoNumeric.Strip($(".diasF > input[id*=txtCostoDiaF]")[0].id));

    var txtDestajo;
    var txtCuadro;
    var txtOtros;
    var txtDiasFestivos;
    var txtAjuste;
    var txtTotal;

    totalDiasFestivos = cntDiasF * costoDiaF;

    $('.totales > input').each(function () {
        var id = this.id;
        if (id.indexOf("txtDestajo") > -1) {
            txtDestajo = $(this);
            totalDestajo = $.fn.autoNumeric.Strip(id);
        } else if (id.indexOf("txtCuadro") > -1) {
            txtCuadro = $(this);
            totalCuadro = $.fn.autoNumeric.Strip(id);
        } else if (id.indexOf("txtOtros") > -1) {
            txtOtros = $(this);
            totalOtros = $.fn.autoNumeric.Strip(id);
        } else if (id.indexOf("txtDiasFestivos") > -1) {
            txtDiasFestivos = $(this);
        } else if (id.indexOf("txtAjuste") > -1) {
            txtAjuste = $(this);
        } else if (id.indexOf("txtTotal") > -1) {
            txtTotal = $(this);
        }
    });

    var totalPdis = parseFloat(0.0);
    var idFormato = txtCuadro[0].id;
    $('.repFilaPar [id*=hdnDiametro], .repFila [id*=hdnDiametro]').each(function () {
        totalPdis += parseFloat($(this).val());
    });

    $('.repFilaPar, .repFila').each(function () {
        var diam = parseFloat($('[id*=hdnDiametro]', this).val());
        var destajo = parseFloat($('[id*=hdnDestajo]', this).val());
        var cuad = $('[id*=hdnCuadro]', this);
        var diasF = $('[id*=hdnDiasF]', this);
        var otr = $('[id*=hdnOtros]', this);
        var tot = $('[id*=hdnTotal]', this);

        //Para los prorrateos visibles
        var cCuad = $('.colCuadro', this);
        var cDiasF = $('.colDiasF', this);
        var cOtros = $('.colOtros', this);
        var cTotal = $('.colTotal', this);

        var aj = $('[id*=txtAjuste]', this);

        //Calcular en los hiddens
        diasF.val(totalDiasFestivos / totalPdis * diam);
        cuad.val(totalCuadro / totalPdis * diam);
        otr.val(totalOtros / totalPdis * diam);

        //Fijar valores en las columnas con formato moneda (txtCuadro.id se usa para copiar el formato)
        cCuad.text($.fn.autoNumeric.Format(idFormato, cuad.val(), { aSign: '$' }));
        cDiasF.text($.fn.autoNumeric.Format(idFormato, diasF.val(), { aSign: '$' }));
        cOtros.text($.fn.autoNumeric.Format(txtAjuste[0].id, otr.val(), { aSign: '$', aNeg: '-' }));

        var ajusteFila = parseFloat($.fn.autoNumeric.Strip(aj[0].id));
        var totalFila = parseFloat(diasF.val()) + parseFloat(cuad.val()) + parseFloat(otr.val()) + ajusteFila + destajo;

        tot.val(totalFila);
        cTotal.text($.fn.autoNumeric.Format(idFormato, totalFila, { aSign: '$' }));
        totalAjuste += ajusteFila;
    });

    granTotal = parseFloat(totalAjuste) + parseFloat(totalCuadro) + parseFloat(totalDestajo) + parseFloat(totalDiasFestivos) + parseFloat(totalOtros);
    txtDiasFestivos.val($.fn.autoNumeric.Format(idFormato, totalDiasFestivos, { aSign: '$' }));
    txtAjuste.val($.fn.autoNumeric.Format(txtAjuste[0].id, totalAjuste, { aSign: '$', aNeg: '-' }));
    txtTotal.val($.fn.autoNumeric.Format(idFormato, granTotal, { aSign: '$' }));

    $('#lblTotalCuadroT').text(txtCuadro.val());
    $('#lblTotalDiasFT').text(txtDiasFestivos.val());
    $('#lblTotalOtrosT').text(txtOtros.val());
    $('#lblTotalAjusteT').text(txtAjuste.val());
    $('#lblGranTotalT').text(txtTotal.val());
}

Sam.Destajo.RecalculaSoldador = function () {
    var totalDestajoRaiz = parseFloat(0.0);
    var totalDestajoRelleno = parseFloat(0.0);
    var totalCuadro = parseFloat(0.0);
    var totalOtros = parseFloat(0.0);
    var totalDiasFestivos = parseFloat(0.0);
    var totalAjuste = parseFloat(0.0);
    var granTotal = parseFloat(0.0);
    var cntDiasF = parseInt($.fn.autoNumeric.Strip($(".diasF > input[id*=txtCantidadDiasF]")[0].id), 10);
    var costoDiaF = parseFloat($.fn.autoNumeric.Strip($(".diasF > input[id*=txtCostoDiaF]")[0].id));

    var txtDestajoRaiz;
    var txtDestajoRelleno;
    var txtCuadro;
    var txtOtros;
    var txtDiasFestivos;
    var txtAjuste;
    var txtTotal;

    totalDiasFestivos = cntDiasF * costoDiaF;

    txtDestajoRaiz = $('.totales > input[id*=txtDestajoRaiz]');
    totalDestajoRaiz = $.fn.autoNumeric.Strip(txtDestajoRaiz[0].id);
    txtDestajoRelleno = $('.totales > input[id*=txtDestajoRelleno]');
    totalDestajoRelleno = $.fn.autoNumeric.Strip(txtDestajoRelleno[0].id);
    txtCuadro = $('.totales > input[id*=txtCuadro]');
    totalCuadro = $.fn.autoNumeric.Strip(txtCuadro[0].id);
    txtOtros = $('.totales > input[id*=txtOtros]');
    totalOtros = $.fn.autoNumeric.Strip(txtOtros[0].id);
    txtDiasFestivos = $('.totales > input[id*=txtDiasFestivos]');
    txtAjuste = $('.totales > input[id*=txtAjuste]');
    txtTotal = $('.totales > input[id*=txtTotal]');

    var totalPdis = parseFloat(0.0);
    var idFormato = txtCuadro[0].id;
    $('.repFilaPar [id*=hdnDiametro], .repFila [id*=hdnDiametro]').each(function () {
        totalPdis += parseFloat($(this).val());
    });

    $('.repFilaPar, .repFila').each(function () {
        var diam = parseFloat($('[id*=hdnDiametro]', this).val());
        var destajoRaiz = parseFloat($('[id*=hdnDestajoRaiz]', this).val());
        var destajoRelleno = parseFloat($('[id*=hdnDestajoRelleno]', this).val());
        var cuad = $('[id*=hdnCuadro]', this);
        var diasF = $('[id*=hdnDiasF]', this);
        var otr = $('[id*=hdnOtros]', this);
        var tot = $('[id*=hdnTotal]', this);

        //Para los prorrateos visibles
        var cCuad = $('.colCuadro', this);
        var cDiasF = $('.colDiasF', this);
        var cOtros = $('.colOtros', this);
        var cTotal = $('.colTotal', this);

        var aj = $('[id*=txtAjuste]', this);

        //Calcular en los hiddens
        diasF.val(totalDiasFestivos / totalPdis * diam);
        cuad.val(totalCuadro / totalPdis * diam);
        otr.val(totalOtros / totalPdis * diam);

        //Fijar valores en las columnas con formato moneda (txtCuadro.id se usa para copiar el formato)
        cCuad.text($.fn.autoNumeric.Format(idFormato, cuad.val(), { aSign: '$' }));
        cDiasF.text($.fn.autoNumeric.Format(idFormato, diasF.val(), { aSign: '$' }));
        cOtros.text($.fn.autoNumeric.Format(txtAjuste[0].id, otr.val(), { aSign: '$', aNeg: '-' }));

        var ajusteFila = parseFloat($.fn.autoNumeric.Strip(aj[0].id));
        var totalFila = parseFloat(diasF.val()) + parseFloat(cuad.val()) + parseFloat(otr.val()) + ajusteFila + destajoRaiz + destajoRelleno;

        tot.val(totalFila);
        cTotal.text($.fn.autoNumeric.Format(idFormato, totalFila, { aSign: '$' }));
        totalAjuste += ajusteFila;
    });

    granTotal = parseFloat(totalAjuste) + parseFloat(totalCuadro) + parseFloat(totalDestajoRaiz) + parseFloat(totalDestajoRelleno) + parseFloat(totalDiasFestivos) + parseFloat(totalOtros);
    txtDiasFestivos.val($.fn.autoNumeric.Format(idFormato, totalDiasFestivos, { aSign: '$' }));
    txtAjuste.val($.fn.autoNumeric.Format(txtAjuste[0].id, totalAjuste, { aSign: '$', aNeg: '-' }));
    txtTotal.val($.fn.autoNumeric.Format(idFormato, granTotal, { aSign: '$' }));

    $('#lblTotalCuadroS').text(txtCuadro.val());
    $('#lblTotalDiasFS').text(txtDiasFestivos.val());
    $('#lblTotalOtrosS').text(txtOtros.val());
    $('#lblTotalAjusteS').text(txtAjuste.val());
    $('#lblGranTotalS').text(txtTotal.val());
}

Sam.Destajo.AbreComentariosArmado = function (idVentana, idHidden) {
    Sam.Destajo.AbreComentariosProceso(idVentana, idHidden, 16);
}

Sam.Destajo.AbreComentariosSoldadura = function (idVentana, idHidden) {
    Sam.Destajo.AbreComentariosProceso(idVentana, idHidden, 17);
}

Sam.Destajo.AbreComentariosProceso = function (idVentana, idHidden, numMensaje) {
    var wnd = $find(idVentana);
    var cnt = $('#' + idHidden).val();
    var divCnt = $('#cmtsProceso');
    var divDest = $('#dvCmtsDestajo');
    wnd.set_title(Sam.MensajesUI(numMensaje));
    divCnt.show();
    divDest.hide();
    divCnt.text(cnt);
    wnd.setSize(230, 120);
    wnd.moveTo(Sam.Destajo.X + 20, Sam.Destajo.Y - 20);
    wnd.show();
}

Sam.Destajo.AbreComentariosDestajo = function (idVentana, idHidden) {
    var wnd = $find(idVentana);
    var cnt = $('#' + idHidden).val();
    var divCnt = $('#cmtsProceso');
    var divDest = $('#dvCmtsDestajo');
    wnd.set_title(Sam.MensajesUI(18));
    divCnt.hide();
    divDest.show();
    $('#txtCmtsDestajo').val(cnt);
    wnd.setSize(230, 175);
    wnd.moveTo(Sam.Destajo.X - 240, Sam.Destajo.Y - 20);
    wnd.show();

    $('#btnCmts').click(function () {
        $('#' + idHidden).val($('#txtCmtsDestajo').val());
        wnd.close();
    });
}

Sam.Destajo.UnbindCmts = function (sender, args) {
    $('#btnCmts').unbind();
}

//////////////////////////////////////////////
//
//           REPORTES
//
/////////////////////////////////////////////
Sam.Reportes.AbreVisorReporte = function (tipo, nombresQs, valoresQs, proyectoID) {
    var url = "/VisorReporte.aspx?EsCustom=false&Tipo=" + escape(tipo) + "&ProyectoID=" + escape(proyectoID);
    Sam.Reportes.AbreVisor(url, nombresQs, valoresQs);
}

Sam.Reportes.AbreVisorReportePersonalizado = function (nombreReporte, nombresQs, valoresQs) {
    var url = "/VisorReporte.aspx?EsCustom=true&NombreReporte=" + escape(nombreReporte);
    Sam.Reportes.AbreVisor(url, nombresQs, valoresQs);
}

Sam.Reportes.AbreVisor = function (url, nombresQs, valoresQs) {
    var chunksNombres = nombresQs.split(",");
    var chunksValores = valoresQs.split(",");

    $.each(chunksNombres, function (indice, valor) {
        url += ("&" + valor + "=" + escape(chunksValores[indice]));
    });

    var features = "menubar=0,toolbar=0,resizable=1,scrollbars=1,height=600,width=800";
    var wnd = window.open(url, "VisorReportes", features);
    wnd.focus();
}

//////////////////////////////////////////////
//
//           CATALOGOS
//
/////////////////////////////////////////////

Sam.Catalogos.RevisaNombreCliente = function (sender, args) {
    var textBoxCliente = $("[id*=txtNombreCliente]:text");
    if (textBoxCliente) {
        args.IsValid = textBoxCliente.val().trim() != "";
        return;
    }
    args.IsValid = false;
}

/////////////////////////////////////////////
//
//          POPUPS
//
/////////////////////////////////////////////

Sam.Utilerias.SetSize = function (oWnd, wd, hg) {

    var width = wd;
    var height = hg;
    //    if ($(window).width() - 20 <= wd) {
    //        width = $(window).width() - 20;
    //    }
    if ($(window).height() - 20 <= hg) {
        height = $(window).height() - 20;
    }

    oWnd.setSize(width, height);
}

Sam.Utilerias.ValidaAnioFecha = function (sender, e) {
    if (e.get_NewDate() != null) {
        var fecha = e.get_NewDate();
        var anio = fecha.getFullYear();

        var fechaActual = new Date();
        var anioMinPermitido = fechaActual.getFullYear() - 1;
        var anioMaxPermitido = fechaActual.getFullYear() + 1;

        if (anio > anioMaxPermitido || anio < anioMinPermitido) {
            alert("El año de la fecha no es permitido, debe ser mayor o igual a " + anioMinPermitido + " y  menor o igual a " + anioMaxPermitido);
        }

    }
}

//////////////////////////////////////////////
//
//           SEGUIMIENTO JUNTAS
//
/////////////////////////////////////////////
Sam.Seguimientos.AjaxRequestEnd = function () {
    Sam.Seguimientos.AttachHandlers();
    Sam.Seguimientos.MuestraGrid();
}

Sam.Seguimientos.AttachHandlers = function () {
    $("#tblHeaders, #tblBody").scrollsync({ targetSelector: "#tblBody", axis: "x" });
    $("#tblCongelados, #tblBody").scrollsync({ targetSelector: "#tblBody", axis: "y" });
    $(window).resize(Sam.Seguimientos.AjustaAncho);
    $("[class=spHeader]").mousedown(function (e) {
        if (e.button === 2) {
            Sam.Seguimientos.AbreMenu(this);
        }
    });
    $("#tblCongelados table[class=repSam] tr, #tblBody table[class=repSam] tr").click(function () {
        var index = this.rowIndex;
        var rowLeft = $("#tblCongelados table[class=repSam] tr:eq(" + index + ")");
        var rowRight = $("#tblBody table[class=repSam] tr:eq(" + index + ")");
        $(rowLeft).toggleClass("selected");
        $(rowRight).toggleClass("selected");
    });
}

Sam.Seguimientos.MuestraGrid = function () {
    var dvOculto = $("#ocultoInicio");
    //hack - mostrar temporalmente para poder medir
    $(dvOculto).css({ position: "absolute", visibility: "hidden", display: "block" });
    Sam.Seguimientos.AjustaAncho(true);
    //regresar a estado original
    $(dvOculto).css({ position: "", visibility: "", display: "block" });
}

Sam.Seguimientos.AjustaAncho = function () {
    var anchoCol = $("#topLeft").width();
    var anchoWrapper = $("[class=phc]").width();
    var anchoAjustado = anchoWrapper - anchoCol;
    $("#tblHeaders").width(anchoAjustado);
    $("#tblBody").width(anchoAjustado);
}

Sam.Seguimientos.AbreMenu = function (cntSpan) {
    var columna = $(cntSpan).attr("data");
    var clientIDMenu = $("#hdnMenuColumnasID").val();
    $("#hdnColumnaSeleccionada").val(columna);
    Sam.Seguimientos.AbreMenuColumna(event, clientIDMenu);
}

Sam.Seguimientos.AbreMenuColumna = function (e, menu) {
    var contextMenu = $find(menu);
    if ((!e.relatedTarget) || (!$telerik.isDescendantOrSelf(contextMenu.get_element(), e.relatedTarget))) {
        contextMenu.show(e);
    }
    $telerik.cancelRawEvent(e);
}

Sam.Seguimientos.MostrarFiltros = function (wndID) {
    var wnd = $find(wndID);
    Sam.Utilerias.SetSize(wnd, 500, 340);
    wnd.show();
    return false;
}

Sam.Seguimientos.ItemMenuClicked = function (sender, args) {
    Sam.Utilerias.MuestraOverlay();
}

Sam.Seguimientos.AplicaFiltros = function (wndID) {
    var wnd = $find(wndID);
    wnd.close();
    Sam.Utilerias.MuestraOverlay();
}

//////////////////////////////////////////////
//
//           PROGRAMA
//
/////////////////////////////////////////////
Sam.Programa.Validaciones = {};

Sam.Programa.Inicializa = function () {
    $('.dentroGrid').each(function () {
        $(this).autoNumeric({ mNum: 10, mDec: 2 });
        $(this).blur(Sam.Programa.BlurTexto);
    });
};

Sam.Programa.BlurTexto = function () {
    var texto = $.trim($(this).val());
    if (texto === "") {
        $(this).val("0");
    }
};

Sam.Programa.Validaciones.MayorIgualCeroMenorOchoNueves = function (sender, args) {
    args.IsValid = false;
    var valor = parseFloat($.fn.autoNumeric.Strip(sender.controltovalidate));
    if (!isNaN(valor) && valor >= 0 && valor <= 99999999.99) {
        args.IsValid = true;
    }
};

Sam.Programa.Validaciones.ValidaPeriodos = function (sender, args) {
    var validadores = Page_Validators;
    var valido = true;
    $.each(validadores, function (i, validador) {
        if (validador.validationGroup === "vgPeriodos") {
            ValidatorValidate(validador);
            if (validador.isvalid === false) {
                valido = false;
                //hack para que no aparezca en el validation summary
                validador.isvalid = true;
            }
        }
        else if (validador.validationGroup === "vgPrincipal") {
            ValidatorValidate(validador);
        }
    });
    args.IsValid = valido;
}