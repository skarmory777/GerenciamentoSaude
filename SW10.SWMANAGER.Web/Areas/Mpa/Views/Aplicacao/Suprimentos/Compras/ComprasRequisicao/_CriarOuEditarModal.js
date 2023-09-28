(function ($) {
    $(function () {

        $(document).ready(function () {
            CamposRequeridos();
        });

        /*==================================================================================================================
            Permissões ↓
          ================================================================================================================== */
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraRequisicao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraRequisicao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraRequisicao.Delete')
        };

        /*==================================================================================================================
            Servicos ↓
          ================================================================================================================== */
        var _compraRequisicaoService = abp.services.app.compraRequisicao;
        var _unidadeService = abp.services.app.unidade;
        var _produtoService = abp.services.app.produto;

        /*==================================================================================================================
            Modals ↓
          ================================================================================================================== */

        /*==================================================================================================================
            Vars Globais ↓
          ================================================================================================================== */
        var _modalManager;
        var grupoRequisicaoAutomatica = null; //← Guarda o grupo de produtos selecionado (caso a Requisicao Automatica sej"e" ativada)
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
        editMode = (($('#is-edit-mode').val() != '') && ($('#is-edit-mode').val() != undefined));

        _compraRequisicaoService.obterModoRequisicaoManual({ async: false, cache: false })
            .done(function (data) {
                modoRequisicaoManual = data
            });

        _compraRequisicaoService.obterModoRequisicaoAutomatico({ async: false, cache: false })
            .done(function (data) {
                modoRequisicaoAutomatico = data
            });

        _compraRequisicaoService.obterMotivoPedidoReposicaoEstoque({ async: false, cache: false })
            .done(function (data) {
                motivoPedidoReposicaoEstoque = data
            });

        $('#cbo-tiporequisicaoid').attr('required', true);
        $('#cbo-motivoPedidoid').attr('required', true);
        $('#dataRequisicao').attr('required', true);
        $('#dataLimiteEntrega').attr('required', true);
        $('#cbo-empresaid').attr('required', true);
        $('#cbo-unidadeorganizacionalid').attr('required', true);

        $('.modal-dialog').css('width', '1800px');

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

        $('input[name="DataHoraVencimento"]').daterangepicker({
            "timePicker": true,
            "timePicker24Hour": true,
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY H:mm" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
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
                $('input[name="DataHoraVencimento"]').val(selDate.format('L LT')).addClass('form-control edited');
            });

        /* ==================================================================================================================
             Init ↓
           ================================================================================================================== */
        this.init = function (modalManager) {
            _modalManager = modalManager;

            _modalManager.getModal().find('#div-btn-fixa-modal').show(); //← botao tipo pin no header do cadastro
            var btnFixaModal = _modalManager.getModal().find('#btn-fixa-modal:last');

            btnFixaModal.on('click', function (e) {
                fixaModal = !fixaModal;
                if (fixaModal) {
                    btnFixaModal.addClass('blue');
                } else {
                    btnFixaModal.removeClass('blue');
                }
            });
        };

        $('#Movimento').on('load', function () {
            var d = new Date();
            var n = d.getDate();
            $('#movimento').val(moment().format("L LT"));
        });         //?????????????????

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
                    }
                }
            });


        /* RetornarLista - É utilizada no actionList do JTable.
           Quando for definir método de serviço que retorne resultset do banco, tenha atenção na passagem de parâmetros.
           As vezes passar parâmetro na estrutura Json (ex.: servico.metodoDeListagem({ id: $('#seletor').val() })) não funciona,
           mas sim se passando o valor diretamente (ex.: servico.metodoDeListagem($('#seletor').val())
           ↓ */
        function retornarLista(filtro) { // ← filtro nao esta sendo utilizado aqui ainda

            var result;

            var itensStr = $('#itens').val(); // ← itensStr recebe o valor do hidden que guarda a lista de itens serializada passada pro model no controller

            if (itensStr == '[]') {
                itensStr = '';
            }

            if (itensStr != '') { // ← Se há itens
                var itensJson = JSON.parse(itensStr);
                result = _compraRequisicaoService.listarItensJson(itensJson, { async: false, cache: false }) //  '"{Result":"OK","Records":' + js + '}'
                    .done(function (data) {
                        espelhoJTable = data; // ← armazena os produtos exibidos
                    });
            }
            else {
                result = _compraRequisicaoService.listarRequisicaoItem($('#id').val())
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
            //localStorage['DivisaoId'] = divisaoId;

            operacaoCrudDinamico = 1;

            $('#titulo-operacao').empty();
            $('#titulo-operacao').append(app.localize('NovoRegistro'));

            //↓ nao entendi

            //if ($('#classes-list').val() != '') {
            //    var list = JSON.parse($('#classes-list').val());
            //}
            //else {
            //    var list = [];
            //}

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
            $('#quantidadeItemId').val('');
            $('#idGrid').val('');
            $('#unidadeId').val(null).trigger("change");
        }

        $('input[name="DataLimiteEntrega').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //   maxDate: new Date(),
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
                $('input[name="DataLimiteEntrega"]').val(selDate.format('L')).addClass('form-control edited');
            });

        $('input[name="DataRequisicao').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //   maxDate: new Date(),
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
                $('input[name="DataRequisicao"]').val(selDate.format('L')).addClass('form-control edited');
            });


        // Requisicao - Items ↓
        //--------------------------------------
        // combo de Produto - item da requisicao
        $('#comboProduto').on('select2:select', function (e) {
            e.preventDefault();

            /*
              Confere se o produto está listado no JTable
              Se nao estiver, busca no serviço se há requisicao pendente para o item
            */
            var itemJaSelecionado = false;
            var itemJaSelecionado = encontrarProdutoNoJTable($('#comboProduto').val());

            if ((operacaoCrudDinamico == 1) || !((operacaoCrudDinamico == 2) && (recordEdicao.produtoId == $('#comboProduto').val()))) {
                if (itemJaSelecionado == true) {

                    var msg;
                    msg = app.localize('ProdutoJaSelecionado');
                    if (_permissions.edit) {
                        msg = msg + '\n' + app.localize('ProdutoJaSelecionadoEditar');
                    };
                    abp.message.warn(msg, app.localize('Atencao'));

                    $('#comboProduto').empty();
                    //$('#quantidadeItemId').val('');
                    //$('#unidadeId').empty();
                    //$('#unidadeId').val('');

                } else {

                    var item; // ← por enquanto, guarda o produto (no futuro, implementar para produto ou serviço) 

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

                    if (!item.possuiRequisicaoDeCompraPendente) {
                        setarUnidadesDoProdutoSelecionado();

                        $('#unidadeId').focus();

                    } else {
                        abp.message.confirm(
                            app.localize('ProdutoPossuiRequisicaoCompra'),
                            function (isConfirmed) {
                                if (isConfirmed) {
                                    setarUnidadesDoProdutoSelecionado();
                                    $('#unidadeId').focus();
                                }
                                else {
                                    $('#comboProduto').empty();
                                    $('#quantidadeItemId').val('');
                                    $('#unidadeId').empty();
                                    $('#unidadeId').val('');
                                    $('#comboProduto').focus();
                                };
                            });
                    };
                };

            };

        });

        // combo Unidade do Produto
        $('#unidadeId').on('select2:select', function () {
            //joga foco na quantidade
            $('#quantidadeItemId').focus();
        });

        // text Quantidade do Produto
        $('#quantidadeItemId').keypress(function (e) {
            if (e.which == 13) {
                $('#salvar-item-id').focus();
                return false;
            }
        });

        //Novo item
        $('#novo-item').click(function (e) {
            e.preventDefault();

            preparaNovoItem();
        });


        //Others ↓
        //--------------------------------------
        $('.close-button').on('click', function () {
            location.href = '/Mpa/ComprasRequisicao';
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

            formObj.Quantidade = retirarMascara($('#quantidadeItemId').val());

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
                //formObj.Quantidade = formObj.Quantidade;
                formObj.UnidadeId = $('#unidadeId').val();
                formObj.ProdutoAprovacaoId = $('#comboProduto').val();
                //formObj.Quantidade = formObj.Quantidade;
                formObj.UnidadeAprovacaoId = $('#unidadeId').val();
                formObj.ModoInclusao = "M"; //← Manual

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

                        itensJson[i].ProdutoAprovacaoId = $('#comboProduto').val();
                        itensJson[i].UnidadeAprovacaoId = $('#unidadeId').val();
                        //TODO: Não deveria setar os campos de auditoria aqui?

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

        //Save dinamico do item
        function salvarItem(input) {
        };

        //Exclusão dinamica do item
        function deleteItem(input) {
            abp.message.confirm(
                app.localize('ExcluirConfirmacao', input.produto.descricao),
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

            $('#quantidadeItemId').val(registro.quantidade);

            $('#comboProduto').focus();

            $('#icone-btn-salvar').removeClass('fa-plus').addClass('fa-check');
        }


        /*==================================================================================================================
            CRUD - Persistencia no BD ↓
          ==================================================================================================================*/
        //Persiste no BD a Requisicao Completa (Requisicao e seus Itens)
        $('#salvar').click(function (e) {

            e.preventDefault()

            try {
                $(this).buttonBusy(true);

                //Operacoes com o form
                var _$requisicaoInformationsForm = $('form[name=requisicaoInformationsForm');

                _$requisicaoInformationsForm.validate();

                if (!_$requisicaoInformationsForm.valid()) {
                    $(this).buttonBusy(false);
                    return;
                }

                var lista = JSON.parse($('#itens').val());

                if (lista.length === 0) {
                    abp.notify.info(app.localize('ItensRequisicaoRequerido'));
                    $(this).buttonBusy(false);
                    return;
                }

                var requisicao = _$requisicaoInformationsForm.serializeFormToObject();

                requisicao.modoRequisicaoId = 1;

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

        //varre o espelho de registros do JTable em busca de um produto pelo id
        //retona valor logico
        function encontrarProdutoNoJTable(id) {

            var result = false;

            var indice;

            for (var i = 0; i < espelhoJTable.items.length; i++) {
                if (espelhoJTable.items[i].produto.id == id) {

                    result = true;

                    break;
                }
            };

            return result;
        };

        $('#grupoId').on('change', function (e) {
            e.preventDefault();
            selectSWWithDefaultValue('.selectProduto', "/api/services/app/Produto/ListarProdutoPorGrupoDropdown", $('#grupoId'));
        });

        selectSWWithDefaultValue('.selectGrupo', "/api/services/app/Grupo/ListarDropdown");
        selectSW('.selectForncedor', "/api/services/app/fornecedor/ListarDropdown");
        selectSW('.selectAtendimento', "/api/services/app/Atendimento/ListarDropdown");
        selectSW('.selectPaciente', "/api/services/app/Paciente/ListarDropdown");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");

        //O desenvolvedor achou que pela lógica deveriam ser listadas apenas unidades do tipo Compra, mas a consultoria informou que unidades de qualquer tipo devem ser carregadas
        //selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadeComprasPorProdutoDropdown', ['produtoId']);

        //selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['produtoId']);
        selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['comboProduto']);

    });

})(jQuery);