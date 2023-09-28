(function ($) {
    app.modals.CriarOuEditarPacientesModal = function () {

        var _pacientesService = abp.services.app.paciente;
        var _modalManager;
        var _$pacienteInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$pacienteInformationForm = _modalManager.getModal().find('form[name=PacienteInformationsForm]');
            _$pacienteInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
            $('div.form-group select').addClass('form-control selectpicker');
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        };

        this.save = function () {
            if (!_$pacienteInformationForm.valid()) {
                return;
            }

            var paciente = _$pacienteInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _pacientesService.criarOuEditar(paciente)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarPacienteModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);