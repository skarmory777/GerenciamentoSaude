(function ($) {
    app.modals.CriarOuEditarTipoEntradaModal = function () {

        var _TiposEntradaService = abp.services.app.tipoEntrada;

        var _modalManager;
        var _$TipoEntradaInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TipoEntradaInformationForm = _modalManager.getModal().find('form[name=TipoEntradaInformationsForm]');
            _$TipoEntradaInformationForm.validate();
        };

        this.save = function () {
            if (!_$TipoEntradaInformationForm.valid()) {
                return;
            }

            var tipoEntrada = _$TipoEntradaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TiposEntradaService.criarOuEditar(tipoEntrada)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoEntradaModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);