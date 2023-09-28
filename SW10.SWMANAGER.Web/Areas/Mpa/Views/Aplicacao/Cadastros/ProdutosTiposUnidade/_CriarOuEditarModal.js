(function ($) {
    app.modals.CriarOuEditarProdutoTipoUnidadeModal = function () {

        var _TiposUnidadeService = abp.services.app.produtoTipoUnidade;

        var _modalManager;
        var _$ProdutoTipoUnidadeInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoTipoUnidadeInformationForm = _modalManager.getModal().find('form[name=ProdutoTipoUnidadeInformationsForm]');
            _$ProdutoTipoUnidadeInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '800px' });
        };

        this.save = function () {
            if (!_$ProdutoTipoUnidadeInformationForm.valid()) {
                return;
            }

            var tipoUnidade = _$ProdutoTipoUnidadeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _TiposUnidadeService.criarOuEditar(tipoUnidade)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("#descricao").val("");
                         $("#descricao").focus();
                     };
                     abp.event.trigger('app.CriarOuEditarProdutoTipoUnidadeModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);