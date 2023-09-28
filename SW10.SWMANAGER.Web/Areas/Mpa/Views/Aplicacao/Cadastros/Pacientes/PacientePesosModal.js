(function () {
    $(function () {
        app.modals.PacientePesoModal = function() {
            var _$pacientesPesoTable = $('#PacientesPesoTable');
            var _pacientesService = abp.services.app.paciente;
            var _pacientePesoService = abp.services.app.pacientePeso;

            var _permissions = {
                create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Create'),
                edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Edit'),
                'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Delete')
            };

            var _createOrEditModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/Pacientes/PacientePesosCriarOuEditar',
                modalClass: 'CriarOuEditarPacientePesoModal'
            });

            this.init = function(modalManager) {
                _$pacientesPesoTable.jtable({
                    title: app.localize('Peso Altura'),
                    paging: true,
                    sorting: true,
                    multiSorting: true,

                    actions: { listAction: { method: atualizarPacientePeso } },

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
                                            console.log('IdPaciente', data);
                                            _createOrEditModal.open({ id: data.record.id, pacienteId:data.record.pacienteId });
                                        });
                                }

                                if (_permissions.delete) {
                                    $('<button class="btn btn-default btn-xs" title="' +
                                            app.localize('Delete') +
                                            '"><i class="fa fa-trash-alt"></i></button>')
                                        .appendTo($span)
                                        .click(function() {
                                            deletePacientes(data.record);
                                        });
                                }

                                return $span;
                            }
                        },
                        dataPesagem: {
                            title: app.localize('DataPesagem'),
                            width: '15%',
                            display: function(data) {
                                var momentObj = new moment(data.record.dataPesagem);
                                if (momentObj.isValid()) {
                                    return momentObj.format("DD/MM/YYYY");
                                }
                            }
                        },
                        valor: {
                            title: app.localize('Peso'),
                            width: '8%'
                        },
                        altura: {
                            title: app.localize('Altura'),
                            width: '8%'
                        },
                        perimetroCefalico: {
                            title: app.localize('PerimetroCefalico'),
                            width: '8%'
                        },
                        imc: {
                            title: app.localize('Imc'),
                            sorting: false,
                            width: '15%'
                        }
                    }
                });

                _$pacientesPesoTable.jtable('option', 'pageSize', 150);

                function getPacientePeso(reload) {
                    if (reload) {
                        console.log(_$pacientesPesoTable);
                        _$pacientesPesoTable.jtable('reload');
                    } else {
                        _$pacientesPesoTable.jtable('load',
                            {
                                filtro: $('#PacientesPesoTableFilter').val()
                            });
                    }
                }

                function atualizarPacientePeso() {
                    return _pacientePesoService.listarIndexAsync($('#pacienteId').val());
                }

                function deletePacientes(PacientePeso) {
                    var message = "";
                    var momentObj = new moment(PacientePeso.dataPesagem);
                    if (momentObj.isValid()) {
                        message = momentObj.format("DD/MM/YYYY");
                    }

                    if (message.length != 0) {
                        message += " - ";
                    }

                    if (PacientePeso.valor) {
                        message += PacientePeso.valor + " KG";
                    }

                    abp.message.confirm(
                        app.localize('DeleteWarning', message),
                        function(isConfirmed) {
                            if (isConfirmed) {
                                _pacientePesoService.excluir(PacientePeso)
                                    .done(function() {
                                        getPacientePeso(true);
                                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                                    });
                            }
                        }
                    );
                }

                $('#CreatePacientePeso').click(function() {
                    _createOrEditModal.open({ pacienteId: $('#pacienteId').val() });
                });
                
                abp.event.on('app.PacientePesosCriarOuEditarModalSaved',
                    function() {
                        _createOrEditModal.close();
                        getPacientePeso(true);
                    });

                getPacientePeso();

                $('.modal-dialog').addClass("modal-lg");

                $('#PacientesPesoTableFilter').focus();
            }
        }
    });
})();