(function ($) {
    app.modals.CriarOuEditarFaturamentoItemTabelaModal = function () {

        var _itensTabelaService = abp.services.app.faturamentoItemTabela;
        var _modalManager;
        var _$itemTabelaInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$itemTabelaInformationForm = _modalManager.getModal().find('form[name=ItemTabelaInformationsForm]');
            _$itemTabelaInformationForm.validate();
         //   $('.modal-dialog:last').css('width', '500px');
        };

        this.save = function () {
            
            var vigencia = $('input[name="VigenciaDataInicio"]').val();

            if (vigencia == '') {
                abp.notify.warn(app.localize('VigenciaNaoPreenchida'));
                return;
            }



            if (!_$itemTabelaInformationForm.valid()) {
                return;
            }

          

            var itemTabela = _$itemTabelaInformationForm.serializeFormToObject();

            itemTabela.Preco = retirarMascara(itemTabela.Preco);
            itemTabela.Filme = retirarMascara(itemTabela.Filme);
            itemTabela.COCH = retirarMascara(itemTabela.COCH);
            itemTabela.HMCH = retirarMascara(itemTabela.HMCH);
            itemTabela.ValorTotal = retirarMascara(itemTabela.ValorTotal);
            itemTabela.Auxiliar = retirarMascara(itemTabela.Auxiliar);
            itemTabela.Porte = retirarMascara(itemTabela.Porte);

            //abp.event.on('app.CriarOuEditarItemTabelaModalSaved', function () {
            //    debugger // MODAL PRECOS
            //    getItensTabela(true);
            //});


           
            _modalManager.setBusy(true);





            _itensTabelaService.criarOuEditar(itemTabela)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                 //    debugger
                     abp.event.trigger('app.CriarOuEditarItemTabelaModalSaved');
                     //getItensTabela(true);

                     
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };


        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }


    };
})(jQuery);