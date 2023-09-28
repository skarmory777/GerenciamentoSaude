(function ($) {
    app.modals.CriarOuEditarFaturamentoBrasLaboratorioModal = function () {

        var _brasLaboratoriosService = abp.services.app.faturamentoBrasLaboratorio;
        var _modalManager;
        var _$brasLaboratorioInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$brasLaboratorioInformationForm = _modalManager.getModal().find('form[name=BrasLaboratorioInformationsForm]');
            _$brasLaboratorioInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '600px');
        };

        this.save = function () {
            if (!_$brasLaboratorioInformationForm.valid()) {
                return;
            }

            var brasLaboratorio = _$brasLaboratorioInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _brasLaboratoriosService.criarOuEditar(brasLaboratorio)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarBrasLaboratorioModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);