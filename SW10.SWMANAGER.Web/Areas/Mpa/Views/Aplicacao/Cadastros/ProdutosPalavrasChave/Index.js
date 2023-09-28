(function () {
    $(function () {
        var _$ProdutosPalavrasChaveTable = $('#ProdutosPalavrasChaveTable');
        var _ProdutosPalavrasChaveService = abp.services.app.produtoPalavraChave;
        var _$filterForm = $('#ProdutosPalavrasChaveFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.PalavraChave.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosPalavrasChave/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosPalavrasChave/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoPalavraChaveModal'
        });

        _$ProdutosPalavrasChaveTable.jtable({

            title: app.localize('ProdutosPalavrasChave'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _ProdutosPalavrasChaveService.listar
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
                                    deleteProdutosPalavrasChave(data.record);
                                });
                        }
                        return $span;
                    }
                },
                id: {
                    title: app.localize('Codigo'),
                    width: '8%'
                },
                palavra: {
                    title: app.localize('Palavra'),
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

        function getProdutosPalavrasChave(reload) {
            if (reload) {
                _$ProdutosPalavrasChaveTable.jtable('reload');
            } else {
                _$ProdutosPalavrasChaveTable.jtable('load', {
                    filtro: $('#ProdutosPalavrasChaveTableFilter').val()
                });
            }
        }

        function deleteProdutosPalavrasChave(produtoPalavraChave) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoPalavraChave.palavra),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosPalavrasChaveService.excluir(produtoPalavraChave)
                            .done(function () {
                                getProdutosPalavrasChave(true);
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

        $('#CreateNewProdutoPalavraChaveButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosPalavrasChaveParaExcelButton').click(function () {
            _ProdutosPalavrasChaveService
                .listarParaExcel({
                    filtro: $('#ProdutosPalavrasChaveTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosPalavrasChaveButton, #RefreshProdutosPalavrasChaveListButton').click(function (e) {
            e.preventDefault();
            getProdutosPalavrasChave();
        });

        abp.event.on('app.CriarOuEditarProdutoPalavraChaveModalSaved', function () {
            getProdutosPalavrasChave(true);
        });

        getProdutosPalavrasChave();

        $('#ProdutosPalavrasChaveTableFilter').focus();
    });
})();