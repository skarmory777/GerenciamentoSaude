(function ($) {
    app.modals.CriarOuEditarNaturalidadeModal = function () {

        var _naturalidadesService = abp.services.app.naturalidade;

        var _modalManager;
        var _$naturalidadeInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$naturalidadeInformationForm = _modalManager.getModal().find('form[name=NaturalidadeInformationsForm]');
            _$naturalidadeInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$naturalidadeInformationForm.valid()) {
                return;
            }

            var naturalidade = _$naturalidadeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _naturalidadesService.criarOuEditar(naturalidade)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarNaturalidadeModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        aplicarDateRange();
        aplicarDateSingle();
        CamposRequeridos();
        aplicarSelect2Padrao();
    };
})(jQuery);