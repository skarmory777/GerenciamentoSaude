(function () {
    $(function () {
        var _$PrescricoesPacienteTable = $('#PrescricoesPacienteTable-' + localStorage["AtendimentoId"]);
        var _PrescricoesPacienteService = abp.services.app.prescricaoMedica;
        var _$filterForm = $('#PrescricoesPacienteFilterForm-' + localStorage["AtendimentoId"]);
        var _selectedDateRange = {
            startDate: moment().startOf('day'), //moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        $('#date-range-prescricao-' + localStorage["AtendimentoId"]).daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AmbulatorioEmergencia.Medico.Prescricao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Assistenciais/_CriarOuEditarMedicoPrescricao',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/PrescricoesPaciente/_CriarOuEditar.js',
            modalClass: 'CriarOuEditarPrescricaoModal'
        });

        $('#PrescricoesPacienteTable-' + localStorage["AtendimentoId"]).jtable({
            //title: app.localize('PrescricoesPaciente'),
            paging: true,
            sorting: true,
            multiSorting: true,
            //selecting: true,
            //multiselect: false,
            //selectingCheckboxes: true,
            actions: {
                listAction: {
                    method: _PrescricoesPacienteService.listarPorPaciente
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    localStorage.removeItem("RespostasList");
                                    localStorage.removeItem("DivisaoId");
                                    localStorage.removeItem("PrescricaoId");
                                    _createOrEditModal.open({ atendimentoId: localStorage["AtendimentoId"], id: data.record.id });
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deletePrescricoesPaciente(data.record);
                                });
                        }
                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%'
                },
                data: {
                    title: app.localize('DataPrescricao'),
                    width: '20%',
                    display: function (data) {
                        return moment(data.record.dataPrescricao).format('L');
                    }
                },
                paciente: {
                    title: app.localize('Paciente'),
                    width: '30%',
                    display: function (data) {
                        return data.record.atendimento.paciente.nomeCompleto;
                    }
                },
                medico: {
                    title: app.localize('Medico'),
                    width: '30%',
                    display: function (data) {
                        return data.record.atendimento.medico.nomeCompleto;
                    }
                },
                //responsavel: {
                //    title: app.localize('Responsavel'),
                //    width: '20%',
                //    display: function (data) {
                //       
                //        var responsavel = data.record.creatorUserId;
                //        var user;
                //        _userService.getUser(responsavel).done(function (data) {
                //            user = data;
                //        });
                //        return user.;
                //    }
                //},
            }
        });

        function getPrescricoesPaciente() {
            $('#PrescricoesPacienteTable-' + localStorage["AtendimentoId"]).jtable('load', createRequestParams());
        }

        function deletePrescricoesPaciente(prescricao) {
            abp.message.confirm(
                app.localize('DeleteWarning', prescricao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _PrescricoesPacienteService.excluir(prescricao)
                            .done(function () {
                                getPrescricoesPaciente();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        //$('#ShowAdvancedFiltersSpan').click(function () {
        //    $('#ShowAdvancedFiltersSpan').hide();
        //    $('#HideAdvancedFiltersSpan').show();
        //    $('#AdvacedAuditFiltersArea').slideDown();
        //});

        //$('#HideAdvancedFiltersSpan').click(function () {
        //    $('#HideAdvancedFiltersSpan').hide();
        //    $('#ShowAdvancedFiltersSpan').show();
        //    $('#AdvacedAuditFiltersArea').slideUp();
        //});

        $('#CreateNewPrescricaoPacienteButton-' + localStorage["AtendimentoId"]).click(function (e) {
            e.preventDefault();
            localStorage.removeItem("RespostasList");
            localStorage.removeItem("DivisaoId");
            localStorage.removeItem("PrescricaoId");
            _createOrEditModal.open({ atendimentoId: localStorage["AtendimentoId"] });
        });

        $('#ExportarPrescricoesPacienteParaExcelButton-' + localStorage["AtendimentoId"]).click(function () {
            _PrescricoesPacienteService
                .listarParaExcel({
                    filtro: $('#PrescricoesPacienteTableFilter-' + localStorage["AtendimentoId"]).val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetPrescricoesPacienteButton-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            getPrescricoesPaciente();
        });

        $('#RefreshPrescricoesPacienteListButton-' + localStorage["AtendimentoId"]).on('click', function (e) {
            e.preventDefault();
            getPrescricoesPaciente();
        });

        //abp.event.on('app.CriarOuEditarPrescricaoModalSaved', function () {
        //    getPrescricoesPaciente();
        //});

        //abp.event.on('app.CriarOuEditarPrescricaoCompletaModalSaved', function () {
        //    getPrescricoesPaciente();
        //});

        //getPrescricoesPaciente();

        //$('#PrescricoesPacienteTableFilter-' + localStorage["AtendimentoId"]).focus();
    });
})();