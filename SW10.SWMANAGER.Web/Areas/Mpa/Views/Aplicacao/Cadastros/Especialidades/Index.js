(function () {
    $(function () {
        var _$EspecialidadesTable = $('#EspecialidadesTable');
        var _EspecialidadesService = abp.services.app.especialidade;
        var _$filterForm = $('#EspecialidadesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Especialidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Especialidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Especialidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Especialidades/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Especialidades/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarEspecialidadeModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Especialidades/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        
        _$EspecialidadesTable.jtable({

            title: app.localize('Especialidades'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _EspecialidadesService.listarEspecialidades
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
                                    deleteEspecialidades(data.record);
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
                    width: '42%'
                },
                cbo: {
                    title: app.localize('Cbo'),
                    width: '42%',
                    display: function (data) {
                        if (data.record.sisCbo) {
                            return data.record.sisCbo.codigo + ' ' + data.record.sisCbo.descricao;
                        }
                    }
                }
            }
        });

        function getEspecialidades(reload) {
            if (reload) {
                _$EspecialidadesTable.jtable('reload');
            } else {
                _$EspecialidadesTable.jtable('load', {
                    filtro: $('#EspecialidadesTableFilter').val()
                });
            }
        }

        function deleteEspecialidades(Especialidade) {
            abp.message.confirm(
                app.localize('DeleteWarning', Especialidade.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _EspecialidadesService.excluir(Especialidade)
                            .done(function () {
                                getEspecialidades(true);
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

        $('#CreateNewEspecialidadeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarEspecialidadesParaExcelButton').click(function () {
            _EspecialidadesService
                .listarParaExcel({
                    filtro: $('#EspecialidadesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetEspecialidadesButton, #RefreshEspecialidadesListButton').click(function (e) {
            e.preventDefault();
            getEspecialidades();
        });

        abp.event.on('app.CriarOuEditarEspecialidadeModalSaved', function () {
            getEspecialidades(true);
        });

        getEspecialidades();

        $('#EspecialidadesTableFilter').focus();
    });
})();