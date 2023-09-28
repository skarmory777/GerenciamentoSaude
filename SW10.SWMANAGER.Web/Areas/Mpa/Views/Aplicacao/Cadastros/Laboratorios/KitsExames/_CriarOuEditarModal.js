(function ($) {
    app.modals.CriarOuEditarKitExameModal = function () {
        var _kitsExamesService = abp.services.app.kitExame;

        var _modalManager;
        var _$KitsExamesInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$KitExameInformationForm = _modalManager.getModal().find('form[name=KitExameInformationsForm]');
            _$KitExameInformationForm.validate();
        };

        this.save = function () {
            if (!_$KitExameInformationForm.valid()) {
                return;
            }

            var KitExame = _$KitExameInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _kitsExamesService.criarOuEditar(KitExame)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarKitExameModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);