(function () {
    $(function () {
        var _$ProdutosAcoesTerapeuticaTable = $('#ProdutosAcoesTerapeuticaTable');
        var _ProdutosAcoesTerapeuticaService = abp.services.app.produtoAcaoTerapeutica;
        var _$filterForm = $('#ProdutosAcoesTerapeuticaFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.AcaoTerapeutica.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.AcaoTerapeutica.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.AcaoTerapeutica.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosAcoesTerapeutica/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosAcoesTerapeutica/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoAcaoTerapeuticaModal'
        });

        _$ProdutosAcoesTerapeuticaTable.jtable({

            title: app.localize('ProdutosAcoesTerapeutica'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ProdutosAcoesTerapeuticaService.listar
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
                                    deleteProdutosAcoesTerapeutica(data.record);
                                });
                        }

                        return $span;
                    }
                },
                id: {
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

        function getProdutosAcoesTerapeutica(reload) {
            if (reload) {
                _$ProdutosAcoesTerapeuticaTable.jtable('reload');
            } else {
                _$ProdutosAcoesTerapeuticaTable.jtable('load', {
                    filtro: $('#ProdutosAcoesTerapeuticaTableFilter').val()
                });
            }
        }

        function deleteProdutosAcoesTerapeutica(produtoAcaoTerapeutica) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoAcaoTerapeutica.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosAcoesTerapeuticaService.excluir(produtoAcaoTerapeutica)
                            .done(function () {
                                getProdutosAcoesTerapeutica(true);
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

        $('#CreateNewProdutoAcaoTerapeuticaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosAcoesTerapeuticaParaExcelButton').click(function () {
            _ProdutosAcoesTerapeuticaService
                .listarParaExcel({
                    filtro: $('#ProdutosAcoesTerapeuticaTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosAcoesTerapeuticaButton, #RefreshProdutosAcoesTerapeuticaListButton').click(function (e) {
            e.preventDefault();
            getProdutosAcoesTerapeutica();
        });

        abp.event.on('app.CriarOuEditarProdutoAcaoTerapeuticaModalSaved', function () {
            getProdutosAcoesTerapeutica(true);
        });

        getProdutosAcoesTerapeutica();

        $('#ProdutosAcoesTerapeuticaTableFilter').focus();
    });
})();