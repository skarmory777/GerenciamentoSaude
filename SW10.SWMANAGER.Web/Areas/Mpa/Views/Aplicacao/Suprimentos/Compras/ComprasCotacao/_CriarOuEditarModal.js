(function ($) {
    $(function () {

        /*==================================================================================================================
            Permissões ↓
          ================================================================================================================== */
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraCotacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraCotacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraCotacao.Delete')
        };

        /*==================================================================================================================
            Servicos ↓
          ================================================================================================================== */
        var _compraRequisicaoService = abp.services.app.compraRequisicao;

        /*==================================================================================================================
            Vars Globais ↓
          ================================================================================================================== */
        var _modalManager;
        var editMode; //← é Modo Edicao?
        var itensJson = []; // ← usado nas operações de crud dinâmico dos itens
        var operacaoCrudDinamico = 1; // ← operacao 1: add item dinamico - operacao 2: delete item dinamico
        var recordEdicao; //← Guarda o registro que está sendo editado
        var espelhoJTable = []; //← Armazena os registros que estão sendo listados no grid. Usado para testar se um item já existe no jtable quando for uma inclusao dinamica
        var modoRequisicaoManual; //← Modo de Requisicao Manual.
        var modoRequisicaoAutomatico; //← Modo de Requisicao automatico.
        var motivoPedidoReposicaoEstoque;
        var fixaModal = false; //TODO: verificar se isto funciona

        /* ==================================================================================================================
             Sets iniciais ↓
           ================================================================================================================== */

        $('#QuantidadeProdutoCotacao').maskMoney({ allowNegative: true, thousands: '.', decimal: ',' }).maskMoney('mask');
        $('#ValorUnitarioProdutoCotacao').maskMoney({ prefix: '', allowNegative: true, thousands: '.', decimal: ',', precision: 5, allowZero: true }).maskMoney('mask');

        $.validator.setDefaults({ ignore: ":hidden:not(select)" });

        $('input[name="Movimento"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
            function (selDate) {
                $('input[name="Movimento"]').val(selDate.format('L')).addClass('form-control edited');
                // obterIdade(selDate);
            });

        $('input[name="Emissao"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
            function (selDate) {
                $('input[name="Emissao"]').val(selDate.format('L')).addClass('form-control edited');
            });

        /*==================================================================================================================
            Operacoes com o Item ↓
          ==================================================================================================================*/


        /*==================================================================================================================
             ↓ Comportamentos dos elementos ↓
          ==================================================================================================================*/
        // Elementos no corpo do Pai da Requisicao ↓
        //-------------------------------------------

        //combo Motivo Pedido (reposicao de estoque, setor, etc)
        //$('#cbo-motivoPedidoid').change(function () {
        //    if ($('#cbo-motivoPedidoid').val() == motivoPedidoReposicaoEstoque.id) {// se escolher reposicao de estoque

        //        //$('#comboGrupoProduto').attr('disabled', false);

        //        var atc = app.localize('Atencao');

        //        if ($('#cbo-estoqueid').val().length == 0) {
        //            abp.message.info(app.localize('RequisicaoCompraSelecionarEstoque'), atc);
        //            $('#cbo-estoqueid').focus();
        //        } else {
        //            //$('#comboGrupoProduto').focus();
        //        }

        //    } else {
        //        //$('#comboGrupoProduto').attr('disabled', true);
        //        //$('#comboGrupoProduto').val(0).change();
        //    };
        //});

        // Requisicao - Items ↓
        //--------------------------------------
        // combo de Produto - item da requisicao
        $('#ComboFornecedor').on('select2:select', function (e) {
            e.preventDefault();

            limparCamposProdutoCotacao();

            getItemTable();
        });

        $("#ComboFornecedor").on("select2:unselecting", function (e) {
            $(this).data('unselecting', true);

            limparCamposProdutoCotacao();
            limparCamposFornecedorProdutoCotacao();

            getItemTable();
        });


        $("#CancelarDadosProdutoCotacao").click(function (e) {
            e.preventDefault();

            limparCamposProdutoCotacao();
        });

        $("#SalvarDadosProdutoCotacao").click(function (e) {
            e.preventDefault();

            if ($("#ComboFornecedor").val() === null || $("#ComboFornecedor").val() === '') {
                swal(app.localize('FornecedorRequerido'), '', 'info');
                return;
            }

            if ($("#ComboFormaPagamento").val() === null || $("#ComboFormaPagamento").val() === '') {
                swal(app.localize('FormaPagamentoRequerida'), '', 'info');
                return;
            }

            if ($("#RequisicaoItemId").val() === '') {
                swal(app.localize('SelecioneItemListaCotacao'), '', 'info');
                return;
            }

            if ($("#QuantidadeProdutoCotacao").val() === '') {
                swal(app.localize('QuantidadeRequerida'), '', 'info');
                return;
            }

            if ($("#ValorUnitarioProdutoCotacao").val() === '') {
                swal(app.localize('ValorUnitarioRequerido'), '', 'info');
                return;
            }

            try {
                // Fornecedor
                var requisicaoItemId = $("#RequisicaoItemId").val();
                var requisicaoId = $("#id").val();
                var fornecedorId = $("#ComboFornecedor").val();
                var formaPagamentoId = $("#ComboFormaPagamento").val();
                var prazoEntregaFornecedor = $("#PrazoEntregaFornecedor").val();
                // Fornecedor x Produto
                var quantidadeProdutoCotacao = $("#QuantidadeProdutoCotacao").val();
                var valorUnitarioProdutoCotacao = parseFloat($("#ValorUnitarioProdutoCotacao").val().replace('.', '')).toFixed(5);
                var prazoProdutoCotacao = $("#PrazoProdutoCotacao").val();
                var laboratorioId = $("#ComboLaboratorio").val();
                var opcaoCompradorProdutoCotacao = $("input[name=opcaoCompradorProdutoCotacao]:checked").val();

                $(this).buttonBusy(true);

                var dto = {};
                dto.RequisicaoId = requisicaoId;
                dto.RequisicaoItemId = requisicaoItemId;
                dto.Quantidade = quantidadeProdutoCotacao;
                dto.ValorUnitario = valorUnitarioProdutoCotacao;
                dto.FornecedorId = fornecedorId;
                dto.FormaPagamentoId = formaPagamentoId;
                dto.PrazoEntregaFornecedorEmDias = prazoEntregaFornecedor;
                dto.PrazoEntregaEmDias = prazoProdutoCotacao;
                dto.LaboratorioId = laboratorioId;
                dto.OpcaoComprador = opcaoCompradorProdutoCotacao === "1" ? true : false;

                _compraRequisicaoService.salvarOuAtualizarDadosFornecedorProduto(dto)
                    .done(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));

                        limparCamposProdutoCotacao();

                        getItemTable();
                    })
                    .always(function () {
                        $(this).buttonBusy(false);
                    });
            } catch (erro) {
                console.log(erro);
                $(this).buttonBusy(false);
            }
        });

        //Others ↓
        //--------------------------------------
        $('.close-button').on('click', function () {
            location.href = '/Mpa/ComprasCotacao';
        });

        /*==================================================================================================================
            CRUD - Dinamico ↓
          ==================================================================================================================*/

        //↓ Edição dinamica do item
        function editarCotacaoProduto(registro) {
            limparCamposProdutoCotacao();

            $("#RequisicaoItemId").val(registro.requisicaoItemId);
            $("#ProdutoCotacao").val(registro.produto.descricao);
            $("#UnidadeProdutoCotacao").val(registro.unidade != null ? registro.unidade.descricao : '');
            $("#QuantidadeProdutoCotacao").val(registro.quantidade);
            $("#ValorUnitarioProdutoCotacao").val(registro.valorUnitario);
            $("#PrazoProdutoCotacao").val(registro.prazoEntregaEmDias);

            if (registro.laboratorioId != null) {
                var newOption = new Option(registro.laboratorio.codigo + " - " + registro.laboratorio.descricao, registro.laboratorio.id, true, true);
                $('#ComboLaboratorio').append(newOption).trigger('change');
            } else {
                $("#ComboLaboratorio").empty().trigger('change')
            }

            if (registro.opcaoComprador != null && registro.opcaoComprador) {
                $("input[name=opcaoCompradorProdutoCotacao][value='1']").attr('checked', 'checked');
            }
            else {
                $("input[name=opcaoCompradorProdutoCotacao][value='2']").attr('checked', 'checked');
            }
        }

        function limparCamposFornecedorProdutoCotacao() {
            $("#PrazoEntregaFornecedor").val('');
        }

        function limparCamposProdutoCotacao() {
            $("#RequisicaoItemId").val('');
            $("#ProdutoCotacao").val('');
            $("#UnidadeProdutoCotacao").val('');
            $("#QuantidadeProdutoCotacao").val('');
            $("#ValorUnitarioProdutoCotacao").val('');
            $("#LaboratorioId").val('');
            $("#PrazoProdutoCotacao").val('');
        }

        /*==================================================================================================================
            CRUD - Persistencia no BD ↓
          ==================================================================================================================*/
        //Persiste no BD a Requisicao Completa (Requisicao e seus Itens)
        $('#salvar').click(function (e) {

            e.preventDefault()

            try {
                debugger

                $(this).buttonBusy(true);

                //Operacoes com o form
                var _$requisicaoInformationsForm = $('form[name=requisicaoInformationsForm');

                _$requisicaoInformationsForm.validate();

                if (!_$requisicaoInformationsForm.valid()) {
                    abp.notify.error(app.localize('ErroSalvar'));
                    $(this).buttonBusy(false);
                    return;
                }

                var requisicao = _$requisicaoInformationsForm.serializeFormToObject();

                _compraRequisicaoService.criarOuEditar(requisicao)
                    .done(function (data) {

                        $(this).buttonBusy(false);

                        abp.notify.info(app.localize('SavedSuccessfully'));

                        // Fixar modal ou nao, apos save
                        if (!fixaModal) {
                            //_modalManager.close();
                            window.setTimeout(function () {
                                location.href = '/mpa/ComprasRequisicao' //    location.href = '/mpa/produtos/CriarOuEditarModal/' + data.id
                            }, 500);

                        } else {
                            if (editMode) {
                                $('#cbo-empresaid').focus();

                            } else {
                                /*Preparar o ambiente para uma nova requisicao
                                  .
                                  .
                                  .
                                  */
                            };
                        };

                    })
                    .always(function () {
                        // $(this).buttonBusy(false);
                    });

            } catch (erro) {
                console.log(erro);
            }

            finally {
                $(this).buttonBusy(false);
            }

        });

        /*==================================================================================================================
            Funcoes ↓
          ==================================================================================================================*/

        selectSW('.selectFornecedor', "/api/services/app/fornecedor/ListarDropdown");
        selectSWWithDefaultValue('.selectFormaPagamento', "/api/services/app/formaPagamento/ListarDropdown");
        selectSWWithDefaultValue('.selectLaboratorio', '/api/services/app/produtoLaboratorio/listarDropdown')

        /*==================================================================================================================
            Grid e operacoes relacionados ↓
          ==================================================================================================================*/

        _$ItemTable = $('#ItemTable'); //← div do grid de itens

        //↓ Grid de itens
        _$ItemTable.jtable
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
                        method: getListaProdutosFornecedores
                    },
                },
                fields:
                {
                    id: {
                        key: true,
                        list: false
                    },

                    idGrid: {
                        list: false
                    },

                    produtoId: {
                        list: false
                    },

                    unidadeId: {
                        list: false
                    },

                    actions: {
                        title: app.localize('Actions'),
                        width: '5%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');

                            if (_permissions.edit) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        editarCotacaoProduto(data.record);
                                    });
                            }

                            return $span;
                        }
                    },

                    fornecedor: {
                        title: app.localize('Fornecedor'),
                        width: '30%',
                        display: function (data) {
                            return data.record.fornecedor.descricao;
                        }
                    },

                    produto: {
                        title: app.localize('Produto'),
                        width: '35%',
                        display: function (data) {
                            if (data.record.isDeleted) {
                                return '<span style="text-decoration:line-through;color:red;">' + data.record.produto.codigo + ' - ' + data.record.produto.descricao + '</span>';
                            }
                            else {
                                return data.record.produto.codigo + ' - ' + data.record.produto.descricao;
                            }
                        }
                    },

                    quantidade: {
                        title: app.localize('Quantidade'),
                        width: '5%',
                        display: function (data) {
                            return data.record.quantidade;
                        }
                    },

                    valorUnitario: {
                        title: app.localize('ValUnit'),
                        width: '10%',
                        display: function (data) {
                            return data.record.valorUnitario != null ? data.record.valorUnitario.toLocaleString('pt-br', { minimumFractionDigits: 2 }) : '';
                        }
                    },

                    opcaoComprador: {
                        title: app.localize('OpcaoComprador'),
                        width: '10%',
                        display: function (data) {
                            return data.record.opcaoComprador != null && data.record.opcaoComprador ? app.localize('Yes') : app.localize('No');
                        }
                    }
                }
            });

        function getListaProdutosFornecedores() {
            result = _compraRequisicaoService.listarCotacaoFornecedorItem($("#id").val(), $('#ComboFornecedor').val(), { async: false, cache: false })
                .done(function (data) {
                    if ($('#ComboFornecedor').val() !== '' && $('#ComboFornecedor').val() !== undefined) {
                        // Define Forma de Pagamento e Prazo de Entrega do Fornecedor
                        if (data.items.length > 0 && data.items[0].formaPagamentoId != null) {
                            var newOption = new Option(data.items[0].formaPagamento.codigo + " - " + data.items[0].formaPagamento.descricao, data.items[0].formaPagamento.id, true, true);
                            // Append it to the select
                            $('#ComboFormaPagamento').append(newOption).trigger('change');
                        } else {
                            $("#ComboFormaPagamento").empty().trigger('change')
                        }

                        if (data.items.length > 0 && data.items[0].prazoEntregaFornecedorEmDias != null) {
                            $("#PrazoEntregaFornecedor").val(data.items[0].prazoEntregaFornecedorEmDias);
                        } else {
                            $("#PrazoEntregaFornecedor").val('');
                        }
                    }
                });

            return result;
        }

        // ↓ getItemTable - "Executa" o JTable
        function getItemTable(reload) {
            if (reload) {
                _$ItemTable.jtable('reload');
            } else {
                _$ItemTable.jtable('load', { filtro: $('#id').val() });//, entradaConfirmada: $('#entradaConfirmadaId').val() });
            }
        }

        var _$ItemTable = $('#ItemTable');

        getItemTable();

        abp.event.on('app.EventCompraRequisicaoSaved', function () {
            getItemTable();
        });
    });

})(jQuery);