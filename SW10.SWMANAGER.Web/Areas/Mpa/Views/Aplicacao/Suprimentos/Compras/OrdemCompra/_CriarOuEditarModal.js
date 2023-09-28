(function ($) {
    $(function () {

        $(document).ready(function () {
            CamposRequeridos();
        });

        /*==================================================================================================================
            Permissões ↓
          ================================================================================================================== */
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.OrdemCompra.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.OrdemCompra.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.OrdemCompra.Delete')
        };

        /*==================================================================================================================
            Servicos ↓
          ================================================================================================================== */
        var _ordemCompraService = abp.services.app.ordemCompra;
        var _produtoService = abp.services.app.produto;

        /*==================================================================================================================
            Modals ↓
          ================================================================================================================== */

        /*==================================================================================================================
            Vars Globais ↓
          ================================================================================================================== */
        var itensJson = []; // ← usado nas operações de crud dinâmico dos itens
        var operacaoCrudDinamico = 1; // ← operacao 1: add item dinamico - operacao 2: delete item dinamico
        var recordEdicao; //← Guarda o registro que está sendo editado
        var espelhoJTable = []; //← Armazena os registros que estão sendo listados no grid. Usado para testar se um item já existe no jtable quando for uma inclusao dinamica

        /* ==================================================================================================================
             Sets iniciais ↓
           ================================================================================================================== */

        $('#cbo-empresaid').attr('required', true);
        $('#cbo-unidadeorganizacionalid').attr('required', true);
        $('#cbo-ordemCompraStatus').attr('required', true);
        $('#dataOrdemPagamento').attr('required', true);

        $('#quantidade').maskMoney({ allowNegative: true, thousands: '.', decimal: ',' }).maskMoney('mask');
        $('#valorUnitario').maskMoney({ prefix: '', allowNegative: true, thousands: '.', decimal: ',', precision: 5, allowZero: true }).maskMoney('mask');
        $('#valorTotal').maskMoney({ prefix: '', allowNegative: true, thousands: '.', decimal: ',', precision: 5, allowZero: true }).maskMoney('mask');
        $('#valorFrete').maskMoney({ prefix: '', allowNegative: true, thousands: '.', decimal: ',', precision: 5, allowZero: true }).maskMoney('mask');
        $('#valorDesconto').maskMoney({ prefix: '', allowNegative: true, thousands: '.', decimal: ',', precision: 5, allowZero: true }).maskMoney('mask');

        $.validator.setDefaults({ ignore: ":hidden:not(select)" });

        $('input[name="DataOrdemCompra"]').daterangepicker({
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
                $('input[name="DataOrdemCompra"]').val(selDate.format('L')).addClass('form-control edited');
            });

        $('input[name="DataPrevistaEntrega"]').daterangepicker({
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
                $('input[name="DataPrevistaEntrega"]').val(selDate.format('L')).addClass('form-control edited');
            });

        $('input[name="DataFinalEntrega"]').daterangepicker({
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
                $('input[name="DataFinalEntrega"]').val(selDate.format('L')).addClass('form-control edited');
            });

        $('#quantidade').blur(function () {
            $("#valorTotal").val(formatarValor(parseFloat(this.value.replaceAll('.', '').replaceAll(',', '.')) * parseFloat($("#valorUnitario").val().replaceAll('.', '').replaceAll(',', '.'))));
        });

        $('#valorUnitario').blur(function () {
            $("#valorTotal").val(formatarValor(parseFloat(this.value.replaceAll('.', '').replaceAll(',', '.')) * parseFloat($("#quantidade").val().replaceAll('.', '').replaceAll(',', '.'))));
        });

        $('#valorTotal').blur(function () {
            $("#quantidade").val(formatarValor(parseFloat(this.value.replaceAll('.', '').replaceAll(',', '.')) / parseFloat($("#valorUnitario").val().replaceAll('.', '').replaceAll(',', '.'))));
        });

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
                        method: retornarLista
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
                        width: '10%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');

                            if (_permissions.edit) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        editItem(data.record);
                                    });
                            }

                            if (_permissions.delete) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        deleteItem(data.record);
                                    });
                            }

                            return $span;
                        }
                    },

                    produto: {
                        title: app.localize('Produto'),
                        width: '70%',
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
                        width: '10%',
                        display: function (data) {
                            if (data.record.quantidade) {

                                if (data.record.isDeleted) {
                                    return posicionarDireita('<span style="text-decoration:line-through;color:red;">' + data.record.quantidade + '</span>');
                                }
                                else {
                                    return posicionarDireita(data.record.quantidade);
                                }
                            }
                        }
                    },

                    unidade: {
                        title: app.localize('Unidade'),
                        width: '12%',
                        display: function (data) {
                            if (data.record.isDeleted) {
                                return '<span style="text-decoration:line-through;color:red;">' + data.record.unidade.descricao + '</span>';
                            }
                            else {
                                return data.record.unidade.descricao;
                            }
                        }
                    },

                    valorUnitario: {
                        title: app.localize('ValorUnitario'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.valorUnitario) {

                                if (data.record.isDeleted) {
                                    return posicionarDireita('<span style="text-decoration:line-through;color:red;">' + data.record.valorUnitario + '</span>');
                                }
                                else {
                                    return posicionarDireita(data.record.valorUnitario);
                                }
                            }
                        }
                    },

                    valorTotal: {
                        title: app.localize('ValorTotal'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.valorTotal) {

                                if (data.record.isDeleted) {
                                    return posicionarDireita('<span style="text-decoration:line-through;color:red;">' + data.record.valorTotal + '</span>');
                                }
                                else {
                                    return posicionarDireita(data.record.valorTotal);
                                }
                            }
                        }
                    },
                }
            });

        function retornarLista(filtro) { // ← filtro nao esta sendo utilizado aqui ainda

            var result;

            var itensStr = $('#itens').val(); // ← itensStr recebe o valor do hidden que guarda a lista de itens serializada passada pro model no controller

            if (itensStr == '[]') {
                itensStr = '';
            }

            if (itensStr != '') { // ← Se há itens
                var itensJson = JSON.parse(itensStr);
                result = _ordemCompraService.listarItensJson(itensJson, { async: false, cache: false }) //  '"{Result":"OK","Records":' + js + '}'
                    .done(function (data) {
                        espelhoJTable = data; // ← armazena os produtos exibidos
                    });
            }
            else {
                result = _ordemCompraService.listarRequisicaoItem($('#id').val())
                    .done(function (data) {
                        espelhoJTable = data; // ← armazena os produtos exibidos
                    });
            }

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


        /*==================================================================================================================
            Operacoes com o Item ↓
          ==================================================================================================================*/
        // Prepara o ambiente para Adicionar um Item
        function preparaNovoItem() {
            operacaoCrudDinamico = 1;

            $('#titulo-operacao').empty();
            $('#titulo-operacao').append(app.localize('NovoRegistro'));

            limparControles();

            $('#comboProduto').focus();

            $('#icone-btn-salvar').removeClass('fa-check').addClass('fa-plus');
        }

        // ↓ vero que é isso
        $('#EstTipoMovimentoId').change(function () {
            configurarCampos();
        });

        //"Limpa" os controles do item
        function limparControles() {
            $('#comboProduto').val(null).trigger("change");
            $('#quantidade').val('');
            $('#valorUnitario').val('');
            $('#valorTotal').val('');
            $('#idGrid').val('');
            $('#unidadeId').val(null).trigger("change");
        }

        // Requisicao - Items ↓
        //--------------------------------------
        // combo de Produto - item da requisicao
        $('#comboProduto').on('select2:select', function (e) {
            e.preventDefault();

            var item; 

            _produtoService.obter($('#comboProduto').val(), { async: false, cache: false })
                .done(function (data) {
                    item = data;
                });

            abp.services.app.produto.obterUnidadePorProduto($('#comboProduto').val())
                .done(function (data) {
                    if (data.items.length == 1) {
                        $('#unidadeId').append($("<option>").val(data.items[0].id).text(data.items[0].descricao)).val(data.items[0].id).trigger("change");
                        return;
                    }
                    $('#unidadeId').val(null).trigger("change");
                });
        });

        // combo Unidade do Produto
        $('#unidadeId').on('select2:select', function () {
            $('#quantidade').focus();
        });

        //Others ↓
        //--------------------------------------
        $('.close-button').on('click', function () {
            location.href = '/Mpa/OrdemCompra';
        });

        /*==================================================================================================================
            CRUD - Dinamico ↓
          ==================================================================================================================*/
        // Salva o item dinamicamente (não persiste no BD)
        $('#salvar-item-id').click(function (e) {
            e.preventDefault();

            //operacoes com o form
            var _$ItemInformationForm = $('form[name=ItemInformationsForm]');

            _$ItemInformationForm.validate();

            if (!_$ItemInformationForm.valid()) {
                return;
            }

            //transforma o form em objeto
            var formObj = _$ItemInformationForm.serializeFormToObject();

            formObj.Quantidade = retirarMascara($('#quantidade').val());

            //operacoes com a lista de itens
            var itensStr = $('#itens').val(); // ← hidden que guarda stringão de itens

            if (itensStr == '[]') {
                itensStr = '';
            }

            if (itensStr != '') {
                itensJson = JSON.parse(itensStr);
            } else {
                itensJson = [];
            };

            var idGrid = $('#idGrid').val();

            if (operacaoCrudDinamico == 1) {
                formObj.IdGrid = itensJson.length == 0 ? 1 : itensJson[itensJson.length - 1].IdGrid + 1;
                formObj.ProdutoId = $('#comboProduto').val();
                formObj.UnidadeId = $('#unidadeId').val();
                formObj.ValorUnitario = parseFloat($("#valorUnitario").val().replace('.', '').replace(',', '.'));
                formObj.ValorTotal = parseFloat($("#valorTotal").val().replace('.', '').replace(',', '.'));
                formObj.IsSistema = false;
                formObj.IsDeleted = false;
                formObj.CreationTime = moment($.now()).format('YYYY-MM-DD hh:mm:ss.SSS');
                formObj.CreatorUserId = abp.session.userId;

                itensJson.push(formObj);

            } else if (operacaoCrudDinamico == 2) {
                for (var i = 0; i < itensJson.length; i++) {
                    if (itensJson[i].IdGrid == idGrid) {
                        itensJson[i].Quantidade = formObj.Quantidade;
                        itensJson[i].ProdutoId = $('#comboProduto').val();
                        itensJson[i].UnidadeId = $('#unidadeId').val();
                        itensJson[i].ValorUnitario = parseFloat($("#valorUnitario").val().replace('.', '').replace(',', '.'));
                        itensJson[i].ValorTotal = parseFloat($("#valorTotal").val().replace('.', '').replace(',', '.'));
                        break;
                    }
                }

                recordEdicao = null;
            }

            //Serializa os itens e seta no hidden
            itensStr = JSON.stringify(itensJson);
            $('#itens').val(itensStr);

            //Limpa os controles do item
            preparaNovoItem();

            abp.notify.info(app.localize('ListaAtualizada'));

            // Refresh no JTable dos itens
            getItemTable();

        });

        //Exclusão dinamica do item
        function deleteItem(input) {
            abp.message.confirm(
                msgDialog = app.localize('ExcluirConfirmacao', input.produto.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        preparaNovoItem();

                        var lista = JSON.parse($('#itens').val());
                        var indice;
                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == input.idGrid) {
                                indice = i;
                                break;
                            }
                        }

                        if (lista[indice].IsDeleted) {
                            lista[indice].IsDeleted = false;
                            lista[indice].DeleterUserId = '';
                        }
                        else {
                            lista[indice].IsDeleted = true;
                            lista[indice].DeleterUserId = abp.session.userId;
                        }

                        $('#itens').val(JSON.stringify(lista));
                        abp.notify.info(app.localize('ListaAtualizada'));
                        getItemTable(true);
                    }
                }
            );
        }

        //↓ Edição dinamica do item
        function editItem(registro) {
            operacaoCrudDinamico = 2;

            recordEdicao = registro;

            $('#titulo-operacao').empty();
            $('#titulo-operacao').append(app.localize('EditarRegistro'));

            $('#idGrid').val(registro.idGrid);

            $('#comboProduto').empty();
            $('#comboProduto').append($("<option/>")
                .val(registro.produtoId)
                .text(registro.produto.descricao))
                .trigger("change");

            setarUnidadesDoProdutoSelecionado();

            $('#unidadeId').empty();
            $('#unidadeId').append($("<option/>")
                .val(registro.unidadeId)
                .text(registro.unidade.descricao))
                .trigger("change");

            $('#quantidade').val(registro.quantidade);
            $('#valorUnitario').val(formatarValor(registro.valorUnitario.toString()));
            $('#valorTotal').val(formatarValor(registro.valorTotal.toString()));

            $('#comboProduto').focus();

            $('#icone-btn-salvar').removeClass('fa-plus').addClass('fa-check');
        }


        /*==================================================================================================================
            CRUD - Persistencia no BD ↓
          ==================================================================================================================*/
        $('#salvar').click(function (e) {
            e.preventDefault()

            try {
                $(this).buttonBusy(true);

                var _$ordemCompraInformationsForm = $('form[name=ordemCompraInformationsForm');

                _$ordemCompraInformationsForm.validate();

                if (!_$ordemCompraInformationsForm.valid()) {
                    $(this).buttonBusy(false);
                    return;
                }

                var lista = JSON.parse($('#itens').val());

                if (lista.length === 0) {
                    abp.notify.info(app.localize('ItensRequisicaoRequerido'));
                    $(this).buttonBusy(false);
                    return;
                }

                var ordemCompra = _$ordemCompraInformationsForm.serializeFormToObject();
                ordemCompra.ValorFrete = parseFloat(ordemCompra.ValorFrete.replace('.', '').replace(',', '.'));
                ordemCompra.ValorDesconto = parseFloat(ordemCompra.ValorDesconto.replace('.', '').replace(',', '.'));

                _ordemCompraService.criarOuEditar(ordemCompra)
                    .done(function (data) {

                        $(this).buttonBusy(false);

                        abp.notify.info(app.localize('SavedSuccessfully'));

                        window.setTimeout(function () {
                            location.href = '/mpa/OrdemCompra'
                        }, 500);
                    })
                    .always(function () {
                    });

            } catch (erro) {
                console.log(erro);
                $(this).buttonBusy(false);
            }
        });

        /*==================================================================================================================
            Funcoes ↓
          ==================================================================================================================*/
        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        function configurarCampos() {
            var valor = $('#EstTipoMovimentoId').val();

            if (valor == '3') {
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

            if (valor == 4) {
                $('#motivoPerdaId').show();
            }
            else {
                $('#motivoPerdaId').hide();
                $('#motivoPerdaId').val('');
            }

        }

        function setarUnidadesDoProdutoSelecionado() {
            //TODO: quando retornar apenas uma unidade, setar o combo automaticamente
            //Carrega combo de unidade com as unidades do produto selecionado
            selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['comboProduto']);
        };

        $('#grupoId').on('change', function (e) {
            e.preventDefault();
            selectSWWithDefaultValue('.selectProduto', "/api/services/app/Produto/ListarProdutoPorGrupoDropdown", $('#grupoId'));
        });

        selectSWWithDefaultValue('.selectGrupo', "/api/services/app/Grupo/ListarDropdown");
        selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['comboProduto']);
    });

})(jQuery);