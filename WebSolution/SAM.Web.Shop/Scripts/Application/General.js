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