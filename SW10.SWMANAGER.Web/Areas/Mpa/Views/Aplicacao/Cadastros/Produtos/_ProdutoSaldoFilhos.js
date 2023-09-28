(function () {
    $(function () {

        //DIVs Abas
        //------------------------------------------------------------------------------------------------------------------------
        var _$ProdutoFilhosSaldoTable = $('#ProdutoFilhosSaldoTable');

        //Serviços
        //------------------------------------------------------------------------------------------------------------------------
        var _produtosService = abp.services.app.produto;

        //ModalManagers
        //------------------------------------------------------------------------------------------------------------------------
        var _produtoSaldoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Produtos/ProdutoSaldoDetalhadoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_ProdutoSaldoDetalhado.js',
            modalClass: 'ProdutoSaldoDetalhadoModal'

        });

        function criarGrid() {
            _$ProdutoFilhosSaldoTable.jtable({
                title: app.localize('Saldo'),
                paging: true,
                sorting: true,
                useBootstrap: true,
                multiSorting: true,

                actions: {
                    listAction: {
                        method: _produtosService.listarProdutoSaldoFilhos
                    }
                },

                fields:
                {
                    id: {
                        key: true,
                        list: false
                    },

                    produtoId: {
                        type: 'hidden',
                        defaultValue: function (data) {
                            return data.record.produtoId;
                        },
                    },

                    actions: {
                        title: app.localize('Actions'),
                        width: '5%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-file-text-o"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _produtoSaldoModal.open({ id: data.record.produtoId, idEstoque: data.record.estoqueId });
                                });
                            return $span;
                        }
                    },

                    descricao: {
                        title: app.localize('Estoque'),
                        width: '13%',
                        //display: function (data) {
                        //    if (data.record.estoque) {
                        //        return data.record.estoque.descricao;
                        //    }
                        //}
                    },

                    ProdutoId: {
                        title: app.localize('Produto'),
                        width: '30%',
                        display: function (data) {
                            if (data.record) {
                                return data.record.descricaoProduto;
                            }
                        }
                    },

                    quantidadeAtual: {
                        title: app.localize('SaldoAtual'),
                        width: '10,25%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.quantidadeAtual + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.quantidadeGerencialAtual + " " + data.record.unidadeGerencial);
                            }
                        }
                    },
                    quantidadeEntradaPendente: {
                        title: app.localize('EntradaPendente'),
                        width: '10,25%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.quantidadeEntradaPendente + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.quantidadeGerencialEntradaPendente + " " + data.record.unidadeGerencial);
                            }
                        }
                    },
                    quantidadeSaidaPendente: {
                        title: app.localize('SaidaPendente'),
                        width: '10,25%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.quantidadeSaidaPendente + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.quantidadeGerencialSaidaPendente + " " + data.record.unidadeGerencial);
                            }
                        }
                    }
                    ,
                    saldoFuturo: {
                        title: app.localize('SaldoFuturo'),
                        width: '10,25%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.saldoFuturo + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.saldoGerencialFuturo + " " + data.record.unidadeGerencial);
                            }
                        }
                    }
                }
            });

        };

        criarGrid();

        function getProdutoSaldoTable(reload) {

            if (reload) {
                _$ProdutoFilhosSaldoTable.jtable('reload');
            } else {
                _$ProdutoFilhosSaldoTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        getProdutoSaldoTable();

        //Botao que persistem as relações de produtos com outros cadastros
        //------------------------------------------------------------------------------------------------------------------------
        $('#btn-atualizarSaldoFilho').click(function (e) {
            e.preventDefault();
            getProdutoSaldoTable();
        });

        //------------------------------------------------------------------------------------------------------------------------
        //Atualizacoes das Grids das Abas após Salvar Inclusoes/Alteracoes
        abp.event.on('app.CriarOuEditarProdutoSaldoModalSaved', function () {
            getProdutoSaldoTable();
        });

    });
})();