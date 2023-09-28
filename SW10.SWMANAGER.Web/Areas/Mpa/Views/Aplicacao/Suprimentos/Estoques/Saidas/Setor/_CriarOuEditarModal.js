
(function ($) {




    $(function () {


      

     

        $('.modal-dialog').css('width', '1800px');

        //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        //$.validator.setDefaults({ ignore: ":hidden:not(select)" });

        //// validation of chosen on change
        //$('ul.ui-autocomplete').css('z-index', '2147483647 !important');

     


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
        var _$EstoquePreMovimentoItemTable = $('#EstoquePreMovimentoItemTable');

        var _createOrEditPreMovimentoItemModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Saidas/CriarOuEditarPreMovimentoItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/saidas/Setor/_CriarOuEditarPreMovimentoItemModal.js',
            modalClass: 'CriarOuEditarPreMovimentoItemModal'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

    

        $('#btn-novo-PreMovimentoItem').click(function (e) {
            e.preventDefault()


            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();


            // preMovimento.Quantidade = retirarMascara(preMovimento.Quantidade);
            //preMovimento.ValorICMS = retirarMascara(preMovimento.ValorICMS);
            //preMovimento.TotalDocumento = retirarMascara(preMovimento.TotalDocumento);
            //preMovimento.ICMSPer = retirarMascara(preMovimento.ICMSPer);
            //preMovimento.DescontoPer = retirarMascara(preMovimento.DescontoPer);
            //preMovimento.ValorDesconto = retirarMascara(preMovimento.ValorDesconto);
            //preMovimento.AcrescimoDecrescimo = retirarMascara(preMovimento.AcrescimoDecrescimo);
            //preMovimento.FretePer = retirarMascara(preMovimento.FretePer);
            //preMovimento.ValorFrete = retirarMascara(preMovimento.ValorFrete);
            //preMovimento.Frete = retirarMascara(preMovimento.Frete);
            //preMovimento.ValorICMS = retirarMascara(preMovimento.ValorICMS);
            //preMovimento.IsEntrada = true;

            //  _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();

            if ($('#id').val() == '' || $('#id').val() == '0') {

                _preMovimentoService.criarGetIdSaida(preMovimento)
                      .done(function (data) {
                          abp.notify.info(app.localize('SavedSuccessfully'));
                          $('#id').val(data.id);
                          $('#DocumentoId').val(data.documento);

                          _createOrEditPreMovimentoItemModal.open({ preMovimentoId: $('#id').val(), id: 0 });

                      })
                     .always(function () {
                         //  _modalManager.setBusy(false);
                     });
            }
            else {

                _createOrEditPreMovimentoItemModal.open({ preMovimentoId: $('#id').val(), id: 0 });
            }
        });

        $('#salvar-PreMovimento').click(function (e) {
            e.preventDefault()

            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();
            var editMode = $('#is-edit-mode').val();

            _preMovimentoService.criarOuEditarSaida(preMovimento)
                  .done(function (data) {

                      if (data.errors.length > 0) {
                          _ErrorModal.open({ erros: data.errors });
                      }
                      else {

                          abp.notify.info(app.localize('SavedSuccessfully'));
                          $('#id').val(data.returnObject.id);

                          $('#divConfirmarSaida').show();

                          //if (data.returnObject.possuiLoteValidade) {
                          //    _createOrEditLoteValidadeModal.open({ preMovimentoId: data.returnObject.id });
                          //}
                          //else {
                             
                             
                          //}
                      }

                  })
                 .always(function () {
                     //  _modalManager.setBusy(false);
                 });
        });

        $('#ConfirmarSaidaButton').click(function (e) {
            e.preventDefault()

            _movimentoService.gerarMovimentoEntrada($('#id').val())
                .done(function (data) {

                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                         location.href = '/mpa/saidas';
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
            location.href = '/mpa/saidas';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/saidas';
        });

        _$EstoquePreMovimentoItemTable.jtable
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
                    method: _estoquePreMovimentoService.listarItensSaida
                },
            },
            fields:
            {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        if (_permissions.edit && $('#PreMovimentoEstadoId').val() != 2) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _createOrEditPreMovimentoItemModal.open({ id: data.record.id, preMovimentoId: $('#id').val(), estoqueId: $('#EstoqueId').val() });
                                });
                        }

                        if (_permissions.delete && $('#PreMovimentoEstadoId').val() != 2) {

                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-o"></i></button>')
                              .appendTo($span)
                              .click(function (e) {
                                  e.preventDefault();
                                  deletePreMovimentoItem(data.record);
                              });
                        }
                        return $span;
                    }
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
                app.localize('DeleteWarning', preMovimentoItem.produto.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _estoquePreMovimentoItemService.excluir(preMovimentoItem.id)
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
                _$EstoquePreMovimentoItemTable.jtable('reload');
            } else {
                _$EstoquePreMovimentoItemTable.jtable('load', { filtro: $('#id').val() });
            }
        }
             

        getEstoquePreMovimentoItemTable();

        $('#SaidaPorId').change(function () {
            configurarCampos();
        });

        function configurarCampos() {
            var valor = $('#SaidaPorId').val();

            if (valor == '1') {
                $('#grupoOrganizacional').hide();
                $('#grupoOrganizacional').val('');
                $('#paciente').show();
                $('#medico').show();
                $('#atendimento').show();
            }
            else {
                $('#grupoOrganizacional').show();
                $('#paciente').hide();
                $('#medico').hide();
                $('#atendimento').hide();

                $('#paciente').val('');
                $('#medico').val('');
                $('#atendimento').val('');
            }
        }

        $('#AtendimentoId').change(function () {
            
            var valor = $('#AtendimentoId').val();

            if (valor == '' || valor == '0') {
                $("#PacienteId").attr("disabled", false).change();
                $("#MedicoSolcitanteId").attr("disabled", false).change();
            }
            else {
                $.ajax({
                    url: "/mpa/Saidas/SelecionarAtendimento/" + valor,
                    success: function (data) {

                        $("#PacienteId").val(data.PacienteId).change()
                                            .selectpicker('refresh');

                        $("#MedicoSolcitanteId").val(data.MedicoId).change()
                                           .selectpicker('refresh');


                        $("#PacienteId").attr("disabled", true).change();
                        $("#MedicoSolcitanteId").attr("disabled", true).change();

                    }

                });
            }


        });

    });

})(jQuery);