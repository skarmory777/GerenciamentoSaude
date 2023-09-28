(function ($) {
    app.modals.CriarOuEditarGrupoDREModal = function () {



        var _grupoDREService = abp.services.app.grupoDRE;

        var _modalManager;
        var _$grupoDREInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$grupoDREInformationsForm = _modalManager.getModal().find('form[name=grupoDREInformationsForm]');
            _$grupoDREInformationsForm.validate();
        };

        this.save = function () {


           
            //if (!_$FormaPagamentoInformationsForm.valid()) {
            //    return;
            //}

            var grupoDRE = _$grupoDREInformationsForm.serializeFormToObject();


            _modalManager.setBusy(true);
            _grupoDREService.criarOuEditar(grupoDRE)
                 .done(function (data) {
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                         //location.reload();//seguindo o projeto pronto
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

     

  

    };
})(jQuery);