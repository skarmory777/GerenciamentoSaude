(function () {
    $(function () {
        $('body').addClass('page-sidebar-closed');
        
        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        const gridPrincipal = $('.grid-principal');
        const gridOcorrencia = $('.grid-ocorrencia');
        const relationCollapse = $('#relation-collapse');
        const faturamentoAppService = abp.services.app.faturarAtendimento;
        const ocorrenciaAppService = abp.services.app.ocorrencia;

        let _selectedDateRange = {
            startDate: moment().add(-1, "days").startOf('day'),
            endDate: moment().endOf('day')
        };
        const pickerOptions = app.createDateRangePickerOptions();


        let gridPrincipalOptions = AgGridHelper.createAgGrid('grid-principal-atendimentos',
        {
                rowSelection: 'single',
                columnDefs: defColumnsPrincipal(),
                [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                    gridPrincipal.css('width', $('.portlet.light').width());
                },
                data: {
                    autoInitialLoad: true,
                    enablePagination: true,
                    callback: faturamentoAppService.listarAtendimentoFaturamentoAuditoriaInterna,
                    getData() {
                        if(typeof(_selectedDateRange.startDate) !== "string" ) {
                            _selectedDateRange.startDate = _selectedDateRange.startDate.format('YYYY-MM-DDT00:00:00Z');
                        }
                        if(typeof(_selectedDateRange.endDate) !== "string" ) {
                            _selectedDateRange.endDate = _selectedDateRange.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                        }

                        const tipoInternacaoValue = $(".chk-tipo-internacao:checked").map((x, el) => $(el).val()).get();
                        let tipoInternacao = null;
                        if (tipoInternacaoValue.some((x)=> x == "internacao") && tipoInternacaoValue.length != 2) {
                            tipoInternacao = 0;
                        } else if (tipoInternacaoValue.some((x) => x == "emergencia") && tipoInternacaoValue.length != 2) {
                            tipoInternacao = 1;
                        }
                        return {
                            empresaId : $(".selectEmpresa").val(),
                            convenioId : $(".selectConvenio").val(),
                            pacienteId : $(".selectPaciente").val(),
                            tipoInternacao,
                            periodo: $(".filtro-periodo:checked").val(),
                            startDate: _selectedDateRange.startDate,
                            endDate: _selectedDateRange.endDate,
                        };
                    },
                }
            });
        gridPrincipalOptions.render(gridPrincipal[0]);
        
        //const gridOcorrenciasOptions = AgGridHelper.createAgGrid('grid-ocorrencia-contas-medicas', {
        //    columnDefs: defColumnsOcorrencias(),
        //    data: {
        //        callback: ocorrenciaAppService.listar,
        //        enablePagination: true,
        //        getData() {
        //            return {
        //                sourceModel: "atendimento",
        //                sourceId: getAtendimentoId(),
        //                relationModel: "atendimento",
        //                relationId: getAtendimentoId()
        //            }
        //        }
        //    },
        //    [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
        //        gridOcorrencia.css('width',$('.portlet.light').width());
        //    },
        //    [AgGridHelper.HOOKS.AFTER_CREATED](hookData) {
        //        hookData.gridOptions.api.sizeColumnsToFit();
        //    },
        //});

        //gridOcorrenciasOptions.render(gridOcorrencia[0])

        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('.selectPaciente', "/api/services/app/paciente/ListarDropdown");
        selectSW('.selectConvenio','/api/services/app/convenio/ListarDropdown');
        
        
        $(".select2").on("change", (event) => {
            gridPrincipalOptions.refresh();
        })

        
        $('.date-custom').daterangepicker(
            $.extend(true, pickerOptions, _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                
                gridPrincipalOptions.refresh();
            });

        $(".chk-tipo-internacao").on("change", () => {
            gridPrincipalOptions.refresh();
        })
        
        $(".filtro-periodo").on("click",function(e) {
            e.stopImmediatePropagation();
           const current = $(this);
           const customDate = $(".date-custom");
           if(current.attr("checked")) {
               if(current.val() === "customizado") {
                   customDate.removeClass("hidden");
               }
               else {
                   customDate.addClass("hidden");                   
               }
           }
            $(".filtro-periodo").not(current).removeAttr("checked");

            gridPrincipalOptions.refresh();
        });
        
        abp.event.on("app.indexFaturarAtendimentoReload", onIndexFaturarAtendimentoReload)

        
        function defColumnsPrincipal() {
            const disableFilterAndMenu = { filter: true, suppressMenu: false, sortable: true,resizable:true};
            return [
                AgGridHelper.columns.action({
                    enableEdit: false,
                    customAction: [{
                        title: 'Entrar Conta Medica',
                        action: onEnterContaMedica,
                        icon: 'fas fa-file-medical',
                        class: 'btn-info'
                    }]
                }),
                AgGridHelper.columns.status('ateStatus',app.localize('Status Atendimento'), disableFilterAndMenu,{
                    cellRendererParams: { 
                        corFundo: 'ateCorFundo', 
                        corTexto: 'ateCorTexto'
                    }
                }),
                AgGridHelper.columns.status('fatStatus', app.localize('Status Faturamento'), disableFilterAndMenu, {
                    cellRendererParams: {
                        corFundo: 'fatContaStatusCor',
                        corTexto: 'FFFFFF'
                    }
                }),
                AgGridHelper.columns.base('codigoAtendimento',app.localize('Codigo Atendimento'), disableFilterAndMenu,{ width: 100 }),
                AgGridHelper.columns.base('codigoPaciente',app.localize('Codigo Paciente'), disableFilterAndMenu,{ width: 100 }),
                AgGridHelper.columns.base('nomeCompleto',app.localize('Paciente'), disableFilterAndMenu, {
                    filter: 'agTextColumnFilter',
                    filterParams: {
                        buttons: ['reset', 'apply'],
                    },
                }),
                AgGridHelper.columns.base('convenio',app.localize('Convenio'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataRegistro',app.localize('Data Atendimento'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataAlta',app.localize('Data Alta'), disableFilterAndMenu),
                AgGridHelper.columns.boolean('isParcial',app.localize('Possui Parcial?'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataInicio',app.localize('Última Parcial'), disableFilterAndMenu),
                AgGridHelper.columns.base('dataParcial',app.localize('Dias Pendentes Parcial'), disableFilterAndMenu,{ width: 75 })
            ];
        }

        function defColumnsOcorrencias() {
            const disableFilterAndMenu = { filter: false, suppressMenu: true };
            return [
                AgGridHelper.columns.action(),
                AgGridHelper.columns.dateTime('data', app.localize('Data'), disableFilterAndMenu),
                AgGridHelper.columns.base('tipoOcorrenciaDescricao', app.localize('Tipo'), disableFilterAndMenu),
                AgGridHelper.columns.base('subTipoOcorrenciaDescricao', app.localize('Sub Tipo'), disableFilterAndMenu),
                AgGridHelper.columns.base('origem', app.localize('Origem'), disableFilterAndMenu),
                AgGridHelper.columns.base('texto', app.localize('Ocorrencia'), disableFilterAndMenu),
                AgGridHelper.columns.base('usuario', app.localize('Usuario'), disableFilterAndMenu),
                AgGridHelper.columns.boolean('isSistema', app.localize('Sistema?'), disableFilterAndMenu),
            ];
        }
        
        
        function getAtendimentoId() {
            const rows = gridPrincipalOptions.getSelectedRows();
            if(rows.length) {
                return rows[0].id;
            }
            return 0;
        }

        function onAddOcorrencias(e) {
            $(this).buttonBusy(true);
        }
        
        function onEnterContaMedica(e,data) {
            const urlData = {
                atendimentoId: data.id,
                contaMedicaId: data.fatContaAtendimentoId
            };
            window.open(`/Mpa/FaturarAtendimento/ContaMedica?${$.param(urlData)}`);
        }
        
        
        function onIndexFaturarAtendimentoReload() {
            const atendimentoId = getAtendimentoId();
            if(atendimentoId === 0) {
                gridPrincipalOptions.render(gridPrincipal[0])
            }
        }
        
    });
})();