(function ($) {
    app.modals.CriarOuEditarRegiaoModal = function () {

        var _RegioesService = abp.services.app.regiao;

        var _modalManager;
        var _$RegiaoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$RegiaoInformationForm = _modalManager.getModal().find('form[name=RegiaoInformationsForm]');
            _$RegiaoInformationForm.validate();
        };

        this.save = function () {
            if (!_$RegiaoInformationForm.valid()) {
                return;
            }

            var regiao = _$RegiaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _RegioesService.criarOuEditar(regiao)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarRegiaoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);