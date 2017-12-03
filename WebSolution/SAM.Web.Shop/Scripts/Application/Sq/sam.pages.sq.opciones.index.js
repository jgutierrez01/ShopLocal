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
            //$('input:radio[name=SeleccionAgregarEditar]').change(function () {
                
            //    if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Add") {
            //        $("#sq-add-form").show();
            //        $("#sq-add-form").css("display", "block");
            //        if ($("#gridAdd-form").find("table").find("tbody").length > 0) {
            //            $("#gridAdd-form").show();
            //            $("#gridAdd-form").css("display", "block");
            //        }                                                           
            //        $("#sq-editar-form").hide();                    
            //        $("#gridEditar-form").hide();
            //        $("#sq-editar-form").css("display", "none");
            //        $("#gridEditar-form").css("display", "none");
            //    }
            //    else if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Edit") {                    
            //        $("#sq-editar-form").show();
            //        $("#sq-editar-form").css("display", "block");                    
            //        if ($("#gridEditar-form").find("table").find("tbody").length > 0) {
            //            $("#gridEditar-form").show();
            //            $("#gridEditar-form").css("display", "block");
            //        }
            //        $("#sq-add-form").hide();
            //        $("#gridAdd-form").hide();
            //        $("#sq-add-form").css("display", "none");
            //        $("#gridAdd-form").css("display", "none");
            //    }
            //});
        
            $('input:radio[name=SeleccionAgregarEditar]').click(function () {
                if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Add") {
                    $("#sq-editar-form").hide();
                    $("#gridEditar-form").hide();
                    $("#sq-add-form").show();
                    if ($("#gridAdd-form").find("table").find("tbody").length > 0) {
                        $("#gridAdd-form").show();                        
                    } else {
                        $("#gridAdd-form").hide();
                    }                    
                }
                else if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Edit") {
                    $("#sq-add-form").hide();
                    $("#gridAdd-form").hide();                    
                    $("#sq-editar-form").show();                    
                    if ($("#gridEditar-form").find("table").find("tbody").length > 0) {
                        $("#gridEditar-form").show();                        
                    } else {
                        $("#gridEditar-form").hide();
                    }
                }
            });
            if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Add") {
                $("#sq-add-form").show();                
                if ($("#gridAdd-form").find("table").find("tbody").length > 0) {
                    $("#gridAdd-form").show();                    
                }
                $("#sq-editar-form").hide();
                $("#gridEditar-form").hide();                
            }
            else if ($('input:radio[name=SeleccionAgregarEditar]:checked').val() == "Edit") {
                $("#sq-editar-form").show();                
                if ($("#gridEditar-form").find("table").find("tbody").length > 0) {
                    $("#gridEditar-form").show();                    
                }
                $("#sq-add-form").hide();
                $("#gridAdd-form").hide();                
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