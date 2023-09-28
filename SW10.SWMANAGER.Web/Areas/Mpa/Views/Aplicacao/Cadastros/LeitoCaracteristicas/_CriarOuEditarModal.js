(function ($) {
    app.modals.CriarOuEditarLeitoCaracteristicaModal = function () {

        var _leitoCaracteristicasService = abp.services.app.leitoCaracteristica;
        var _modalManager;
        var _$leitoCaracteristicaInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$leitoCaracteristicaInformationForm = _modalManager.getModal().find('form[name=LeitoCaracteristicaInformationsForm]');
            _$leitoCaracteristicaInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$leitoCaracteristicaInformationForm.valid()) {
                return;
            }

            var leitoCaracteristica = _$leitoCaracteristicaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _leitoCaracteristicasService.criarOuEditar(leitoCaracteristica)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarLeitoCaracteristicaModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

    };
})(jQuery);