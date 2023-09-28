(function ($) {
    app.modals.CriarOuEditarFormatacaoModal = function () {

        var _formatacaoService = abp.services.app.formatacao;

        var _modalManager;
        var _$formatacaoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$formatacaoInformationForm = _modalManager.getModal().find('form[name=formatacaoInformationsForm]');
            _$formatacaoInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$formatacaoInformationForm.valid()) {
                return;
            }

            var formatacao = _$formatacaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _formatacaoService.criarOuEditar(formatacao)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarFormatacaoModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);