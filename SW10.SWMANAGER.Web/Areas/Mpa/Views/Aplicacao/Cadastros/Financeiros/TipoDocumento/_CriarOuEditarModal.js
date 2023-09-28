(function ($) {
    app.modals.CriarOuEditarTipoDocumentoModal = function () {

        let _args = null;

        $(document).ready(function () {
            CamposRequeridos();
        });

        var _tipoDocumentoService = abp.services.app.tipoDocumento;

        var _modalManager;
        var _$tipoDocumentoInformationsForm = null;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {            
            _args = modalManager.getArgs();
            _modalManager = modalManager;
            App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
            _$tipoDocumentoInformationsForm = _modalManager.getModal().find('form[name=tipoDocumentoInformationsForm]');
            _$tipoDocumentoInformationsForm.validate();
        };

        this.save = function () {

            if (!_$tipoDocumentoInformationsForm.valid()) {
                return;
            }

            var tipoDocumento = _$tipoDocumentoInformationsForm.serializeFormToObject();
            _modalManager.setBusy(true);

            App.startPageLoading({ animate: true }); document.querySelector('.loadingCommon').style.display = null;
            _tipoDocumentoService.criarOuEditar(tipoDocumento)
                 .done(function (data) {
                     App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                     }
                 })
                .always(function () {
                    App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);