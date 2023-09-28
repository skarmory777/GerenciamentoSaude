(function () {
    $(function () {
        var _$ProdutosSubClasseTable = $('#ProdutosSubClasseTable');
        var _ProdutosSubClasseService = abp.services.app.produtoSubClasse;
        var _$filterForm = $('#ProdutosSubClasseFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.SubClasse.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.SubClasse.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.SubClasse.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosSubClasse/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosSubClasse/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoSubClasseModal'
        });

        _$ProdutosSubClasseTable.jtable({

            title: app.localize('ProdutosSubClasse'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _ProdutosSubClasseService.listar
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
                                    deleteProdutosSubClasse(data.record);
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

        function getProdutosSubClasse(reload) {
            if (reload) {
                _$ProdutosSubClasseTable.jtable('reload');
            } else {
                _$ProdutosSubClasseTable.jtable('load', {
                    filtro: $('#ProdutosSubClasseTableFilter').val()
                });
            }
        }

        function deleteProdutosSubClasse(ProdutoSubClasse) {

            abp.message.confirm(
                app.localize('DeleteWarning', ProdutoSubClasse.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosSubClasseService.excluir(ProdutoSubClasse)
                            .done(function () {
                                getProdutosSubClasse(true);
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

        $('#CreateNewProdutoSubClasseButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosSubClasseParaExcelButton').click(function () {
            _ProdutosSubClasseService
                .listarParaExcel({
                    filtro: $('#ProdutosSubClasseTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosSubClasseButton, #RefreshProdutosSubClasseListButton').click(function (e) {
            e.preventDefault();
            getProdutosSubClasse();
        });

        abp.event.on('app.CriarOuEditarProdutoSubClasseModalSaved', function () {
            getProdutosSubClasse(true);
        });

        getProdutosSubClasse();

        $('#ProdutosSubClasseTableFilter').focus();
    });
})();