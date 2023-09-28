(function ($) {
    app.modals.CriarOuEditarPaisModal = function () {

        var _paisesService = abp.services.app.pais;

        var _modalManager;
        var _$paisInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$paisInformationForm = _modalManager.getModal().find('form[name=PaisInformationsForm]');
            _$paisInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$paisInformationForm.valid()) {
                return;
            }

            var pais = _$paisInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _paisesService.criarOuEditar(pais)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarPaisModalSaved');
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