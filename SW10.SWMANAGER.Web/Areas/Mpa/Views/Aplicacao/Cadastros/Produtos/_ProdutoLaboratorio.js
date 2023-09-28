(function () {
    $(function () {

        //DIVs Abas
        //------------------------------------------------------------------------------------------------------------------------
        var _$ProdutoLaboratorioTable = $('#LaboratorioTable');

        //Serviços
        //------------------------------------------------------------------------------------------------------------------------
        var _produtosLaboratoriosService = abp.services.app.produtoLaboratorio;
        var _produtosService = abp.services.app.produto;

        //ModalManagers
        //------------------------------------------------------------------------------------------------------------------------
        var _createOrEditProdutoLaboratorioModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Produtos/CriarOuEditarProdutoLaboratorioModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoLaboratorioModal.js',
            modalClass: 'CriarOuEditarProdutoLaboratorioModal'
        });

        _$ProdutoLaboratorioTable.jtable({
            title: app.localize('Laboratorio'),
            paging: true,
            sorting: true,
            useBootstrap: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _produtosLaboratoriosService.listarPorProduto
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
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                               
                                e.preventDefault();
                                _createOrEditProdutoEstoqueModal.open({ id: data.record.id, idProduto: $("#id").val() });

                            });

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                deleteProdutoEstoque(data.record);
                            });

                        return $span;
                    }
                },
               
                descricao: {
                    title: app.localize('Descricao'),
                    width: '47%',
                    display: function (data) {
                        if (data.record.estoque) {
                            return data.record.estoque.descricao;
                        }
                    }
                },

                EstoqueMinimo: {
                    title: app.localize('EstoqueMinimo'),
                    width: '15%',
                    display: function (data) {
                        if (data.record) {
                            return data.record.estoqueMinimo;
                        }
                    }
                },

                EstoqueMaximo: {
                    title: app.localize('EstoqueMaximo'),
                    width: '15%',
                    display: function (data) {
                        if (data.record) {
                            return data.record.estoqueMaximo;
                        }
                    }
                },


               

                PontoPedido: {
                    title: app.localize('PontoPedido'),
                    width: '15%',
                    display: function (data) {
                        if (data.record) {
                            return data.record.pontoPedido;
                        }
                    }
                }
            }
        });

        function deleteProdutoEstoque(produtoEstoque) {
            abp.message.confirm(
            app.localize('DeleteWarning', produtoEstoque.estoque.descricao),
            function (isConfirmed) {
                //console.log(isConfirmed);
                if (isConfirmed) {
                   
                    _produtosLaboratoriosService.excluir(produtoEstoque)
                        .done(function () {
                            getProdutoEstoqueTable(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                }
            }
            );
        }

        function getProdutoEstoqueTable(reload) {

            if (reload) {
                _$ProdutoLaboratorioTable.jtable('reload');
            } else {
                _$ProdutoLaboratorioTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        getProdutoEstoqueTable();

        //Botao que persistem as relações de produtos com outros cadastros
        //------------------------------------------------------------------------------------------------------------------------
        $('#btn-novo-produto-estoque').click(function (e) {
            e.preventDefault();
            _createOrEditProdutoEstoqueModal.open({ id: null, idProduto: $("#id").val() });
        });

        //------------------------------------------------------------------------------------------------------------------------
        //Atualizacoes das Grids das Abas após Salvar Inclusoes/Alteracoes
        abp.event.on('app.CriarOuEditarProdutoEstoqueModalSaved', function () {
            getProdutoEstoqueTable();
        });

    });
})();