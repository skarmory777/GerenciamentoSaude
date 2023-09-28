(function ($) {
    app.modals.CriarOuEditarTipoTabelaDominioModal = function () {

        var _TiposTabelaDominioService = abp.services.app.tipoTabelaDominio;

        var _modalManager;
        var _$TipoTabelaDominioInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TipoTabelaDominioInformationForm = _modalManager.getModal().find('form[name=TipoTabelaDominioInformationsForm]');
            _$TipoTabelaDominioInformationForm.validate();

            // Preencher dropdown com ultimo codigo salvo caso haja (e caso nao seja 'edit mode'
            if (typeof ($('[name=Id').val()) === 'undefined')
                codigoAutoSet($("#ultimo-codigo-salvo").attr("data"), "#codigo-tipo-tabela-dominio-id");
        };

        this.save = function () {
            if (!_$TipoTabelaDominioInformationForm.valid()) {
                return;
            }

            var tipoTabelaDominio = _$TipoTabelaDominioInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TiposTabelaDominioService.criarOuEditar(tipoTabelaDominio)
                 .done(function () {

                     // Manter registro do ultimo codigo salvo em div: data
                     $("#ultimo-codigo-salvo").attr("data", $("#codigo-tipo-tabela-dominio-id").val());

                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoTabelaDominioModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        // Atribuindo ultimo Codigo salvo ao input
        function codigoAutoSet(ultimoCodigoSalvo, input) {

            // Falta checar se codigo ja existe

            if (ultimoCodigoSalvo != "nenhum") {
                if (~ultimoCodigoSalvo.indexOf(".")) {
                    var casasDecimais = (parseInt(ultimoCodigoSalvo.substr(ultimoCodigoSalvo.indexOf(".") + 1)) + 1).toString();
                    var parteInteira = (parseInt(ultimoCodigoSalvo.substr(0, ultimoCodigoSalvo.indexOf('.'))));
                    if (casasDecimais == "100") {
                        ultimoCodigoSalvo = (parteInteira + 1).toString();
                    } else {
                        ultimoCodigoSalvo = parteInteira + "." + casasDecimais;
                    }
                } else {
                    ultimoCodigoSalvo = ultimoCodigoSalvo + ".1";
                }

                $(input).val(ultimoCodigoSalvo).addClass('edited');
            }
        };

    };
})(jQuery);