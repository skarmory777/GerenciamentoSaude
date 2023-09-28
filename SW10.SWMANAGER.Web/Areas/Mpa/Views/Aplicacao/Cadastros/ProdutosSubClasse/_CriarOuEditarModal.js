(function ($) {
    app.modals.CriarOuEditarProdutoSubClasseModal = function () {

        var _ProdutoSubClasseService = abp.services.app.produtoSubClasse;

        var _modalManager;
        var _$ProdutoSubClasseInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoSubClasseInformationForm = _modalManager.getModal().find('form[name=ProdutoSubClasseInformationsForm]');
            _$ProdutoSubClasseInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {

            if (!_$ProdutoSubClasseInformationForm.valid()) {
                return;
            }

            var produtoSubClasse = _$ProdutoSubClasseInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ProdutoSubClasseService.criarOuEditar(produtoSubClasse)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarProdutoSubClasseModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);