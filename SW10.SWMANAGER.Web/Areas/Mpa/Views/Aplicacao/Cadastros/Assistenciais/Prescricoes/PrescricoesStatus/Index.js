(function () {
    $(function () {
        var _$PrescricoesStatusTable = $('#PrescricoesStatusTable');
        var _PrescricoesStatusService = abp.services.app.prescricaoStatus;
        var _$filterForm = $('#PrescricoesStatusFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoStatus.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoStatus.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoStatus.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PrescricoesStatus/CriarOuEditar',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesStatus/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPrescricaoStatusModal'
        });

        _$PrescricoesStatusTable.jtable({

            title: app.localize('PrescricaoStatus'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _PrescricoesStatusService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
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
                                    deletePrescricoesStatus(data.record);
                                });
                        }

                        return $span;
                    }
                },
                cor: {
                    title: app.localize('Cor'),
                    width: '10%',
                    display: function (data) {
                        var $span = $('<div></div>');
                        $('<span class="sw-btn-display" style="background-color:' + data.record.cor + ';"></span>')
                            .appendTo($span);
                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '20%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '50%'
                },
            }
        });

        function getPrescricoesStatus(reload) {
            if (reload) {
                _$PrescricoesStatusTable.jtable('reload');
            } else {
                _$PrescricoesStatusTable.jtable('load', {
                    filtro: $('#PrescricoesStatusTableFilter').val()
                });
            }
        }

        function deletePrescricoesStatus(prescricaoStatus) {

            abp.message.confirm(
                app.localize('DeleteWarning', prescricaoStatus.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _PrescricoesStatusService.excluir(prescricaoStatus)
                            .done(function () {
                                getPrescricoesStatus(true);
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

        $('#CreateNewPrescricaoStatusButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarPrescricoesStatusParaExcelButton').click(function () {
            _PrescricoesStatusService
                .listarParaExcel({
                    filtro: $('#PrescricoesStatusTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetPrescricoesStatusButton, #RefreshPrescricoesStatusListButton').click(function (e) {
            e.preventDefault();
            getPrescricoesStatus();
        });

        abp.event.on('app.CriarOuEditarPrescricaoStatusModalSaved', function () {
            getPrescricoesStatus(true);
        });

        getPrescricoesStatus();

        $('#PrescricoesStatusTableFilter').focus();
    });
})();