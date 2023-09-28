(function ($) {
    app.modals.CriarOuEditarTecnicoModal = function () {
        var _tecnicosService = abp.services.app.tecnico;

        var _modalManager;
        var _$TecnicosInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TecnicoInformationForm = _modalManager.getModal().find('form[name=TecnicoInformationsForm]');
            _$TecnicoInformationForm.validate();
        };

        this.save = function () {
            if (!_$TecnicoInformationForm.valid()) {
                return;
            }

            var Tecnico = _$TecnicoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _tecnicosService.criarOuEditar(Tecnico)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarTecnicoModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);