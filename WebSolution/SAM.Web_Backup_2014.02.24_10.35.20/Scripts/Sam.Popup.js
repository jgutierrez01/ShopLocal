//namespace global
if (!Sam) {
    var Sam = {};
}

Sam.Popup = {};

Sam.Popup.VentanaPadre = function() {
    var oWindow = null;
    if (window.radWindow) {
        oWindow = window.radWindow;
    }
    else if (window.frameElement.radWindow) {
        oWindow = window.frameElement.radWindow;
    }
    return oWindow.BrowserWindow;
}


Sam.Popup.ValidaCheckSeleccionado = function (sender, args) {
    var alguno = false;
    $("#contenedorChk > input[type=checkbox]").each(function (i, item) {
        if (item.checked) {
            alguno = true;
        }
    });
    args.IsValid = alguno;
}