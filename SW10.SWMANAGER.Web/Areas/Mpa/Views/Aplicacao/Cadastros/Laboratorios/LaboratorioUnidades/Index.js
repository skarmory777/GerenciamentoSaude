(function () {
    $(function () {
        var _$LaboratorioUnidadesTable = $('#LaboratorioUnidadesTable');
        var _LaboratorioUnidadesService = abp.services.app.laboratorioUnidade;
        var _$filterForm = $('#LaboratorioUnidadesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.LaboratorioUnidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.LaboratorioUnidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.LaboratorioUnidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LaboratoriosUnidades/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/LaboratorioUnidades/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarLaboratorioUnidadeModal'
        });

        _$LaboratorioUnidadesTable.jtable({

            title: app.localize('LaboratorioUnidades'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _LaboratorioUnidadesService.listar
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
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteLaboratorioUnidades(data.record);
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

        function getLaboratorioUnidades(reload) {
            if (reload) {
                _$LaboratorioUnidadesTable.jtable('reload');
            } else {
                _$LaboratorioUnidadesTable.jtable('load', {
                    filtro: $('#LaboratorioUnidadesTableFilter').val()
                });
            }
        }

        function deleteLaboratorioUnidades(LaboratorioUnidade) {

            abp.message.confirm(
                app.localize('DeleteWarning', LaboratorioUnidade.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _LaboratorioUnidadesService.excluir(LaboratorioUnidade)
                            .done(function () {
                                getLaboratorioUnidades(true);
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

        $('#CreateNewLaboratorioUnidadesButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarLaboratorioUnidadesParaExcelButton').click(function () {
            _LaboratorioUnidadesService
                .listarParaExcel({
                    filtro: $('#LaboratorioUnidadesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetLaboratorioUnidadesButton, #RefreshLaboratorioUnidadesListButton').click(function (e) {
            e.preventDefault();
            getLaboratorioUnidades();
        });

        abp.event.on('app.CriarOuEditarLaboratorioUnidadeModalSaved', function () {
            getLaboratorioUnidades(true);
        });

        getLaboratorioUnidades();

        $('#LaboratorioUnidadesTableFilter').focus();


    });
})();