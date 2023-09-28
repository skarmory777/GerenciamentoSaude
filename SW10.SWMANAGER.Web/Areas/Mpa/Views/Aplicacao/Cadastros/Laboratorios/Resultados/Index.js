(function () {
    $(function () {
        var _$resultadosTable = $('#ResultadosTable');
        var _$atendimentosTable = $('#AtendimentosTable');
        var _$examesTable = $('#ExamesTable');
        var _resultadosService = abp.services.app.resultado;
        var _atendimentosService = abp.services.app.atendimento;
        var _examesService = abp.services.app.resultadoExame;
        var _$resultadosFilterForm = $('#ResultadosFilterForm');
        var _$atendimentosFilterForm = $('#AtendimentosFilterForm');
        var _$examesFilterForm = $('#ExamesFilterForm');
        var _solicitacaoExameItem = abp.services.app.solicitacaoExameItem;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Resultado.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Resultado.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Resultado.Delete')
        };

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        var todayYesterdayProperty = {};
        var pickerOptions = app.createDateRangePickerOptions();
        todayYesterdayProperty[app.localize('Yesterday/Today')] = [moment().add(-1, "days").startOf('day'), moment().endOf('day')];
        pickerOptions.ranges = Object.assign(todayYesterdayProperty, pickerOptions.ranges);

        $('#date-field').daterangepicker($.extend(true, pickerOptions, _selectedDateRange), function (start, end, label) {
            _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
            _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');

            reloadFn();
        });


        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Resultados/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Resultados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarResultadoModal'
        });

        var _createColetaPorSolcitacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Resultados/CriarColetaDeExamesSolicitado',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Resultados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarResultadoModal'
        });

        var _createOrEditResultadoExameModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ResultadosExames/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ResultadosExames/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarResultadoExameModal'
        });

        _$atendimentosTable.jtable({

            title: app.localize('Atendimentos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: false, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column

            actions: {
                listAction: {
                    method: _atendimentosService.listarAtendimentos
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                unidade: {
                    title: app.localize('Unidade'),
                    width: '15%',
                },
                codigoAtendimento: {
                    title: app.localize('Atendimento'),
                    width: '20%',
                    display: function (data) {
                        if (data.record.codigoAtendimento != null) {
                            var ans = zeroEsquerda(data.record.codigoAtendimento, '10');
                            return ans;
                        }
                        else {
                            return '';
                        }
                    }
                },
                paciente: {
                    title: app.localize('Paciente'),
                    width: '20%',
                },
                tipoLeito: {
                    title: app.localize('TipoLeito'),
                    width: '20%',
                },
                leitoAtual: {
                    title: app.localize('Leito'),
                    width: '15%',
                },
                dataRegistro: {
                    title: app.localize('DataInternacao'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataRegistro != null) {
                            return moment(data.record.dataRegistro).format('L')
                        }
                        else {
                            return '';
                        }
                    }
                },
                dataInicioConta: {
                    title: app.localize('DataInicioConta'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataInicioConta != null) {
                            return moment(data.record.dataInicioConta).format('L')
                        }
                        else {
                            return '';
                        }
                    }
                },
                dataFim: {
                    title: app.localize('DataFim'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataFim != null) {
                            return moment(data.record.dataFim).format('L')
                        }
                        else {
                            return '';
                        }
                    }
                },
                covenio: {
                    title: app.localize('Convenio'),
                    width: '10%',
                },
                matricula: {
                    title: app.localize('Matricula'),
                    width: '10%',
                },
                guia: {
                    title: app.localize('Guia'),
                    width: '10%',
                },
                numeroGuia: {
                    title: app.localize('NumeroGuia'),
                    width: '10%',
                },
                empresa: {
                    title: app.localize('Empresa'),
                    width: '10%',
                },
                dataAlta: {
                    title: app.localize('DataAlta'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.dataAlta != null) {
                            return moment(data.record.dataAlta).format('L')
                        }
                        else {
                            return '';
                        }
                    }
                },
                plano: {
                    title: app.localize('Plano'),
                    width: '10%',
                },
                codigoPaciente: {
                    title: app.localize('CodigoPaciente'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.codigoPaciente != null) {
                            var ans = zeroEsquerda(data.record.codigoPaciente, '10');
                            return ans;
                        }
                        else {
                            return '';
                        }
                    }
                },
            },
            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = _$atendimentosTable.jtable('selectedRows');
                //getContas();
                if ($selectedRows.length > 0) {
                    //Show selected rows
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        $('#atendimento-id').val(record.id);
                        getResultados();
                        $('#exibir-sw-div-retratil-resultados-table').click();
                    });
                }
                else {
                    $('#atendimento-id').val('');
                    getResultados();
                    $('#omitir-sw-div-retratil-resultados-table').click();
                }
            },
        });

        function listarAtendimento() {
            _$examesSolicitadosTable.childTable.jtable('load', {
                id: localStorage("AteId")
            });
        }

        _$resultadosTable.jtable({

            title: app.localize('Coletas'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: false, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column

            actions: {
                listAction: {
                    method: _resultadosService.listar
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

                                    deleteResultados(data.record);
                                });
                        }

                        return $span;
                    }
                },

                nic: {
                    title: app.localize('NIC'),
                    width: '8%',
                    display: function (data) {
                        return zeroEsquerda(data.record.nic, '10');
                    }
                    //sorting: false,
                },
                dataColeta: {
                    title: app.localize('DataColeta'),
                    width: '12%',
                    display: function (data) {
                        if (data.record.dataColeta) {
                            return moment(data.record.dataColeta).format('L LT');
                        }
                    }

                    //sorting: false,
                },
                paciente: {
                    title: app.localize('Paciente'),
                    width: '20%',
                    //sorting: false,
                },
                isRn: {
                    title: app.localize('Rn'),
                    width: '8%',

                    display: function (data) {

                        if (data.record.isRn) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },
                medicoSolicitante: {
                    title: app.localize('MedicoSolicitante'),
                    width: '8%',
                    //sorting: false,
                },
                tecnico: {
                    title: app.localize('Tecnico'),
                    width: '8%',
                    //sorting: false,
                },
                dataTecnico: {
                    title: app.localize('DataTecnico'),
                    width: '8%',
                    //sorting: false,
                },
                dataEntrega: {
                    title: app.localize('DataEntrega'),
                    width: '8%',
                    //sorting: false,
                },
                entreguePor: {
                    title: app.localize('EntreguePor'),
                    width: '8%',
                    //sorting: false,
                },
            },
            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = _$resultadosTable.jtable('selectedRows');
                //getContas();
                if ($selectedRows.length > 0) {
                    //Show selected rows
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        $('#resultado-id-exame').val(record.id);
                        getExames();
                        $('#exibir-sw-div-retratil-exames-table').click();
                    });
                }
                else {
                    $('#resultado-id-exame').val('');
                    getExames();
                    $('#omitir-sw-div-retratil-exames-table').click();
                }
            },
        });

        _$examesTable.jtable({

            title: app.localize('Exames'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _examesService.listarIndex
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
                                .click(function (e) {
                                    e.preventDefault();
                                    _createOrEditResultadoExameModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteResultados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                mneumonio: {
                    title: app.localize('Mneumonio'),
                    width: '15%'
                },
                dataColeta: {
                    title: app.localize('DataColeta'),
                    width: '15%'
                },
                exameId: {
                    title: app.localize('NumeroExame'),
                    width: '15%'
                },
                nomeExame: {
                    title: app.localize('NomeExame'),
                    width: '15%'
                },
                usuarioIncluidoId: {
                    title: app.localize('Incluido'),
                    width: '15%'
                },
                dataIncluido: {
                    title: app.localize('DataIncluido'),
                    width: '15%'
                },
                usuarioDigitadoId: {
                    title: app.localize('Digitado'),
                    width: '15%'
                },
                dataDigitado: {
                    title: app.localize('DataDigitado'),
                    width: '15%'
                },
                usuarioConferidoId: {
                    title: app.localize('Conferido'),
                    width: '15%'
                },
                dataConferido: {
                    title: app.localize('DataConferido'),
                    width: '15%'
                },
                dataPendente: {
                    title: app.localize('DataPendente'),
                    width: '15%'
                },
                usuarioImpressoId: {
                    title: app.localize('Impresso'),
                    width: '15%'
                },
                dataImpresso: {
                    title: app.localize('DataImpresso'),
                    width: '15%'
                },
                dataEnvioEmail: {
                    title: app.localize('DataEnvioEmail'),
                    width: '15%'
                },
            },
        });

        function getResultados(reload) {
           
            if (reload) {
                _$resultadosTable.jtable('reload');
            } else {

                _$resultadosTable.jtable('load', {
                    filtro: $('#ResultadosTableFilter').val(),
                    tipoAtendimento: $('#tipo-atendimento').val(),
                    empresaId: $('#cbo-empresas').val(),

                    unidadeId: $('#unidade-organizacional-id').val(),
                    pacienteId: $('#paciente-id').val(),
                    medicoId: $('#medico-id').val(),
                    convenioId: $('#convenio-id').val(),

                    startDate: _selectedDateRange.startDate,
                    endDate: _selectedDateRange.endDate,

                    // atendimentoId: $('#atendimento-id').val(),
                });
            }
        }

        function getAtendimentos() {
            _$atendimentosTable.jtable('load', createRequestParams());
        }

        function getExames() {
            _$examesTable.jtable('load', {
                filtro: _$examesFilterForm.val(),
                id: $('#resultado-id-exame').val()
            });
        }

        function deleteResultados(Resultado) {

            abp.message.confirm(
                app.localize('DeleteWarning', Resultado.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _resultadosService.excluir(Resultado)
                            .done(function () {
                                getResultados(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$atendimentosFilterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            if (_selectedDateRange.startDate.format) {
                prms.startDate = _selectedDateRange.startDate.format("YYYY-MM-DD 00:00:00");
                prms.endDate = _selectedDateRange.endDate.format("YYYY-MM-DD 23:59:59");
            }
            else {
                prms.startDate = _selectedDateRange.startDate;
                prms.endDate = _selectedDateRange.endDate;
            }
            console.log(prms);
            return prms;
        }

        $('#filtro-data').on('change', function (e) {
            e.preventDefault();
            switch ($(this).val()) {
                case "Atendimento":
                    $('#date-field-area').show(); //.removeClass('hidden');
                    break;
                case "Internado":
                    $('#date-field-area').hide(); //.addClass('hidden');
                    break;
                default:
                    $('#date-field-area').show()//.removeClass('hidden');
            }
        });

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAtendimentosFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAtendimentosFiltersArea').slideUp();
        });

        $('#CreateNewResultadoButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
            //location.href = '/mpa/resultados/criaroueditarmodal?atendimentoId=' + $('#atendimento-id').val();
        });

        //$('#CreateNewResultadoExameButton').click(function (e) {
        //    e.preventDefault();
        //    _createOrEditResultadoExameModal.open({ resultadoId: $('#resultado-id-exame').val() });
        //});

        $('#ExportarResultadosParaExcelButton').click(function () {
            _resultadosService
                .listarParaExcel({
                    filtro: $('#ResultadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.CriarOuEditarResultadoModalSaved', function () {
            getResultados(true);
        });

        abp.event.on('app.CriarOuEditarResultadoExameModalSaved', function () {
            getExames(true);
        });

        reloadFn();

        $('#ResultadosTableFilter').focus();

        $("#refreshButton").on("click", function () {
            reloadFn();
        });

        function reloadFn() {
            setTimeout(() => {
                getResultados();
                //getAtendimentos();
                getExamesSolicitados();
            }, 0);
        }

        //  aplicarSelect2Padrao();

        $('.select2').css('width', '100%');

        $('.select2').on("change", () => { reloadFn() });

        $("#tipo-atendimento").on("change", () => { reloadFn() });

        //$('body').addClass('page-sidebar-closed');

        //$('.page-sidebar-menu').addClass('page-sidebar-menu-closed');


        var list = [];

        var _$examesSolicitadosTable = $('#examesSolicitadosTable');
        
        _$examesSolicitadosTable.jtable({

            title: app.localize('ExamesSolicitacao'),
            paging: true,
            sorting: true,
            multiSorting: true,
            //selecting: true,
            //selectingCheckboxes: true,
            //multiselect: true,
            edit: false,
            create: false,
            openChildAsAccordion: true,

            actions: {
                listAction: {
                    method: _solicitacaoExameItem.listarExamesLaboratoriaisNaoColetados
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
                //                    _createRegistroPorSolcitacaoModal.open({ ids: list });
                //                    //location.href = 'ContasPagar/CriarOuEditarModal/' + data.record.id
                //                });
                //        }


                //        return $span;
                //    }
                //},
                exames: {
                    title: '',
                    width: '1%',
                    sorting: false,
                    edit: false,
                    create: false,
                    display: function (ateData) {
                        //Create an image that will be used to open child table
                        //var $img = $('<button class="btn btn-default"><i class="fa fa-stethoscope" aria-hidden="true"></i></button>'); //< src="Common/Images/exame_lab.png" title="Exames" />');
                        var $img = $('<img src="Common/Images/exame_lab.png" title="Exames" style="width:20px;" />');
                        //Open child table when user clicks the image
                       
                        $img.on('click', function () {
                            //e.stopPropagation();
                            //e.preventDefault();
                            localStorage["AteId"] = ateData.record.atendimentoId;
                            list = [];
                            _$examesSolicitadosTable.jtable('openChildTable',
                                    $img.closest('tr'),
                                    {
                                        title: ateData.record.pacienteDescricao + ' - Exames',
                                        selecting: true,
                                        selectingCheckboxes: true,
                                        multiselect: true,
                                        actions: {
                                            listAction: {
                                                method: _solicitacaoExameItem.listarAtendimento
                                                //'/mpa/assistenciais/listarAtendimento?id=' + ateData.record.id
                                            }
                                        },
                                        fields: {
                                            id: {
                                                type: 'hidden',
                                                defaultValue: ateData.record.id
                                            },
                                            //id: {
                                            //    key: true,
                                            //    create: false,
                                            //    edit: false,
                                            //    list: false
                                            //},
                                            guiaNumero: {
                                                title: app.localize('GuiaNumero'),
                                                width: '20%',
                                                //options: { '1': 'Home phone', '2': 'Office phone', '3': 'Cell phone' }
                                            },
                                            faturamentoItem: {
                                                title: app.localize('Exame'),
                                                width: '40%',
                                                //options: { '1': 'Home phone', '2': 'Office phone', '3': 'Cell phone' }
                                            },
                                            material: {
                                                title: app.localize('Material'),
                                                width: '40%'
                                            }
                                        },
                                        selectionChanged: function () {

                                            //var $selectedRows = _$examesSolicitadosTable.jtable('selectedRows');
                                            var $selectedRows = $('#examesSolicitadosTable>.jtable-main-container>.jtable>tbody>.jtable-child-row .jtable-row-selected');
                                            list = [];

                                            if ($selectedRows.length > 0) {
                                                //Show selected rows
                                                $('#criarRegistroButton').enable(true);

                                                $selectedRows.each(function () {
                                                    var record = $(this).data('record');
                                                    $('#atendimento-id').val(localStorage['AteId']);
                                                    list.push(record.id);
                                                });

                                            }
                                            else {
                                                $('#atendimento-id').val('');
                                                $('#criarRegistroButton').enable(false);
                                            }
                                            //getExamesSolicitados();
                                            //listarAtendimento();
                                        }
                                    }, function (data) { //opened handler
                                        data.childTable.jtable('load', { id: localStorage["AteId"] });
                                    });
                        });
                        //Return image to show on the person row
                        return $img;
                    }
                },
                codigo: {
                    title: app.localize('Número'),
                    width: '4%',
                    display: function (data) {
                        if (data) {
                            return data.record.id;
                        }
                    }
                },
                tipoAtendimento: {
                    title: app.localize('AtendimentoTipo'),
                    width: '7%',
                    display: function (data) {
                        if (data) {
                            return data.record.tipoAtendimento;
                        }
                    }
                },
                leito: {
                    title: app.localize('Leito'),
                    width: '12%',
                    display: function (data) {
                        return data.record.leito
                    }
                },

                Paciente: {
                    title: app.localize('Paciente'),
                    width: '7%',
                    display: function (data) {
                        if (data) {
                            return data.record.pacienteDescricao;
                        }
                    }
                },

                //Exame: {
                //    title: app.localize('Exame'),
                //    width: '7%',
                //    display: function (data) {
                //        return data.record.exame;
                //    }
                //},


                dataSolicitacao: {
                    title: app.localize('DataSolicitacao'),
                    width: '7%',
                    type: 'date',
                    //format: 'dd/mm/yyyy',
                    display: function (data) {
                        if (data.record.dataSolicitacao) {
                            return moment(data.record.dataSolicitacao).format('L');
                        }
                    }
                },
                dataAtendimento: {
                    title: app.localize('DataAtendimento'),
                    width: '7%',
                    type: 'date',
                    //format: 'dd/mm/yyyy',
                    display: function (data) {
                        if (data.record.dataAtendimento) {
                            return moment(data.record.dataAtendimento).format('L');
                        }
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
        });


        function getExamesSolicitados(reload) {
            if (reload) {
                _$examesSolicitadosTable.jtable('reload');
            } else {
                _$examesSolicitadosTable.jtable('load', createRequestParams());
            }
        }


        $('#criarColetaButton').click(function (e) {
            e.preventDefault();

            if (list.length > 0) {

                //    location.href = '@Url.Action("CriarColetaDeExamesSolicitado", new { ids : list })';

                var jsonList = JSON.stringify(list);


                //  location.href = '/mpa/resultados/CriarColetaDeExamesSolicitado?listIds=' + jsonList;





                //$.ajax({
                //    type: "POST",
                //    url: '/mpa/resultados/CriarColetaDeExamesSolicitado',
                //    data: { ids: list },
                //    success: function (result) {
                //        window.location = result;
                //    },
                //    error: function (xhr, ajaxOptions, thrownError) {
                //    },

                //});




                _createColetaPorSolcitacaoModal.open({ listIds: jsonList });
            }

        });

        selectSW('.selectConvenio', "/api/services/app/convenio/ListarDropdown");
        selectSW('.selectPaciente', "/api/services/app/paciente/ListarDropdown");
        selectSW('.selectMedico', "/api/services/app/medico/ListarDropdown");
        selectSW('.selectUnidade', "/api/services/app/UnidadeOrganizacional/ListarDropdownPorUsuario");
        
        

    });
})();