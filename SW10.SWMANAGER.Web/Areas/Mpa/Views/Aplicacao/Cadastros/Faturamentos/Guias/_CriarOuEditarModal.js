(function ($) {
    app.modals.CriarOuEditarFaturamentoGuiaModal = function () {

        var _guiasService = abp.services.app.faturamentoGuia;
        var _modalManager;
        var _$guiaInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$guiaInformationForm = _modalManager.getModal().find('form[name=GuiaInformationsForm]');
            _$guiaInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '650px');
            $('.modal-dialog').css('top', '100px');
        };

        this.save = function () {
            if (!_$guiaInformationForm.valid()) {
                return;
            }

            var guia = _$guiaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _guiasService.criarOuEditar(guia)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarGuiaModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);