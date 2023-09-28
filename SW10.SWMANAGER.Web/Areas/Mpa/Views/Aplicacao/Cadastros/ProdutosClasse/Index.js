(function () {
    $(function () {
        var _$ProdutosClasseTable = $('#ProdutosClasseTable');
        var _ProdutosClasseService = abp.services.app.produtoClasse;
        var _$filterForm = $('#ProdutosClasseFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Classe.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Classe.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Classe.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosClasse/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosClasse/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoClasseModal'
        });

        _$ProdutosClasseTable.jtable({

            title: app.localize('ProdutosClasse'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _ProdutosClasseService.listar
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
                                    deleteProdutosClasse(data.record);
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

        function getProdutosClasse(reload) {
            if (reload) {
                _$ProdutosClasseTable.jtable('reload');
            } else {
                _$ProdutosClasseTable.jtable('load', {
                    filtro: $('#ProdutosClasseTableFilter').val()
                });
            }
        }

        function deleteProdutosClasse(produtoClasse) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoClasse.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosClasseService.excluir(produtoClasse)
                            .done(function () {
                                getProdutosClasse(true);
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

        $('#CreateNewProdutoClasseButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosClasseParaExcelButton').click(function () {
            _ProdutosClasseService
                .listarParaExcel({
                    filtro: $('#ProdutosClasseTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosClasseButton, #RefreshProdutosClasseListButton').click(function (e) {
            e.preventDefault();
            getProdutosClasse();
        });

        abp.event.on('app.CriarOuEditarProdutoClasseModalSaved', function () {
            getProdutosClasse(true);
        });

        getProdutosClasse();

        $('#ProdutosClasseTableFilter').focus();
    });
})();