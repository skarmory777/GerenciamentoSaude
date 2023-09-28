(function ($) {
    app.modals.CriarOuEditarModal = function () {

        var _contasMedicasService = abp.services.app.conta;

        var _modalManager;
        var _$contaMedicaInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$contaMedicaInformationForm = _modalManager.getModal().find('form[name=ContaMedicaInformationsForm]');
            _$contaMedicaInformationForm.validate({ ignore: "" });
            $('.modal-dialog:last').css('width', '1250px');
        };

        this.save = function () {
            if (!_$contaMedicaInformationForm.valid()) {
                return;
            }

            var contaMedica = _$contaMedicaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _contasMedicasService.criarOuEditar(contaMedica)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarContaMedicaModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);