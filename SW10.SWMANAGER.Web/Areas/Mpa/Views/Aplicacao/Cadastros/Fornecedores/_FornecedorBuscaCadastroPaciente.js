//(function () {
//    $(function () {
//        var _$PacientesTable = $('#PacientesTable');
//        var _PacientesService = abp.services.app.paciente;
//        var _$filterForm = $('#PacientesFilterForm');

//        var _permissions = {
//            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Create'),
//            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Edit'),
//            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Paciente.Delete')
//        };

//        var _createOrEditModal = new app.ModalManager({
//            viewUrl: abp.appPath + 'Mpa/Pacientes/CriarOuEditarModal',
//            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_CriarOuEditarModal.js',
//            modalClass: 'CriarOuEditarPacienteModal'
//        });

//        var _userPermissionsModal = new app.ModalManager({
//            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
//            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_PermissionsModal.js',
//            modalClass: 'UserPermissionsModal'
//        });
//        _$PacientesTable.jtable({

//            title: app.localize('Pacientes'),
//            paging: true,
//            sorting: true,
//            multiSorting: true,

//            actions: {
//                listAction: {
//                    method: _PacientesService.listarParaIndex
//                }
//            },

//            fields: {
//                id: {
//                    key: true,
//                    list: false
//                },
//                actions: {
//                    title: app.localize('Actions'),
//                    width: '12%',
//                    sorting: false,
//                    display: function (data) {
//                        var $span = $('<span></span>');
//                        if (_permissions.edit) {
//                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
//                                .appendTo($span)
//                                .click(function () {
//                                    _createOrEditModal.open({ id: data.record.id });
//                                });
//                        }

//                        if (_permissions.delete) {
//                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
//                                .appendTo($span)
//                                .click(function () {
//                                    deletePacientes(data.record);
//                                });
//                        }

//                        return $span;
//                    }
//                },
//                nomeCompleto: {
//                    title: app.localize('NomeCompleto'),
//                    width: '15%'
//                },
//                rg: {
//                    title: app.localize('Rg'),
//                    width: '8%'
//                },
//                cpf: {
//                    title: app.localize('Cpf'),
//                    width: '8%'
//                },
//                nascimento: {
//                    title: app.localize('Nascimento'),
//                    width: '8%',
//                    display: function (data) {
//                        return moment(data.record.nascimento).format('L');
//                    }
//                },
//                telefone: {
//                    title: app.localize('Telefone'),
//                    sorting: false,
//                    width: '15%',
//                    display: function (data) {
//                        return data.record.telefone1;
//                    }
//                },
//                nomeMae: {
//                    title: app.localize('NomeMae'),
//                    width: '15%'
//                },
//                nomePai: {
//                    title: app.localize('NomePai'),
//                    width: '15%'
//                }//,
//                //convenio: {
//                //    title: app.localize('Convenio'),
//                //    sorting: false,
//                //    width: '15%',
//                //    display: function (data) {
//                //        return data.record.convenio.nomeFantasia;
//                //    }
//                //},
//                //plano: {
//                //    title: app.localize('Estado'),
//                //    sorting: false,
//                //    width: '15%',
//                //    display: function (data) {
//                //        return data.record.plano.nome;
//                //    }
//                //}
//            }
//        });

//        function getPacientes(reload) {
//            if (reload) {
//                _$PacientesTable.jtable('reload');
//            } else {
//                _$PacientesTable.jtable('load', {
//                    filtro: $('#PacientesTableFilter').val()
//                });
//            }
//        }

//        function deletePacientes(Paciente) {

//            abp.message.confirm(
//                app.localize('DeleteWarning', Paciente.primeiroNome),
//                function (isConfirmed) {
//                    if (isConfirmed) {
//                        _PacientesService.excluir(Paciente)
//                            .done(function () {
//                                getPacientes(true);
//                                abp.notify.success(app.localize('SuccessfullyDeleted'));
//                            });
//                    }
//                }
//            );
//        }

//        function createRequestParams() {
//            var prms = {};
//            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
//            return $.extend(prms);
//        }

//        $('#ShowAdvancedFiltersSpan').click(function () {
//            $('#ShowAdvancedFiltersSpan').hide();
//            $('#HideAdvancedFiltersSpan').show();
//            $('#AdvacedAuditFiltersArea').slideDown();
//        });

//        $('#HideAdvancedFiltersSpan').click(function () {
//            $('#HideAdvancedFiltersSpan').hide();
//            $('#ShowAdvancedFiltersSpan').show();
//            $('#AdvacedAuditFiltersArea').slideUp();
//        });

//        $('#CreateNewPacienteButton').click(function () {
//            _createOrEditModal.open();
//        });

//        $('#ExportarPacientesParaExcelButton').click(function () {
//            _PacientesService
//                .listarParaExcel({
//                    filtro: $('#PacientesTableFilter').val(),
//                    //sorting: $(''),
//                    maxResultCount: $('span.jtable-page-size-change select').val()
//                })
//                .done(function (result) {
//                    app.downloadTempFile(result);
//                });
//        });

//        $('#GetPacientesButton, #RefreshPacientesListButton').click(function (e) {
//            e.preventDefault();
//            getPacientes();
//        });

//        abp.event.on('app.CriarOuEditarPacienteModalSaved', function () {
//            getPacientes(true);
//        });

//        getPacientes();

//        $('#PacientesTableFilter').focus();
//    });
//})();