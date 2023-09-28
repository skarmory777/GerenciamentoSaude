(function ($) {
    app.modals.CriarOuEditarMotivoTransferenciaLeitoModal = function () {

        var _MotivosTransferenciaLeitoService = abp.services.app.motivoTransferenciaLeito;

        var _modalManager;
        var _$MotivoTransferenciaLeitoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$MotivoTransferenciaLeitoInformationForm = _modalManager.getModal().find('form[name=MotivoTransferenciaLeitoInformationsForm]');
            _$MotivoTransferenciaLeitoInformationForm.validate();
        };

        this.save = function () {
            if (!_$MotivoTransferenciaLeitoInformationForm.valid()) {
                return;
            }

            var MotivoTransferenciaLeito = _$MotivoTransferenciaLeitoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _MotivosTransferenciaLeitoService.criarOuEditar(MotivoTransferenciaLeito)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarMotivoTransferenciaLeitoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);