(function () {
    $(function () {

        //DIVs Abas
        //------------------------------------------------------------------------------------------------------------------------
        var _$ProdutoSaldoTable = $('#ProdutoSaldoTable');

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

        //function criarGrid() {
        //    _$ProdutoSaldoTable.jtable({
        //        title: app.localize('Saldo'),
        //        paging: true,
        //        sorting: true,
        //        useBootstrap: true,
        //        multiSorting: true,

        //        actions: {
        //            listAction: {
        //                method: _produtosService.listarProdutoSaldo
        //            }
        //        },

        //        fields:
        //        {
        //            id: {
        //                key: true,
        //                list: false
        //            },

        //            produtoId: {
        //                type: 'hidden',
        //                defaultValue: function (data) {
        //                    return data.record.produtoId;
        //                },
        //            },

        //            actions: {
        //                title: app.localize('Actions'),
        //                width: '5%',
        //                sorting: false,
        //                display: function (data) {
        //                    var $span = $('<span></span>');
        //                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-file-text-o"></i></button>')
        //                        .appendTo($span)
        //                        .click(function (e) {
        //                           
        //                            e.preventDefault();
        //                            _produtoSaldoModal.open({ id: data.record.produtoId, idEstoque: data.record.estoqueId });
        //                        });
        //                    return $span;
        //                }
        //            },

        //            descricao: {
        //                title: app.localize('Estoque'),
        //                width: '13%',
        //                //display: function (data) {
        //                //    if (data.record.estoque) {
        //                //        return data.record.estoque.descricao;
        //                //    }
        //                //}
        //            },
        //            quantidadeAtual: {
        //                title: app.localize('SaldoAtual'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.quantidadeAtual + " " + $('#codigo-unidade-referencial').val());
        //                    }
        //                }
        //            },
        //            quantidadeEntradaPendente: {
        //                title: app.localize('EntradaPendente'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.quantidadeEntradaPendente + " " + $('#codigo-unidade-referencial').val());
        //                    }
        //                }
        //            },
        //            quantidadeSaidaPendente: {
        //                title: app.localize('SaidaPendente'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.quantidadeSaidaPendente + " " + $('#codigo-unidade-referencial').val());
        //                    }
        //                }
        //            }
        //            ,
        //            saldoFuturo: {
        //                title: app.localize('SaldoFuturo'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.saldoFuturo + " " + $('#codigo-unidade-referencial').val());
        //                    }
        //                }
        //            },


        //            quantidadeGerencialAtual: {
        //                title: app.localize('SaldoAtual'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.quantidadeGerencialAtual + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            },
        //            quantidadeGerencialEntradaPendente: {
        //                title: app.localize('EntradaPendente'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.quantidadeGerencialEntradaPendente + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            },
        //            quantidadeGerencialSaidaPendente: {
        //                title: app.localize('SaidaPendente'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.quantidadeGerencialSaidaPendente + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            },
        //            saldoGerencialFuturo: {
        //                title: app.localize('SaldoFuturo'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.saldoGerencialFuturo + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            }

        //        }
        //    });

        //};

        //function criarGrid() {
        _$ProdutoSaldoTable.jtable({
            title: app.localize('Saldo'),
            paging: true,
            sorting: true,
            useBootstrap: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _produtosService.listarProdutoSaldo
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
                quantidadeAtual: {
                    title: app.localize('SaldoAtual'),
                    width: '10,25%',
                    display: function (data) {
                        if (data.record) {
                            return (data.record.quantidadeAtual + " " + $('#codigo-unidade-referencial').val() + ' / ' +
                                data.record.quantidadeGerencialAtual + " " + $('#codigo-unidade-gerencial').val());
                        }
                    }
                },
                quantidadeEntradaPendente: {
                    title: app.localize('EntradaPendente'),
                    width: '10,25%',
                    display: function (data) {
                        if (data.record) {
                            return (data.record.quantidadeEntradaPendente + " " + $('#codigo-unidade-referencial').val() + ' / ' +
                                data.record.quantidadeGerencialEntradaPendente + " " + $('#codigo-unidade-gerencial').val());
                        }
                    }
                },
                quantidadeSaidaPendente: {
                    title: app.localize('SaidaPendente'),
                    width: '10,25%',
                    display: function (data) {
                        if (data.record) {
                            return (data.record.quantidadeSaidaPendente + " " + $('#codigo-unidade-referencial').val() + ' / ' +
                                data.record.quantidadeGerencialSaidaPendente + " " + $('#codigo-unidade-gerencial').val());
                        }
                    }
                }
                ,
                saldoFuturo: {
                    title: app.localize('SaldoFuturo'),
                    width: '10,25%',
                    display: function (data) {
                        if (data.record) {
                            return (data.record.saldoFuturo + " " + $('#codigo-unidade-referencial').val() + ' / ' +
                                data.record.saldoGerencialFuturo + " " + $('#codigo-unidade-gerencial').val());
                        }
                    }
                }
            }
        });

        //};

        //_$ProdutoSaldoTable.jtable({
        //    title: app.localize('Saldo'),
        //    paging: true,
        //    sorting: true,
        //    useBootstrap: true,
        //    multiSorting: true,

        //    actions: {
        //        listAction: {
        //          method: _produtosService.listarProdutoSaldo
        //        }
        //    },

        //    fields:
        //    {
        //        id: {
        //            key: true,
        //            list: false
        //        },

        //        produtoId: {
        //            type: 'hidden',
        //            defaultValue: function (data) {
        //                return data.record.produtoId;
        //            },
        //        },

        //        actions: {
        //            title: app.localize('Actions'),
        //            width: '5%',
        //            sorting: false,
        //            display: function (data) {
        //                var $span = $('<span></span>');
        //                $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-file-text-o"></i></button>')
        //                    .appendTo($span)
        //                    .click(function (e) {
        //                       
        //                        e.preventDefault();
        //                        _produtoSaldoModal.open({ id: data.record.produtoId, idEstoque: data.record.estoqueId });
        //                    });
        //                return $span;
        //            }
        //        },

        //        descricao: {
        //            title: app.localize('Estoque'),
        //            width: '13%',
        //            //display: function (data) {
        //            //    if (data.record.estoque) {
        //            //        return data.record.estoque.descricao;
        //            //    }
        //            //}
        //        },
        //        quantidadeAtual: {
        //            title: app.localize('SaldoAtual'),
        //            width: '10,25%',
        //            display: function (data) {
        //                if (data.record) {
        //                    return (data.record.quantidadeAtual + " " + $('#codigo-unidade-referencial').val());
        //                }
        //            }
        //        },
        //        quantidadeEntradaPendente: {
        //            title: app.localize('EntradaPendente'),
        //            width: '10,25%',
        //            display: function (data) {
        //                if (data.record) {
        //                    return (data.record.quantidadeEntradaPendente + " " + $('#codigo-unidade-referencial').val());
        //                }
        //            }
        //        },
        //        quantidadeSaidaPendente: {
        //            title: app.localize('SaidaPendente'),
        //            width: '10,25%',
        //            display: function (data) {
        //                if (data.record) {
        //                    return (data.record.quantidadeSaidaPendente + " " + $('#codigo-unidade-referencial').val());
        //                }
        //            }
        //        }
        //        ,
        //        saldoFuturo: {
        //            title: app.localize('SaldoFuturo'),
        //            width: '10,25%',
        //            display: function (data) {
        //                if (data.record) {
        //                    return (data.record.saldoFuturo + " " + $('#codigo-unidade-referencial').val());
        //                }
        //            }
        //        },


        //            quantidadeGerencialAtual: {
        //                title: app.localize('SaldoAtual'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.quantidadeGerencialAtual + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }                        
        //            },
        //            quantidadeGerencialEntradaPendente: {
        //                title: app.localize('EntradaPendente'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.quantidadeGerencialEntradaPendente + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            },
        //            quantidadeGerencialSaidaPendente: {
        //                title: app.localize('SaidaPendente'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.quantidadeGerencialSaidaPendente + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            },
        //            saldoGerencialFuturo: {
        //                title: app.localize('SaldoFuturo'),
        //                width: '10,25%',
        //                display: function (data) {
        //                    if (data.record) {
        //                        return (data.record.saldoGerencialFuturo + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            }

        //    }
        //});

        //criarGrid();

        function getProdutoSaldoTable(reload) {
            if (reload) {
                _$ProdutoSaldoTable.jtable('reload');
            } else {
                _$ProdutoSaldoTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        //getProdutoSaldoTable();

        //Botao que persistem as relações de produtos com outros cadastros
        //------------------------------------------------------------------------------------------------------------------------
        $('#btn-atualizar').click(function (e) {
            e.preventDefault();
            getProdutoSaldoTable();
        });

        //------------------------------------------------------------------------------------------------------------------------
        //Atualizacoes das Grids das Abas após Salvar Inclusoes/Alteracoes
        abp.event.on('app.CriarOuEditarProdutoSaldoModalSaved', function () {
            getProdutoSaldoTable();
        });
        $('#href_saldo').on('click', function (e) {
            e.preventDefault();
            //criarGrid();
            getProdutoSaldoTable();
        });

    });
})();