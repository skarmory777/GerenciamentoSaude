(function () {
    $(function () {
        var _$TabelaResultadosTable = $('#TabelaResultadosTable');
        var _TabelaResultadosService = abp.services.app.tabelaResultado;
        var _$filterForm = $('#TabelaResultadosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.TabelaResultado.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.TabelaResultado.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.TabelaResultado.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TabelasResultados/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/TabelaResultados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTabelaResultadoModal'
        });

        _$TabelaResultadosTable.jtable({

            title: app.localize('TabelaResultados'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TabelaResultadosService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _createOrEditModal.open({ id: data.record.id, tabelaId: $('#tabela-id').val() });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteTabelaResultados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Código'),
                    width: '15%'
                },
                descricao: {
                    title: app.localize('Nome'),
                    width: '15%'
                },
            }
        });

        function getTabelaResultados(reload) {
            if (reload) {
                _$TabelaResultadosTable.jtable('reload');
            } else {
                _$TabelaResultadosTable.jtable('load', {
                    filtro: $('#TabelaResultadosTableFilter').val(),
                    principalId: $('#tabela-id').val()
                });
            }
        }

        function deleteTabelaResultados(TabelaResultado) {

            abp.message.confirm(
                app.localize('DeleteWarning', TabelaResultado.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TabelaResultadosService.excluir(TabelaResultado)
                            .done(function () {
                                getTabelaResultados(true);
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

        $('#CreateNewTabelaResultadoButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open({ tabelaId: $('#tabela-id').val() });
        });

        $('#ExportarTabelaResultadosParaExcelButton').click(function () {
            _TabelaResultadosService
                .listarParaExcel({
                    filtro: $('#TabelaResultadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTabelaResultadosButton, #RefreshTabelaResultadosListButton').click(function (e) {
            e.preventDefault();
            getTabelaResultados();
        });

        abp.event.on('app.CriarOuEditarTabelaResultadoModalSaved', function () {
            getTabelaResultados(true);
        });

        getTabelaResultados();

        $('#TabelaResultadosTableFilter').focus();


    });
})();