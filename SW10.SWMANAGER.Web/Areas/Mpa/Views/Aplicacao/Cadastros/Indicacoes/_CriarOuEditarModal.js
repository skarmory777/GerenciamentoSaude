(function ($) {
    app.modals.CriarOuEditarIndicacaoModal = function () {

        var _indicacaosService = abp.services.app.indicacao;

        var _modalManager;
        var _$indicacaoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$indicacaoInformationForm = _modalManager.getModal().find('form[name=IndicacaoInformationsForm]');
            _$indicacaoInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$indicacaoInformationForm.valid()) {
                return;
            }

            var indicacao = _$indicacaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _indicacaosService.criarOuEditar(indicacao)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarIndicacaoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
    aplicarDateRange();
    aplicarDateSingle();
    aplicarSelect2Padrao();
    CamposRequeridos();
})(jQuery);