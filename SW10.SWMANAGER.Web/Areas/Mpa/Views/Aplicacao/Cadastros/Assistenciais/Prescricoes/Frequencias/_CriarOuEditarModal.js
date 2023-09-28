(function ($) {
    app.modals.CriarOuEditarFrequenciaModal = function () {

        var _FrequenciasService = abp.services.app.frequencia;

        var _modalManager;
        var _$FrequenciaInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$FrequenciaInformationForm = _modalManager.getModal().find('form[name=FrequenciaInformationsForm]');
            _$FrequenciaInformationForm.validate();
        };

        this.save = function () {
            if (!_$FrequenciaInformationForm.valid()) {
                return;
            }

            var frequencia = _$FrequenciaInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);
            _FrequenciasService.criarOuEditar(frequencia)
                 .done(function () {
                     abp.notify.success(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarFrequenciaModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('.input-group-addon').on('click', function (e) {
            e.preventDefault();
            if ($('#hora-inicial').val() == '' || $('#hora-inicial').val() == null) {
                abp.notify.error(app.localize('InformarValor'));
                $('#hora-inicial').focus();
                return;
            }
            if ($('#intervalo').length == 0 || $('#intervalo').val() == 0) {
                abp.notify.error(app.localize('InformarValor'));
                $('#intervalo').focus();
                return;
            }
            $('#horarios').val(definirHorarios($('#intervalo').val(), $('#hora-inicial').val()));
        });

    };
})(jQuery);