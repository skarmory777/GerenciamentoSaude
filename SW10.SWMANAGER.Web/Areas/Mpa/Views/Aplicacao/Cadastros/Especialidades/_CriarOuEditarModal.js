(function ($) {
    app.modals.CriarOuEditarEspecialidadeModal = function () {

        var _especialidadesService = abp.services.app.especialidade;

        var _modalManager;
        var _$especialidadeInformationForm = null;

        var fixaModal = false;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$especialidadeInformationForm = _modalManager.getModal().find('form[name=EspecialidadeInformationsForm]');
            _$especialidadeInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css('width', '600px');

            _modalManager.getModal().find('#div-btn-fixa-modal').show();

            var btnFixaModal = _modalManager.getModal().find('#btn-fixa-modal:last');

            btnFixaModal.on('click', function (e) {
                fixaModal = !fixaModal;
                if (fixaModal) {
                    btnFixaModal.addClass('blue');
                } else {
                    btnFixaModal.removeClass('blue');
                }
            });
        };

        this.save = function () {
            if (!_$especialidadeInformationForm.valid()) {
                return;
            }

            var especialidade = _$especialidadeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _especialidadesService.criarOuEditar(especialidade)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));

                     var editMode = $('#is-edit-mode').val();

                     // Fixar modal ou nao, apos save
                     if (!fixaModal) {
                         _modalManager.close();
                     } else {
                         if (!editMode) {
                             limparFormulario();
                         };

                         $('#descricao-id').focus();
                     };

                     abp.event.trigger('app.CriarOuEditarEspecialidadeModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        function limparFormulario() {
            $('.ohyeah').val("");
            $('.combo-cboclass').val("");
            $('.combo-cboclass').change();
            $('.combo-cbosusclass').val("");
            $('.combo-cbosusclass').change();
            $('#chk-is-ativo').attr('checked', false);
        };
    };
})(jQuery);