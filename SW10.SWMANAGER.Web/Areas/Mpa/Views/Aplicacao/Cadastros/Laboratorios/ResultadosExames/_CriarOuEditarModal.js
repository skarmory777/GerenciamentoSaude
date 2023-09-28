(function ($) {
    app.modals.CriarOuEditarResultadoModal = function () {
        var _resultadosExamesService = abp.services.app.resultadoExame;

        var _modalManager;
        var _$ResultadoExamesExamesInformationForm = null;


        this.init = function (modalManager) {

           
            //$('.modal-dialog').css('width', '500px');
            _modalManager = modalManager;
            $('.modal-dialog').css('width', '900px');
            _$ResultadoExameInformationForm = _modalManager.getModal().find('form[name=ResultadoExameInformationsForm]');
            _$ResultadoExameInformationForm.validate();
            $('.select2').css('width', '100%');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            if (!_$ResultadoExameInformationForm.valid()) {
                return;
            }
           

            var ResultadoExame = _$ResultadoExameInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _resultadosExamesService.criarOuEditar(ResultadoExame)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarResultadoExameModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        CamposRequeridos();
        aplicarDateSingle();
       // aplicarSelect2Padrao();

       
        selectSW('.selectExame', "/api/services/app/faturamentoItem/ListarExameLaboratorialDropdown");
      



    };
})(jQuery);