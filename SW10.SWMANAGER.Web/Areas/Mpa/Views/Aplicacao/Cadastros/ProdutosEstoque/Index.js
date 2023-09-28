(function () {
    $(function () {
        var _$ProdutosEstoqueTable = $('#ProdutosEstoqueTable');
        var _ProdutosEstoqueService = abp.services.app.produtoEstoque;
        var _$filterForm = $('#ProdutosEstoqueFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosEstoque/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosEstoque/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoEstoqueModal'
        });

        _$ProdutosEstoqueTable.jtable({

            title: app.localize('ProdutosEstoque'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ProdutosEstoqueService.listar
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteProdutosEstoque(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '2%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getProdutosEstoque(reload) {
            if (reload) {
                _$ProdutosEstoqueTable.jtable('reload');
            } else {
                _$ProdutosEstoqueTable.jtable('load', {
                    filtro: $('#ProdutosEstoqueTableFilter').val()
                });
            }
        }

        function deleteProdutosEstoque(produtoEstoque) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoEstoque.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosEstoqueService.excluir(produtoEstoque)
                            .done(function () {
                                getProdutosEstoque(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewProdutoEstoqueButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosEstoqueParaExcelButton').click(function () {
            _ProdutosEstoqueService
                .listarParaExcel({
                    filtro: $('#ProdutosEstoqueTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosEstoqueButton, #RefreshProdutosEstoqueListButton').click(function (e) {
            e.preventDefault();
            getProdutosEstoque();
        });

        abp.event.on('app.CriarOuEditarProdutoEstoqueModalSaved', function () {
            getProdutosEstoque(true);
        });

        //getProdutosEstoque();

        $('#ProdutosEstoqueTableFilter').focus();
    });
})();