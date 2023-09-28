(function ($) {
    app.modals.CriarOuEditarProdutoUnidadeModal = function () {

        var _ProdutoUnidadeService = abp.services.app.produtoUnidade;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoUnidadeInformationForm = _modalManager.getModal().find('form[name=ProdutoUnidadeInformationsForm]');
            _$ProdutoUnidadeInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '450px' });
            $('.selectpicker').selectpicker('refresh');

            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
           
            if (!_$ProdutoUnidadeInformationForm.valid()) {
                return;
            }

            var produtoUnidade = _$ProdutoUnidadeInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);

            ////console.log(JSON.stringify(produtoUnidade));

            _ProdutoUnidadeService.criarOuEditar(produtoUnidade)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarProdutoUnidadeModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);