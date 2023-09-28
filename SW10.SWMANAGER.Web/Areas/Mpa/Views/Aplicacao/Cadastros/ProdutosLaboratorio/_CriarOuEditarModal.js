(function ($) {
    app.modals.CriarOuEditarProdutoLaboratorioModal = function () {

        var _produtoLaboratorioService = abp.services.app.produtoLaboratorio;

        var _modalManager;
        var _$ProdutoLaboratorioInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoLaboratorioInformationForm = _modalManager.getModal().find('form[name=ProdutoLaboratorioInformationsForm]');
            _$ProdutoLaboratorioInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '600px' });

            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
           
            if (!_$ProdutoLaboratorioInformationForm.valid()) {
                return;
            }

            var produtoLaboratorio = _$ProdutoLaboratorioInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _produtoLaboratorioService.criarOuEditar(produtoLaboratorio)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("#descricao").val("");

                         $('#BrasLaboratorioIdSelec2').empty();
                         $('#BrasLaboratorioIdSelec2').val("");

                         $("#descricao").focus();
                     };
                     abp.event.trigger('app.CriarOuEditarProdutoLaboratorioModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);