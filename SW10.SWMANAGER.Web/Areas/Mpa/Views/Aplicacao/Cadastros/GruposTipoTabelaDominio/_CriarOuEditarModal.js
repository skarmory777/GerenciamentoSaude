(function ($) {
    app.modals.CriarOuEditarGrupoTipoTabelaDominioModal = function () {

        var _GruposTipoTabelaDominioService = abp.services.app.grupoTipoTabelaDominio;

        var _modalManager;
        var _$GrupoTipoTabelaDominioInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$GrupoTipoTabelaDominioInformationForm = _modalManager.getModal().find('form[name=GrupoTipoTabelaDominioInformationsForm]');
            _$GrupoTipoTabelaDominioInformationForm.validate();

            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        };

        this.save = function () {
            if (!_$GrupoTipoTabelaDominioInformationForm.valid()) {
                return;
            }

            var grupoTipoTabelaDominio = _$GrupoTipoTabelaDominioInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _GruposTipoTabelaDominioService.criarOuEditar(grupoTipoTabelaDominio)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarGrupoTipoTabelaDominioModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);