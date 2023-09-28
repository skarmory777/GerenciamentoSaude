(function ($) {
    app.modals.CriarOuEditarProdutoEmpresaModal = function () {

        var _produtosEmpresaService = abp.services.app.produtoEmpresa;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ProdutoEmpresaInformationForm = _modalManager.getModal().find('form[name=ProdutoEmpresaInformationsForm]');
            _$ProdutoEmpresaInformationForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '600px' });
            $('.selectpicker').selectpicker('refresh');

            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('ul.ui-autocomplete').css('z-index', '2147483647');

            //
            //if (($("#cbo-empresas").select('option').length == 1) && ($("#cbo-empresas").val() == 0)) {
            //    $("#cbo-empresas").prop('selectedIndex', 1);
            //};

        };

        this.save = function () {
            
            if (!_$ProdutoEmpresaInformationForm.valid()) {
                return;
            }

            var produtoEmpresa = _$ProdutoEmpresaInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);

            //console.log(JSON.stringify(produtoEmpresa));

            _produtosEmpresaService.criarOuEditar(produtoEmpresa)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarProdutoEmpresaModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);