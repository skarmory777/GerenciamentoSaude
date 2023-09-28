(function ($) {
    app.modals.CriarOuEditarProdutoCodigoMedicamentoModal = function () {

        var _ProdutoCodigoMedicamentoService = abp.services.app.produtoCodigoMedicamento;

        var _modalManager;
        var _$ProdutoCodigoMedicamentoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoCodigoMedicamentoInformationForm = _modalManager.getModal().find('form[name=ProdutoCodigoMedicamentoInformationsForm]');
            _$ProdutoCodigoMedicamentoInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '500px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
            if (!_$ProdutoCodigoMedicamentoInformationForm.valid()) {
                return;
            }

            var produtoCodigoMedicamento = _$ProdutoCodigoMedicamentoInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);

            ////console.log(JSON.stringify(produtoCodigoMedicamento));

            _ProdutoCodigoMedicamentoService.criarOuEditar(produtoCodigoMedicamento)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     if ($("#creatorUserId").val() > 0) {
                         _modalManager.close();
                     } else {
                         $("#descricao").val("");
                         $("#codigo").val("");
                         $("#codigo").focus();
                     };
                     abp.event.trigger('app.CriarOuEditarProdutoCodigoMedicamentoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);