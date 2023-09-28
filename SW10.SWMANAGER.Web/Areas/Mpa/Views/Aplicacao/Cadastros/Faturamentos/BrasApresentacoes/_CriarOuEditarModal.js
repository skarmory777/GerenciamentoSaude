(function ($) {
    app.modals.CriarOuEditarFaturamentoBrasApresentacaoModal = function () {

        var _brasApresentacoesService = abp.services.app.faturamentoBrasApresentacao;
        var _modalManager;
        var _$brasApresentacaoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$brasApresentacaoInformationForm = _modalManager.getModal().find('form[name=BrasApresentacaoInformationsForm]');
            _$brasApresentacaoInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '600px');
        };

        this.save = function () {
            if (!_$brasApresentacaoInformationForm.valid()) {
                return;
            }

            var brasApresentacao = _$brasApresentacaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _brasApresentacoesService.criarOuEditar(brasApresentacao)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarBrasApresentacaoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);