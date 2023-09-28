(function () {
    $(function () {
        var _$MotivosAltaTable = $('#MotivosAltaTable');
        var _MotivosAltaService = abp.services.app.motivoAlta;
        var _$filterForm = $('#MotivosAltaFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.MotivoAlta.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.MotivoAlta.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.MotivoAlta.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/MotivosAlta/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/MotivosAlta/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMotivoAltaModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/MotivosAlta/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        _$MotivosAltaTable.jtable({

            title: app.localize('MotivosAlta'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _MotivosAltaService.listar
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
                                    deleteMotivosAlta(data.record);
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
                    width: '25%'
                },
                tipoAlta: {
                    title: app.localize('TipoAlta'),
                    width: '25%',
                    display: function (data) {
                        if (data.record.motivoAltaTipoAlta) {
                            return data.record.motivoAltaTipoAlta.descricao;
                        }
                    }
                }
            }
        });

        function getMotivosAlta(reload) {
            if (reload) {
                _$MotivosAltaTable.jtable('reload');
            } else {
                _$MotivosAltaTable.jtable('load', {
                    filtro: $('#MotivosAltaTableFilter').val()
                });
            }
        }

        function deleteMotivosAlta(MotivoAlta) {

            abp.message.confirm(
                app.localize('DeleteWarning', MotivoAlta.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _MotivosAltaService.excluir(MotivoAlta)
                            .done(function () {
                                getMotivosAlta(true);
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

        $('#CreateNewMotivoAltaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarMotivosAltaParaExcelButton').click(function () {
            _MotivosAltaService
                .listarParaExcel({
                    filtro: $('#MotivosAltaTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMotivosAltaButton, #RefreshMotivosAltaListButton').click(function (e) {
            e.preventDefault();
            getMotivosAlta();
        });

        abp.event.on('app.CriarOuEditarMotivoAltaModalSaved', function () {
            getMotivosAlta(true);
        });

        getMotivosAlta();

        $('#MotivosAltaTableFilter').focus();
    });
})();