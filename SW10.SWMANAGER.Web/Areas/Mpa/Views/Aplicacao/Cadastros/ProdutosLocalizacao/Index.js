(function () {
    $(function () {
        var _$ProdutosLocalizacaoTable = $('#ProdutosLocalizacaoTable');
        var _ProdutosLocalizacaoService = abp.services.app.produtoLocalizacao;
        var _$filterForm = $('#ProdutosLocalizacaoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.LocalizacaoProduto.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.LocalizacaoProduto.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.LocalizacaoProduto.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosLocalizacao/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosLocalizacao/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoLocalizacaoModal'
        });

        _$ProdutosLocalizacaoTable.jtable({

            title: app.localize('ProdutosLocalizacao'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _ProdutosLocalizacaoService.listar
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
                                    deleteProdutosLocalizacao(data.record);
                                });
                        }
                        return $span;
                    }
                },
                id: {
                    title: app.localize('Codigo'),
                    width: '8%'
                },
                sigla: {
                    title: app.localize('Sigla'),
                    width: '8%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '66%'
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

        function getProdutosLocalizacao(reload) {
            if (reload) {
                _$ProdutosLocalizacaoTable.jtable('reload');
            } else {
                _$ProdutosLocalizacaoTable.jtable('load', {
                    filtro: $('#ProdutosLocalizacaoTableFilter').val()
                });
            }
        }

        function deleteProdutosLocalizacao(produtoLocalizacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoLocalizacao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosLocalizacaoService.excluir(produtoLocalizacao)
                            .done(function () {
                                getProdutosLocalizacao(true);
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

        $('#CreateNewProdutoLocalizacaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosLocalizacaoParaExcelButton').click(function () {
            _ProdutosLocalizacaoService
                .listarParaExcel({
                    filtro: $('#ProdutosLocalizacaoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosLocalizacaoButton, #RefreshProdutosLocalizacaoListButton').click(function (e) {
            e.preventDefault();
            getProdutosLocalizacao();
        });

        abp.event.on('app.CriarOuEditarProdutoLocalizacaoModalSaved', function () {
            getProdutosLocalizacao(true);
        });

        getProdutosLocalizacao();

        $('#ProdutosLocalizacaoTableFilter').focus();
    });
})();