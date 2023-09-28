(function () {
    app.modals.ImpressorasModal = function() {
        var _$impressoraForm = null;
        var _modalManager;
        this.init = function(modalManager) {
            _modalManager = modalManager;
            if ($.cookie("impressora_etiqueta_visitante")) {
                $("input[name='impressora_etiqueta_visitante']").val($.cookie("impressora_etiqueta_visitante"));
            }

            if (!$.cookie("impressora_etiqueta_paciente")) {
                $("input[name='impressora_etiqueta_paciente']").val($.cookie("impressora_etiqueta_paciente"));
            }

            if (!$.cookie("impressora_pulseira")) {
                $("input[name='impressora_pulseira']").val($.cookie("impressora_pulseira"));
            }

            if (!$.cookie("impressora_terminal_de_senha")) {
                $("input[name='impressora_terminal_de_senha']").val($.cookie("impressora_terminal_de_senha"));
            }
            
            _$impressoraForm = _modalManager.getModal().find('form[name=impressoraForm]');
            _$impressoraForm.validate({
                ignore: "",
                rules: {
                    impressora_etiqueta_visitante: "required",
                    impressora_etiqueta_paciente: "required",
                    impressora_terminal_de_senha: "required",
                }
            });
        };

        this.save = function() {

            if (!_$impressoraForm.valid()) {
                return;
            }

            _modalManager.setBusy(true);

            var cookieInfo = { expires: 10950, path: "/" };  // 30 anos

            $.cookie("impressora_etiqueta_visitante",
                $("input[name='impressora_etiqueta_visitante']").val(),
                cookieInfo);

            $.cookie("impressora_etiqueta_paciente", $("input[name='impressora_etiqueta_paciente']").val(), cookieInfo);

            $.cookie("impressora_pulseira", $("input[name='impressora_pulseira']").val(), cookieInfo);

            $.cookie("impressora_terminal_de_senha", $("input[name='impressora_terminal_de_senha']").val(), cookieInfo);
            _modalManager.close();
        };
    };

})();