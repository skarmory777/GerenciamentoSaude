(function ($) {
    app.modals.CriarOuEditarProdutoGrupoTratamentoModal = function () {

        var _GruposTratamentoService = abp.services.app.produtoGrupoTratamento;

        var _modalManager;
        var _$ProdutoGrupoTratamentoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoGrupoTratamentoInformationForm = _modalManager.getModal().find('form[name=ProdutoGrupoTratamentoInformationsForm]');
            _$ProdutoGrupoTratamentoInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '800px' });

        };

        this.save = function () {
            if (!_$ProdutoGrupoTratamentoInformationForm.valid()) {
                return;
            }

            var grupoTratamento = _$ProdutoGrupoTratamentoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _GruposTratamentoService.criarOuEditar(grupoTratamento)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("#descricao").val("");
                         $("#descricao").focus();
                     };
                     abp.event.trigger('app.CriarOuEditarProdutoGrupoTratamentoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);