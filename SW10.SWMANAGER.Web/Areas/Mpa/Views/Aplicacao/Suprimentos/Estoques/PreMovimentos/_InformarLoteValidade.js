(function () {

    $(function () {
        iValidador.init();

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
                    method: _loteValidadeService.ListarPorMovimentacaoLoteValidade
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
                                .click(function () {
                                    //_createOrEditModal.open({ id: data.record.id });
                                });
                   


                        return $span;
                    }
                },
                Produto: {
                    title: app.localize('Produto '),
                    width: '15%',
                    display: function (data) {
                        return data.record.produto.descricao;
                    }
                },
                Validade: {
                    title: app.localize('Validade'),
                    width: '15%',
                    display: function (data) {
                        return moment(data.record.Validade).format('L LT');
                    }
                },
                //Lote: {
                //    title: app.localize('Lote'),
                //    width: '15%',
                //    display: function (data) {
                //        return data.record.Lote;
                //    }
                //},
                //Fabricante: {
                //    title: app.localize('Fabricante'),
                //    width: '15%',
                //    display: function (data) {
                //        return data.record.Fabricante.descricao;
                //    }
                //}
            }

        });

    })
})