(function ($) {
    app.modals.CriarOuEditarProdutoAcaoTerapeuticaModal = function () {

        var _produtoAcaoTerapeuticaService = abp.services.app.produtoAcaoTerapeutica;

        var _modalManager;
        var _$ProdutoAcaoTerapeuticaInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoAcaoTerapeuticaInformationForm = _modalManager.getModal().find('form[name=ProdutoAcaoTerapeuticaInformationsForm]');
            _$ProdutoAcaoTerapeuticaInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '800px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
          
            if (!_$ProdutoAcaoTerapeuticaInformationForm.valid()) {
                return;
            }

            var produtoAcaoTerapeutica = _$ProdutoAcaoTerapeuticaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _produtoAcaoTerapeuticaService.criarOuEditar(produtoAcaoTerapeutica)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("#descricao").val("");
                         $("#descricao").focus();
                     };
                     abp.event.trigger('app.CriarOuEditarProdutoAcaoTerapeuticaModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);