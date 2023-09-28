(function ($) {

    app.modals.ListaExamesModal = function () {

        var intervalId = null;
        var varCounter = 0;
        let unique = null;
        let uniqueDataHora = null;
        let _args = null;
        let lista = [];
        var _$subGrupoTable = null;

        $(document).ready(function () {
            $('#ResultadosTableFilter').focus();
            CamposRequeridos();
            document.querySelector('.modal-dialog').style.width = '98%';
        });

        var _evolucaoResultadosService = abp.services.app.evolucaoResultados;

        var _modalManager;
        var _$listaExamesInformationsForm = null;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        var _selectedDateRange = {
            "momentFormatStart": "DD/MM/YYYY",
            "momentFormatEnd": "DD/MM/YYYY",
            startDate: moment().subtract(30, 'd').startOf('day'),
            endDate: moment().startOf('day').endOf('day')
        };

        createDateRangePicker($('#date-field'), _selectedDateRange);



        $('#date-field').click(function () {
            document.querySelectorAll('.daterangepicker')[document.querySelectorAll('.daterangepicker').length - 1].style.display = 'block';
        });

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

        $('input[name="ambulatorioEmergencia"]').on('click', function (e) {
            e.stopPropagation();
            if ($(this).attr('id') == 'rdo-is-ambulatorio-emergencia') {
                if (document.querySelector('#rdo-is-ambulatorio-emergencia').value == 'true') {
                    document.querySelector('#rdo-is-ambulatorio-emergencia').value = 'false';
                } else {
                    document.querySelector('#rdo-is-ambulatorio-emergencia').checked = false;
                    document.querySelector('#rdo-is-ambulatorio-emergencia').value = 'true';
                }
            }
        });

        $('input[name="internacao"]').on('click', function (e) {
            e.stopPropagation();
            if ($(this).attr('id') == 'rdo-is-internacao') {
                if (document.querySelector('#rdo-is-internacao').value == 'true') {
                    document.querySelector('#rdo-is-internacao').value = 'false';
                } else {
                    document.querySelector('#rdo-is-internacao').checked = false;
                    document.querySelector('#rdo-is-internacao').value = 'true';
                }
            }
        });

        $('input[name="DesseAtendimentoOuPaciente"]').on('change', function (e) {
            e.stopPropagation();
            if ($(this).attr('id') == 'rdo-is-paciente') {
                $('#is-atendimento').val('false');
                $('#is-paciente').val('true');
            }
            else {
                $('#is-atendimento').val('true');
                $('#is-paciente').val('false');
            }
        });

        let gridOptionsColetas = {
            enableBrowserTooltips: false,
            components: {
                customTooltip: CustomTooltip,
            },
            tooltipShowDelay: 0,
            rowData: null,
            columnDefs: null
        };

        let gridOptionsCulturas = {
            enableBrowserTooltips: false,
            components: {
                customTooltip: CustomTooltip,
            },
            tooltipShowDelay: 0,
            rowData: null,
            columnDefs: null
        };
        

        function showBsModal() {
            setTimeout(() => { $('#comparativoEvolucaoResultadoTableFilter').get(0).focus(); }, 1)
            setTimeout(() => {
                obterResultados();
            }, 1000);
        }
        this.init = function (modalManager) {

            _args = modalManager.getArgs();
            _modalManager = modalManager;

            modalManager.onClose(() => {
                $('#getResultadosButton').off("click");
                if (gridOptionsColetas.api && gridOptionsColetas.api.destroy) {
                    gridOptionsColetas.api.destroy();
                }

                if (gridOptionsCulturas.api && gridOptionsCulturas.api.destroy) {
                    gridOptionsCulturas.api.destroy();
                }
            });

            // create the grid passing in the div to use together with the columns & data we want to use


            if (_args.id != undefined && _args.atendimentoId != undefined) {
                document.querySelector('#rdo-is-paciente').checked = true;
                document.querySelector('#rdo-is-ambulatorio-emergencia').checked = true;
                document.querySelector('#rdo-is-internacao').checked = true;
            }

            _$listaExamesInformationsForm = _modalManager.getModal().find('form[name=listaExamesInformationsForm]');
            _$listaExamesInformationsForm.validate();

            showBsModal();
        };

        function createDateRangePicker(inputTag, selectedDateRange) {
            var baseOptions = app.createDateRangePickerOptions();
            var options = $.extend(true, baseOptions, selectedDateRange);
            $(inputTag).daterangepicker(options).on('apply.daterangepicker', function (ev, picker) {
                ev.stopPropagation();
                if (!options["singleDatePicker"]) {
                    //$(this).val(picker.startDate.format(options["momentFormatStart"]) + ' - ' + picker.endDate.format(options["momentFormatEnd"]));

                    selectedDateRange.startDate = picker.startDate.format('YYYY-MM-DDT00:00:00Z');
                    selectedDateRange.endDate = picker.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                }
                else {

                    $(this).val(picker.startDate.format(options["momentFormatStart"]));
                    if (options["timePicker"]) {
                        selectedDateRange.startDate = picker.startDate.format('YYYY-MM-DDTHH:mm:ssZ');
                    }
                    else {
                        selectedDateRange.startDate = picker.startDate.format('YYYY-MM-DDT00:00:00Z');
                    }

                }

                obterResultados();
            }).on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
            });
        }


        function decodeEntities(encodedString) {
            var textArea = document.createElement('textarea');
            textArea.innerHTML = encodedString;
            return textArea.value;
        }

        function obterResultados() {
            _args['filtro'] = $('#comparativoEvolucaoResultadoTableFilter').val();
            _args['dateStart'] = _selectedDateRange.startDate;
            _args['dateEnd'] = _selectedDateRange.endDate;
            _args['isAmbulatorioEmergencia'] = document.querySelector('#rdo-is-ambulatorio-emergencia').checked;
            _args['isInternacao'] = document.querySelector('#rdo-is-internacao').checked;
            _args['isDesseAtendimento'] = document.querySelector('#rdo-is-atendimento').checked;
            _args['isDessePaciente'] = document.querySelector('#rdo-is-paciente').checked;

            _evolucaoResultadosService.listaEvolucaoResultado(_args)
                .done(function (res) {
                    document.querySelector('#pessoaPaciente').innerText = decodeEntities(_args.nomePaciente);
                    createColetas(res.coletas, gridOptionsColetas);
                    createCulturas(res.culturas,gridOptionsCulturas);
                    
                    
                    function createColetas(resultColetas, gridOptions) {
                        if (gridOptions.api && gridOptions.api.destroy) {
                            gridOptions.api.destroy();
                        }
                        setTimeout(function () {
                            createTable(resultColetas, gridOptions, '#coletasTable')
                        },1000);
                    }
                    
                    function createCulturas(resultCulturas, gridOptions) {
                        if (gridOptions.api && gridOptions.api.destroy) {
                            gridOptions.api.destroy();
                        }
                        setTimeout(function () {
                            createTable(resultCulturas, gridOptions, '#culturasTable')
                        },1000);
                    }
                    
                    function createTable(data, gridOptions , gridEl) {
                        let lista = [];
                        lista = data;
                        let datas = [];

                        _.forEach(lista, function (item, index) {
                            if (item.resultados && item.resultados.length) {
                                _.forEach(item.resultados,
                                    function (itemResultado, index) {
                                        if (!_.includes(datas, itemResultado.dataColeta)) {
                                            datas.push(itemResultado.dataColeta);
                                        }
                                    }
                                );

                            }
                        });


                        datas = datas.sort((a, b) => new moment(b) - new moment(a));

                        let columnDefs = [
                            {
                                headerName: 'Nome do exame',
                                field: "itemDescricao",
                                pinned: 'left', lockPinned: true, resizable: true,
                            },
                            {
                                headerName: 'Informação',
                                field: "itemInfo",
                                pinned: 'left', lockPinned: true, resizable: true,
                            },
                            {
                                headerName: 'Valor de referência',
                                field: "referencia",
                                pinned: 'left', lockPinned: true, resizable: true,
                            }
                        ];

                        _.forEach(datas, (item, index) => {
                            columnDefs.push({
                                headerName: moment(item).format("DD/MM/YY HH:mm"),
                                tooltipComponent: 'customTooltip',
                                tooltipField: 'referencia',
                                headerClass: "customHeader",
                                resizable:true,
                                tooltipComponentParams: {color: '#ececec'},
                                cellStyle: (params) => {
                                    let data = params.data;
                                    const dataColuna = item;
                                    if (data.resultados && data.resultados.length) {
                                        let resultadoItem = _.find(data.resultados, (o) => {
                                            return moment(o.dataColeta).isSame(moment(dataColuna));
                                        });
                                        if (resultadoItem && _.isArray(resultadoItem)) {
                                            resultadoItem = resultadoItem[0];
                                        }

                                        if (resultadoItem) {
                                            params.data.resultadoItem = resultadoItem;
                                            if (!resultadoItem.numerico) {
                                                return;
                                            } else {
                                                return {
                                                    color: resultadoItem.corTexto,
                                                    backgroundColor: resultadoItem.corFundo
                                                };
                                            }
                                        }
                                    }
                                    return;
                                },
                                valueGetter: (params) => {
                                    let data = params.data;
                                    const dataColuna = item;
                                    if (data.resultados && data.resultados.length) {
                                        let resultadoItem = _.find(data.resultados, (o) => {
                                            return moment(o.dataColeta).isSame(moment(dataColuna));
                                        });
                                        if (resultadoItem && _.isArray(resultadoItem)) {
                                            resultadoItem = resultadoItem[0];
                                        }

                                        if (resultadoItem) {
                                            params.data.resultadoItem = resultadoItem;
                                            if (!resultadoItem.numerico) {
                                                return resultadoItem.resultado;
                                            } else {
                                                return resultadoItem.resultado;
                                            }
                                        }
                                    }
                                    return null;
                                }
                            });
                        });
                        gridOptions.rowData = null;
                        gridOptions.columnDefs = columnDefs;
                        const eGridDiv = document.querySelector(gridEl);

                        gridOptions.onGridReady = () => {
                            updateLayoutData(gridOptions,eGridDiv,lista);
                        }

                        

                        let grid = new agGrid.Grid(eGridDiv, gridOptions);

                        App.stopPageLoading();
                        document.querySelector('.loadingCommon').style.display = 'none';
                        return;
                    }
                });
        }
        
        function updateLayoutData(gridOptions, eGridDiv, lista){
            setTimeout(() => {
                const params = {force: true};
                gridOptions.api.refreshCells(params);
                eGridDiv.style.width = $('#grupoContaAdministrativaInformationsTab').width();
                eGridDiv.style.width = '100%';
                gridOptions.api.setRowData(lista);
                gridOptions.api.doLayout()
                gridOptions.api.sizeColumnsToFit()
                
            },1000);
        }

        function updateLayout(gridOptions, eGridDiv){
            setTimeout(() => {
                const params = {force: true};
                gridOptions.api.refreshCells(params);
                eGridDiv.style.width = $('#grupoContaAdministrativaInformationsTab').width();
                eGridDiv.style.width = '100%';
                gridOptions.api.doLayout()
                gridOptions.api.sizeColumnsToFit()
                
            },1000);
        }

        $('#getResultadosButton').click(function (e) {
            e.preventDefault();
            obterResultados();
        });

        $(".evolucao-resultados").find('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            if ($(e.target).data("tabpane") === "coletas") {
                updateLayout(gridOptionsColetas,document.querySelector("#coletasTable"))
            } else {
                updateLayout(gridOptionsCulturas,document.querySelector("#culturasTable"))
            } 
        })

        //getResultados();

        $('#ResultadosTableFilter').focus();

        //selectSW('.selectUnidadeOrganizacional', "/api/services/app/unidadeOrganizacional/ListarDropdown");



    };


})(jQuery);