(function () {
    $(function () {
        app.modals.PacienteDiagnosticosModal = function() {
            var _$pacienteDiagnosticosTable = $('#PacienteDiagnosticosTable');
            var _pacienteDiagnosticoService = abp.services.app.pacienteDiagnostico;

            var _permissions = {
                create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Create'),
                edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Edit'),
                'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Delete')
            };

            var _createOrEditModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/Assistenciais/CriarOuEditarPacienteDiagnosticosModal',
                modalClass: 'PacienteDiagnosticoCriarModal'
            });

            this.init = function(modalManager) {
                _$pacienteDiagnosticosTable.jtable({
                    title: app.localize('Paciente Diagnosticos'),
                    paging: true,
                    sorting: true,
                    multiSorting: true,

                    actions: { listAction: { method: atualizarPacienteDiagnosticos } },

                    fields: {
                        id: { key: true, list: false },

                        actions: {
                            title: app.localize('Actions'),
                            width: '12%',
                            sorting: false,
                            display: function(data) {
                                var $span = $('<span></span>');
                                if (_permissions.edit) {
                                    $('<button class="btn btn-default btn-xs" title="' +
                                            app.localize('Edit') +
                                            '"><i class="fa fa-edit"></i></button>')
                                        .appendTo($span)
                                        .click(function() {
                                            _createOrEditModal.open({ id: data.record.id, pacienteId:data.record.pacienteId });
                                        });
                                }

                                if (_permissions.delete) {
                                    $('<button class="btn btn-default btn-xs" title="' +
                                            app.localize('Delete') +
                                            '"><i class="fa fa-trash-alt"></i></button>')
                                        .appendTo($span)
                                        .click(function() {
                                            deletePacienteDiagnostico(data.record);
                                        });
                                }

                                return $span;
                            }
                        },
                        dataDiagnostico: {
                            title: app.localize('DataDiagnostico'),
                            width: '15%',
                            display: function(data) {
                                var momentObj = new moment(data.record.dataDiagnostico);
                                if (momentObj.isValid()) {
                                    return momentObj.format("DD/MM/YYYY");
                                }
                            }
                        },
                        atendimentoId: {
                            title: app.localize('Atendimento'),
                            width: '8%'
                        },
                        diagnostico: {
                            title: app.localize('Diagnostico'),
                            width: '40%',
                            display: function (data) {
                                if(data.record.grupoCID)
                                {
                                    return data.record.grupoCID.codigo + " - " + data.record.grupoCID.descricao;
                                }
                            }
                        }
                    }
                });

                _$pacienteDiagnosticosTable.jtable('option', 'pageSize', 150);

                function getPacienteDiagnosticos(reload) {
                    if (reload) {
                        _$pacienteDiagnosticosTable.jtable('reload');
                    } else {
                        _$pacienteDiagnosticosTable.jtable('load',
                            {
                                filtro: $('#PacientesDiagnosticosTableFilter').val()
                            });
                    }
                }

                function atualizarPacienteDiagnosticos() {
                    return _pacienteDiagnosticoService.listarIndexDiagnosticosPorPaciente($('#pacienteId').val());
                }

                function deletePacienteDiagnostico(PacienteDiagnostico) {
                    var message = "";
                    var momentObj = new moment(PacienteDiagnostico.dataDiagnostico);
                    if (momentObj.isValid()) {
                        message = momentObj.format("DD/MM/YYYY");
                    }

                    if (message.length != 0) {
                        message += " - ";
                    }

                    if (PacienteDiagnostico.diagnostico) {
                        message += PacienteDiagnostico.diagnostico ;
                    }

                    abp.message.confirm(
                        app.localize('DeleteWarning', message),
                        function(isConfirmed) {
                            if (isConfirmed) {
                                _pacienteDiagnosticoService.excluir(PacienteDiagnostico)
                                    .done(function() {
                                        getPacienteDiagnosticos(true);
                                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                                    });
                            }
                        }
                    );
                }

                $('#CreatePacienteDiagnostico').click(function() {
                    _createOrEditModal.open({ pacienteId: $('#pacienteId').val() });
                });
                
                abp.event.on('app.PacienteDiagnosticosCriarOuEditarModalSaved',
                    function() {
                        _createOrEditModal.close();
                        getPacienteDiagnosticos(true);
                    });

                getPacienteDiagnosticos();

                $('.modal-dialog').addClass("modal-lg");

                $('#PacientesDiagnosticoTableFilter').focus();
            }
        }
    });
})();