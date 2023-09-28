(function () {
    $(function () {

        //DIVs Abas
        //------------------------------------------------------------------------------------------------------------------------
        var _$ProdutoEmpresaTable = $('#ProdutoEmpresaTable');

        //Serviços
        //------------------------------------------------------------------------------------------------------------------------
        var _produtosEmpresaService = abp.services.app.produtoEmpresa;
        var _produtosService = abp.services.app.produto;

        //ModalManagers
        //------------------------------------------------------------------------------------------------------------------------
        var _createOrEditProdutoEmpresaModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Produtos/CriarOuEditarProdutoEmpresaModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoEmpresaModal.js',
            modalClass: 'CriarOuEditarProdutoEmpresaModal'
        });

        //Grid
        //------------------------------------------------------------------------------------------------------------------------
        _$ProdutoEmpresaTable.jtable({
            title: app.localize('Empresa'),
            paging: true,
            sorting: true,
            useBootstrap: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _produtosService.listarProdutosEmpresasPorProduto
                }
            },
            fields:
            {
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
                        //  if (_permissions.edit) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                //_createOrEditModal.open({ id: data.record.id });
                                //_createOrEditProdutoEmpresaModal.open({ id: data.record.id, idproduto: $("#id").val() });

                                _createOrEditProdutoEmpresaModal.open({ id: data.record.id, EmpresaReferencialId: $('#Empresareferencial-search').val(), EmpresagerencialId: $('#Empresagerencial-search').val(), idProduto: $("#id").val() });
                            });
                        // }

                        // if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                //console.log(JSON.stringify(data.record));
                                deleteProdutoEmpresa(data.record);
                            });
                        // }

                        return $span;
                    }
                },
                //empresaId: {
                //    title: app.localize('Codigo'),
                //    width: '7%'
                //},
                descricao: {
                    title: app.localize('Descricao'),
                    width: '92%',
                    display: function (data) {
                        if (data.record.empresa) {
                            return data.record.empresa.nomeFantasia;
                        }
                    }
                }
            }
        });

        function deleteProdutoEmpresa(ProdutoEmpresa) {
            //console.log(JSON.stringify(ProdutoEmpresa));

            abp.message.confirm(
            app.localize('DeleteWarning', ProdutoEmpresa.empresa.nomeFantasia),
            function (isConfirmed) {
                if (isConfirmed) {
                    _produtosEmpresaService.excluir(ProdutoEmpresa)
                        .done(function () {
                            getProdutoEmpresaTable(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                }
            });
        }

        function getProdutoEmpresaTable(reload) {
            if (reload) {
                _$ProdutoEmpresaTable.jtable('reload');
            } else {
                _$ProdutoEmpresaTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        //getProdutoEmpresaTable();

        //Botao que persiste as relações de produtos com outros cadastros
        //------------------------------------------------------------------------------------------------------------------------
        var _servico = abp.services.app.user;

        $('#btn-novo-produto-empresa').click(function (e) {
            e.preventDefault();

            //var emp = _servico.getUserEmpresas(3)
            //    .done(function (data) {
            //        //console.log(JSON.stringify(data));
            //    });

            _createOrEditProdutoEmpresaModal.open({ id: null, idProduto: $("#id").val() });

        });

        //------------------------------------------------------------------------------------------------------------------------
        //Atualizacoes das Grids das Abas após Salvar Inclusoes/Alteracoes
        abp.event.on('app.CriarOuEditarProdutoEmpresaModalSaved', function () {
            //getProdutos(true);
            getProdutoEmpresaTable(true);
        });
        $('#href_empresa').on('click', function (e) {
            e.preventDefault();
            getProdutoEmpresaTable();
        })
    });
})();