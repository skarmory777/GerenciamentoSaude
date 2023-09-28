(function ($) {
    app.modals.CriarOuEditarModal = function () {

        var _InstituicoesTransferenciaService = abp.services.app.instituicaoTransferencia;

        var _modalManager;
        var _$InstituicaoTransferenciaInformationForm = null;


        this.init = function (modalManager) {

            //console.log('modal open caceta');
            _modalManager = modalManager;

            _$InstituicaoTransferenciaInformationForm = _modalManager.getModal().find('form[name=InstituicaoTransferenciaInformationsForm]');
            _$InstituicaoTransferenciaInformationForm.validate();
        };

        this.save = function () {
            if (!_$InstituicaoTransferenciaInformationForm.valid()) {
                return;
            }

            var instituicaoTransferencia = _$InstituicaoTransferenciaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _InstituicoesTransferenciaService.criarOuEditar(instituicaoTransferencia)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarInstituicaoTransferenciaModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);