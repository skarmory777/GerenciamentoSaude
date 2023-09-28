(function ($) {
    app.modals.CriarOuEditarTipoLeitoModal = function () {

        var _TiposLeitoService = abp.services.app.tipoLeito;

        var _modalManager;
        var _$TipoLeitoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TipoLeitoInformationForm = _modalManager.getModal().find('form[name=TipoLeitoInformationsForm]');
            _$TipoLeitoInformationForm.validate();
        };

        this.save = function () {
            if (!_$TipoLeitoInformationForm.valid()) {
                return;
            }

            var tipoLeito = _$TipoLeitoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TiposLeitoService.criarOuEditar(tipoLeito)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoLeitoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);