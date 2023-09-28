(function ($) {
    app.modals.CriarOuEditarIntervaloModal = function () {

        var _intervalosService = abp.services.app.intervalo;

        var _modalManager;
        var _$intervaloInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$intervaloInformationForm = _modalManager.getModal().find('form[name=IntervaloInformationsForm]');
            _$intervaloInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$intervaloInformationForm.valid()) {
                return;
            }

            var intervalo = _$intervaloInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _intervalosService.criarOuEditar(intervalo)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarIntervaloModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        aplicarDateRange();
        aplicarDateSingle();
        aplicarSelect2Padrao();
        CamposRequeridos();
    };
})(jQuery);