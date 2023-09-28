(function ($) {
    app.modals.CriarOuEditarProfissaoModal = function () {

        var _profissoesService = abp.services.app.profissao;

        var _modalManager;
        var _$profissaoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$profissaoInformationForm = _modalManager.getModal().find('form[name=ProfissaoInformationsForm]');
            _$profissaoInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$profissaoInformationForm.valid()) {
                return;
            }

            var profissao = _$profissaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _profissoesService.criarOuEditar(profissao)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarProfissaoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        aplicarDateSingle();
        aplicarDateRange();
        aplicarSelect2Padrao();
        CamposRequeridos();

    };
})(jQuery);