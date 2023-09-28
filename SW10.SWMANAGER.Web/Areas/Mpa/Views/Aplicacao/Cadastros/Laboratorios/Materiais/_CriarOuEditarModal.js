(function ($) {
    app.modals.CriarOuEditarMaterialModal = function () {
        var _materiaisService = abp.services.app.material;

        var _modalManager;
        var _$MateriaisInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$MaterialInformationForm = _modalManager.getModal().find('form[name=MaterialInformationsForm]');
            _$MaterialInformationForm.validate();
        };

        this.save = function () {
            if (!_$MaterialInformationForm.valid()) {
                return;
            }

            var Material = _$MaterialInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _materiaisService.criarOuEditar(Material)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarMaterialModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);