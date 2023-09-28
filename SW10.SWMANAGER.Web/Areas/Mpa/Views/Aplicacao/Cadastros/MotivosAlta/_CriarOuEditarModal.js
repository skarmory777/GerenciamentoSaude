(function ($) {
    app.modals.CriarOuEditarMotivoAltaModal = function () {

        var _motivosAltaService = abp.services.app.motivoAlta;
        var _modalManager;
        var _$motivoAltaInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$motivoAltaInformationForm = _modalManager.getModal().find('form[name=MotivoAltaInformationsForm]');
            _$motivoAltaInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$motivoAltaInformationForm.valid()) {
                return;
            }

            var motivoAlta = _$motivoAltaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _motivosAltaService.criarOuEditar(motivoAlta)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarMotivoAltaModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);