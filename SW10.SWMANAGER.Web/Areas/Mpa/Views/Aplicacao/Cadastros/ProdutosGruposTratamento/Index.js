(function () {
    $(function () {
        var _$ProdutosGruposTratamentoTable = $('#ProdutosGruposTratamentoTable');
        var _ProdutosGruposTratamentoService = abp.services.app.produtoGrupoTratamento;
        var _$filterForm = $('#ProdutosGruposTratamentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.GrupoTratamento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.GrupoTratamento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosSuprimentos.GrupoTratamento.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ProdutosGruposTratamento/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosGruposTratamento/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProdutoGrupoTratamentoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosGruposTratamento/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$ProdutosGruposTratamentoTable.jtable({

            title: app.localize('ProdutosGruposTratamento'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ProdutosGruposTratamentoService.listar
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
                                    deleteProdutosGruposTratamento(data.record);
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

        function getProdutosGruposTratamento(reload) {
            if (reload) {
                _$ProdutosGruposTratamentoTable.jtable('reload');
            } else {
                _$ProdutosGruposTratamentoTable.jtable('load', {
                    filtro: $('#ProdutosGruposTratamentoTableFilter').val()
                });
            }
        }

        function deleteProdutosGruposTratamento(produtoGrupoTratamento) {

            abp.message.confirm(
                app.localize('DeleteWarning', produtoGrupoTratamento.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProdutosGruposTratamentoService.excluir(produtoGrupoTratamento)
                            .done(function () {
                                getProdutosGruposTratamento(true);
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

        $('#CreateNewProdutoGrupoTratamentoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProdutosGruposTratamentoParaExcelButton').click(function () {
            _ProdutosGruposTratamentoService
                .listarParaExcel({
                    filtro: $('#ProdutosGruposTratamentoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProdutosGruposTratamentoButton, #RefreshProdutosGruposTratamentoListButton').click(function (e) {
            e.preventDefault();
            getProdutosGruposTratamento();
        });

        abp.event.on('app.CriarOuEditarProdutoGrupoTratamentoModalSaved', function () {
            getProdutosGruposTratamento(true);
        });

        getProdutosGruposTratamento();

        $('#ProdutosGruposTratamentoTableFilter').focus();
    });
})();