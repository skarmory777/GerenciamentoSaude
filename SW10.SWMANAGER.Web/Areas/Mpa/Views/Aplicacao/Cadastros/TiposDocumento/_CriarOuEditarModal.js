(function ($) {
    app.modals.CriarOuEditarTipoDocumentoModal = function () {

        var _TiposDocumentoService = abp.services.app.tipoDocumento;

        var _modalManager;
        var _$TipoDocumentoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TipoDocumentoInformationForm = _modalManager.getModal().find('form[name=TipoDocumentoInformationsForm]');
            _$TipoDocumentoInformationForm.validate();
        };

        this.save = function () {
            if (!_$TipoDocumentoInformationForm.valid()) {
                return;
            }

            var tipoDocumento = _$TipoDocumentoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TiposDocumentoService.criarOuEditar(tipoDocumento)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoDocumentoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);