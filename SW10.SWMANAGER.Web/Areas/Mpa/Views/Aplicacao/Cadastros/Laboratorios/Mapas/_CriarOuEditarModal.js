(function ($) {
    app.modals.CriarOuEditarMapaModal = function () {
        var _mapasService = abp.services.app.mapa;

        var _modalManager;
        var _$MapasInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$MapaInformationForm = _modalManager.getModal().find('form[name=MapaInformationsForm]');
            _$MapaInformationForm.validate();
        };

        this.save = function () {
            if (!_$MapaInformationForm.valid()) {
                return;
            }

            var Mapa = _$MapaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _mapasService.criarOuEditar(Mapa)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarMapaModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);