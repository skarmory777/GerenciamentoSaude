(function ($) {
    app.modals.CriarOuEditarGruposCentroCustoModal = function () {

        var _grupoCentroCustoService = abp.services.app.grupoCentroCusto;
        var _modalManager;
        var _$grupoCentroCustoInformationForm = null;

        
        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$grupoCentroCustoInformationForm = _modalManager.getModal().find('form[name=GrupoCentroCustoInformationsForm]');
            _$grupoCentroCustoInformationForm.validate();
            //$('.modal-dialog').css({ 'width': '35%', 'max-width': '600px' });
            //$('select').addClass('form-control edited');

            
        };

        this.save = function () {
            if (!_$grupoCentroCustoInformationForm.valid()) {
                return;
            }
            var _grupoCentroCusto = _$grupoCentroCustoInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);

            _grupoCentroCustoService.criarOuEditar(_grupoCentroCusto)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarGrupoCentroCustoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });

        //$('#Estado').change(function (data) {

        //});
        
        aplicarDateRange();
        aplicarDateSingle();
        aplicarSelect2Padrao();
        CamposRequeridos();
    };
})(jQuery);