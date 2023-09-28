(function ($) {
    app.modals.CriarOuEditarMetodoModal = function () {
        var _metodosService = abp.services.app.metodo;

        var _modalManager;
        var _$MetodosInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$MetodoInformationForm = _modalManager.getModal().find('form[name=MetodoInformationsForm]');
            _$MetodoInformationForm.validate();
        };

        this.save = function () {
            if (!_$MetodoInformationForm.valid()) {
                return;
            }

            var Metodo = _$MetodoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _metodosService.criarOuEditar(Metodo)
                .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarMetodoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);