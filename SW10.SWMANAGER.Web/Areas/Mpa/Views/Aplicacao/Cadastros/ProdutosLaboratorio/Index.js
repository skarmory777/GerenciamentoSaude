(function () {
    $(function () {
        var _$ProdutosLaboratorioTable = $('#ProdutosLaboratorioTable');
        var _ProdutosLaboratorioService = abp.services.app.produtoLaboratorio;
        var _$filterForm = $('#ProdutosLaboratorioFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Laboratorio.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Laboratorio.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Laboratorio.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosLaboratorio/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosLaboratorio/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoLaboratorioModal'
        });

        _$ProdutosLaboratorioTable.jtable({

            title: app.localize('ProdutosLaboratorio'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _ProdutosLaboratorioService.listar
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
                                    deleteProdutosLaboratorio(data.record);
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

        function getProdutosLaboratorio(reload) {
            if (reload) {
                _$ProdutosLaboratorioTable.jtable('reload');
            } else {
                _$ProdutosLaboratorioTable.jtable('load', {
                    filtro: $('#ProdutosLaboratorioTableFilter').val()
                });
            }
        }

        function deleteProdutosLaboratorio(produtoLaboratorio) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoLaboratorio.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosLaboratorioService.excluir(produtoLaboratorio)
                            .done(function () {
                                getProdutosLaboratorio(true);
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

        $('#CreateNewProdutoLaboratorioButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosLaboratorioParaExcelButton').click(function () {
            _ProdutosLaboratorioService
                .listarParaExcel({
                    filtro: $('#ProdutosLaboratorioTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosLaboratorioButton, #RefreshProdutosLaboratorioListButton').click(function (e) {
            e.preventDefault();
            getProdutosLaboratorio();
        });

        abp.event.on('app.CriarOuEditarProdutoLaboratorioModalSaved', function () {
            getProdutosLaboratorio(true);
        });

        getProdutosLaboratorio();

        $('#ProdutosLaboratorioTableFilter').focus();
    });
})();