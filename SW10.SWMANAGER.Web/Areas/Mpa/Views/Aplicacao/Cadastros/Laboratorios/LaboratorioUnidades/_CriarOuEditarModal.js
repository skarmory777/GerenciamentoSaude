(function ($) {
    app.modals.CriarOuEditarLaboratorioUnidadeModal = function () {
        var _laboratoriosunidadesService = abp.services.app.laboratorioUnidade;

        var _modalManager;
        var _$LaboratorioUnidadesInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$LaboratorioUnidadeInformationForm = _modalManager.getModal().find('form[name=LaboratorioUnidadeInformationsForm]');
            _$LaboratorioUnidadeInformationForm.validate();
        };

        this.save = function () {
            if (!_$LaboratorioUnidadeInformationForm.valid()) {
                return;
            }

            var LaboratorioUnidade = _$LaboratorioUnidadeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _laboratoriosunidadesService.criarOuEditar(LaboratorioUnidade)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarLaboratorioUnidadeModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);