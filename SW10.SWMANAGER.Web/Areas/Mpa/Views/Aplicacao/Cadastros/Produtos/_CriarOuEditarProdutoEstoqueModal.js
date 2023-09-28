(function ($) {
    app.modals.CriarOuEditarProdutoEstoqueModal = function () {

        var _produtoEstoqueService = abp.services.app.produtoEstoque;

        var _modalManager;
        var _$ProdutoEstoqueInformationsForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;


            _$ProdutoEstoqueInformationsForm = _modalManager.getModal().find('form[name=ProdutoEstoqueInformationsForm]');
            _$ProdutoEstoqueInformationsForm.validate();

            $('.modal-dialog').css({ 'width': '90%', 'max-width': '600px' });

            $('#EstoqueMinimoId').bind('keydown', soNums);
            $('#EstoqueMaximoId').bind('keydown', soNums);
            $('#PontoPedidoId').bind('keydown', soNums);

            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            ////Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            //$('ul.ui-autocomplete').css('z-index', '2147483647');
        };


        function soNums(e) {

            //teclas adicionais permitidas (tab,delete,backspace,setas direita e esquerda)
            //keyCodesPermitidos = new Array(8, 9, 37, 39, 46);
            keyCodesPermitidos = new Array(8, 9, 16, 37, 39, 188, 46);

            //numeros e 0 a 9 do teclado alfanumerico
            for (x = 48; x <= 57; x++) {
                keyCodesPermitidos.push(x);
            }

            //numeros e 0 a 9 do teclado numerico
            for (x = 96; x <= 105; x++) {
                keyCodesPermitidos.push(x);
            }

            //Pega a tecla digitada
            keyCode = e.which;

            //alert(keyCode);

            //Verifica se a tecla digitada é permitida
            if ($.inArray(keyCode, keyCodesPermitidos) != -1) {
                return true;
            }
            return false;
        }

        this.save = function () {
            if (!_$ProdutoEstoqueInformationsForm.valid()) {
                return;
            }

            var produtoEstoque = _$ProdutoEstoqueInformationsForm.serializeFormToObject();
            _modalManager.setBusy(true);

            _produtoEstoqueService.criarOuEditar(produtoEstoque)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarProdutoEstoqueModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);