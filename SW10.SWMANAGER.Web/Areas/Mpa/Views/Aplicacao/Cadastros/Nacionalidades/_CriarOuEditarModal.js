(function ($) {
    app.modals.CriarOuEditarNacionalidadeModal = function () {

        var _nacionalidadesService = abp.services.app.nacionalidade;

        var _modalManager;
        var _$nacionalidadeInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$nacionalidadeInformationForm = _modalManager.getModal().find('form[name=NacionalidadeInformationsForm]');
            _$nacionalidadeInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$nacionalidadeInformationForm.valid()) {
                return;
            }

            var nacionalidade = _$nacionalidadeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _nacionalidadesService.criarOuEditar(nacionalidade)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarNacionalidadeModalSaved');
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