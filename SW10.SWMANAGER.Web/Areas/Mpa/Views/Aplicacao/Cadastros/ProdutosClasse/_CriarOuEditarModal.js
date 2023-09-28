(function ($) {
    app.modals.CriarOuEditarProdutoClasseModal = function () {

        var _ProdutoClasseService = abp.services.app.produtoClasse;

        var _modalManager;
        var _$ProdutoClasseInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoClasseInformationForm = _modalManager.getModal().find('form[name=ProdutoClasseInformationsForm]');
            _$ProdutoClasseInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
            if (!_$ProdutoClasseInformationForm.valid()) {
                return;
            }

            var produtoClasse = _$ProdutoClasseInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ProdutoClasseService.criarOuEditar(produtoClasse)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarProdutoClasseModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);