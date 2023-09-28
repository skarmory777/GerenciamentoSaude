(function () {
    $(function () {
        var _$MotivosTransferenciaLeitoTable = $('#MotivosTransferenciaLeitoTable');
        var _MotivosTransferenciaLeitoService = abp.services.app.MotivoTransferenciaLeito;
        var _$filterForm = $('#MotivosTransferenciaLeitoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.MotivoTransferenciaLeito.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.MotivoTransferenciaLeito.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.MotivoTransferenciaLeito.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/MotivosTransferenciaLeito/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/MotivosTransferenciaLeito/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMotivoTransferenciaLeitoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/MotivosTransferenciaLeito/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$MotivosTransferenciaLeitoTable.jtable({

            title: app.localize('MotivoTransferenciaLeito'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _MotivosTransferenciaLeitoService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '2%',
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
                                    deleteMotivosTransferenciaLeito(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codMotivoTransferenciaLeito: {
                    title: app.localize('CodMotivoTransferenciaLeito'),
                    width: '4%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '8%'
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

        function getMotivosTransferenciaLeito(reload) {
            if (reload) {
                _$MotivosTransferenciaLeitoTable.jtable('reload');
            } else {
                _$MotivosTransferenciaLeitoTable.jtable('load', {
                    filtro: $('#MotivosTransferenciaLeitoTableFilter').val()
                });
            }
        }

        function deleteMotivosTransferenciaLeito(MotivoTransferenciaLeito) {

            abp.message.confirm(
                app.localize('DeleteWarning', MotivoTransferenciaLeito.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _MotivosTransferenciaLeitoService.excluir(MotivoTransferenciaLeito)
                            .done(function () {
                                getMotivosTransferenciaLeito(true);
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

        $('#CreateNewMotivoTransferenciaLeitoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarMotivosTransferenciaLeitoParaExcelButton').click(function () {
            _MotivosTransferenciaLeitoService
                .listarParaExcel({
                    filtro: $('#MotivosTransferenciaLeitoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMotivosTransferenciaLeitoButton, #RefreshMotivosTransferenciaLeitoListButton').click(function (e) {
            e.preventDefault();
            getMotivosTransferenciaLeito();
        });

        abp.event.on('app.CriarOuEditarMotivoTransferenciaLeitoModalSaved', function () {
            getMotivosTransferenciaLeito(true);
        });

        getMotivosTransferenciaLeito();

        $('#MotivosTransferenciaLeitoTableFilter').focus();
    });
})();