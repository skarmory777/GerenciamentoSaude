(function () {
    $(function () {
        var _$ProdutosTiposUnidadeTable = $('#ProdutosTiposUnidadeTable');
        var _ProdutosTiposUnidadeService = abp.services.app.produtoTipoUnidade;
        var _$filterForm = $('#ProdutosTiposUnidadeFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.TipoUnidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.TipoUnidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.TipoUnidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosTiposUnidade/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosTiposUnidade/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoTipoUnidadeModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosTiposUnidade/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$ProdutosTiposUnidadeTable.jtable({

            title: app.localize('ProdutosTiposUnidade'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                  method: _ProdutosTiposUnidadeService.listar
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
                                    deleteProdutosTiposUnidade(data.record);
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

        function getProdutosTiposUnidade(reload) {
            if (reload) {
                _$ProdutosTiposUnidadeTable.jtable('reload');
            } else {
                _$ProdutosTiposUnidadeTable.jtable('load', {
                    filtro: $('#ProdutosTiposUnidadeTableFilter').val()
                });
            }
        }

        function deleteProdutosTiposUnidade(produtoTipoUnidade) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoTipoUnidade.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosTiposUnidadeService.excluir(produtoTipoUnidade)
                            .done(function () {
                                getProdutosTiposUnidade(true);
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

        $('#CreateNewProdutoTipoUnidadeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosTiposUnidadeParaExcelButton').click(function () {
            _ProdutosTiposUnidadeService
                .listarParaExcel({
                    filtro: $('#ProdutosTiposUnidadeTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosTiposUnidadeButton, #RefreshProdutosTiposUnidadeListButton').click(function (e) {
            e.preventDefault();
            getProdutosTiposUnidade();
        });

        abp.event.on('app.CriarOuEditarProdutoTipoUnidadeModalSaved', function () {
            getProdutosTiposUnidade(true);
        });

        getProdutosTiposUnidade();

        $('#ProdutosTiposUnidadeTableFilter').focus();
    });
})();