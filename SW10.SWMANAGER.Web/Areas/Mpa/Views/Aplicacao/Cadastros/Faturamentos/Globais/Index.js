(function () {
    $(function () {
        var _$TabelasTable = $('#FaturamentoTabelasTable');
        var _TabelasService = abp.services.app.faturamentoGlobal;
        var _$filterForm = $('#FaturamentoTabelasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Tabelas.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Tabelas.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Tabelas.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoGlobais/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Globais/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFaturamentoTabelaModal'
        });

        _$TabelasTable.jtable({

            title: app.localize('Tabelas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TabelasService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '30%',
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
                                    deleteTabelas(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '70%'
                }
            }
        });

        function getTabelas(reload) {
            if (reload) {
                _$TabelasTable.jtable('reload');
            } else {
                _$TabelasTable.jtable('load', {
                    filtro: $('#FaturamentoTabelasTableFilter').val()
                });
            }
        }

        function deleteTabelas(tabela) {

            abp.message.confirm(
                app.localize('DeleteWarning', tabela.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TabelasService.excluir(tabela)
                            .done(function () {
                                getTabelas(true);
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

        $('#CreateNewFaturamentoTabelaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarFaturamentoTabelasParaExcelButton').click(function () {
            _TabelasService
                .listarParaExcel({
                    filtro: $('#FaturamentoTabelasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFaturamentoTabelasButton, #RefreshFaturamentoTabelasListButton').click(function (e) {
            e.preventDefault();
            getTabelas();
        });

        abp.event.on('app.CriarOuEditarFaturamentoTabelaModalSaved', function () {
            getTabelas(true);
        });

        getTabelas();

        $('#FaturamentoTabelasTableFilter').focus();
    });
})();