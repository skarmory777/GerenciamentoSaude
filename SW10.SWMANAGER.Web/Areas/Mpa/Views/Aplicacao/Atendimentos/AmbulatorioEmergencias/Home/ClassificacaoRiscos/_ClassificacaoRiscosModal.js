(function ($) {
    app.modals.CriarOuEditarClassificacaoRiscosModal = function () {

        var _classificacoesRiscoService = abp.services.app.classificacaoRisco;
        var _modalManager;
        var _$classificacaoRiscoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$classificacaoRiscoInformationForm = _modalManager.getModal().find('form[name=ClassificacaoRiscoInformationsForm]');
            _$classificacaoRiscoInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
            $('div.form-group select').addClass('form-control selectpicker');
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        };

        this.save = function () {
            if (!_$classificacaoRiscoInformationForm.valid()) {
                return;
            }

            var classificacaoRisco = _$classificacaoRiscoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _classificacoesRiscoService.criarOuEditar(classificacaoRisco)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarClassificacaoRiscoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);