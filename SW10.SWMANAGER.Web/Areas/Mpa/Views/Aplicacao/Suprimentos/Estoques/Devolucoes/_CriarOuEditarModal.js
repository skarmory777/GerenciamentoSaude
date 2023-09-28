
(function ($) {
    $(function () {


        this.init = function (modalManager) {
            _modalManager = modalManager;
           
            atendimentoChange();
        };



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
            viewUrl: abp.appPath + 'Mpa/Devolucoes/CriarOuEditarPreMovimentoItemModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Devolucoes/_CriarOuEditarPreMovimentoItemModal.js',
            modalClass: 'CriarOuEditarPreMovimentoItemModal'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });



        $('#btn-novo-PreMovimentoItem').click(function (e) {
            e.preventDefault()


            var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm]');

            _$preMovimentoInformationsForm.validate();

            if (!_$preMovimentoInformationsForm.valid()) {
                return;
            }
            

            var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();

            if ($("#Emissao").val()) {
                preMovimento.Emissao = moment($("#Emissao").val(), "DD/MM/YYYY").format();
            }
            

            var editMode = $('#is-edit-mode').val();

            if ($('#id').val() == '' || $('#id').val() == '0') {

                _preMovimentoService.criarGetIdDevolucao(preMovimento)
                      .done(function (data) {
                          abp.notify.info(app.localize('SavedSuccessfully'));
                          $('#id').val(data.returnObject.id);
                          $('#DocumentoId').val(data.returnObject.documento);
                         $('#creatorUserId').val(data.returnObject.creatorUserId);

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

            _preMovimentoService.criarOuEditarDevolucoes(preMovimento)
                  .done(function (data) {
                      ;

                      if (data.errors.length > 0) {
                          _ErrorModal.open({ erros: data.errors });
                      }
                      else {

                          if (data.warnings.length > 0) {
                              //  _ErrorModal.open({ erros: data.warnings });

                            


                              swal({
                                  title: " ",
                                  text: data.warnings[0].descricao,
                                  type: "warning",
                                  showCancelButton: false,
                                  confirmButtonColor: "#DD6B55",
                                  confirmButtonText: "Ok",
                                  closeOnConfirm: false
                              },
                            function () {
                                location.href = '/mpa/devolucoes';
                            });
                          }
                          else
                          {
                              location.href = '/mpa/devolucoes';
                          }
                      }

                  })
                 .always(function () {
                     //  _modalManager.setBusy(false);
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


        $(document).ready(function () {

            atendimentoChange();
        });

        $('.close').on('click', function () {
            location.href = '/mpa/devolucoes';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/devolucoes';
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

                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
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
                        if (data.record.validade && moment(data.record.validade).format('DD/MM/YYYY') != '01/01/0001') {
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

        $('#EstTipoMovimentoId').change(function () {
            configurarCampos();
        });

        function configurarCampos() {

           

            var valor = $('#EstTipoMovimentoId').val();

            if (valor == '3') {
                $('#grupoOrganizacional').hide();
                $('#UnidadeOrganizacionalId').val('');
                $('#paciente').show();
                $('#medico').show();
                $('#atendimento').show();

                selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoPorSaidaAtendimentoDropdown", ['EstoqueId', 'atendimentoId']);
            }
            else {
                $('#grupoOrganizacional').show();
                $('#paciente').hide();
                $('#medico').hide();
                $('#atendimento').hide();

                $('#paciente').val('');
                $('#medico').val('');
                $('#atendimento').val('');

                selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoPorSaidaSetorDropdown", ['EstoqueId', 'grupoOrganizacional']);

            }



            if (valor == 4) {
                $('#motivoPerdaId').show();
            }
            else {
                $('#motivoPerdaId').hide();
                $('#motivoPerdaId').val('');
            }

        }

        $('#atendimentoId').change(function () {
            atendimentoChange();
        });



        function atendimentoChange() {
           
            var valor = $('#atendimentoId').val();
            debugger;
            if (valor == '' || valor == '0') {
                // $("#MedicoSolcitanteId").attr("disabled", false).change();
                $("#divMedico").removeClass('hidden');
                $("#medicoSolcitanteId").addClass('hidden');
                $("#pacienteInputId").addClass('hidden');
                $("#divPaciente").removeClass('hidden');

            }
            else {
                $.ajax({
                    url: "/mpa/Saidas/SelecionarAtendimento/" + valor,
                    success: function (data) {
                        debugger;
                        $("#pacienteInputId").removeClass('hidden');
                        $("#divPaciente").addClass('hidden');
                        //$("#MedicoSolcitanteId").val(data.MedicoId).change()
                        //                   .selectpicker('refresh');

                        //  $("#MedicoSolcitanteId").attr("disabled", true).change();

                        $("#medicoSolcitanteId").removeClass('hidden');
                        $("#divMedico").addClass('hidden');

                        $("#pacienteInputId").val(data.PacienteNome);

                        $("#medicoSolcitanteId").val(data.MedicoNome);

                       
                    }
                });
            }
        }


        $('#codigoBarra').on('keypress', function (event) {
            //Tecla 13 = Enter
            debugger;

            if (event.which == 13) {
                event.preventDefault();



                var _$preMovimentoInformationsForm = $('form[name=preMovimentoInformationsForm');

                _$preMovimentoInformationsForm.validate();

                if (!_$preMovimentoInformationsForm.valid()) {
                    return;
                }


                var preMovimento = _$preMovimentoInformationsForm.serializeFormToObject();
                var editMode = $('#is-edit-mode').val();

             

                if ($('#id').val() == '' || $('#id').val() == '0') {

                    _preMovimentoService.criarGetIdDevolucao(preMovimento)
                        .done(function (data) {
                            debugger;
                            if (data.returnObject.id > 0) {

                                abp.notify.info(app.localize('SavedSuccessfully'));
                                $('#id').val(data.returnObject.id);
                                $('#DocumentoId').val(data.returnObject.documento);

                                inserirProdutoCodigoBarra();

                            }
                        })
                        .always(function () {
                            //  _modalManager.setBusy(false);
                        });
                }
                else {
                    inserirProdutoCodigoBarra();
                }
            }


        });


        function inserirProdutoCodigoBarra() {


            var estoquePreMovimentoItemAppService = abp.services.app.estoquePreMovimentoItem;
            estoquePreMovimentoItemAppService.criarDevolucaoPorCodigoBarra($('#codigoBarra').val(), $('#EstoqueId').val(), $('#id').val(), $('#quantidade').val())
                .done(function (data) {
                    debugger;
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {

                        getEstoquePreMovimentoItemTable();
                    }

                    $('#codigoBarra').val('');
                    $('#quantidade').val('1');
                    $('#codigoBarra').focus();
                });

        }



        $('#EstoqueId').change(function () {
            selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosComSaidaDropdown", ['EstoqueId']);
            selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoPorSaidaAtendimentoDropdown", ['EstoqueId', 'atendimentoId']);
        });

        selectSWMultiplosFiltros('.selectAtendimento', "/api/services/app/Atendimento/ListarAtendimentosComSaidaDropdown", ['EstoqueId']);
        selectSW('.selectPaciente', "/api/services/app/Paciente/ListarDropdown");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");
        selectSW('.selectTipoDevolucao', "/api/services/app/tipomovimento/ListarDropdownDevolucao");
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('.selectEstoque', "/api/services/app/estoque/ResultDropdownList");
        selectSW('.selectUnidadeOrganizacional', "/api/services/app/unidadeorganizacional/ListarDropdown");
        

    });

})(jQuery);