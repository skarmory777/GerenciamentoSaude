(function ($) {
    app.modals.CriarOuEditarTipoConta = function () {

        let _args = null;

        $(document).ready(function () {
            CamposRequeridos();
        });

        var _tipoContaService = abp.services.app.tipoContaCorrente;

        var _modalManager;
        var _$tipoContaInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            
            _args = modalManager.getArgs();
            _modalManager = modalManager;
            App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';

            _$tipoContaInformationsForm = _modalManager.getModal().find('form[name=tipoContaInformationsForm]');
            _$tipoContaInformationsForm.validate();
        };

        this.save = function () {
            
            if (!_$tipoContaInformationsForm.valid()) {
                return;
            }

            var tipoConta = _$tipoContaInformationsForm.serializeFormToObject();
            
            _modalManager.setBusy(true);

            App.startPageLoading({ animate: true });document.querySelector('.loadingCommon').style.display = null;
            _tipoContaService.criarOuEditar(tipoConta)
                 .done(function (data) {
                     App.stopPageLoading();document.querySelector('.loadingCommon').style.display = 'none';
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
                    App.stopPageLoading();document.querySelector('.loadingCommon').style.display = 'none';
                    _modalManager.setBusy(false);
                });
        };       

    };
})(jQuery);