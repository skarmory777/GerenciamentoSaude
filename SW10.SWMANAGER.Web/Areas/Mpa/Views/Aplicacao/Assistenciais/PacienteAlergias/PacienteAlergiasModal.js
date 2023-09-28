(function () {
    $(function () {
        app.modals.PacienteAlergiasModal = function() {
            var _$pacienteAlergiasTable = $('#PacienteAlergiasTable');
            var _pacienteAlergiaService = abp.services.app.pacienteAlergias;

            var _permissions = {
                create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Create'),
                edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Edit'),
                'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Delete')
            };

            var _createOrEditModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/Assistenciais/CriarOuEditarPacienteAlergiasModal',
                modalClass: 'PacienteAlergiaCriarModal'
            });

            this.init = function(modalManager) {
                _$pacienteAlergiasTable.jtable({
                    title: app.localize('Paciente Alergias'),
                    paging: true,
                    sorting: true,
                    multiSorting: true,

                    actions: { listAction: { method: atualizarPacienteAlergias } },

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
                                            deletePacienteAlergia(data.record);
                                        });
                                }

                                return $span;
                            }
                        },
                        dataCadastro: {
                            title: app.localize('DataCadastro'),
                            width: '15%',
                            display: function(data) {
                                var momentObj = new moment(data.record.dataCadastro);
                                if (momentObj.isValid()) {
                                    return momentObj.format("DD/MM/YYYY");
                                }
                            }
                        },
                        atendimentoId: {
                            title: app.localize('Atendimento'),
                            width: '8%'
                        },
                        alergia: {
                            title: app.localize('Alergia'),
                            width: '40%'
                        }
                    }
                });

                _$pacienteAlergiasTable.jtable('option', 'pageSize', 150);

                function getPacienteAlergias(reload) {
                    if (reload) {
                        _$pacienteAlergiasTable.jtable('reload');
                    } else {
                        _$pacienteAlergiasTable.jtable('load',
                            {
                                filtro: $('#PacientesAlergiasTableFilter').val()
                            });
                    }
                }

                function atualizarPacienteAlergias() {
                    return _pacienteAlergiaService.listarIndexAlergiasPorPaciente($('#pacienteId').val());
                }

                function deletePacienteAlergia(PacienteAlergia) {
                    var message = "";
                    var momentObj = new moment(PacienteAlergia.dataCadastro);
                    if (momentObj.isValid()) {
                        message = momentObj.format("DD/MM/YYYY");
                    }

                    if (message.length != 0) {
                        message += " - ";
                    }

                    if (PacienteAlergia.alergia) {
                        message += PacienteAlergia.alergia;
                    }

                    abp.message.confirm(
                        app.localize('DeleteWarning', message),
                        function(isConfirmed) {
                            if (isConfirmed) {
                                _pacienteAlergiaService.excluir(PacienteAlergia)
                                    .done(function() {
                                        getPacienteAlergias(true);
                                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                                    });
                            }
                        }
                    );
                }

                $('#CreatePacienteAlergia').click(function() {
                    _createOrEditModal.open({ pacienteId: $('#pacienteId').val() });
                });
                
                abp.event.on('app.PacienteAlergiasCriarOuEditarModalSaved',
                    function() {
                        _createOrEditModal.close();
                        getPacienteAlergias(true);
                    });

                getPacienteAlergias();

                $('.modal-dialog').addClass("modal-lg");

                $('#PacientesAlergiaTableFilter').focus();
            }
        }
    });
})();