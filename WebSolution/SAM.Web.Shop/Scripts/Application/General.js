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