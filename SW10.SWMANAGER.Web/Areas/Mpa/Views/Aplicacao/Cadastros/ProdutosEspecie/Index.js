
(function () {
    $(function () {
        var _$ProdutosEspecieTable = $('#ProdutosEspecieTable');
        var _ProdutosEspecieService = abp.services.app.produtoEspecie;
        var _$filterForm = $('#ProdutosEspecieFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Especie.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Especie.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.Especie.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosEspecie/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosEspecie/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoEspecieModal'
        });

        _$ProdutosEspecieTable.jtable({

            title: app.localize('ProdutosEspecie'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _ProdutosEspecieService.listar
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
                                    deleteProdutosEspecie(data.record);
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

        function getProdutosEspecie(reload) {
            if (reload) {
                _$ProdutosEspecieTable.jtable('reload');
            } else {
                _$ProdutosEspecieTable.jtable('load', {
                    filtro: $('#ProdutosEspecieTableFilter').val()
                });
            }
        }

        function deleteProdutosEspecie(produtoEspecie) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoEspecie.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosEspecieService.excluir(produtoEspecie)
                            .done(function () {
                                getProdutosEspecie(true);
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

        $('#CreateNewProdutoEspecieButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosEspecieParaExcelButton').click(function () {
            _ProdutosEspecieService
                .listarParaExcel({
                    filtro: $('#ProdutosEspecieTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosEspecieButton, #RefreshProdutosEspecieListButton').click(function (e) {
            e.preventDefault();
            getProdutosEspecie();
        });

        abp.event.on('app.CriarOuEditarProdutoEspecieModalSaved', function () {
            getProdutosEspecie(true);
        });

        getProdutosEspecie();

        $('#ProdutosEspecieTableFilter').focus();
    });
})();