(function ($) {
    app.modals.CriarOuEditarProdutoSubstanciaModal = function () {

        var _ProdutosSubstanciaervice = abp.services.app.produtoSubstancia;

        var _modalManager;
        var _$ProdutoSubstanciaInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoSubstanciaInformationForm = _modalManager.getModal().find('form[name=ProdutoSubstanciaInformationsForm]');
            _$ProdutoSubstanciaInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '800px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
            if (!_$ProdutoSubstanciaInformationForm.valid()) {
                return;
            }

            var ProdutoSubstancia = _$ProdutoSubstanciaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ProdutosSubstanciaervice.criarOuEditar(ProdutoSubstancia)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("#descricao").val("");
                         $("#descricao").focus();
                     };
                     abp.event.trigger('app.CriarOuEditarProdutoSubstanciaModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);