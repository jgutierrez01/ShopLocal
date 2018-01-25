namespace("sam.pages.workstatus");

(function ($, undefinied) {
    
    "use strict";

    sam.pages.workstatus.results = function ($container) {

        var $cnt = $container;
        var $ejecutar = true;

        var init = function () {     

            //$('tr').on('click', function (e) {
                
            //    if($ejecutar)
            //    {
            //        var ordenTrabajoSpoolId = $('td', this).eq(0).text();
            //        var href = window.location.href;
            //        var n = getIndex(href);
            //        var url = href.substr(0, n) + "/ControlNumber/CertificationReports?controlNumberId=" + ordenTrabajoSpoolId;
            //        window.open(url, 'details');
            //    }
            //});

            $("#sendReport").button().click(function () {
                var process = $('#process').val();

                $.getJSON('/Controls/SendEmail', { process: process }, function (data) {
                    alert(data);
                });
            });

            $("a.deleteResults").on('click', function (s) {          

                var currentCulture = $("html").prop("lang");
                var answer = '';
                if (currentCulture == "en-US") {
                     answer = confirm('Confirm delete of this control number?');
                }
                else {
                     answer = confirm('¿Confirma la eliminación del Número de Control?');
                }
           
                $ejecutar = false;

                return answer;
            });

            function getIndex(cadena) {
                var aux = 0;
                var i = 0;

                for (i; i < cadena.length; i++) {
                    if (cadena.charAt(i) == '/') {
                        aux++;
                        if (aux >= 3) {
                            break;
                        }
                    }
                }
                return i;
            }
        };

        return {
            init: init
        };
    };

})(jQuery);
