(function () {
    app.modals.ImpressorasLaboratorioModal = function() {
        var _$impressoraForm = null;
        var _modalManager;
        this.init = function(modalManager) {
            _modalManager = modalManager;
            if ($.cookie("impressora_laboratorio")) {
                $("input[name='impressora_laboratorio']").val($.cookie("impressora_laboratorio"));
            }
            
            _$impressoraForm = _modalManager.getModal().find('form[name=impressoraForm]');
            _$impressoraForm.validate({
                ignore: "",
                rules: {
                    impressora_laboratorio: "required"
                }
            });
        };

        this.save = function() {

            if (!_$impressoraForm.valid()) {
                return;
            }

            _modalManager.setBusy(true);

            const cookieInfo = { expires: 10950, path: "/" };  // 30 anos

            $.cookie("impressora_laboratorio",
                $("input[name='impressora_laboratorio']").val(),
                cookieInfo);
            _modalManager.close();
        };
    };

})();