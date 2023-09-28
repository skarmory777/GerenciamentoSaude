(function () {
    $(function () {
        var _$ProdutosCodigosMedicamentoTable = $('#ProdutosCodigosMedicamentoTable');
        var _produtosCodigoMedicamentoService = abp.services.app.produtoCodigoMedicamento;
        var _$filterForm = $('#ProdutosCodigosMedicamentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.ProdutosCodigoMedicamento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.ProdutosCodigoMedicamento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.ProdutosCodigoMedicamento.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosCodigosMedicamento/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosCodigosMedicamento/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoCodigoMedicamentoModal'
        });

        _$ProdutosCodigosMedicamentoTable.jtable({

            title: app.localize('ProdutosCodigosMedicamento'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _produtosCodigoMedicamentoService.listar
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
                                    deleteProdutosCodigoMedicamento(data.record);
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

        function getProdutosCodigoMedicamento(reload) {
            if (reload) {
                _$ProdutosCodigosMedicamentoTable.jtable('reload');
            } else {
                _$ProdutosCodigosMedicamentoTable.jtable('load', {
                    filtro: $('#ProdutosCodigosMedicamentoTableFilter').val()
                });
            }
        }

        function deleteProdutosCodigoMedicamento(produtoCodigoMedicamento) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoCodigoMedicamento.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _produtosCodigoMedicamentoService.excluir(produtoCodigoMedicamento)
                            .done(function () {
                                getProdutosCodigoMedicamento(true);
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

        $('#CreateNewProdutoCodigoMedicamentoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosCodigoMedicamentoParaExcelButton').click(function () {
            _produtosCodigoMedicamentoService
                .listarParaExcel({
                    filtro: $('#ProdutosCodigoMedicamentoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosCodigoMedicamentoButton, #RefreshProdutosCodigoMedicamentoListButton').click(function (e) {
            e.preventDefault();
            getProdutosCodigoMedicamento();
        });

        abp.event.on('app.CriarOuEditarProdutoCodigoMedicamentoModalSaved', function () {
            getProdutosCodigoMedicamento(true);
        });

        getProdutosCodigoMedicamento();

        $('#ProdutosCodigoMedicamentoTableFilter').focus();
    });
})();