(function () {
    $(function () {

        //DIVs Abas
        //------------------------------------------------------------------------------------------------------------------------
        var _$ProdutoRelacaoCompraTable = $('#ProdutoRelacaoCompraTable');

        //Serviços
        //------------------------------------------------------------------------------------------------------------------------
        var _produtosRelacaoCompraService = abp.services.app.produtoRelacaoCompra;

        //ModalManagers
        //------------------------------------------------------------------------------------------------------------------------
        var _createOrEditProdutoRelacaoCompraModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Produtos/CriarOuEditarProdutoRelacaoCompraModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoRelacaoCompraModal.js',
            modalClass: 'CriarOuEditarProdutoRelacaoCompraModal'
        });

        _$ProdutoRelacaoCompraTable.jtable({
            title: app.localize('Compra'),
            paging: true,
            sorting: true,
            useBootstrap: true,
            multiSorting: true,

            actions: {
                listAction: {
                    // method: _produtosRelacaoCompraService.listarPorProduto
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
                        return $('#id').val();
                    },
                },

                actions: {
                    title: app.localize('Actions'),
                    width: '4%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        //if (_permissions.edit) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                               
                                e.preventDefault();
                                //                                _createOrEditProdutoRelacaoCompraModal.open({ id: data.record.id });
                                _createOrEditProdutoRelacaoCompraModal.open({ id: data.record.id, idProduto: $("#id").val() });

                            });

                        //if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                //console.log(data.record);
                                deleteProdutoRelacaoCompra(data.record);
                            });

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '5%',
                    create: false,
                    edit: false,
                    display: function (data) {
                        if (data.record.produtoCompra) {
                            return data.record.produtoCompra.codigo;
                        }
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '40%',
                    display: function (data) {
                        if (data.record.produtoCompra) {
                            return data.record.produtoCompra.descricao;
                        }
                    }
                }
            }
        });

        function deleteProdutoRelacaoCompra(ProdutoRelacaoCompra) {
            //console.log(ProdutoRelacaoCompra);
            abp.message.confirm(
            app.localize('DeleteWarning', ProdutoRelacaoCompra.produtoCompra.descricao),
            function (isConfirmed) {
                //app.localize('DeleteWarning', ProdutoRelacaoCompra.descricao)
                //console.log(isConfirmed);
                if (isConfirmed) {
                    //if (isConfirmed) {
                    _produtosRelacaoCompraService.excluir(ProdutoRelacaoCompra.id)
                        .done(function () {
                            //console.log('delete');
                            sessionStorage['ProdutoId'] = ProdutoRelacaoCompra.produtoId;
                            getProdutoRelacaoCompraTable(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                }
            }
            );
        }

        function getProdutoRelacaoCompraTable(reload) {

            if (reload) {
                _$ProdutoRelacaoCompraTable.jtable('reload');
            } else {
                _$ProdutoRelacaoCompraTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        getProdutoRelacaoCompraTable();

        //Botao que persistem as relações de produtos com outros cadastros
        //------------------------------------------------------------------------------------------------------------------------
        $('#btn-novo-produtorelacaocompra').click(function (e) {
            e.preventDefault();
            _createOrEditProdutoRelacaoCompraModal.open({ id: null, idProduto: $("#id").val() });
        });

        //------------------------------------------------------------------------------------------------------------------------
        //Atualizacoes das Grids das Abas após Salvar Inclusoes/Alteracoes
        abp.event.on('app.CriarOuEditarProdutoRelacaoCompraModalSaved', function () {
            getProdutoRelacaoCompraTable(true);
        });

    });
})();