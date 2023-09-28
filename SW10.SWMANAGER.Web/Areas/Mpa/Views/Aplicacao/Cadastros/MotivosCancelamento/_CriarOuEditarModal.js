(function ($) {
    app.modals.CriarOuEditarMotivoCancelamentoModal = function ()
    {
        var _MotivosCancelamentoService = abp.services.app.motivoCancelamento;

        var _modalManager;
        var _$MotivoCancelamentoInformationForm = null;

        this.init = function (modalManager)
        {
            _modalManager = modalManager;
            _$MotivoCancelamentoInformationForm = _modalManager.getModal().find('form[name=MotivoCancelamentoInformationsForm]');
            _$MotivoCancelamentoInformationForm.validate();
        };

        this.save = function ()
        {
            if (!_$MotivoCancelamentoInformationForm.valid())
            {
                return;
            }

            var motivoCancelamento = _$MotivoCancelamentoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _MotivosCancelamentoService.criarOuEditar(motivoCancelamento)
                 .done(function ()
                 {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarMotivoCancelamentoModalSaved');
                 })
                .always(function ()
                {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);