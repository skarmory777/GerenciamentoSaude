(function () {
    $(function () {

        $(document).ready(function () {
          
            $('#criarRegistroButton').hide();
        });

        var _$registroExemesTable = $('#registroExemesTable');
        var _registroExemesService = abp.services.app.registroExemes;
        var _solicitacaoExameItem = abp.services.app.solicitacaoExameItem;
        var _$contasPagarFilterForm = $('#contasPagarFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Imagens.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Imagens.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Diagnosticos.Imagens.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/RegistroExames/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/Registro/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });

        var _createRegistroPorSolcitacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/RegistroExames/CriarRegistroDeExameSolicitado',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/Registro/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });

        var _createPorExameFaturadosNaoRegistrados = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/RegistroExames/CriarPorExameFaturadosNaoRegistrados',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/Registro/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });

        _$registroExemesTable.jtable({
            title: app.localize('RegistrosExames'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            multiselect: true,

            actions: {
                listAction: {
                    method: _registroExemesService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
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
                        return $span;
                    }
                },
                Registro: {
                    title: app.localize('Registro'),
                    width: '5%',
                    display: function (data) {
                        return data.record.codigo;
                    }
                },

                Paciente: {
                    title: app.localize('Paciente'),
                    width: '15%',
                    display: function (data) {
                        if (data) {
                            return data.record.pacienteDescricao;
                        }
                    }
                },

                Exame: {
                    title: app.localize('Exame'),
                    width: '20%',
                    display: function (data) {
                        return data.record.exame;
                    }
                },
                Medico: {
                    title: app.localize('Medico'),
                    width: '15%',
                    display: function (data) {
                        return data.record.medico;
                    }
                },

                DataSolicitacao: {
                    title: app.localize('DataRegistro'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.data) {
                            return moment(data.record.data).format('L LTS');
                        }
                    }
                },
                Leito: {
                    title: app.localize('Leito'),
                    width: '10%',
                    display: function (data) {
                        return data.record.leito;
                    }
                },
                UnidadeOrganizacional: {
                    title: app.localize('UnidadeOrganizacional'),
                    width: '10%',
                    display: function (data) {
                        return data.record.unidadeOrganizacional;
                    }
                },

                ConvenioDescricao: {
                    title: app.localize('Convenio'),
                    width: '20%',
                    display: function (data) {
                        if (data) {
                            return data.record.convenioDescricao;
                        }
                    }
                },

            }
        });

        function getRegistros(reload) {
           
            if (reload) {
                _$registroExemesTable.jtable('reload');
            } else {
                _$registroExemesTable.jtable('load', {
                    convenioId: $('#convenioIndexId').val(),
                    pacienteId: $('#pacienteIndexId').val(),
                    emissaoDe: _selectedDateRangePeriodo.startDate,
                    emissaoAte: _selectedDateRangePeriodo.endDate                    
                });
            }
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

        $('.novo-lancamento').click(function (e) {

            e.preventDefault();
           
            _createOrEditModal.open();
            //location.href = 'RegistroExames/CriarOuEditarModal/';
        });

        $('#refreshLancamentosButton').click(function (e) {
            e.preventDefault();

            $('#registroExemesTable').show();
            $('#registroExemesSolicitadosTable').hide();
            $('#examesFaturadosSemRegistroTable').hide();
            $('#criarRegistroButton').hide();
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
        $('#emissao').val('');

        var list = [];

        var _$registroExemesSolicitadosTable = $('#registroExemesSolicitadosTable');

        _$registroExemesSolicitadosTable.jtable({

            title: app.localize('ExamesSolicitacao'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            multiselect: true,

            actions: {
                listAction: {
                    method: _solicitacaoExameItem.listarExamesImagensNaoRegistrados
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (data.record.accessNumber) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('AccessNumber') + '"><i class="fas fa-images"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    window.open(data.record.accessNumber);
                                });
                        }


                        return $span;
                    }
                },

                Paciente: {
                    title: app.localize('Paciente'),
                    width: '25%',
                    display: function (data) {
                        if (data) {
                            return data.record.pacienteDescricao;
                        }
                    }
                },

                Exame: {
                    title: app.localize('Exame'),
                    width: '45%',
                    display: function (data) {
                        return data.record.exame;
                    }
                },

                Data: {
                    title: app.localize('Data'),
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.data).format('L LST');
                    }
                },

               

                ConvenioDescricao: {
                    title: app.localize('Convenio'),
                    width: '15%',
                    display: function (data) {
                        if (data) {
                            return data.record.convenioDescricao;
                        }
                    }
                }

            }
            ,

            selectionChanged: function () {

                var $selectedRows = _$registroExemesSolicitadosTable.jtable('selectedRows');
                list = [];

                if ($selectedRows.length > 0) {
                    //Show selected rows
                    $('#criarRegistroButton').enable(true);
                 
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        $('#atendimentoId').val(record.atendimentoId);
                        list.push(record.id);
                    });
                  
                }
                else
                {
                    $('#atendimentoId').val('');
                    $('#criarRegistroButton').enable(false);
                }
                getExamesSolicitados();

            }
        });

        function getExamesSolicitados(reload) {
            if (reload) {
                _$registroExemesSolicitadosTable.jtable('reload');
            } else {
                _$registroExemesSolicitadosTable.jtable('load', {                    
                    atendimentoId: $('#atendimentoId').val(),
                    convenioId: $('#convenioIndexId').val(),
                    pacienteId: $('#pacienteIndexId').val(),
                    emissaoDe: _selectedDateRangePeriodo.startDate,
                    emissaoAte: _selectedDateRangePeriodo.endDate
                });
            }
        }

        $('#buscarExamesSolicitadosButton').click(function (e) {
            e.preventDefault();

            $('#atendimentoId').val('');

            $('#registroExemesTable').hide();
            $('#examesFaturadosSemRegistroTable').hide();

            $('#registroExemesSolicitadosTable').show();
            $('#criarRegistroButton').show();
            $('#criarRegistroButton').enable(false);
            $('#hdnConsulta').val('S');

            getExamesSolicitados();

        });

        var _$examesFaturadosSemRegistroTable = $('#examesFaturadosSemRegistroTable');
        
        _$examesFaturadosSemRegistroTable.jtable({

            title: app.localize('ExamesSolicitacao'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,
            multiselect: true,

            actions: {
                listAction: {
                    method: _registroExemesService.listarExamesFaturadosSemregistros
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                //actions: {
                //    title: app.localize('Actions'),
                //    width: '6%',
                //    sorting: false,
                //    display: function (data) {
                //        var $span = $('<span></span>');
                //        if (_permissions.edit) {
                //            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                //                .appendTo($span)
                //                .click(function () {
                //                   
                //                    _createPorExameFaturadosNaoRegistrados.open({ ids: list });
                //                    //location.href = 'ContasPagar/CriarOuEditarModal/' + data.record.id
                //                });
                //        }


                //        return $span;
                //    }
                //},

                Paciente: {
                    title: app.localize('Paciente'),
                    width: '7%',
                    display: function (data) {
                        if (data) {
                            return data.record.pacienteDescricao;
                        }
                    }
                },

                Exame: {
                    title: app.localize('Exame'),
                    width: '7%',
                    display: function (data) {
                        return data.record.exame;
                    }
                },




                ConvenioDescricao: {
                    title: app.localize('Convenio'),
                    width: '7%',
                    display: function (data) {
                        if (data) {
                            return data.record.convenioDescricao;
                        }
                    }
                }

            }
          ,

            selectionChanged: function () {

                var $selectedRows = _$examesFaturadosSemRegistroTable.jtable('selectedRows');
                list = [];

                if ($selectedRows.length > 0) {
                    //Show selected rows
                    $('#criarRegistroButton').enable(true);
                   
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        $('#atendimentoId').val(record.atendimentoId);
                        list.push(record.id);
                    });

                }
                else {
                    $('#atendimentoId').val('');
                    $('#criarRegistroButton').enable(false);
                }
                getExamesFaturadosSemRegistro();

            }
        });

        function getExamesFaturadosSemRegistro(reload) {
           
            if (reload) {
                _$examesFaturadosSemRegistroTable.jtable('reload');
            } else {
                _$examesFaturadosSemRegistroTable.jtable('load', {
                    convenioId: $('#convenioIndexId').val(),
                    atendimentoId: $('#atendimentoId').val(),
                    pacienteId: $('#pacienteIndexId').val(),
                    emissaoDe: _selectedDateRangePeriodo.startDate,
                    emissaoAte: _selectedDateRangePeriodo.endDate,
                });
            }
        }

        $('#buscarExamesFaturadosSemRegistroButton').click(function (e) {
            e.preventDefault();

            $('#atendimentoId').val('');

            $('#registroExemesTable').hide();
            $('#registroExemesSolicitadosTable').hide();
            $('#examesFaturadosSemRegistroTable').show();
            $('#criarRegistroButton').show();
            $('#criarRegistroButton').enable(false);
            $('#hdnConsulta').val('F');

            getExamesFaturadosSemRegistro();

        });

        $('#RefreshRegistroExames').on('click', function (e) {
            e.preventDefault();
            $('#atendimentoId').val('');
            $('#registroExemesTable').show();
            $('#registroExemesSolicitadosTable').hide();
            $('#examesFaturadosSemRegistroTable').hide();
            $('#criarRegistroButton').show();
            $('#criarRegistroButton').enable(false);
            $('#hdnConsulta').val('');
            getRegistros();
        });

        $('#criarRegistroButton').click(function (e) {
            e.preventDefault();

            if (list.length > 0) {

                if ($('#hdnConsulta').val() == 'F') {
                    _createPorExameFaturadosNaoRegistrados.open({ ids: list });
                }
                else if ($('#hdnConsulta').val() == 'S') {
                    _createRegistroPorSolcitacaoModal.open({ ids: list });
                }
            }
        });

        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selectPaciente', "/api/services/app/Paciente/ListarDropdown");
    });
})();