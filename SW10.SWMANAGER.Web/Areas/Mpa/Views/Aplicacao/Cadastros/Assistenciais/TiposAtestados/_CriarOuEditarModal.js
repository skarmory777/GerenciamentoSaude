(function ($) {
    app.modals.CriarOuEditarTipoAtestadoModal = function () {

        var _TiposAtestadosService = abp.services.app.tipoAtestado;

        var _modalManager;
        var _$TipoAtestadoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TipoAtestadoInformationForm = _modalManager.getModal().find('form[name=TipoAtestadoInformationsForm]');
            _$TipoAtestadoInformationForm.validate();
        };

        this.save = function () {
            if (!_$TipoAtestadoInformationForm.valid()) {
                return;
            }

            var tipoAtestado = _$TipoAtestadoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TiposAtestadosService.criarOuEditar(tipoAtestado)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoAtestadoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);