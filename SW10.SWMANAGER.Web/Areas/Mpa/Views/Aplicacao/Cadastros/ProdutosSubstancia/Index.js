
(function () {
    $(function () {
        var _$ProdutosSubstanciaTable = $('#ProdutosSubstanciaTable');
        var _ProdutosSubstanciaService = abp.services.app.produtoSubstancia;
        var _$filterForm = $('#ProdutosSubstanciaFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Substancia.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Substancia.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Substancia.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosSubstancia/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosSubstancia/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoSubstanciaModal'
        });

        _$ProdutosSubstanciaTable.jtable({

            title: app.localize('ProdutosSubstancia'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _ProdutosSubstanciaService.listar
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
                                    deleteProdutosSubstancia(data.record);
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

        function getProdutosSubstancia(reload) {
            if (reload) {
                _$ProdutosSubstanciaTable.jtable('reload');
            } else {
                _$ProdutosSubstanciaTable.jtable('load', {
                    filtro: $('#ProdutosSubstanciaTableFilter').val()
                });
            }
        }

        function deleteProdutosSubstancia(ProdutoSubstancia) {

            abp.message.confirm(
                app.localize('DeleteWarning', ProdutoSubstancia.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosSubstanciaService.excluir(ProdutoSubstancia)
                            .done(function () {
                                getProdutosSubstancia(true);
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

        $('#CreateNewProdutoSubstanciaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosSubstanciaParaExcelButton').click(function () {
            _ProdutosSubstanciaService
                .listarParaExcel({
                    filtro: $('#ProdutosSubstanciaTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosSubstanciaButton, #RefreshProdutosSubstanciaListButton').click(function (e) {
            e.preventDefault();
            getProdutosSubstancia();
        });

        abp.event.on('app.CriarOuEditarProdutoSubstanciaModalSaved', function () {
            getProdutosSubstancia(true);
        });

        getProdutosSubstancia();

        $('#ProdutosSubstanciaTableFilter').focus();
    });
})();