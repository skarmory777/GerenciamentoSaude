(function ($) {
    app.modals.CriarOuEditarCapituloCIDModal = function () {

        var _CapitulosCIDService = abp.services.app.capituloCID;

        var _modalManager;
        var _$CapituloCIDInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$CapituloCIDInformationForm = _modalManager.getModal().find('form[name=CapituloCIDInformationsForm]');
            _$CapituloCIDInformationForm.validate();
        };

        this.save = function () {
            if (!_$CapituloCIDInformationForm.valid()) {
                return;
            }

            var capituloCID = _$CapituloCIDInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _CapitulosCIDService.criarOuEditar(capituloCID)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarCapituloCIDModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

    };
})(jQuery);