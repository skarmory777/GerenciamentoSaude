(function ($) {
    app.modals.CriarOuEditarSetorModal = function () {
        var _setorsService = abp.services.app.setor;

        var _modalManager;
        var _$SetorsInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$SetorInformationForm = _modalManager.getModal().find('form[name=SetorInformationsForm]');
            _$SetorInformationForm.validate();
        };

        this.save = function () {
            if (!_$SetorInformationForm.valid()) {
                return;
            }

            var Setor = _$SetorInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _setorsService.criarOuEditar(Setor)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarSetorModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        
    };
})(jQuery);