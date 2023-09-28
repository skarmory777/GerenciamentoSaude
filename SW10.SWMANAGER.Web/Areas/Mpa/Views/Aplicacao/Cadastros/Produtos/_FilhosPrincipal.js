(function () {
    $(function () {

        //DIVs Abas
        //------------------------------------------------------------------------------------------------------------------------
        var _$ProdutoFilhosSaldoTable = $('#FilhosPrincipalTable');

        //Serviços
        //------------------------------------------------------------------------------------------------------------------------
        var _produtosService = abp.services.app.produto;

        function criarGrid() {
            _$ProdutoFilhosSaldoTable.jtable({
                title: app.localize('ProdutoMesmoPrincipal'),
                paging: true,
                sorting: true,
                useBootstrap: true,
                multiSorting: true,
                actions: {
                    listAction: {
                        method: _produtosService.listarProdutoMesmoPrincipalComSaldo
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
                                    location.href = '/mpa/produtos/CriarOuEditarModal/' + data.record.produtoId
                                });
                            return $span;
                        }
                    },
                    codigo: {
                        title: app.localize('Codigo')
                    },
                    descricaoProduto: {
                        title: app.localize('Produto'),
                        width: '25%',
                    },
                    quantidadeAtual: {
                        title: app.localize('SaldoAtual'),
                        width: '17,5%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.quantidadeAtual + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.quantidadeGerencialAtual + " " + data.record.unidadeGerencial);
                            }
                        }
                    },
                    quantidadeEntradaPendente: {
                        title: app.localize('EntradaPendente'),
                        width: '17,5%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.quantidadeEntradaPendente + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.quantidadeGerencialEntradaPendente + " " + data.record.unidadeGerencial);
                            }
                        }
                    },
                    quantidadeSaidaPendente: {
                        title: app.localize('SaidaPendente'),
                        width: '17,5%',
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
                        width: '17,5%',
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
                _$ProdutoFilhosSaldoTable.jtable('load', { id: $('#id').val(), principalId: $('#produtoPrincipalId').val() } );
            }
        }

        if (($('#produtoPrincipalId').val() != "") && ($('#produtoPrincipalId').val() != null) && ($('#produtoPrincipalId').val() != undefined)) {
          getProdutoSaldoTable();
        }

    });
})();