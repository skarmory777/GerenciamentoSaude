(function ($) {
    app.modals.CriarOuEditarProdutoEstoqueModal = function () {

        var _ProdutoEstoqueService = abp.services.app.produtoEstoque;

        var _modalManager;
        var _$ProdutoEstoqueInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoEstoqueInformationForm = _modalManager.getModal().find('form[name=ProdutoEstoqueInformationsForm]');
            _$ProdutoEstoqueInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
            if (!_$ProdutoEstoqueInformationForm.valid()) {
                return;
            }

            var produtoEstoque = _$ProdutoEstoqueInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ProdutoEstoqueService.criarOuEditar(produtoEstoque)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarProdutoEstoqueModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);