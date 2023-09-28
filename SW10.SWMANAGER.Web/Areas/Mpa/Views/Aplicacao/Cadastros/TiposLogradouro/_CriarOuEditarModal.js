(function ($) {
    app.modals.CriarOuEditarTipoLogradouroModal = function () {

        var _tipoLogradouroService = abp.services.app.tipoLogradouro;

        var _modalManager;
        var _$tipoLogradouroInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$tipoLogradouroInformationForm = _modalManager.getModal().find('form[name=TipoLogradouroInformationsForm]');
            _$tipoLogradouroInformationForm.validate();
            $('.modal-dialog').css('width', '90%');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$tipoLogradouroInformationForm.valid()) {
                return;
            }

            var _tipoLogradouro = _$tipoLogradouroInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _tipoLogradouroService.criarOuEditar(_tipoLogradouro)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTipoLogradouroModalSaved');
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
        CamposRequeridos();
        aplicarSelect2Padrao();

    };
})(jQuery);