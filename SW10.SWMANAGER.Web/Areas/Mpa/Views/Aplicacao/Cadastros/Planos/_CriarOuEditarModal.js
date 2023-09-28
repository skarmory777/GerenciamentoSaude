(function ($) {
    app.modals.CriarOuEditarPlanoModal = function () {

        var _planosService = abp.services.app.plano;

        var _modalManager;
        var _$planoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$planoInformationForm = _modalManager.getModal().find('form[name=PlanoInformationsForm]');
            _$planoInformationForm.validate();

            $('.modal-dialog:last').css('top', '50px');
            abp.ui.clearBusy()
        };

        this.save = function () {
            if (!_$planoInformationForm.valid()) {
                return;
            }

            var conf = $('#convenio-id').val();
            if (conf === 0) {
                abp.notify.info(app.localize("ConvenioInvalido"));
                $('#convenio-id').val(0);
                $('#convenio-search').focus();
                return;
            }

            var plano = _$planoInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            plano.ConvenioId = conf;

            _planosService.criarOuEditar(plano)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarPlanoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        CamposRequeridos();
        aplicarDateSingle();
        aplicarDateRange();
        aplicarSelect2Padrao();
        $('.select2').css('width', '100%');
    };

})(jQuery);