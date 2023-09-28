(function () {
    $(() => {
        $('body').addClass('page-sidebar-closed');
        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        
        let _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };
        const pickerOptions = app.createDateRangePickerOptions();
        pickerOptions.max = moment().add(7,"days");
        pickerOptions.maxDate = moment().add(7,"days");

        $('.date-custom').daterangepicker(
            $.extend(true, pickerOptions, _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59Z');
                atualizaAgGrids();
            });
        
        const agGrid = $("#grid");
        const laboratorioPainelAppService = abp.services.app.laboratorioPainel;

        const baixaExamesModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LaboratorioPainel/BaixaExames',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Baixas/BaixaModal.js',
            modalClass: 'BaixaModal'
        });

        const addPendenciaModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LaboratorioPainel/AdicionarPendencia',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Pendencias/PendenciaModal.js',
            modalClass: 'PendenciaModal'
        });

        let gridOptions = AgGridHelper.createAgGrid('grid-laboratorio-painel', {
            columnDefs: colunaGrid(),
            rowSelection: 'single',
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                agGrid.css('width', $('.portlet.light').width());
            },
            [AgGridHelper.HOOKS.AFTER_CREATED](hookData) {
                hookData.gridOptions.api.sizeColumnsToFit();
            },
            onSelectionChanged:onSelectionChanged,
            onCellContextMenu: (data) => {
               CreateMenuContext(data);
               $(data.event.target).trigger('contextmenu');
            },
            data: {
                autoInitialLoad: true,
                enablePagination: true,
                callback: laboratorioPainelAppService.retornaPainelData,
                getData() {
                    const currentValue = $(".dashboard-stat-main").filter((index,item) => $(item).data("checked")).data("value");
                    const currentTab = $(".tab-pane.fade.active.in");
                    return getDataFilter(currentValue, currentTab)
                },
            }
        })

        function getDataFilter(tipo, el) {
            if(typeof(_selectedDateRange.startDate) !== "string" ) {
                _selectedDateRange.startDate = _selectedDateRange.startDate.format('YYYY-MM-DDT00:00:00Z');
            }
            if(typeof(_selectedDateRange.endDate) !== "string" ) {
                _selectedDateRange.endDate = _selectedDateRange.endDate.format('YYYY-MM-DDT23:59:59Z');
            }

            return {
                tipo,
                startDate: _selectedDateRange.startDate,
                endDate: _selectedDateRange.endDate,
                unidadesOrganizacionais: getUnidadesOrganizacionais(el),
                labResultadoStatus: getSubItemStatus()
            };

            function getUnidadesOrganizacionais(el) {
                const items = el.find(".dashboard-stat-sub-item");
                const itemsSelected = items.filter(function() { return $(this).data("checked") == true});
                if(itemsSelected.length === 0 ) {
                    return items.filter(function() { return $(this).data("checked") == false})
                        .map(function() {return $(this).data("value")})
                        .get();
                }
                return itemsSelected
                    .map(function() {return $(this).data("value")})
                    .get();
            }

            function getSubItemStatus() {
                const items = $(".dashboard-stat-sub-item-status");
                const itemsSelected = items.filter(function() { return $(this).data("checked") == true});
                if(itemsSelected.length === 0 ) {
                    return items.filter(function() { return $(this).data("checked") == false})
                        .map(function() {return $(this).data("value")})
                        .get();
                }
                return itemsSelected
                    .map(function() {return $(this).data("value")})
                    .get();
            }
        }

        abp.event.on("app.baixaColetaExame", (data) => {
            onVerificaExamesBaixa(data)
        })
        
        atualizaGrid();

        if (!$.cookie("impressora_laboratorio")) {
            const _impressorasModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/Impressoras/ImpressorasLaboratorioModal',
                scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Impressoras/ImpressorasLaboratorioModal.js',
                modalClass: 'ImpressorasLaboratorioModal'
            });
            _impressorasModal.open();
        }

        $("#codigoBarra").on('keypress', function (event) {
            if (event.which === 13) {
                abp.ui.setBusy();
                let codigo = $(this).val();
                if (codigo == null || codigo == "" || codigo == undefined) {
                    abp.notify.error("Não foi possível achar a solicitação correspondente a etiqueta");
                    abp.ui.clearBusy()
                    return;
                }
                laboratorioPainelAppService.buscarPorSolicitacao(codigo).then(res => {
                    
                    if (!res || _.isUndefined(res)) {
                        abp.notify.error("Não foi possível achar a solicitação correspondente a etiqueta");
                        return;
                    }
                    debugger
                    abrirDetalhamentoMessage(res);

                }).always(() => {
                    abp.ui.clearBusy()
                });
            }
        });
        
        function abrirDetalhamentoMessage(res) {
            
            if(res.hasBaixa)
            {
                sweetAlertDetalhamento(cbSweetAlertDetalhamento);
                return;
            }
            
            return abrirDetalhamento(res.solicitacaoId);
            
            function sweetAlertDetalhamento() {
                const opts = {
                    type: 'warning',
                    title: 'Coleta',
                    text: 'Deseja abrir o detalhamento ou dar baixa na coleta?',
                    showCancelButton: true,
                    cancelButtonText: 'Detalhamento',
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: 'Dar Baixa'
                };
                return $.Deferred(function ($dfd) {
                    sweetAlert(opts, function (isConfirmed) {
                        cbSweetAlertDetalhamento && cbSweetAlertDetalhamento(isConfirmed);
                        $dfd.resolve(isConfirmed);
                    });
                });
            }
            
            function cbSweetAlertDetalhamento(isConfirmed) {
                if(isConfirmed) {
                    return darBaixa()
                } else {
                    return abrirDetalhamento(res.solicitacaoId);
                }
                
                function darBaixa() {
                    baixaExamesModal.open({
                        resultadoId: res.resultadoId,
                        resultadoExameIds: res.resultadoExameIds
                    });
                }
                
            }
        }

        function onVerificaExamesBaixa(data) {
            laboratorioPainelAppService.verificaExamesBaixa(data).then(res=> {
                if(res && res.resultadoExameIds && res.resultadoExameIds.length) {
                    addPendenciaModal.open({
                        resultadoId: res.resultadoId,
                        resultadoExameIds: res.resultadoExameIds,
                        blockClose: true
                    });
                }
            })
        }
        
        
        

        $('a.active[data-toggle="tab"]').trigger("click");

        $(".dashboard-stat-main").on("click", function(event) {
            let el = $(this);
            el.data("checked", !el.data("checked"))
            if(el.data("checked")) {
                el.css("opacity",1)
                $(".dashboard-stat-main").not(el).data("checked", false).css("opacity",0.45);
            } else {
                el.css("opacity",0.45).removeClass("active").removeClass("in")
            }
            

            updateGrid()

        })
        
        $(".dashboard-stat-sub-item").on("click", function(event) {
            let el = $(this);
            el.data("checked", !el.data("checked"))
            if(el.data("checked")) {
                el.css("opacity",1)
            } else {
                el.css("opacity",0.45)
            }
            
            updateGrid()
            
        })
        
        function updateGrid() {
            gridOptions.refresh();
        }

        function arrayContains(needle, arrhaystack) {
            return (arrhaystack.indexOf(needle) > -1);
        }

        function onSelectionChanged(e) {
            //$("#updateGrid").trigger('click');
            const selectedRows = e.api.getSelectedRows();
            if (selectedRows != null && selectedRows.length !== 0) {
                abrirDetalhamento(selectedRows[0].id)
            }
        }
        
        function abrirDetalhamento(id) {
            window.open(`/Mpa/LaboratorioPainel/Detalhamento?id=${id}`)
        }
        
        const currentTabPane = "";

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            gridOptions.render(agGrid[0])
            atualizarContadores()
        })

        function atualizarContadores() {
            if(typeof(_selectedDateRange.startDate) !== "string" ) {
                _selectedDateRange.startDate = _selectedDateRange.startDate.format('YYYY-MM-DDT00:00:00Z');
            }
            if(typeof(_selectedDateRange.endDate) !== "string" ) {
                _selectedDateRange.endDate = _selectedDateRange.endDate.format('YYYY-MM-DDT23:59:59Z');
            }
            
            laboratorioPainelAppService.retornaContadores({
                startDate: _selectedDateRange.startDate,
                endDate: _selectedDateRange.endDate,
                unidadesOrganizacionais: $(".tab-pane.fade.active.in")
                    .find(".dashboard-stat-sub-item")
                    .filter(function() { return $(this).data("checked") == true})
                    .map(function() {return $(this).data("value")})
                    .get(),
                labResultadoStatus:
                    $(".dashboard-stat-sub-item-status")
                        .filter(function() { return $(this).data("checked") == true})
                        .map(function() {return $(this).data("value")})
                        .get()
            }).then(res=> {
                $(".urgenteValor").text(res.urgenteValor);
                $(".rotinaValor").text(res.rotinaValor);
                $(".pendenteValor").text(res.pendenteValor);
                $(".culturaValor").text(res.culturaValor);
                $(".emColetaValor").text(res.emColetaValor);
                $(".coletadoValor").text(res.coletadoValor);

                $(".emColetaValor").text(res.emColetaValor);
                
                $(".dashboard-stat-sub-item").find(".valor").text(0);
                
                if(res.unidadesUrgente) {
                    setaContadoresTab($(".tabUrgente"), res.unidadesUrgente);
                }

                if(res.unidadesRotina) {
                    setaContadoresTab($(".tabRotina"), res.unidadesRotina);
                }

                if(res.unidadesPendente) {
                    setaContadoresTab($(".tabPendente"), res.unidadesPendente);
                }

                if(res.unidadesCultura) {
                    setaContadoresTab($(".tabCultura"), res.unidadesCultura);
                }

                atualizaContadoresStatus(res);
                
                function setaContadoresTab(tab,unidades) {
                    let subContadores = tab.find('.dashboard-stat-sub-item');
                    _.forEach(unidades, (unidade) => {
                        subContadores.filter(function () {
                            const el = $(this);
                            if(el.data("value") === unidade.unidadeOrganizacionalId) {
                                el.find(".valor").text(unidade.valor);
                            }
                        })
                    })
                }
                
                function atualizaContadoresStatus(res) {
                    const statusEl = $(".dashboard-stat-sub-item-status");
                    statusEl.find(".valor").text(0);
                    let currentValue = $(".dashboard-stat-main").filter((index,item) => $(item).data("checked")).data("value");
                    currentValue = currentValue[0].toUpperCase() + currentValue.substring(1);
                    let status = res[`status${currentValue}`];
                    if(status && status.length) {
                        _.forEach(status, (statusItem) => {
                            statusEl.filter((index, el) => $(el).data("id") === statusItem.status).find(".valor").text(statusItem.valor);
                        })
                    }
                }
            })
        }


        const gridTabUrgenteOptions = {
            enableBrowserTooltips: true,
            tooltipShowDelay: 10,
            rowData: null,
            columnDefs: null,
            rowSelection: 'single',
            rowClassRules: {
                // apply green to 2008
                'ag-grid-row-red': function (params) { return params.data.protocolo !== null && params.data.protocolo !== ''; },
            },
            //onCellContextMenu: (data) => {
            //    CreateMenuContext(data.event.target);
            //    $(data.event.target).trigger('contextmenu');
            //},
            onCellClicked: (data) => {
                CreateDetalhamento(data);
            },
            onGridReady: () => {
                atualizaRenderAgGrid(gridTabUrgenteOptions);
                atualizaAgGrid(gridTabUrgenteOptions);
            }
            //onCellMouseOver: (data) => {
            //    CreateMenuContext(data);
            //}
        };

        function colunaGrid() {
            return [
                AgGridHelper.columns.base('codigo','Código', {width: 120}),
                AgGridHelper.columns.base('prioridade','Prioridade', {width: 120}),
                // AgGridHelper.columns.base('status','Status Solicitação', {width: 120}),
                AgGridHelper.columns.status('resultadoStatusDescricao','Status Coleta', {width: 120}, {
                cellRendererParams: {
                    corFundo: 'resultadoStatusCorFundo',
                    corFonte: 'resultadoStatusCorFonte'
                }}),
                AgGridHelper.columns.base('protocolo','Protocolo'),
                AgGridHelper.columns.dateTime('dataSolicitacao','Data Solicitação'),
                AgGridHelper.columns.base('nomePaciente','Paciente', {width: 120}),
                AgGridHelper.columns.integer('qtdExames','Qtd Exames', {width: 120}),
                AgGridHelper.columns.dateTime('dataAtendimento','Data Atendimento'),
                AgGridHelper.columns.base('tipoAtendimento','Tipo AtendimentoExames'),
                AgGridHelper.columns.base('leitoNome','Leito'),
                AgGridHelper.columns.base('convenioNome','Convenio'),
            ];
        }

        function createTabRotina() {

        }

        function createTabPendentes() {

        }

        $("#updateGrid").on("click", (event) => {
            atualizaAgGrids()
        });

        function atualizaAgGrids() {
            atualizarContadores();
            gridOptions.render(agGrid[0])

        }

        function atualizaGrid() {
            const updateGrid = $("#updateGrid");
            updateGrid.show();
            if (updateGrid.length) {
                if (!updateGrid.data("time")) {
                    updateGrid.data("time", 60);
                }
                updateGrid.find(".stopTimer").removeClass("hidden");
                updateGrid.find(".playTimer").addClass("hidden");

                if (!updateGrid.data("status")) {
                    updateGrid.data("status", "play");
                }
                if (!updateGrid.data("interval-id")) {
                    updateGrid.data("interval-id", setInterval(doTimer, 1000))
                }
            }

            function doTimer() {
                let time = parseInt(updateGrid.data("time"), 10);
                time -= 1;
                updateGrid.data("time", time);

                if (time === 0) {
                    atualizaAgGrids();
                    updateGrid.data("time", 60);
                    doTimer();
                    return;
                }

                updateGrid.find(".timerContent").html(`<i class="fa fa-clock"></i><span>${time} segundos para atualizar</span>`);

            }
        }
    })
    
})();