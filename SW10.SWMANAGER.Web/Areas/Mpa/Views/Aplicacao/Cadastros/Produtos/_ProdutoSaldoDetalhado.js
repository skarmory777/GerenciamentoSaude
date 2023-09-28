(function ($) {
    app.modals.ProdutoSaldoDetalhadoModal = function () {

        //------------------------------------------------------------------------------------------------------------------------
        var _$ProdutoSaldoTable = $('#ProdutoSaldoDetalhadoTable');

        //Serviços
        //------------------------------------------------------------------------------------------------------------------------
        var _produtosService = abp.services.app.produto;

        //------------------------------------------------------------------------------------------------------------------------
        var _modalManager;
        //var _$UnidadeInformationForm = null;

        this.init = function (modalManager) {

            //_modalManager = modalManager;

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '1000px' });

            $('.save-button').hide();

            ////Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };

        };

        //---------------------------------------------------------------------------------------------------


        //function criarGrid() {

        //    _$ProdutoSaldoTable.jtable({
        //        title: app.localize('Saldo'),
        //        paging: true,
        //        sorting: true,
        //        useBootstrap: true,
        //        multiSorting: true,

        //        actions: {
        //            listAction: {
        //                method: _produtosService.listarProdutoSaldoDetalhes
        //            }
        //        },

        //        fields:
        //        {
        //            //id: {
        //            //    key: true,
        //            //    list: false
        //            //},

        //            //produtoId: {
        //            //    type: 'hidden',
        //            //    defaultValue: function (data) {
        //            //        return $('#produtoId').val();
        //            //    },
        //            //},

        //            nomeLaboratorio: {
        //                title: app.localize('Laboratorio'),
        //                width: '20%'
        //            },
                    
        //            lote: {
        //                title: app.localize('Lote'),
        //                width: '8%'
        //            },

        //            validade: {
        //                title: app.localize('Validade'),
        //                width: '8%',
        //                display: function (data) {
        //                    return moment(data.record.validade).format('L');
        //                }
        //            },

        //            quantidadeAtual: {
        //                title: app.localize('SaldoAtual'),
        //                width: '8%',
        //                display: function (data) {
        //                    if (data.record) {
        //                       
        //                        return (data.record.quantidadeAtual + " " + $('#codigo-unidade-referencial').val());
        //                    }
        //                }
        //            },
        //            quantidadeEntradaPendente: {
        //                title: app.localize('EntradaPendente'),
        //                width: '8%',
        //                display: function (data) {
        //                    if (data.record) {
        //                       
        //                        return (data.record.quantidadeEntradaPendente + " " + $('#codigo-unidade-referencial').val());
        //                    }
        //                }
        //            },
        //            quantidadeSaidaPendente: {
        //                title: app.localize('SaidaPendente'),
        //                width: '8%',
        //                display: function (data) {
        //                    if (data.record) {
        //                       
        //                        return (data.record.quantidadeSaidaPendente + " " + $('#codigo-unidade-referencial').val());
        //                    }
        //                }
        //            }
        //            ,
        //            saldoFuturo: {
        //                title: app.localize('SaldoFuturo'),
        //                width: '8%',
        //                display: function (data) {
        //                    if (data.record) {
        //                       
        //                        return (data.record.saldoFuturo + " " + $('#codigo-unidade-referencial').val());
        //                    }
        //                }
        //            },

        //            quantidadeGerencialAtual: {
        //                title: app.localize('SaldoAtual'),
        //                width: '8%',
        //                display: function (data) {
        //                    if (data.record) {
        //                       
        //                        return (data.record.quantidadeGerencialAtual + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }                        
        //            },
        //            quantidadeGerencialEntradaPendente: {
        //                title: app.localize('EntradaPendente'),
        //                width: '8%',
        //                display: function (data) {
        //                    if (data.record) {
        //                       
        //                        return (data.record.quantidadeGerencialEntradaPendente + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            },
        //            quantidadeGerencialSaidaPendente: {
        //                title: app.localize('SaidaPendente'),
        //                width: '8%',
        //                display: function (data) {
        //                    if (data.record) {
        //                       
        //                        return (data.record.quantidadeGerencialSaidaPendente + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            },
        //            saldoGerencialFuturo: {
        //                title: app.localize('SaldoFuturo'),
        //                width: '8%',
        //                display: function (data) {
        //                    if (data.record) {
        //                       
        //                        return (data.record.saldoGerencialFuturo + " " + $('#codigo-unidade-gerencial').val());
        //                    }
        //                }
        //            }
        //        }
        //    });
        //};


        function criarGrid() {

            _$ProdutoSaldoTable.jtable({
                title: app.localize('SaldoRefGer'),
                paging: true,
                sorting: true,
                useBootstrap: true,
                multiSorting: true,

                actions: {
                    listAction: {
                        method: _produtosService.listarProdutoSaldoDetalhes
                    }
                },

                fields:
                {

                    nomeLaboratorio: {
                        title: app.localize('Laboratorio'),
                        width: '20%'
                    },

                    lote: {
                        title: app.localize('Lote'),
                        width: '8%'
                    },

                    validade: {
                        title: app.localize('Validade2'),
                        width: '8%',
                        display: function (data) {
                            return moment(data.record.validade).format('L');
                        }
                    },

                    quantidadeAtual: {
                        title: app.localize('SaldoAtual'),
                        width: '8%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.quantidadeAtual + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.quantidadeGerencialAtual + " " + data.record.unidadeGerencial);
                            }
                        }
                    },
                    quantidadeEntradaPendente: {
                        title: app.localize('EntradaPendente'),
                        width: '8%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.quantidadeEntradaPendente + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.quantidadeGerencialEntradaPendente + " " +  data.record.unidadeGerencial);
                            }
                        }
                    },
                    quantidadeSaidaPendente: {
                        title: app.localize('SaidaPendente'),
                        width: '8%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.quantidadeSaidaPendente + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.quantidadeGerencialSaidaPendente + " " +  data.record.unidadeGerencial);
                            }
                        }
                    }
                    ,
                    saldoFuturo: {
                        title: app.localize('SaldoFuturo'),
                        width: '8%',
                        display: function (data) {
                            if (data.record) {
                                return (data.record.saldoFuturo + " " + data.record.unidadeReferencia + ' / ' +
                                    data.record.saldoGerencialFuturo + " " +  data.record.unidadeGerencial);
                            }
                        }
                    }
                    //,

                    //quantidadeGerencialAtual: {
                    //    title: app.localize('SaldoAtual'),
                    //    width: '8%',
                    //    display: function (data) {
                    //        if (data.record) {
                    //           
                    //            return (data.record.quantidadeGerencialAtual + " " + $('#codigo-unidade-gerencial').val());
                    //        }
                    //    }
                    //},
                    //quantidadeGerencialEntradaPendente: {
                    //    title: app.localize('EntradaPendente'),
                    //    width: '8%',
                    //    display: function (data) {
                    //        if (data.record) {
                    //           
                    //            return (data.record.quantidadeGerencialEntradaPendente + " " + $('#codigo-unidade-gerencial').val());
                    //        }
                    //    }
                    //},
                    //quantidadeGerencialSaidaPendente: {
                    //    title: app.localize('SaidaPendente'),
                    //    width: '8%',
                    //    display: function (data) {
                    //        if (data.record) {
                    //           
                    //            return (data.record.quantidadeGerencialSaidaPendente + " " + $('#codigo-unidade-gerencial').val());
                    //        }
                    //    }
                    //},
                    //saldoGerencialFuturo: {
                    //    title: app.localize('SaldoFuturo'),
                    //    width: '8%',
                    //    display: function (data) {
                    //        if (data.record) {
                    //           
                    //            return (data.record.saldoGerencialFuturo + " " + $('#codigo-unidade-gerencial').val());
                    //        }
                    //    }
                    //}
                }
            });
        };

        //Grid
        //----------------------------------------------------------------------------------------------------
        criarGrid();

        function resetGrid() {
            _$ProdutoSaldoTable.jtable('destroy');
            criarGrid();
        }

        function getProdutoSaldoTable(reload) {

            criarGrid();
            if (reload) {
                _$ProdutoSaldoTable.jtable('reload');
            } else {
                _$ProdutoSaldoTable.jtable('load', { id: $('#idProduto').val(), estoqueId: $('#idEstoque').val() });
            }
        }

        getProdutoSaldoTable();

    };
})(jQuery);