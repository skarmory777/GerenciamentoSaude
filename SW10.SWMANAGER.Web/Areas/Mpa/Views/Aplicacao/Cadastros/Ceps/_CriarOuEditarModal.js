(function ($) {
    app.modals.CriarOuEditarCepModal = function () {

        var _cepsService = abp.services.app.cep;

        var _modalManager;
        var _$cepsInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$cepInformationForm = _modalManager.getModal().find('form[name=CepInformationsForm]');
            _$cepInformationForm.validate();
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            if (!_$cepInformationForm.valid()) {
                return;
            }

            var cep = _$cepInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _cepsService.criarOuEditar(cep)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarCepModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
        $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });

        $('#Estado').change(function (data) {

        });
        $('#btn-buscar-cep').on('click', function (e) {
            e.preventDefault();

            if ($('#CEP').val() == "") {
                abp.notify.error(app.localize('InformarCep'));
            }
            else
                buscarCep($('#CEP').val());
        });
        aplicarDateRange();
        aplicarDateSingle();
        aplicarSelect2Padrao();
        CamposRequeridos();
    };
})(jQuery);