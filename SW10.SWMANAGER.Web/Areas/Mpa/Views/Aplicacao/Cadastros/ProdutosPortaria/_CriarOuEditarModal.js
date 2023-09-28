(function ($) {
    app.modals.CriarOuEditarProdutoPortariaModal = function () {

        var _ProdutoPortariaService = abp.services.app.produtoPortaria;

        var _modalManager;
        var _$ProdutoPortariaInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoPortariaInformationForm = _modalManager.getModal().find('form[name=ProdutoPortariaInformationsForm]');
            _$ProdutoPortariaInformationForm.validate();

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
            if (!_$ProdutoPortariaInformationForm.valid()) {
                return;
            }

            var produtoPortaria = _$ProdutoPortariaInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);

            ////console.log(JSON.stringify(produtoPortaria));

            _ProdutoPortariaService.criarOuEditar(produtoPortaria)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("#codigo").val("");
                         $(".text-editor").jqteVal('');
                         $("#codigo").focus();
                     };
                     abp.event.trigger('app.CriarOuEditarProdutoPortariaModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);