namespace("sam");

(function ($, undefinied) {

    "use strict";

    sam.init = (function () {

        // Clone original methods we want to call into
        var originalMethods = {
            min: $.validator.methods.min,
            max: $.validator.methods.max,
            range: $.validator.methods.range
        };

        // Tell the validator that we want numbers parsed using Globalize
        $.validator.methods.number = function (value, element) {
            var val = Globalize.parseFloat(value);
            return this.optional(element) || ($.isNumeric(val));
        };

        // Tell the validator that we want dates parsed using Globalize
        $.validator.methods.date = function (value, element) {
            var val = Globalize.parseDate(value);
            return this.optional(element) || (val);
        };

        // Tell the validator that we want numbers parsed using Globalize, 
        // then call into original implementation with parsed value
        $.validator.methods.min = function (value, element, param) {
            var val = Globalize.parseFloat(value);
            return originalMethods.min.call(this, val, element, param);
        };

        $.validator.methods.max = function (value, element, param) {
            var val = Globalize.parseFloat(value);
            return originalMethods.max.call(this, val, element, param);
        };

        $.validator.methods.range = function (value, element, param) {
            var val = Globalize.parseFloat(value);
            return originalMethods.range.call(this, val, element, param);
        };

        $.validator.unobtrusive.adapters.add("requiredif", ["source", "value"], function (options) {
            options.rules["requiredif"] = {
                source: options.params["source"],
                value: options.params["value"]
            };
            options.messages["requiredif"] = options.message;
        });

        $.validator.addMethod("requiredif", function (value, element, param) {
            var sourceValue = $("input[id^='" + param.source + "']:checked").val();

            if (param.value === sourceValue) {
                return $.validator.methods.required.call(this, value, element, param);
            }

            return true;
        });

        var setCulture = function() {
            var currentCulture = $("html").prop("lang");
            if (currentCulture) {
                Globalize.culture(currentCulture);
                i18n.init({ lng: currentCulture, fallbackLng: "es-MX", resGetPath: "/locales?lng=__lng__&ns=__ns__" });
            } else {
                Globalize.culture("es-MX");
                i18n.init({ lng: "es-MX", resGetPath: "/locales?lng=__lng__&ns=__ns__" });
            }
        };

        var setupDatepickers = function () {
            //sets up the stuff
            var lang = "es",
                dateFormat = "dd/mm/yyyy";

            if (Globalize.culture().name.contains("en")) {
                lang = "en";
                dateFormat = "mm/dd/yyyy";
            }

            $(".input-group.date").datepicker({ autoclose: true, language: lang, format: dateFormat });
        };

        var bindLanguageSwitcher = function() {
            $("#spanish").click(function () {
                sam.utils.switchLanguage("es-MX");
            });

            $("#english").click(function () {
                sam.utils.switchLanguage("en-US");
            });
        };

        var setupPinger = function() {
            setInterval(function() {
                $.get("/home/ping");
            }, 300000);
        };

        $(function () {
            setCulture();
            setupDatepickers();
            bindLanguageSwitcher();
            setupPinger();
        });

    })(); //Singleton

})(jQuery);