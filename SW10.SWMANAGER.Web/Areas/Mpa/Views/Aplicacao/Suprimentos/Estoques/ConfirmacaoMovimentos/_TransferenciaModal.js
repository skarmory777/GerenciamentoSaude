﻿
(function ($) {




    $(function () {

        $('.modal-dialog').css('width', '1800px');

        $('#Movimento').on('load', function () {
            var d = new Date();
            var n = d.getDate();
            $('#movimento').val(moment().format("L LT"));
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };

        var iValidador = {
            init: function () {
                // Execute seus códigos iniciais
                // ...
                //alert('Entrou no validador agora!');
                // Chame as funções desejadas...
                iValidador.outraFuncao();
            },
            outraFuncao: function () {
                // Códigos desejados...
            }
        };


        var _preMovimentoService = abp.services.app.estoquePreMovimento;
        var _movimentoService = abp.services.app.estoqueMovimento;


        var _estoquePreMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _$EstoqueTransferenciaItemTable = $('#EstoqueTransferenciaItemTable');

        var _createOrEditTransferenciaItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Transferencias/CriarOuEditarTransferenciaItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Transferencias/_CriarOuEditarTransferenciaItemModal.js',
            modalClass: 'CriarOuEditarTransferenciaItemModal'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

    

        $('#btn-novo-TransferenciaItem').click(function (e) {
            e.preventDefault()


            var _$transferenciaInformationsForm = $('form[name=transferenciaInformationsForm');

            _$transferenciaInformationsForm.validate();

            if (!_$transferenciaInformationsForm.valid()) {
                return;
            }

            var transferencia = _$transferenciaInformationsForm.serializeFormToObject();


        

            //  _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();

            if ($('#id').val() == '' || $('#id').val() == '0') {

            

                _preMovimentoService.transferirProduto(transferencia)
                      .done(function (data) {
                          abp.notify.info(app.localize('SavedSuccessfully'));
                          $('#id').val(data.returnObject.id);
                          $('#DocumentoId').val(data.returnObject.documento);

                          _createOrEditTransferenciaItemModal.open({ transferenciaId: $('#id').val(), id: 0 });

                      })
                     .always(function () {
                         //  _modalManager.setBusy(false);
                     });
            }
            else {

                _createOrEditTransferenciaItemModal.open({ transferenciaId: $('#id').val(), id: 0 });
            }
        });

        $('#salvar-PreMovimento').click(function (e) {
            e.preventDefault()


            var _$transferenciaInformationsForm = $('form[name=transferenciaInformationsForm');

            _$transferenciaInformationsForm.validate();

            if (!_$transferenciaInformationsForm.valid()) {
                return;
            }

            var transferencia = _$transferenciaInformationsForm.serializeFormToObject();

           
            var editMode = $('#is-edit-mode').val();

            _preMovimentoService.transferirProduto(transferencia)
                  .done(function (data) {

                    
                      if (data.errors.length > 0) {
                          _ErrorModal.open({ erros: data.errors });
                      }
                      else {

                          abp.notify.info(app.localize('SavedSuccessfully'));
                          $('#id').val(data.returnObject.id);

                          //$('#divConfirmarSaida').show();
                          location.href = '/mpa/confirmacaoMovimentos';

                      }

                  })
                 .always(function () {
                     //  _modalManager.setBusy(false);
                 });
        });

        $('#ConfirmarSaidaButton').click(function (e) {
            e.preventDefault()

            _movimentoService.gerarMovimentoTransferencia($('#id').val())
                .done(function (data) {

                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        location.href = '/mpa/ConfirmacaoMovimentos';
                    }
                });
        });




        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        function salvar(e) {

        }

        abp.event.on('app.CriarOuEditarPreMovimentoItemModalSaved', function () {
            getEstoquePreMovimentoItemTable();
        });

        var _estoquePreMovimentoService = abp.services.app.estoquePreMovimento;

        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        $('.close').on('click', function () {
            location.href = '/mpa/confirmacaoMovimentos';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/confirmacaoMovimentos';
        });

        _$EstoqueTransferenciaItemTable.jtable
        ({
            title: app.localize('Item'),
            paging: true,
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,
            actions:
            {
                listAction:
                {
                    method: _estoquePreMovimentoService.listarItensTranferencia
                },
            },
            fields:
            {
                id: {
                    key: true,
                    list: false
                },
            
                TransferenciaItemId:{
                    type: 'hidden',
                    defaultValue: function (data) {
                        return data.record.transferenciaItemId;
                    },
                },
                PreMovimentoId: {
                    type: 'hidden',
                    defaultValue: function (data) {
                        return $('#id').val();
                    },
                },
                ProdutoId: {
                    title: app.localize('Produto'),
                    width: '30%',
                    display: function (data) {
                        if (data.record.produto) {
                            return data.record.produto;
                        }
                    }
                },

                Unidade: {
                    title: app.localize('Unidade'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.unidade) {
                            return data.record.unidade;
                        }
                    }
                },

                quantidade: {
                    title: app.localize('Quantidade'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.quantidade) {
                            return data.record.quantidade;
                        }
                    }
                },

                Lote: {
                    title: app.localize('Lote'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.lote) {
                            return data.record.lote;
                        }
                    }
                },

                Validade: {
                    title: app.localize('Validade2'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.validade &&  moment(data.record.validade).format('DD/MM/YYYY' )!='01/01/0001' )
                        {
                            return moment(data.record.validade).format('DD/MM/YYYY');
                        }
                    }
                }
                ,

                Laboratorio: {
                    title: app.localize('Laboratorio'),
                    width: '30%',
                    display: function (data) {
                        if (data.record.laboratorio) {
                            return data.record.laboratorio;
                        }
                    }
                }


            }
        });


        function deletePreMovimentoItem(preMovimentoItem) {

            abp.message.confirm(
                app.localize('DeleteWarning', preMovimentoItem.produto),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _estoquePreMovimentoItemService.excluirItemTransferencia(preMovimentoItem.transferenciaItemId)
                            .done(function () {
                                getEstoquePreMovimentoItemTable(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function getEstoquePreMovimentoItemTable(reload) {

            if (reload) {
                _$EstoqueTransferenciaItemTable.jtable('reload');
            } else {
                _$EstoqueTransferenciaItemTable.jtable('load', { filtro: $('#id').val() });
            }
        }
             

        getEstoquePreMovimentoItemTable();

      


    });

})(jQuery);