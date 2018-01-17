//Function to activate waiting screen
function loadingStart() {
    $(document).on('keypress keydown keyup', function (e) {
        if (e.which === 8 || e.which === 38 || e.which === 40) {
            e.stopImmediatePropagation();
            return false;
        };
        return false;
    });
    $("body").attr("style", "pointer-events:none;");
    $("body").addClass("loading");
}

//Function to deactivate waiting screen
function loadingStop() {
    $("body").removeAttr("style", "pointer-events:none;");
    $("body").removeClass("loading");
    $(document).off('keypress keydown keyup');
}

var isMobile = {
    Android: function () {
        return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function () {
        return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function () {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function () {
        return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function () {
        return navigator.userAgent.match(/IEMobile/i);
    },
    any: function () {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
    }
};

function filtroGeneral(val) {
    return {
        mode: "row",
        extra: false,
        operators: {
            string: {
                startswith: $("html").prop("lang") != "en-US" ? "Empieza con" : "Starts with",
                eq: $("html").prop("lang") != "en-US" ? "Es igual a" : "Is equal to",
                neq: $("html").prop("lang") != "en-US" ? "No es igual a" : "Is not equal to",
            },
        },
        cell: {
            showOperators: false,
            operator: "contains"
        }
    }
}


function filtroTexto() {
    return {
        cell: {
            operator: "contains",
            template: function (args) {
                args.css("width", "90%").addClass("k-textbox").keydown(function (e) {
                    setTimeout(function () {
                        $(e.target).trigger("change");
                        $(e.target).trigger("focus");
                    });
                });
            },
            showOperators: false
        }
    }
}

function filtroNumero() {
    return {
        extra: true,
        cell: {
            operator: "equals",
            template: function (args) {
                $(args).prop('type', 'number');
                args.css("width", "90%").addClass("k-textbox").keydown(function (e) {
                    setTimeout(function () {
                        $(e.target).trigger("change");
                        $(e.target).trigger("focus");
                    });
                });
            },
            showOperators: false
        }
    }
}

function filtroSI_NO() {
    return {
        multi: false,
        messages: {
            isTrue: "Si",
            isFalse: "No"
        }       
    }
}
