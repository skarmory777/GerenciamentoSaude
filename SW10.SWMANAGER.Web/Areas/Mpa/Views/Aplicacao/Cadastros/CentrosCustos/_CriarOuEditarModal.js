(function ($) {
    app.modals.CriarOuEditarCentroCustoModal = function () {

        var _CentrosCustosService = abp.services.app.centroCusto;

        var _modalManager;
        var _$CentroCustoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$CentroCustoInformationForm = _modalManager.getModal().find('form[name=CentroCustoInformationsForm]');
            _$CentroCustoInformationForm.validate();

            $('.modal-dialog').css({ 'width': '60%', 'max-width': '500px' });
        };

        this.save = function () {
            if (!_$CentroCustoInformationForm.valid()) {
                return;
            }

            var centroCusto = _$CentroCustoInformationForm.serializeFormToObject();

            //console.log(JSON.stringify(centroCusto));

            _modalManager.setBusy(true);
            _CentrosCustosService.criarOuEditar(centroCusto)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarCentroCustoModalSaved');
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