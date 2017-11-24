namespace("sam.pages.sq.opciones");

(function ($, undefinied) {

    "use strict";

    sam.pages.sq.opciones.index = function ($container, $containerWkS) {

        var $cnt = $container,
            $cuadranteContainer,
            $numeroControlContainer,
            $cntWkS = $cnt.find("#sq-form");
        var $ejecutar = true;

        var init = function () {

            var $selectedOpcionAgregarEditar;

            
            $('input:radio[name=SeleccionAgregarEditar]').change(function () {
                
                if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Add") {
                    $("#sq-add-form").show();
                    $("#gridAdd-form").show();
                    $("#sq-editar-form").hide();
                    $("#gridEditar-form").hide();
                }
                else if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Edit") {
                    $("#sq-add-form").hide();
                    $("#gridAdd-form").hide();
                    $("#sq-editar-form").show();
                    $("#gridEditar-form").show();
                }
            });
        
            $('input:radio[name=SeleccionAgregarEditar]').click(function () {
                if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Add") {
                    $("#sq-add-form").show();
                    $("#gridAdd-form").show();
                    $("#sq-editar-form").hide();
                    $("#gridEditar-form").hide();
                }
                else if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Edit") {
                    $("#sq-add-form").hide();
                    $("#gridAdd-form").hide();
                    $("#sq-editar-form").show();
                    $("#gridEditar-form").show();
                }
            });
            if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Add") {
                $("#sq-add-form").show();
                $("#gridAdd-form").show();
                $("#sq-editar-form").hide();
                $("#gridEditar-form").hide();
            }
            else if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Edit") {
                $("#sq-add-form").hide();
                $("#gridAdd-form").hide();
                $("#sq-editar-form").show();
                $("#gridEditar-form").show();
            }
            else {
                $("#sq-add-form").hide();
                $("#gridAdd-form").hide();
                $("#sq-editar-form").hide();
                $("#gridEditar-form").hide();
            }
        };

        return {
            init: init
        };
    };

})(jQuery);