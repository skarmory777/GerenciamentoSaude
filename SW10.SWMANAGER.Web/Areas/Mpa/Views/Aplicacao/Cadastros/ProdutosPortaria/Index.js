(function () {
    $(function () {
        var _$ProdutosPortariaTable = $('#ProdutosPortariaTable');
        var _ProdutosPortariaService = abp.services.app.produtoPortaria;
        var _$filterForm = $('#ProdutosPortariaFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Portaria.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Portaria.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Portaria.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosPortaria/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosPortaria/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoPortariaModal'
        });

        _$ProdutosPortariaTable.jtable({

            title: app.localize('ProdutosPortaria'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _ProdutosPortariaService.listar
                }
            },
            fields: {
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
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
                                    deleteProdutosPortaria(data.record);
                                });
                        }
                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '8%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    
                    width: '74%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getProdutosPortaria(reload) {
            if (reload) {
                _$ProdutosPortariaTable.jtable('reload');
            } else {
                _$ProdutosPortariaTable.jtable('load', {
                    filtro: $('#ProdutosPortariaTableFilter').val()
                });
            }
        }

        function deleteProdutosPortaria(produtoPortaria) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoPortaria.codigo),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosPortariaService.excluir(produtoPortaria)
                            .done(function () {
                                getProdutosPortaria(true);
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

        $('#CreateNewProdutoPortariaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosPortariaParaExcelButton').click(function () {
            _ProdutosPortariaService
                .listarParaExcel({
                    filtro: $('#ProdutosPortariaTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosPortariaButton, #RefreshProdutosPortariaListButton').click(function (e) {
            e.preventDefault();
            getProdutosPortaria();
        });

        abp.event.on('app.CriarOuEditarProdutoPortariaModalSaved', function () {
            getProdutosPortaria(true);
        });

        getProdutosPortaria();

        $('#ProdutosPortariaTableFilter').focus();
    });
})();