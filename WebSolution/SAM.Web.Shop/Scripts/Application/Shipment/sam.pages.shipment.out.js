namespace("sam.pages.shipment");


(function ($, undefinied) {

    "use strict";

    sam.pages.shipment.out = function ($container) {

        var $cnt = $container;
        
        var init = function (p) {

            $('tr').on('click', function (e) {
              
                var ordenTrabajoSpoolId = $('td', this).eq(0).text();
                var href = window.location.href;
                var n = getIndex(href);
                var url = href.substr(0, n) + "/ControlNumber?controlNumberId=" + ordenTrabajoSpoolId;
                window.open(url, 'details');
            });


            $("#sendReport").button().click(function () {
                var process = $('#process').val();

                $.getJSON('/Controls/SendEmail', { process: process }, function (data) {

                    alert(data);
                });
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
