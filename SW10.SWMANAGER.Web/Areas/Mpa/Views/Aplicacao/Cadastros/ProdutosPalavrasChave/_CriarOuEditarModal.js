(function ($) {
    app.modals.CriarOuEditarProdutoPalavraChaveModal = function () {

        var _ProdutoPalavraChaveService = abp.services.app.produtoPalavraChave;

        var _modalManager;
        var _$ProdutoPalavraChaveInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoPalavraChaveInformationForm = _modalManager.getModal().find('form[name=ProdutoPalavraChaveInformationsForm]');
            _$ProdutoPalavraChaveInformationForm.validate();

            //$('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('.modal-dialog').css({ 'width': '100%', 'max-width': '800px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
            $('.text-editor').jqte();
        };

        this.save = function () {
            if (!_$ProdutoPalavraChaveInformationForm.valid()) {
                return;
            }

            var produtoPalavraChave = _$ProdutoPalavraChaveInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ProdutoPalavraChaveService.criarOuEditar(produtoPalavraChave)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("#palavra").val("");
                         $(".text-editor").jqteVal('');
                         $("#palavra").focus();
                     };

                     abp.event.trigger('app.CriarOuEditarProdutoPalavraChaveModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);