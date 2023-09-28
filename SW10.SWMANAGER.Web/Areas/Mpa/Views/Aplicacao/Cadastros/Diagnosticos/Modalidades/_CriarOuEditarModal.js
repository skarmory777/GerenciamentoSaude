(function ($) {
    app.modals.CriarOuEditarModalidadeModal = function () {
        var _ModalidadesService = abp.services.app.modalidade;

        var _modalManager;
        var _$ModalidadesInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ModalidadeInformationForm = _modalManager.getModal().find('form[name=ModalidadeInformationsForm]');
            _$ModalidadeInformationForm.validate();
            $('#isParecer').click();
        };

        this.save = function () {
            if (!_$ModalidadeInformationForm.valid()) {
                return;
            }

            var Modalidade = _$ModalidadeInformationForm.serializeFormToObject();

            Modalidade.IsParecer = $('#isParecer').swChkValor();

            _modalManager.setBusy(true);

            _ModalidadesService.criarOuEditar(Modalidade)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarModalidadeModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);