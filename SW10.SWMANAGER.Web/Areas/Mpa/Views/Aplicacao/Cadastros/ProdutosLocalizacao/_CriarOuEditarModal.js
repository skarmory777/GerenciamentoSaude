(function ($) {
    app.modals.CriarOuEditarProdutoLocalizacaoModal = function () {

        var _ProdutosLocalizacaoService = abp.services.app.produtoLocalizacao;

        var _modalManager;
        var _$ProdutoLocalizacaoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoLocalizacaoInformationForm = _modalManager.getModal().find('form[name=ProdutoLocalizacaoInformationsForm]');
            _$ProdutoLocalizacaoInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '500px' });
        };

        this.save = function () {
           
            if (!_$ProdutoLocalizacaoInformationForm.valid()) {
                return;
            }

            var ProdutoLocalizacao = _$ProdutoLocalizacaoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ProdutosLocalizacaoService.criarOuEditar(ProdutoLocalizacao)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("#descricao").val("");
                         $("#sigla").val("");
                         $("#sigla").focus();
                     };
                     abp.event.trigger('app.CriarOuEditarProdutoLocalizacaoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);