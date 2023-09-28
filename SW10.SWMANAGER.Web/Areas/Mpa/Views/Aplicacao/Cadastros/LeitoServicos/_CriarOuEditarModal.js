(function ($) {
    app.modals.CriarOuEditarLeitoServicoModal = function () {

        var _leitoServicosService = abp.services.app.leitoServico;
        var _modalManager;
        var _$leitoServicoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$leitoServicoInformationForm = _modalManager.getModal().find('form[name=LeitoServicoInformationsForm]');
            _$leitoServicoInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$leitoServicoInformationForm.valid()) {
                return;
            }

            var leitoServico = _$leitoServicoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _leitoServicosService.criarOuEditar(leitoServico)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarLeitoServicoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

    };
})(jQuery);