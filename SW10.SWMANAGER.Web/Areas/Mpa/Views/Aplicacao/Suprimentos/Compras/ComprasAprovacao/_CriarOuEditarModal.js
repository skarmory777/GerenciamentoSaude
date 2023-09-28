(function ($) {
    $(function () {

        $(document).ready(function () {
            CamposRequeridos();
        });

        /*==================================================================================================================
            Permissões ↓
          ================================================================================================================== */
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraAprovacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraAprovacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.CompraAprovacao.Delete')
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
        //var _ErrorModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        //});


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
        var isAceitarTodos;


        /* ==================================================================================================================
             Sets iniciais ↓
           ================================================================================================================== */
        setAceitarTodosItems();

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

        if (!editMode) { // ← Se for nova requisicao
            /* $('#comboGrupoProduto').attr('disabled', true);*/ //← combo inicia desabilitado
            $('#modoRequisicaoId').val(modoRequisicaoManual.id); //← Modo da requisição inicia como Manual

        } else { // ← Se for edição            
            //$('#comboGrupoProduto').attr('disabled', $('#modoRequisicaoId').val() == modoRequisicaoAutomatico.id);

        }

        //$('.modal-dialog').css('width', '1800px');

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


        /*==================================================================================================================
            Operacoes com o Item ↓
          ==================================================================================================================*/
        // Prepara o ambiente para Adicionar um Item
        function preparaNovoItem() {
            //localStorage['DivisaoId'] = divisaoId;

            operacaoCrudDinamico = 1;

            $('#titulo-operacao').empty();
            //$('#titulo-operacao').append(app.localize('NovoRegistro'));

            //↓ nao entendi

            //if ($('#classes-list').val() != '') {
            //    var list = JSON.parse($('#classes-list').val());
            //}
            //else {
            //    var list = [];
            //}

            limparControles();

            $('#comboProdutoAprovacao').focus();

            //$('#icone-btn-salvar').removeClass('fa-check').addClass('fa-plus');
        }

        // ↓ vero que é isso
        $('#EstTipoMovimentoId').change(function () {
            configurarCampos();
        });

        //"Limpa" os controles do item
        function limparControles() {
            $('#comboProdutoAprovacao').val(null).trigger("change");
            $('#quantidadeAprovacao').val('');
            $('#idGrid').val('');
            $('#comboUnidadeAprovacao').val(null).trigger("change");

            $('#textProduto').val("");
            $('#textUnidade').val("");
            $('#textQuantidadeItem').val("");
        }

        $('#aceitarTodosId').click(function (e) {
            e.preventDefault();

            var msg;

            if (isAceitarTodos) {
                msg = app.localize("AceitarTodosConfirm");
            } else {
                msg = app.localize("RecusarTodosConfirm");
            };

            abp.message.confirm(
                app.localize(msg),
                function (isConfirmed) {
                    if (isConfirmed) {
                        var itensStr = $('#itens').val(); // ← hidden que guarda stringão de itens

                        if (itensStr == '[]') {
                            itensStr = '';
                        }

                        if (itensStr != '') {
                            itensJson = JSON.parse(itensStr);
                        } else {
                            itensJson = [];
                        };

                        for (var i = 0; i < itensJson.length; i++) {

                            if (isAceitarTodos) {
                                itensJson[i].QuantidadeAprovacao = itensJson[i].Quantidade;
                            } else {
                                itensJson[i].QuantidadeAprovacao = "";
                                itensJson[i].ProdutoAprovacaoId = null;
                                itensJson[i].UnidadeAprovacaoId = null;
                            }
                        }

                        itensStr = JSON.stringify(itensJson);
                        $('#itens').val(itensStr);

                        setAceitarTodosItems();

                        abp.notify.info(app.localize('ListaAtualizada'));

                        // Refresh no JTable dos itens
                        getItemTable();
                    }
                });

        });

        /*==================================================================================================================
             ↓ Comportamentos dos elementos ↓
          ==================================================================================================================*/
        // Elementos no corpo do Pai da Requisicao ↓
        //-------------------------------------------

        $('#quantidadeAprovacao').on('blur', function () {
            if ($('#quantidadeAprovacao').val() == "") {
                $('#quantidadeAprovacao').val(0);
            }
        });

        //combo Motivo Pedido (reposicao de estoque, setor, etc)
        $('#cbo-motivoPedidoid').change(function () {
            if ($('#cbo-motivoPedidoid').val() == motivoPedidoReposicaoEstoque.id) {// se escolher reposicao de estoque

                //$('#comboGrupoProduto').attr('disabled', false);

                var atc = app.localize('Atencao');

                if ($('#cbo-estoqueid').val().length == 0) {
                    abp.message.info(app.localize('RequisicaoCompraSelecionarEstoque'), atc);
                    $('#cbo-estoqueid').focus();
                } else {
                    //$('#comboGrupoProduto').focus();
                }

            } else {
                //$('#comboGrupoProduto').attr('disabled', true);
                //$('#comboGrupoProduto').val(0).change();
            };
        });

        //Combo Grupo de Produtos
        // ↓ Ao selecionar um Grupo de Produto, testa se um estoque foi escolhido
        $('#comboGrupoProduto').on('select2:opening', function (e) {
            if (!($('#cbo-estoqueid').val().length > 0)) {
                e.preventDefault();
                abp.message.warn(app.localize('RequisicaoCompraSelecionarEstoque'), app.localize('Atencao'));
                $('#cbo-estoqueid').focus();
                return false;
            };
        });

        //Combo Grupo de Produtos
        //↓ quando selecionar um grupo, parte para a Requisicao Automatica...
        $('#comboGrupoProduto').on('select2:select', function () {
            //1 - Só pode realizar req aut se for Nova Requisicao? confirmar quando Marcio responder
            //1 - Há produtos disponíveis?
            //2 - Deseja realizar a Req Automatica?

            if (!editMode) {

                abp.message.confirm(
                    app.localize('RealizarRequisicaoAutomatica', ""),
                    function (isConfirmed) {
                        if (isConfirmed) {

                            _compraRequisicaoService.listarRequisicaoAutomatica({ filtro: $('#comboGrupoProduto option:selected').val(), estoqueId: $('#cbo-estoqueid').val() })
                                .done(function (data) {
                                    if (data.items.length > 0) {
                                        //Serializa os itens e seta no hidden
                                        itensStr = JSON.stringify(data.items);

                                        console.log(itensStr);

                                        $('#itens').val(itensStr);

                                        //Limpa os controles do item
                                        //preparaNovoItem();

                                        abp.notify.info(data.items.length);

                                        abp.notify.info(app.localize('ListaAtualizada'));

                                        // Refresh no JTable dos itens
                                        getItemTable();

                                    } else {
                                        abp.message.warn(app.localize('RequisicaoAutomaticaRetornouVazio'), app.localize('Atencao'));
                                    };
                                });

                        }
                    }
                );


            } else {
                abp.message.warn(app.localize('RequisicaoAutomaticaNaoPermitida'), app.localize('Atencao'));
            };

            //grupoRequisicaoAutomatica = $('#comboGrupoProduto').val();

            //TODO: ↓ implementar pra pegar automatico o modo da requisicao
            $('#modoRequisicaoId').val(modoRequisicaoAutomatico.id);
        });

        // combo Estoque
        $('#cbo-estoqueid').on('select2:select', function () {
            ////var tam = $('#comboGrupoProduto').val().length; //Uncaught TypeError: Cannot read property 'length' of null
            //var temVal = $('#comboGrupoProduto').val() == null;

            ////if (temVal) {
            //grupoRequisicaoAutomatica = null; // ← porque fiz isso?????? rs
            ////};

            //$('#comboGrupoProduto').empty()
        });


        // Requisicao - Items ↓
        //--------------------------------------
        // combo de Produto - item da requisicao
        $('#comboProdutoAprovacao').on('select2:select', function (e) {
            e.preventDefault();

            /*
              Confere se o produto está listado no JTable
              Se nao estiver, busca no serviço se há requisicao pendente para o item
            */
            var itemJaSelecionado = false;
            var itemJaSelecionado = encontrarProdutoNoJTable($('#comboProdutoAprovacao').val());

            if ((operacaoCrudDinamico == 1) || !((operacaoCrudDinamico == 2) && (recordEdicao.produtoId == $('#comboProdutoAprovacao').val()))) {
                if (itemJaSelecionado == true) {

                    var msg;
                    msg = app.localize('ProdutoJaSelecionado');
                    if (_permissions.edit) {
                        msg = msg + '\n' + app.localize('ProdutoJaSelecionadoEditar');
                    };
                    abp.message.warn(msg, app.localize('Atencao'));

                    $('#comboProdutoAprovacao').empty();
                    //$('#quantidadeAprovacao').val('');
                    //$('#comboUnidadeAprovacao').empty();
                    //$('#comboUnidadeAprovacao').val('');

                } else {

                    var item; // ← por enquanto, guarda o produto (no futuro, implementar para produto ou serviço)

                    _produtoService.obter($('#comboProdutoAprovacao').val(), { async: false, cache: false })
                        .done(function (data) {
                            item = data;
                        });

                    if (!item.possuiRequisicaoDeCompraPendente) {
                        setarUnidadesDoProdutoSelecionado();

                        $('#comboUnidadeAprovacao').focus();

                    } else {
                        abp.message.confirm(
                            app.localize('ProdutoPossuiRequisicaoCompra'),
                            function (isConfirmed) {
                                if (isConfirmed) {
                                    setarUnidadesDoProdutoSelecionado();
                                    $('#comboUnidadeAprovacao').focus();
                                }
                                else {
                                    $('#comboProdutoAprovacao').empty();
                                    $('#quantidadeAprovacao').val('');
                                    $('#comboUnidadeAprovacao').empty();
                                    $('#comboUnidadeAprovacao').val('');
                                    $('#comboProdutoAprovacao').focus();
                                };
                            });
                    };
                };

            };

        });

        // combo Unidade do Produto
        $('#comboUnidadeAprovacao').on('select2:select', function () {
            //joga foco na quantidade
            $('#quantidadeAprovacao').focus();
        });

        // text Quantidade do Produto
        $('#quantidadeAprovacao').keypress(function (e) {
            if (e.which == 13) {
                $('#salvarItemId').focus();
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
            location.href = '/Mpa/ComprasAprovacao';
        });



        /*==================================================================================================================
            CRUD - Dinamico ↓
          ==================================================================================================================*/
        // Salva o item dinamicamente (não persiste no BD)
        $('#salvarItemId').click(function (e) {
            e.preventDefault();

            //operacoes com o form
            var _$ItemInformationForm = $('form[name=ItemInformationsForm]');

            _$ItemInformationForm.validate();

            if (!_$ItemInformationForm.valid()) {
                return;
            }

            //transforma o form em objeto
            var formObj = _$ItemInformationForm.serializeFormToObject();

            formObj.QuantidadeAprovacao = retirarMascara($('#quantidadeAprovacao').val());

            if (formObj.QuantidadeAprovacao == 0) {
                $('#comboProdutoAprovacao').empty();
                $('#comboUnidadeAprovacao').empty();
                $('#quantidadeAprovacao').val("");
                formObj.QuantidadeAprovacao = "";
            }

            //if (formObj.QuantidadeAprovacao) {
            //};

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
                formObj.IdGrid = itensJson.length == 0 ? 1 : itensJson[itensJson.length - 1].IdGrid + 1; //← inc no idGrid
                formObj.ProdutoId = $('#comboProdutoAprovacao').val();
                //formObj.Quantidade = formObj.Quantidade;
                formObj.comboUnidadeAprovacao = $('#comboUnidadeAprovacao').val();
                //formObj.ModoInclusao = "M"; //← Manual

                formObj.IsSistema = false;
                formObj.IsDeleted = false;

                formObj.CreationTime = moment($.now()).format('YYYY-MM-DD hh:mm:ss.SSS');
                formObj.CreatorUserId = abp.session.userId;

                itensJson.push(formObj);

            } else if (operacaoCrudDinamico == 2) {
                for (var i = 0; i < itensJson.length; i++) {
                    if (itensJson[i].IdGrid == idGrid) {

                        itensJson[i].QuantidadeAprovacao = formObj.QuantidadeAprovacao;
                        itensJson[i].ProdutoAprovacaoId = $('#comboProdutoAprovacao').val();
                        itensJson[i].UnidadeAprovacaoId = $('#comboUnidadeAprovacao').val();
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

            $('#salvarItemId').removeClass('blue');
            $('#cancelarEdicaoItemId').removeClass('corfundo-cancel-close');
            $('.crudDyn').addClass('grey');
            $('.crudDyn').attr('disabled', true);
            $('.aprovacao').attr('disabled', true); //← desabilita os controles para aprovacao

            setAceitarTodosItems();

            abp.notify.info(app.localize('ListaAtualizada'));

            // Refresh no JTable dos itens
            getItemTable();

        });

        $('#cancelarEdicaoItemId').click(function (e) {
            e.preventDefault();

            //Limpa os controles do item
            limparControles();

            $('#salvarItemId').removeClass('blue');
            $('#cancelarEdicaoItemId').removeClass('corfundo-cancel-close');
            $('.crudDyn').addClass('grey');
            $('.crudDyn').attr('disabled', true);
            $('.aprovacao').attr('disabled', true); //← desabilita os controles para aprovacao
        });

        //Save dinamico do item
        function salvarItem(input) {
        };

        //Exclusão dinamica do item
        function deleteItem(input) {

            var msgDialog;

            if (lista[indice].IsDeleted) {
                //msgDialog = app.localize('RestaurarWarning', input.produto.descricao);
                msgDialog = app.localize('RestaurarConfirmacao', input.produto.descricao);
            } else {
                //msgDialog = app.localize('DeleteWarning', input.produto.descricao);
                msgDialog = app.localize('ExcluirConfirmacao', input.produto.descricao);
            };

            abp.message.confirm(
                msgDialog,
                function (isConfirmed) {
                    if (isConfirmed) {
                        //var lista = JSON.parse($('#classes-list').val());

                        preparaNovoItem();

                        var lista = JSON.parse($('#itens').val());
                        //var classe;
                        var indice;
                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == input.idGrid) {
                                //classe = lista[i];
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
                        //localStorage["ClassesList"] = JSON.stringify(lista);
                        abp.notify.info(app.localize('ListaAtualizada'));
                        //abp.event.trigger('app.CriarOuEditarPrescricaoModalSaved');
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

            $('#textProduto').val(registro.produto.descricao);
            $('#textUnidade').val(registro.unidade.descricao);
            $('#textQuantidadeItem').val(registro.quantidade);

            var idProduto;
            var descricaoProduto;
            if (registro.produtoAprovacao) {
                idProduto = registro.produtoAprovacaoId;
                descricaoProduto = registro.produtoAprovacao.descricao;
            } else {
                idProduto = registro.produtoId;
                descricaoProduto = registro.produto.descricao;
            }

            $('#comboProdutoAprovacao').empty();
            $('#comboProdutoAprovacao').append($("<option/>")
                .val(idProduto)
                .text(descricaoProduto))
                .attr('readonly', true)
                .trigger("change");

            setarUnidadesDoProdutoSelecionado();

            var idUnidade;
            var descricaoUnidade;
            if (registro.unidadeAprovacao) {
                idUnidade = registro.unidadeAprovacaoId;
                descricaoUnidade = registro.unidadeAprovacao.descricao;
            } else {
                idUnidade = registro.unidadeId;
                descricaoUnidade = registro.unidade.descricao;
            }

            $('#comboUnidadeAprovacao').empty();
            $('#comboUnidadeAprovacao').append($("<option/>")
                .val(idUnidade)
                .text(descricaoUnidade))
                .attr('readonly', true)
                .trigger("change");


            if (registro.quantidadeAprovacao == null) {
                $('#quantidadeAprovacao').val(registro.quantidade);
            } else {
                $('#quantidadeAprovacao').val(registro.quantidadeAprovacao);
            }

            $('.crudDyn').removeClass('grey');
            $('#salvarItemId').addClass('blue');
            $('#cancelarEdicaoItemId').addClass('corfundo-cancel-close');
            $('.crudDyn').attr('disabled', false);
            $('.aprovacao').attr('disabled', false); //← habilita os controles para aprovacao

            $('#comboProdutoAprovacao').focus();

            // $('#icone-btn-salvar').removeClass('fa-plus').addClass('fa-check');
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
                    abp.notify.error(app.localize('ErroSalvar'));
                    $(this).buttonBusy(false);
                    return;
                }

                var requisicao = _$requisicaoInformationsForm.serializeFormToObject();

                debugger;

                var isAceitarAprovacao = aceitarAprovacao();

                var msg;

                if (isAceitarAprovacao) {
                    msg = app.localize("GravarAprovarRequisicaoConfirm");

                    requisicao.aprovacaoStatusId = 2;
                } else {
                    msg = app.localize("GravarRecusarRequisicaoConfirm");
                    requisicao.aprovacaoStatusId = 5;
                }

                abp.message.confirm(
                    app.localize(msg),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            if (requisicao.aprovacaoStatusId !== 5) {
                                _compraRequisicaoService.aprovarOuRecusarRequisicao(requisicao)
                                    .done(function (data) {

                                        $(this).buttonBusy(false);

                                        abp.notify.info(app.localize('SavedSuccessfully'));

                                        // Fixar modal ou nao, apos save
                                        if (!fixaModal) {
                                            //_modalManager.close();
                                            window.setTimeout(function () {
                                                location.href = '/mpa/ComprasAprovacao' //    location.href = '/mpa/produtos/CriarOuEditarModal/' + data.id
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
                            } else {
                                $(this).buttonBusy(false);
                                $('#ModalNegarAprovacaoCompra').modal('show');
                            }
                        }
                    });
            } catch (erro) {
                console.log(erro);
            }
            finally {
                $(this).buttonBusy(false);
            }
        });

        $("#negarRequisicao").click(function (e) {
            $('#ModalNegarAprovacaoCompra').modal('show');
        });

        $("#confirmarNegacaoCompra").click(function (e) {

            e.preventDefault()

            try {
                debugger;

                abp.ui.setBusy();

                //Operacoes com o form
                var _$requisicaoInformationsForm = $('form[name=requisicaoInformationsForm');

                _$requisicaoInformationsForm.validate();

                if (!_$requisicaoInformationsForm.valid()) {
                    abp.notify.error(app.localize('ErroSalvar'));
                    abp.ui.clearBusy();
                    return;
                }

                if ($("#TextoMotivoNegacaoAprovacaoCompra").val() === '') {
                    abp.notify.error(app.localize('MotivoNegacaoAprovacaoCompra'));
                    abp.ui.clearBusy();
                    return;
                }

                var requisicao = _$requisicaoInformationsForm.serializeFormToObject();

                requisicao.aprovacaoStatusId = 5;
                requisicao.observacao = $("#TextoMotivoNegacaoAprovacaoCompra").val();

                _compraRequisicaoService.aprovarOuRecusarRequisicao(requisicao)
                    .done(function (data) {

                        abp.ui.clearBusy();

                        abp.notify.info(app.localize('SavedSuccessfully'));

                        // Fixar modal ou nao, apos save
                        if (!fixaModal) {
                            //_modalManager.close();
                            window.setTimeout(function () {
                                location.href = '/mpa/ComprasAprovacao' //    location.href = '/mpa/produtos/CriarOuEditarModal/' + data.id
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
                abp.ui.clearBusy();
            }
        });

        /*==================================================================================================================
            Funcoes ↓
          ==================================================================================================================*/
        function resetarCamposAprovacaoItem(registro) {
            var itensStr = $('#itens').val(); // ← hidden que guarda stringão de itens

            if (itensStr == '[]') {
                itensStr = '';
            }

            if (itensStr != '') {
                itensJson = JSON.parse(itensStr);
            } else {
                itensJson = [];
            };

            var idGrid = registro.idGrid;

            for (var i = 0; i < itensJson.length; i++) {

                if (itensJson[i].IdGrid == idGrid) {
                    itensJson[i].QuantidadeAprovacao = "";
                    itensJson[i].ProdutoAprovacaoId = null;
                    itensJson[i].UnidadeAprovacaoId = null;

                    break;
                }
            }


            itensStr = JSON.stringify(itensJson);
            $('#itens').val(itensStr);

            setAceitarTodosItems();

            abp.notify.info(app.localize('ListaAtualizada'));

            // Refresh no JTable dos itens
            getItemTable();
        }


        function setAceitarTodosItems() {

            debugger;

            var itensStr = $('#itens').val(); // ← hidden que guarda stringão de itens

            if (itensStr == '[]') {
                itensStr = '';
            }

            if (itensStr != '') {
                itensJson = JSON.parse(itensStr);
            } else {
                itensJson = [];
            };

            isAceitarTodos = false;
            for (var i = 0; i < itensJson.length; i++) {
                if (itensJson[i].QuantidadeAprovacao == null || itensJson[i].QuantidadeAprovacao == "") {
                    isAceitarTodos = true;
                    break;
                }
            }

            if (isAceitarTodos) {
                $('#aceitarTodosId').text(app.localize("AceitarTodos"));
            } else {
                $('#aceitarTodosId').text(app.localize("RecusarTodos"));
            };

        }

        function aceitarAprovacao() {
            debugger;

            var itensStr = $('#itens').val(); // ← hidden que guarda stringão de itens

            if (itensStr == '[]') {
                itensStr = '';
            }

            if (itensStr != '') {
                itensJson = JSON.parse(itensStr);
            } else {
                itensJson = [];
            };

            var isAceitar = false;

            for (var i = 0; i < itensJson.length; i++) {
                if (itensJson[i].QuantidadeAprovacao != null || (itensJson[i].QuantidadeAprovacao != "")) {
                    if (itensJson[i].QuantidadeAprovacao > 0) {
                        isAceitar = true;
                        break;
                    }
                }
            }

            return isAceitar;
        }

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
            //Carrega combo de unidade com as unidades do produto selecionado
            //selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['produtoId']);
            selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['comboProdutoAprovacao']);
            //selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadeComprasPorProdutoDropdown', ['produtoId']);

            //TODO: quando retornar apenas uma unidade, setar o combo automaticamente
            var comboUnidade = $('#comboUnidadeAprovacao');

            if ($('#comboUnidadeAprovacao').children('option').length == 1) {
                $('#comboUnidadeAprovacao option:first-child').attr("selected", "selected");
                $('#comboUnidadeAprovacao').trigger("change");
            };
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

        selectSW('.selectForncedor', "/api/services/app/fornecedor/ListarDropdown");
        selectSW('.selectAtendimento', "/api/services/app/Atendimento/ListarDropdown");
        selectSW('.selectPaciente', "/api/services/app/Paciente/ListarDropdown");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");
        selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoDropdown");

        //selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadeComprasPorProdutoDropdown', ['produtoId']);
        //selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['produtoId']);
        selectSWMultiplosFiltros('.selectProdutoUnidade', '/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown', ['comboProdutoAprovacao']);


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
                rowUpdated: function (event, data) {
                    if (data) {
                        if (data.record) {

                            if (data.record.produtoAprovacao == null) {
                                data.row[0].cells[4].setAttribute('bgcolor', "#f9f9f9");
                                data.row[0].cells[5].setAttribute('bgcolor', "#f9f9f9");
                                data.row[0].cells[6].setAttribute('bgcolor', "#f9f9f9");
                            };

                            data.row.click();
                        }
                    }
                },

                rowInserted: function (event, data) {
                    if (data) {

                        if (data.record) {

                            if (data.record.produtoAprovacao == null) {
                                data.row[0].cells[4].setAttribute('bgcolor', "#f9f9f9");
                                data.row[0].cells[5].setAttribute('bgcolor', "#f9f9f9");
                                data.row[0].cells[6].setAttribute('bgcolor', "#f9f9f9");
                            };

                            data.row.click();
                        }
                    }
                },

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
                        width: '8%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');

                            if (_permissions.edit) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('AprovarEsteItem') + '"><i class="fa fa-edit"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();

                                        //editItem(data.record.idGrid);
                                        editItem(data.record);
                                    });
                            }

                            if (_permissions.edit /*&& data.record.produtoAprovacao*/) {

                                $('<button class="btn btn-default btn-xs" id="cancelarAprovacaoItem" title="' + app.localize('CancelarAprovacaoDesteItem') + '" ' + (data.record.produtoAprovacao && data.record.produtoAprovacao != null && data.record.quantidadeAprovacao > 0 ? "" : "disabled='disabled'") + '><i class="fa fa-eraser"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        abp.message.confirm(
                                            app.localize('CancelarAprovacaoItem', data.record.produtoAprovacao.descricao),
                                            function (isConfirmed) {
                                                if (isConfirmed) {
                                                    resetarCamposAprovacaoItem(data.record);
                                                }
                                            });

                                    });
                            }

                            return $span;
                        }
                    },

                    produto: {
                        title: app.localize('Produto'),
                        width: '29%',
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

                            if (data.record.quantidade) {

                                if (data.record.isDeleted) {
                                    return posicionarDireita('<span style="text-decoration:line-through;color:red;">' + data.record.quantidade + '</span>');
                                }
                                else {
                                    return data.record.quantidade;
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

                    //Campos da aprovacao
                    ,
                    produtoAprovacao: {
                        title: app.localize('ProdutoAprovacao'),
                        width: '29%',
                        display: function (data) {
                            if (data.record.produtoAprovacao && data.record.produtoAprovacao != null) {
                                if (data.record.isDeleted) {
                                    return '<span style="text-decoration:line-through;color:red;">' + data.record.produtoAprovacao.codigo + ' - ' + data.record.produtoAprovacao.descricao + '</span>';
                                }
                                else {
                                    return data.record.produtoAprovacao.codigo + ' - ' + data.record.produtoAprovacao.descricao;
                                }
                            }
                        }
                    },

                    quantidadeAprovacao: {
                        title: app.localize('QtdAprovacao'),
                        width: '5%',
                        display: function (data) {
                            if (data.record.quantidadeAprovacao && data.record.quantidadeAprovacao != null) {
                                if (data.record.isDeleted) {
                                    return posicionarDireita('<span style="text-decoration:line-through;color:red;">' + data.record.quantidadeAprovacao + '</span>');
                                }
                                else {
                                    return data.record.quantidadeAprovacao;
                                }
                            }
                        }
                    },

                    unidadeAprovacao: {
                        title: app.localize('UnidadeAprovacao'),
                        width: '12%',
                        display: function (data) {
                            if (data.record.unidadeAprovacao && data.record.unidadeAprovacao != null) {
                                if (data.record.isDeleted) {
                                    return '<span style="text-decoration:line-through;color:red;">' + data.record.unidadeAprovacao.descricao + '</span>';
                                }
                                else {
                                    return data.record.unidadeAprovacao.descricao;
                                }
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
    });

})(jQuery);