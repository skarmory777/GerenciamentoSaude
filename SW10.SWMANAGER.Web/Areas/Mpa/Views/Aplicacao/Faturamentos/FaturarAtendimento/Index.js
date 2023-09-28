(function () {
    $(function () {
        $('body').addClass('page-sidebar-closed');
        
        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        const gridPrincipal = $('.grid-principal');
        const gridContaMedicas = $('.grid-conta-medica');
        const gridGuias = $('.grid-guias');
        const gridOcorrencia = $('.grid-ocorrencia');
        const gridItensFaturamento = $('.grid-itens-faturamento');
        const relationCollapse = $('#relation-collapse');
        const gridCallback = null;
        const faturamentoAppService = abp.services.app.faturarAtendimento;
        const ocorrenciaAppService = abp.services.app.ocorrencia;

        let _selectedDateRange = {
            startDate: moment().add(-1, "days").startOf('day'),
            endDate: moment().endOf('day')
        };
        const pickerOptions = app.createDateRangePickerOptions();

        const _createOrEditContaMedicaModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturarAtendimento/CriarOuEditarContaMedicaModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ContaMedica/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarContaMedicaModal',
            focusFunction:()=> {
                $.fn.modal.Constructor.prototype.enforceFocus = function () {};
            }
        });

        let gridPrincipalOptions = AgGridHelper.createAgGrid('grid-principal-atendimentos',
        {
            rowSelection: 'single',
            onSelectionChanged: onSelectionGridPrincipalChanged,
            columnDefs: defColumnsPrincipal(),
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridPrincipal.css('width', $('.portlet.light').width());
            },
            data: {
                autoInitialLoad: true,
                enablePagination: true,
                callback: faturamentoAppService.listarAtendimentoFaturamento,
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
        
        const gridContasMedicasOptions = AgGridHelper.createAgGrid('grid-contas-medicas', {
            columnDefs: defColumnsContasMedicas(),
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridContaMedicas.css('width',$('.portlet.light').width());
            },
            data: {
                callback: faturamentoAppService.listarContaMedica,
                getData() {
                    const rows = gridPrincipalOptions.getSelectedRows();
                    let atendimentoId = null;
                    if (rows && rows.length) {
                        atendimentoId = rows[0].id
                    }
                    return {atendimentoId}
                },
            },
            editarItem(data) {
                const params = {
                    atendimentoId: getAtendimentoId(),
                    contaMedicaId: data.id
                };
                _createOrEditContaMedicaModal.open(params)
            }
        });

        // const gridGuiasOptions = AgGridHelper.createAgGrid('grid-guias-contas-medicas', {
        //     columnDefs:defColumnsGuias(),
        //     beforeCreate(el, options) {
        //         gridGuias.css('width',$('.portlet.light').width());
        //     },
        // });
        

        const gridOcorrenciasOptions = AgGridHelper.createAgGrid('grid-ocorrencia-contas-medicas', {
            columnDefs: defColumnsOcorrencias(),
            data: {
                callback: ocorrenciaAppService.listar,
                enablePagination: true,
                getData() {
                    return {
                        sourceModel: "atendimento",
                        sourceId: getAtendimentoId(),
                        relationModel: "atendimento",
                        relationId: getAtendimentoId()
                    }
                }
            },
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridOcorrencia.css('width',$('.portlet.light').width());
            },
            [AgGridHelper.HOOKS.AFTER_CREATED](hookData) {
                hookData.gridOptions.api.sizeColumnsToFit();
            },
        });

        gridOcorrenciasOptions.render(gridOcorrencia[0])

        const gridItensFaturamentoOptions = AgGridHelper.createAgGrid('grid-itens-faturamento-contas-medicas',{
            
            columnDefs: defColumnsItensFaturamento(),
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridItensFaturamento.css('width',$('.portlet.light').width());
            },
            [AgGridHelper.HOOKS.AFTER_CREATED](hookData) {
                hookData.gridOptions.api.sizeColumnsToFit();
            },
            data: {
                callback: faturamentoAppService.listarFaturamentoItem,
                enablePagination: true,
                getData() {
                    const rows = gridPrincipalOptions.getSelectedRows();
                    let atendimentoId = null;
                    if (rows && rows.length) {
                        atendimentoId = rows[0].id
                    }
                    return {atendimentoId}
                }
            }
        });
        
        gridItensFaturamentoOptions.render(gridItensFaturamento[0]);

        
        
        $(".tooltip-info").tooltip({
            html:true, 
            container: 'body', 
            template: `<div class='tooltip' role='tooltip'><div class='tooltip-arrow'></div><div class='tooltip-inner' style='width:100% !important;max-width: 350px;'></div></div>`
        });

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

        relationCollapse.collapse({toggle: false})
        updatePrincipalSize(false);
        

        tabChange("#tab-conta-medica");
        
        function defColumnsPrincipal() {
            const disableFilterAndMenu = { filter: true, suppressMenu: false, sortable: true,resizable:true};
            return [
                AgGridHelper.columns.action({enableEdit:true}),
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
        
        function defColumnsContasMedicas() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                AgGridHelper.columns.action({
                    enableEdit:true,
                    enableDelete:true,
                    customAction: [{
                            title: 'Entrar Conta Medica',
                            action: onEnterContaMedica,
                            icon: 'fas fa-file-medical',
                            class:'btn-info'
                    }]},disableFilterAndMenu,{width: 150}),
                AgGridHelper.columns.status('fatContaStatus', app.localize('Status'), disableFilterAndMenu, {
                    width: 175,
                    cellRendererParams: {
                        corFundo: 'fatContaStatusCor',
                        corTexto: 'FFFFFF'
                    } }),
                AgGridHelper.columns.base('codigo',app.localize('Codigo'), disableFilterAndMenu,{ width: 50 }),
                AgGridHelper.columns.dateTime('dataInicio',app.localize('Inicio'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataFim',app.localize('Fim'), _.clone(disableFilterAndMenu)),
                AgGridHelper.columns.base('diasFaturados',app.localize('Dias Faturados'), disableFilterAndMenu,{ width: 100 }),
                AgGridHelper.columns.base('convenio',app.localize('Convenio'), disableFilterAndMenu),
                AgGridHelper.columns.base('plano',app.localize('Plano'), disableFilterAndMenu),
                AgGridHelper.columns.base('tipoDeGuia',app.localize('Tipo de Guia'), disableFilterAndMenu),
                AgGridHelper.columns.base('matricula',app.localize('Matricula'), disableFilterAndMenu),
                AgGridHelper.columns.base('numeroDeGuia',app.localize('Numero de Guia'), disableFilterAndMenu)
            ];
        }
        
        function defColumnsGuias() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                AgGridHelper.columns.base('id',app.localize('Id'), disableFilterAndMenu,{ width: 50 }),
                AgGridHelper.columns.dateTime('dataInicio',app.localize('Inicio'),disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataFim',app.localize('Fim'), disableFilterAndMenu),
                AgGridHelper.columns.base('convenio',app.localize('Convenio'),disableFilterAndMenu),
                AgGridHelper.columns.base('plano',app.localize('Plano'), disableFilterAndMenu),
                AgGridHelper.columns.base('tipoGuia',app.localize('Tipo de Guia'), disableFilterAndMenu,{ width: 200 }),
                AgGridHelper.columns.base('matricula',app.localize('Matricula'), disableFilterAndMenu),
                AgGridHelper.columns.base('numeroGuia',app.localize('Numero Guia'), disableFilterAndMenu),
            ]
        }
        
        function defColumnsOcorrencias() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                AgGridHelper.columns.action(),
                AgGridHelper.columns.dateTime('data',app.localize('Data'), disableFilterAndMenu),
                AgGridHelper.columns.base('tipoOcorrenciaDescricao',app.localize('Tipo'),  disableFilterAndMenu),
                AgGridHelper.columns.base('subTipoOcorrenciaDescricao',app.localize('Sub Tipo'),  disableFilterAndMenu),
                AgGridHelper.columns.base('origem',app.localize('Origem'),  disableFilterAndMenu),
                AgGridHelper.columns.base('texto',app.localize('Ocorrencia'),  disableFilterAndMenu),
                AgGridHelper.columns.base('usuario',app.localize('Usuario'),  disableFilterAndMenu),
                AgGridHelper.columns.boolean('isSistema',app.localize('Sistema?'),  disableFilterAndMenu),
            ];
        }
        
        function defColumnsItensFaturamento() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                //AgGridHelper.columns.action(),
                AgGridHelper.columns.dateTime('data',app.localize('Data'),  disableFilterAndMenu),
                AgGridHelper.columns.base('descricao',app.localize('Descrição'),  disableFilterAndMenu),
                AgGridHelper.columns.base('quantidade',app.localize('Quantidade'),  disableFilterAndMenu)
            ]
        }
        
        function onSelectionGridPrincipalChanged(e) {
            const selectedRow = gridPrincipalOptions.getSelectedRows();
            if(selectedRow != null || selectedRow.length === 0) {
                relationCollapse.collapse('show');
                $('.nav-tabs a[data-toggle="tab"]').on('shown.bs.tab', changeTabs);
                updatePrincipalSize(true)
            }
            else {
                relationCollapse.collapse('hide');
                $('.nav-tabs a[data-toggle="tab"]').off('shown.bs.tab', changeTabs);
                updatePrincipalSize(false)
            }
        }
        
        function updatePrincipalSize(show) {
            const principalContent = $(".row-grid-content");
            if(!show) {
                gridPrincipal.css("height", $(window).height() - principalContent.position().top - 100 - $("page-header.navbar.navbar-fixed-top").height());
            }
            else {
                gridPrincipal.css("height", $(window).height() - principalContent.position().top - parseInt(relationCollapse.css('max-height'))  - 100 - $("page-header.navbar.navbar-fixed-top").height());
                setTimeout(()=> {
                    debugger;
                    const nodes = gridPrincipalOptions.getApi().getSelectedNodes();
                    gridPrincipalOptions.getApi().ensureNodeVisible(nodes[0],'middle');
                    
                    gridContasMedicasOptions.render(gridContaMedicas[0]);
                    //gridGuiasOptions.render(gridGuias[0]);
                    gridOcorrenciasOptions.render(gridOcorrencia[0]);
                    gridItensFaturamentoOptions.render(gridItensFaturamento[0]);
                },100)
                
            }
        }
        
        function getAtendimentoId() {
            const rows = gridPrincipalOptions.getSelectedRows();
            if(rows.length) {
                return rows[0].id;
            }
            return 0;
        }
        function tabChange(target) {
            const tabActionItem = $(".add-tab-button");
            switch(target)
            {
                case "#tab-conta-medica":
                {
                    tabActionItem.show();
                    tabActionItem.off('click').on('click',onAddContaMedica);
                    tabActionItem.html("<i class='fas fa-file-invoice'></i> Adicionar Conta Medica");
                    gridContasMedicasOptions.render(gridContaMedicas[0]);
                    break;
                }

                case "#tab-guias":
                {
                    tabActionItem.hide();
                    //gridGuiasOptions.render(gridGuias[0]);
                    break;
                }

                case "#tab-ocorrencias":
                {
                    tabActionItem.show();
                    tabActionItem.off('click').on('click',onAddOcorrencias);
                    tabActionItem.html("<i class='fas fa-laptop-medical'></i> Adicionar Ocorrencias");
                    gridOcorrenciasOptions.render(gridOcorrencia[0]);
                    break;
                }
                case "#tab-itens-faturamento":
                {
                    tabActionItem.hide();
                    gridItensFaturamentoOptions.render(gridItensFaturamento[0]);
                    break;
                }
            }
        }
        function changeTabs (e) {
            console.log($(e.target).attr("href"));
            tabChange($(e.target).attr("href"))
        }
        
        
        function onAddContaMedica(e) {
            const atendimentoId = getAtendimentoId();
            if(atendimentoId !== 0) {
                $(this).buttonBusy(true);
                _createOrEditContaMedicaModal.open({atendimentoId});
                $(this).buttonBusy(false);
            }
        }

        function onAddOcorrencias(e) {
            $(this).buttonBusy(true);
        }
        
        function onEnterContaMedica(e,data) {
            const urlData = {
                atendimentoId: getAtendimentoId(),
                contaMedicaId: data.id
            };
            window.open(`/Mpa/FaturarAtendimento/ContaMedica?${$.param(urlData)}`);
        }
        
        
        function onIndexFaturarAtendimentoReload() {
            const atendimentoId = getAtendimentoId();
            if(atendimentoId === 0) {
                gridPrincipalOptions.render(gridPrincipal[0])
            } else {
                gridContasMedicasOptions.render(gridContaMedicas[0]);
                // gridGuiasOptions.render(gridGuias[0]);
                gridOcorrenciasOptions.render(gridOcorrencia[0]);
                gridItensFaturamentoOptions.render(gridItensFaturamento[0]);
            }
        }
        
    });
})();