(function () {
    $(function () {
        var _$EstoquesTable = $('#EstoquesTable');
        var _EstoquesService = abp.services.app.estoque;
        var _$filterForm = $('#EstoquesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Estoque.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            //viewUrl: abp.appPath + 'Mpa/Estoques/CriarOuEditarModal',
            viewUrl: abp.appPath + 'Mpa/ProdutosEstoque/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Estoques/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarEstoqueModal'
        });

        _$EstoquesTable.jtable({

            title: app.localize('Estoques'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _EstoquesService.listar
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
                                    deleteEstoques(data.record);
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

        function getEstoques(reload) {
            if (reload) {
                _$EstoquesTable.jtable('reload');
            } else {
                _$EstoquesTable.jtable('load', {filtro: $('#EstoquesTableFilter').val()});
            }
        }

        function deleteEstoques(estoque) {

            abp.message.confirm(
                app.localize('DeleteWarning', estoque.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _EstoquesService.excluir(estoque)
                            .done(function () {
                                getEstoques(true);
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

        $('#CreateNewEstoqueButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarEstoquesParaExcelButton').click(function () {
            _EstoquesService
                .listarParaExcel({
                    filtro: $('#EstoquesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetEstoquesButton, #RefreshEstoquesListButton').click(function (e) {
            e.preventDefault();
            getEstoques();
        });

        abp.event.on('app.CriarOuEditarEstoqueModalSaved', function () {
            getEstoques(true);
        });

        getEstoques();

        $('#EstoquesTableFilter').focus();
    });
})();