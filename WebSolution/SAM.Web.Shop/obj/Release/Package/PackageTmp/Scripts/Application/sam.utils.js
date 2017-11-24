namespace("sam");


(function ($, undefinied) {


    "use strict";


    sam.utils = (function () {


        var $dlg = null,
            dlgHtml = '<div class="modal" id="waitModal" tabindex="-1" role="dialog" aria-labelledby="modalTitle" aria-hidden="true"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="modalTitle">{0}</h4></div><div class="modal-body"><div class="progress progress-striped active"><div class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div></div></div></div></div></div>';


        var switchLanguage = function (lang, params) {
            $.removeCookie('language', { path: '/' });
            $.cookie('language', lang, { path: '/' });


            var currentLocation = window.location;
            var href = currentLocation.href;

            window.location.reload();            
        };


        var showPleaseWait = function () {
            if ($dlg === null) {
                $dlg = $(dlgHtml.format(i18n.t("Messages.PleaseWait")));
            }


            $dlg.modal({
                keyboard: false
            });
        };


        var hidePleaseWait = function () {
            $dlg.modal("hide");
        };


        return {
            switchLanguage: switchLanguage,
            showPleaseWait: showPleaseWait,
            hidePleaseWait: hidePleaseWait
        };


    })(); //Singleton
    function contar(cadena) {
        var aux = 0;
        var i = 0;


        for (i; i < cadena.length; i++) {
            if (cadena.charAt(i) == '/') {
                aux++;
                if (aux >= 4) {
                    break;
                }
            }
        }
        return i;
    }

})(jQuery);
