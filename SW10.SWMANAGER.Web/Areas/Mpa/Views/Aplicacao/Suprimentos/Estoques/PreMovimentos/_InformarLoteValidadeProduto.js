(function () {
    app.modals.EstoquePreMovimentoLoteValidadeProdutoViewModel = function () {






        alert('sddf');

        $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        $('ul.ui-autocomplete').css('z-index', '2147483647');

        //CriarAutoComplete('empresa-search', 'Empresa-id', '/mpa/empresas/autocompleteDescricao', app.localize("Empresa"));
        //  CriarAutoComplete('fornecedor-search', 'Forncedor-id', '/mpa/fornecedores/autocomplete', app.localize("Fornecedor"));       


        var _loteValidadeService = abp.services.app.estoquePreMovimentoItem;

        _$LoteValidadeTable.jtable({

            title: app.localize('LoteValidade'),
            paging: true,
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,


            actions: {
                listAction: {
                    method: _loteValidadeService.listarPorMovimentacaoLoteValidadeProduto
                }
            },

            fields: {
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


                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                _createOrEditLoteValidadeModal.open({ preMovimentoItemId: data.record.id });
                            });


                        return $span;
                    }
                },

                //ProdutoId: {
                //    type: 'hidden',
                //    defaultValue: function (data) {
                //        if (data.record.produto) {
                //            return data.record.produto.id;
                //        }
                //    }
                //},

                //Quantidade: {
                //    title: app.localize('Quantidade'),
                //    width: '20%',
                //    display: function (data) {
                //        if (data.record.quantidade) {
                //            return data.record.quantidade;
                //        }
                //    }
                //},

                //Lote: {
                //    title: app.localize('Lote'),
                //    width: '20%',
                //    display: function (data) {
                //        if (data.record.estoquePreMovimentoLoteValidade) {
                //            return data.record.estoquePreMovimentoLoteValidade.lote;
                //        }
                //    }
                //},


                //Lote: {
                //    title: app.localize('Lote'),
                //    width: '20%',
                //    display: function (data) {
                //        if (data.record.estoquePreMovimentoLoteValidade) {
                //            return data.record.estoquePreMovimentoLoteValidade.lote;
                //        }
                //    }
                //},

                //Validade: {
                //    title: app.localize('Validade'),
                //    width: '20%',
                //    display: function (data) {
                //        if (data.record.estoquePreMovimentoLoteValidade) {
                //            return moment(data.record.estoquePreMovimentoLoteValidade.Validade).format("L"); ;
                //        }
                //    }
                //},

            }

        });

        function getLoteValidadeTable(reload) {

            if (reload) {
                $(LoteValidadeTable).jtable('reload');
            } else {
                 $(LoteValidadeTable).jtable('load', { preMovimentoId: $('#PreMovimentoId').val(), produtoId: $('#produtoId').val() });
            }
        }


        getLoteValidadeTable();


    }
})