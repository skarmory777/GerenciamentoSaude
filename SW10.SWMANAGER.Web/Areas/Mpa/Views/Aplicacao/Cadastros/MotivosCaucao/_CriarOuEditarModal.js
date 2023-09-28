(function ($)
{
    app.modals.CriarOuEditarMotivoCaucaoModal = function ()
    {
        var _MotivosCaucaoService = abp.services.app.motivoCaucao;

        var _modalManager;
        var _$MotivoCaucaoInformationForm = null;

        this.init = function (modalManager)
        {
            _modalManager = modalManager;
            _$MotivoCaucaoInformationForm = _modalManager.getModal().find('form[name=MotivoCaucaoInformationsForm]');
            _$MotivoCaucaoInformationForm.validate();
        };

        this.save = function ()
        {
            if (!_$MotivoCaucaoInformationForm.valid())
            {
                return;
            }

            var motivoCaucao = _$MotivoCaucaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _MotivosCaucaoService.criarOuEditar(motivoCaucao)
                 .done(function ()
                 {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarMotivoCaucaoModalSaved');
                 })
                .always(function ()
                {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);