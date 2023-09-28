(function () {
    $(function () {
        var _$PrescricoesItensStatusTable = $('#PrescricoesItensStatusTable');
        var _PrescricoesItensStatusService = abp.services.app.prescricaoItemStatus;
        var _$filterForm = $('#PrescricoesItensStatusFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItemStatus.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItemStatus.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.PrescricaoItemStatus.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PrescricoesItensStatus/CriarOuEditar',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItensStatus/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPrescricaoItemStatusModal'
        });

        _$PrescricoesItensStatusTable.jtable({
            //title: app.localize('PrescricaoItemStatus'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _PrescricoesItensStatusService.listar
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
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
                                    deletePrescricoesItensStatus(data.record);
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

        function getPrescricoesItensStatus(reload) {
            if (reload) {
                _$PrescricoesItensStatusTable.jtable('reload');
            } else {
                _$PrescricoesItensStatusTable.jtable('load', {
                    filtro: $('#PrescricoesItensStatusTableFilter').val()
                });
            }
        }

        function deletePrescricoesItensStatus(prescricaoItemStatus) {

            abp.message.confirm(
                app.localize('DeleteWarning', prescricaoItemStatus.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _PrescricoesItensStatusService.excluir(prescricaoItemStatus)
                            .done(function () {
                                getPrescricoesItensStatus(true);
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

        $('#CreateNewPrescricaoItemStatusButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarPrescricoesItensStatusParaExcelButton').click(function () {
            _PrescricoesItensStatusService
                .listarParaExcel({
                    filtro: $('#PrescricoesItensStatusTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetPrescricoesItensStatusButton, #RefreshPrescricoesItensStatusListButton').click(function (e) {
            e.preventDefault();
            getPrescricoesItensStatus();
        });

        abp.event.on('app.CriarOuEditarPrescricaoItemStatusModalSaved', function () {
            getPrescricoesItensStatus(true);
        });

        getPrescricoesItensStatus();

        $('#PrescricoesItensStatusTableFilter').focus();
    });
})();