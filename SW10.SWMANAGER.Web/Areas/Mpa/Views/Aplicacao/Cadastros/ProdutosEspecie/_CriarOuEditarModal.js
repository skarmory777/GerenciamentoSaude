(function ($) {
    app.modals.CriarOuEditarProdutoEspecieModal = function () {

        var _ProdutosEspecieervice = abp.services.app.produtoEspecie;

        var _modalManager;
        var _$ProdutoEspecieInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoEspecieInformationForm = _modalManager.getModal().find('form[name=ProdutoEspecieInformationsForm]');
            _$ProdutoEspecieInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
        };

        this.save = function () {
            if (!_$ProdutoEspecieInformationForm.valid()) {
                return;
            }

            var ProdutoEspecie = _$ProdutoEspecieInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ProdutosEspecieervice.criarOuEditar(ProdutoEspecie)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarProdutoEspecieModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);