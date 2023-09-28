(function ($) {
    app.modals.CriarOuEditarOrcamentosModal = function () {

        var _orcamentosService = abp.services.app.orcamento;
        var _modalManager;
        var _$orcamentoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$orcamentoInformationForm = _modalManager.getModal().find('form[name=OrcamentoInformationsForm]');
            _$orcamentoInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
            $('div.form-group select').addClass('form-control selectpicker');
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        };

        this.save = function () {
            if (!_$orcamentoInformationForm.valid()) {
                return;
            }

            var orcamento = _$orcamentoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _orcamentosService.criarOuEditar(orcamento)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarOrcamentoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);