(function () {
    $(function () {
        var _$examesLaboratorioTable = $('#examesLaboratorioTable');
        var _resultadoService = abp.services.app.resultado;
        var _$contasPagarFilterForm = $('#contasPagarFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Imagens.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Imagens.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Imagens.Delete')
        };


        //var _createOrEditModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/GestaoLaudos/CriarOuEditarModal',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/GestaoLaudos/_CriarOuEditarModal.js',
        //    modalClass: 'CriarOuEditarModal'
        //});


        var _ModalVisualizarExamePorColeta = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ResultadoLaboratorio/ModalVisualizarExamePorRegistroColeta',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Laboratorio/_ModalVisualizacaoResultado.js',
            modalClass: 'ModalVisualizacaoResultado'
        });

        _$examesLaboratorioTable.jtable({

            title: app.localize('Exames'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            multiselect: true,


            actions: {
                listAction: {
                    method: _resultadoService.listar
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
                                    //_createOrEditModal.open({ id: data.record.id });
                                    location.href = 'ResultadoLaboratorio/CriarOuEditarResultadoExame/' + data.record.id
                                });
                        }


                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-binoculars"></i></button>')
                       .appendTo($span)
                       .click(function () {
                           _ModalVisualizarExamePorColeta.open({ coletaId: data.record.id });
                           // location.href = 'GestaoLaudos/CriarOuEditarModal/' + data.record.id
                       });



                        return $span;
                    }
                },

                //Status: {
                //    title: app.localize('Status'),
                //    width: '10%',
                //    display: function (data) {

                //        if (data.record.status == 1) {
                //            return app.localize('Pendente');
                //        }
                //        else if (data.record.status == 2) {
                //            return app.localize('Parecer');
                //        } else if (data.record.status == 3) {
                //            return app.localize('Laudo');
                //        } else if (data.record.status == 4) {
                //            return app.localize('LaudoRevisado');
                //        }

                //    }
                //},

                nic: {
                    title: app.localize('Codigo'),
                    width: '8%',
                    display: function (data) {
                        return zeroEsquerda(data.record.nic, 10);
                    }
                },

                //InternacaoAmbulatorio: {
                //    title: app.localize('InternacaoAmbulatorio'),
                //    width: '10%',
                //    display: function (data) {
                //        return data.record.InternacaoAmbulatorio;
                //    }
                //},

                dataColeta: {
                    title: app.localize('DataColeta'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataColeta != null) {
                            return moment(data.record.dataColeta).format('L LT')
                        }
                        else {
                            return '';
                        }
                    }
                },

                empresa: {
                    title: app.localize('Empresa'),
                    width: '20%',
                },
                Paciente: {
                    title: app.localize('Paciente'),
                    width: '20%',
                    display: function (data) {
                        if (data) {
                            return data.record.paciente;
                        }
                    }
                },

                MedicoSolitante: {
                    title: app.localize('MedicoSolitante'),
                    width: '20%',
                    display: function (data) {
                        if (data) {

                            return data.record.nomeMedicoSolicitante;
                        }
                    }
                },

                Tecnico: {
                    title: app.localize('Tecnico'),
                    width: '20%',
                    display: function (data) {
                        if (data) {

                            return data.record.tecnico;
                        }
                    }
                },

                //Exame: {
                //    title: app.localize('Exame'),
                //    width: '20%',
                //    display: function (data) {
                //        return data.record.exame;
                //    }
                //},



                //ConvenioDescricao: {
                //    title: app.localize('Convenio'),
                //    width: '7%',
                //    display: function (data) {
                //        if (data) {
                //            return data.record.convenioDescricao;
                //        }
                //    }
                //},




                //QtdContraste: {
                //    title: app.localize('QtdContraste'),
                //    width: '7%',
                //    display: function (data) {
                //        if (data) {
                //            return data.record.qtdContraste;
                //        }
                //    }
                //},

            }
        });

        function getRegistros(reload) {

            if (reload) {
                _$examesLaboratorioTable.jtable('reload');
            } else {
                _$examesLaboratorioTable.jtable('load', {
                    // filtro: $('#tableFilter').val()
                    convenioId: $('#convenioIndexId').val(),
                    pacienteId: $('#pacienteIndexId').val(),
                    //emissaoDe: _selectedDateRangePeriodo.startDate,
                    //emissaoAte: _selectedDateRangePeriodo.endDate,
                    startDate: _selectedDateRangePeriodo.startDate,
                    endDate: _selectedDateRangePeriodo.endDate,
                    horarioInicial: $('#convenioIndexId').val(),
                    horarioFinal: $('#convenioIndexId').val(),
                    modalidadeId: $('#modalidadeId').val(),
                    empresaId: $('#cbo-empresa-id').val()
                });
            }
        }

        function deleteRegistro(record) {

            abp.message.confirm(
                app.localize('DeleteWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _$contasPagarTable.excluir(record)
                            .done(function () {
                                getRegistros(true);
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
            $('#AdvacedLancamentosFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedLancamentosFiltersArea').slideUp();
        });



        $('#refreshLancamentosButton').click(function (e) {
            e.preventDefault();
            getRegistros();
        });

        abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
            getRegistros(true);
        });

        $('#tableFilter').focus();

        var _selectedDateRangePeriodo = {
            startDate: moment().startOf('month'),
            endDate: moment().endOf('month'),
            maxDate: "31/12/2030"
        };

        _$contasPagarFilterForm.find('input.periodo').daterangepicker(
        $.extend(true, app.createDateRangePickerOptions(), _selectedDateRangePeriodo),
        function (start, end, label) {
            _selectedDateRangePeriodo.startDate = start.format('YYYY-MM-DDT00:00:00Z');
            _selectedDateRangePeriodo.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
        });










        $('#emissao').on('cancel.daterangepicker', function (ev, picker) {
            //do something, like clearing an input
            $('#emissao').val('');
        });

        $('#vencimento').on('cancel.daterangepicker', function (ev, picker) {
            //do something, like clearing an input

            $('#vencimento').val('');
        });



        getRegistros();

        aplicarSelect2Padrao();
        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selectPaciente', "/api/services/app/Paciente/ListarDropdown");
        selectSW('.selectModalidade', "/api/services/app/Modalidade/ListarDropdown");
    });
})();