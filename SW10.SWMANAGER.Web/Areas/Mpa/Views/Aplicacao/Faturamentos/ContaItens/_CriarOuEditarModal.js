(function ($) {
    app.modals.CriarOuEditarContaItemModal = function () {

        var _contaItensService = abp.services.app.faturamentoContaItem;
        var _modalManager;
        var _$contaItensInformationForm = null;
        var fixaModal = true;

        $(document).ready(function () {

            $('#valorItem').mask('000.000,00', { reverse: true });
        });

        




        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$contaItemInformationForm = _modalManager.getModal().find('form[name=ContaItemInformationsForm]');
            _$contaItemInformationForm.validate();
            $('.modal-dialog:last').css('width', '800px');
            $('.modal-dialog:last').css('top', '40px');
            
            // Fixar modal apos save
            
            _modalManager.getModal().find('#div-btn-fixa-modal').show();

            var btnFixaModal = _modalManager.getModal().find('#btn-fixa-modal:last');
            btnFixaModal.addClass('blue');

            btnFixaModal.on('click', function (e) {
                fixaModal = !fixaModal;
                if (fixaModal) {
                    btnFixaModal.addClass('blue');
                } else {
                    btnFixaModal.removeClass('blue');
                }
            });// Fim - fixa modal
            
            // Botoes de Percentual
            $("#btn-100-perc-item").on('click', function (e) {
                $('#percentual-item').val(100);
            });
            $("#btn-70-perc-item").on('click', function (e) {
                $('#percentual-item').val(70);
            });
            $("#btn-50-perc-item").on('click', function (e) {
                $('#percentual-item').val(50);
            });
        };

        this.save = function () {
            var itemSelecionado = $("#cbo-fat-item").val();
            
            if(!itemSelecionado || itemSelecionado == 0 || itemSelecionado == '0'){
                abp.notify.warn(app.localize('NenhumItemSelecionado'));
                return;
            }
            // Conferir se item possui preço cadastrado pra este caso
            var contaId = parseInt($('#conta-id').val());
            var _url = '/Mpa/ContasMedicas/VerificarCadastroPrecoItem';
            
            $.ajax({
                type: "POST",
                url: _url,
                data: { contaId: contaId, itemId: parseInt(itemSelecionado) },
                success: function (result) {
                    //if (result == 'False') {
                    //    abp.notify.warn(app.localize('ItemSemPrecoCadastrado'));
                    //    return;
                    //} else {

                        if (!_$contaItemInformationForm.valid()) {
                            return;
                        }

                        var contaItem = _$contaItemInformationForm.serializeFormToObject();

                        contaItem.ValorItem = retirarMascara(contaItem.ValorItem);

                        _modalManager.setBusy(true);

                        abp.services.app.faturamentoContaItem.criarOuEditar(contaItem)
                            .done(function () {
                               
                                abp.notify.info(app.localize('SavedSuccessfully'));

                                abp.event.trigger('app.CriarOuEditarContaItemModalSaved');

                                // Fixar modal ou nao, apos save
                                if (!fixaModal) {
                                    _modalManager.close();
                                } else {
                                    $('#CreateNewContaItemButton').click();
                                    _modalManager.close();
                                }

                              //  $('input').val('');
                                _modalManager.getModal().find('.select2').val('').trigger('change');
                            })
                            .always(function () {
                                _modalManager.setBusy(false);
                            });
                   // }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(thrownError);
                   
                },
                complete: function () { console.log('ajax controller ok'); }
            });
            
            //if (!_$contaItemInformationForm.valid()) {
            //    return;
            //}
            
            //var contaItem = _$contaItemInformationForm.serializeFormToObject();

            //_modalManager.setBusy(true);
            
            //abp.services.app.faturamentoContaItem.criarOuEditar(contaItem)
            //     .done(function () {
            //         abp.notify.info(app.localize('SavedSuccessfully'));
                     
            //         abp.event.trigger('app.CriarOuEditarContaItemModalSaved');

            //         // Fixar modal ou nao, apos save
            //         if (!fixaModal) {
            //             _modalManager.close();
            //         } else {
            //             $('#CreateNewContaItemButton').click();
            //             _modalManager.close();
            //         }

            //         $('input').val('');
            //         //_modalManager.getModal().find('.select2').select2().empty().trigger('change');

            //         _modalManager.getModal().find('.select2').val('').trigger('change');
            //     })
            //    .always(function () {
            //        _modalManager.setBusy(false);
            //    });
        };


        //selectSW(".selectPacote", "/api/services/app/faturamentoItem/ListarDropdownPacote");
        selectSW(".selectPacoteRelacionado", "/api/services/app/faturamentoPacote/ListarDropdownPacoteConta", $('#conta-id'));

    };
})(jQuery);