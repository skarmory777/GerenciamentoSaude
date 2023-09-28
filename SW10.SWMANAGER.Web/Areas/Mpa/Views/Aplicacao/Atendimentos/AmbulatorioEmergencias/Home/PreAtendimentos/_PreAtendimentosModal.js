(function ($) {
    app.modals.CriarOuEditarPreAtendimentosModal = function () {

        var _preAtendimentosService = abp.services.app.preAtendimento;
        var _modalManager;
        var _$preAtendimentoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$preAtendimentoInformationForm = _modalManager.getModal().find('form[name=PreAtendimentoInformationsForm]');
            _$preAtendimentoInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
            $('div.form-group select').addClass('form-control selectpicker');
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        };

        this.save = function () {
            if (!_$preAtendimentoInformationForm.valid()) {
                return;
            }

            var preAtendimento = _$preAtendimentoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _preAtendimentosService.criarOuEditar(preAtendimento)
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