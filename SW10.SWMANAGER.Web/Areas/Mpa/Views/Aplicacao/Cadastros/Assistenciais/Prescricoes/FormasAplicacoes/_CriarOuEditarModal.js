(function ($) {
    app.modals.CriarOuEditarFormaAplicacaoModal = function () {

        var _FormasAplicacoesService = abp.services.app.formaAplicacao;

        var _modalManager;
        var _$FormaAplicacaoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$FormaAplicacaoInformationForm = _modalManager.getModal().find('form[name=FormaAplicacaoInformationsForm]');
            _$FormaAplicacaoInformationForm.validate();
        };

        this.save = function () {
            if (!_$FormaAplicacaoInformationForm.valid()) {
                return;
            }

            var formaAplicacao = _$FormaAplicacaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _FormasAplicacoesService.criarOuEditar(formaAplicacao)
                 .done(function () {
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarFormaAplicacaoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);