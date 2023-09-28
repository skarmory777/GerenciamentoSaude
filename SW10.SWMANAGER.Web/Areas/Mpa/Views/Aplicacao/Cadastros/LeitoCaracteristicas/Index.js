(function () {
    $(function () {
        var _$LeitoCaracteristicasTable = $('#LeitoCaracteristicasTable');
        var _LeitoCaracteristicasService = abp.services.app.leitoCaracteristica;
        var _$filterForm = $('#LeitoCaracteristicasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.LeitoCaracteristica.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.LeitoCaracteristica.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.LeitoCaracteristica.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LeitoCaracteristicas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/LeitoCaracteristicas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarLeitoCaracteristicaModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/LeitoCaracteristicas/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$LeitoCaracteristicasTable.jtable({

            title: app.localize('LeitoCaracteristicas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _LeitoCaracteristicasService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '7%',
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
                                    deleteLeitoCaracteristicas(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                codigo: {
                    title: app.localize('Codigo'),
                    width: '8%'
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '20%'
                }
                ,
                ramal: {
                    title: app.localize('Ramal'),
                    width: '10%'
                }
            }
        });

        function getLeitoCaracteristicas(reload) {
            if (reload) {
                _$LeitoCaracteristicasTable.jtable('reload');
            } else {
                _$LeitoCaracteristicasTable.jtable('load', {
                    filtro: $('#LeitoCaracteristicasTableFilter').val()
                });
            }
        }

        function deleteLeitoCaracteristicas(LeitoCaracteristica) {

            abp.message.confirm(
                app.localize('DeleteWarning', LeitoCaracteristica.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _LeitoCaracteristicasService.excluir(LeitoCaracteristica)
                            .done(function () {
                                getLeitoCaracteristicas(true);
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

        $('#CreateNewLeitoCaracteristicaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarLeitoCaracteristicasParaExcelButton').click(function () {
            _LeitoCaracteristicasService
                .listarParaExcel({
                    filtro: $('#LeitoCaracteristicasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetLeitoCaracteristicasButton, #RefreshLeitoCaracteristicasListButton').click(function (e) {
            e.preventDefault();
            getLeitoCaracteristicas();
        });

        abp.event.on('app.CriarOuEditarLeitoCaracteristicaModalSaved', function () {
            getLeitoCaracteristicas(true);
        });

        getLeitoCaracteristicas();

        $('#LeitoCaracteristicasTableFilter').focus();
    });
})();