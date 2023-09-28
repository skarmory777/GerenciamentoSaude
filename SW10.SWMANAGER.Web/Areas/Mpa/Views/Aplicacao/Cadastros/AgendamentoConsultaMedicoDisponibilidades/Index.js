(function () {
    $(function () {
        var _$AgendamentoConsultaMedicoDisponibilidadesTable = $('#AgendamentoConsultaMedicoDisponibilidadesTable');
        var _AgendamentoConsultaMedicoDisponibilidadesService = abp.services.app.agendamentoConsultaMedicoDisponibilidade;
        var _$filterForm = $('#AgendamentoConsultaMedicoDisponibilidadesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.AgendamentoConsultaMedicoDisponibilidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.AgendamentoConsultaMedicoDisponibilidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.AgendamentoConsultaMedicoDisponibilidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AgendamentoConsultaMedicoDisponibilidades/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/AgendamentoConsultaMedicoDisponibilidades/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarAgendamentoConsultaMedicoDisponibilidadeModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/AgendamentoConsultaMedicoDisponibilidades/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$AgendamentoConsultaMedicoDisponibilidadesTable.jtable({

            title: app.localize('AgendamentoConsultaMedicoDisponibilidades'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _AgendamentoConsultaMedicoDisponibilidadesService.listar
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
                                    deleteAgendamentoConsultaMedicoDisponibilidades(data.record);
                                });
                        }

                        return $span;
                    }
                },
                medico: {
                    title: app.localize('Medico'),
                    width: '15%',
                    display: function (data) {
                        return data.record.medico.nomeCompleto;
                    }
                },
                especialidade: {
                    title: app.localize('MedicoEspecialidade'),
                    width: '15%',
                    display: function (data) {
                        return data.record.medicoEspecialidade.especialidade.nome;
                    }
                },
                dataInicio: {
                    title: app.localize('DataInicio'),
                    sorting: false,
                    width: '15%',
                    display: function (data) {
                        return moment(data.record.dataInicio).format('L');
                    }
                },
                dataFim: {
                    title: app.localize('DataFim'),
                    sorting: false,
                    width: '15%',
                    display: function (data) {
                        return moment(data.record.dataFim).format('L');
                    }
                },
                horaInicio: {
                    title: app.localize('Horario'),
                    sorting: false,
                    width: '15%',
                    display: function (data) {
                        return moment(data.record.horaInicio).format('LT') + " - " + moment(data.record.horaFim).format('LT');
                    }
                }            }
        });

        function getAgendamentoConsultaMedicoDisponibilidades(reload) {
            if (reload) {
                _$AgendamentoConsultaMedicoDisponibilidadesTable.jtable('reload');
            } else {
                _$AgendamentoConsultaMedicoDisponibilidadesTable.jtable('load', {
                    filtro: $('#AgendamentoConsultaMedicoDisponibilidadesTableFilter').val()
                });
            }
        }

        function deleteAgendamentoConsultaMedicoDisponibilidades(agendamentoConsultaMedicoDisponibilidade) {

            abp.message.confirm(
                app.localize('DeleteWarning', moment(agendamentoConsultaMedicoDisponibilidade.HoraInicio).format('LT') + '' + moment(agendamentoConsultaMedicoDisponibilidade.HoraFim).format('LT')),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _AgendamentoConsultaMedicoDisponibilidadesService.excluir(agendamentoConsultaMedicoDisponibilidade)
                            .done(function () {
                                getAgendamentoConsultaMedicoDisponibilidades(true);
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

        $('#CreateNewAgendamentoConsultaMedicoDisponibilidadeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarAgendamentoConsultaMedicoDisponibilidadesParaExcelButton').click(function () {
            _AgendamentoConsultaMedicoDisponibilidadesService
                .listarParaExcel({
                    filtro: $('#AgendamentoConsultaMedicoDisponibilidadesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetAgendamentoConsultaMedicoDisponibilidadesButton, #RefreshAgendamentoConsultaMedicoDisponibilidadesListButton').click(function (e) {
            e.preventDefault();
            getAgendamentoConsultaMedicoDisponibilidades();
        });

        abp.event.on('app.CriarOuEditarAgendamentoConsultaMedicoDisponibilidadeModalSaved', function () {
            getAgendamentoConsultaMedicoDisponibilidades(true);
        });

        getAgendamentoConsultaMedicoDisponibilidades();

        $('#AgendamentoConsultaMedicoDisponibilidadesTableFilter').focus();


    });
})();