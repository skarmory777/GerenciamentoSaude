(function ($) {
    app.modals.CriarOuEditarPreAtendimentoModal = function () {

        var _modalManager;
        var _$preAtendimentoInformationForm = null;

        var _AtendimentosService = abp.services.app.atendimento;

        //console.log("focus");
        $('#atendimentoTipo').focus();

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$preAtendimentoInformationForm = _modalManager.getModal().find('form[name=PreAtendimentoInformationsForm]');
            $('.modal-dialog').css({ 'width': '800%', 'max-width': '1320px' });
            _$preAtendimentoInformationForm.validate({ ignore: "" });
        };

        this.save = function () {

            if (!_$preAtendimentoInformationForm.valid()) {
                return;
            }

            var preAtendimento = _$preAtendimentoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _AtendimentosService.criarOuEditar(preAtendimento)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarPreAtendimentoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);