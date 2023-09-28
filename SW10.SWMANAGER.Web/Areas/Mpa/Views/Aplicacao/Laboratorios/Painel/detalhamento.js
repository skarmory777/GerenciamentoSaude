(function () {
    $(() => {
        const evolucaoResultadosService = abp.services.app.evolucaoResultados;
        const solicitacaoExameService = abp.services.app.solicitacaoExame;
        const resultadoAppService = abp.services.app.resultado;
        const resultadoExameAppService = abp.services.app.resultadoExame;
        const eGridColetaDiv = document.querySelector('#gridDetalhamentoColeta');
        const eGridColetaExameDiv = $('#gridDetalhamentoColetaExame');
        const eGridColetaResultadoDiv = document.querySelector('#gridDetalhamentoColetaResultado');
        const gridDetalhamentoSolicitacaoExamesDiv = $('#gridDetalhamentoExames');
        const laboratorioPainelAppService = abp.services.app.laboratorioPainel;
        const createOrEditColetaPorSolicitacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Resultados/CriarOuEditarModalPorSolicitacao',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Resultados/_CriarOuEditarModalPorSolicitacao.js',
            modalClass: 'CriarOuEditarResultadoModal'
        });
        const createOrEditColetaModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Resultados/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Resultados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarResultadoModal'
        });
        const imprimirExamesColetaModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LaboratorioPainel/ImprimirEtiquetas',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Etiquetas/ImprimirEtiquetaModal.js',
            modalClass: 'ImprimirEtiquetaModal'
        });
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

        const detalhamentoExameModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LaboratorioPainel/DetalhamentoExame',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/DetalhamentoExame/DetalhamentoExameModal.js',
            modalClass: 'DetalhamentoExameModal'
        });
        
        const imprimirEtiquetasPorSolicitacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/LaboratorioPainel/ImprimirEtiquetasPorSolicitacao',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Etiquetas/ImprimirEtiquetaModal.js',
            modalClass: 'ImprimirEtiquetaModal'
        });
        const _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });
        //context.init({ preventDoubleContext: false });
        $('body').addClass('page-sidebar-closed');
        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
           if ($(e.target).data("tabpane") === "tabColetaExame") {
                gridDetalhamentoColetaExameOptions.render(eGridColetaExameDiv[0])
            } else if ($(e.target).data("tabpane") === "tabColetaResultado") {
                createTabColetaResultados()
            } else if ($(e.target).data("tabpane") === "tabSolicitacoes") {
                gridDetalhamentoSolicitacaoExamesOptions.render(gridDetalhamentoSolicitacaoExamesDiv[0])
            }
        })

        init();
        
        $(".modal-dialog").addClass("w-90");
        let selectedRow = null;

        let gridDetalhamentoSolicitacaoExamesOptions = AgGridHelper.createAgGrid('grid-detalhamento-solicitacao-exames', {
            columnDefs: createDetalhamentoSolicitacaoExameColumns(),
            rowSelection: 'multiple',
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridDetalhamentoSolicitacaoExamesDiv.css('width', $('.portlet.light').width());
            },
            [AgGridHelper.HOOKS.AFTER_CREATED](hookData) {
                hookData.gridOptions.api.sizeColumnsToFit();
            },
            onSelectionChanged: () => {
            },
            onRowDataChanged: () => {
                gridDetalhamentoSolicitacaoExamesOptions.getApi().forEachLeafNode((node) => {
                    if (node.data.statusId == 1 || node.data.statusId == ""|| node.data.statusId == null) {
                        node.setSelected(true);
                    }
                });
            },
            enableBrowserTooltips: false,
            components: {
                medicoSolicitanteTooltip: medicoSolicitanteTooltip,
                pendenciaTooltip:pendenciaTooltip,
                exameTooltip:exameTooltip
            },
            tooltipShowDelay: 0,
            data: {
                autoInitialLoad: true,
                enablePagination: true,
                callback: solicitacaoExameService.retornaExamesPorSolicitacaoId,
                getData() {
                    return {
                        id: $("input[name='SolicitacaoExameId']").val()
                    }
                },
            }
        })

        let gridDetalhamentoColetaExameOptions = AgGridHelper.createAgGrid('grid-detalhamento-coleta-exame', {
            columnDefs: createDetalhamentoColetaExameColumns(),
            rowSelection: 'multiple',
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                eGridColetaExameDiv.css('width', $('.portlet.light').width());
            },
            [AgGridHelper.HOOKS.AFTER_CREATED](hookData) {
                hookData.gridOptions.api.sizeColumnsToFit();
            },
            onSelectionChanged: () => {
                atualizaBtns();
            },
            onCellDoubleClicked: (event) => {
                if(event.data) {
                    detalhamentoExameModal.open({resultadoId:$("input[name='LabResultadoId']").val(), resultadoExameId: event.data.id});
                }
            },
            onRowDataChanged: () => {
                gridDetalhamentoColetaExameOptions.getApi().forEachLeafNode((node) => {
                    if (node.data.statusId === 1 || node.data.statusId == undefined || node.data.statusId == '') {
                        node.setSelected(true);
                    }
                });
            },
            enableBrowserTooltips: false,
            components: {
                medicoSolicitanteTooltip: medicoSolicitanteTooltip,
                pendenciaTooltip:pendenciaTooltip,
                exameTooltip:exameTooltip
            },
            tooltipShowDelay: 0,
            data: {
                autoInitialLoad: true,
                enablePagination: true,
                callback: resultadoAppService.obterResultadoExamesPorResultadoId,
                getData() {
                    return {
                        id: $("input[name='LabResultadoId']").val()
                    }
                },
            }
        })
        
        function atualizarColeta() {
            abp.event.trigger('app.atualizaColeta', {
                resultadoId: $("input[name='LabResultadoId']").val()
            });
            gridDetalhamentoColetaExameOptions.refresh()
            atualizaBtns();
            carregarDashboardColeta();
        }
        
        function atualizaBtns() {
            const rows = gridDetalhamentoColetaExameOptions.getSelectedRows();
            const btnBaixa = _.filter(rows, x=> x.statusId != 1 && x.statusId != 2 && x.statusId != 5);
            const btnPendencia = _.filter(rows, x=> !x.isPendencia);
            const btnResolverPendencia = _.filter(rows, x=> x.isPendencia);
            if(btnBaixa && btnBaixa.length == 0) {
                $(".btn-baixa").show();
            } else {
                $(".btn-baixa").hide();
            }

            if(btnPendencia && btnPendencia.length) {
                $(".btn-add-pendencia").show();
            } else {
                $(".btn-add-pendencia").hide();
            }

            if(btnResolverPendencia && btnResolverPendencia.length) {
                $(".btn-resolver-pendencia").show();
            } else {
                $(".btn-resolver-pendencia").hide();
            }
        }

        function carregarDados() {
            carregarDashboardColeta();
        }

        function carregarEventos() {

            const _selectedDateRange = {
                "momentFormatStart": "DD/MM/YYYY",
                "momentFormatEnd": "DD/MM/YYYY",
                startDate: moment().subtract(30, 'd').startOf('day'),
                endDate: moment().startOf('day').endOf('day')
            };

            createDateRangePickerResultado($('.date-range-resultado'), _selectedDateRange);

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
            
            $(".btn-baixa").on("click", (event) => {
                const btn = $(event.target);
                btn.buttonBusy(true);

                abp.message.confirm(
                    "Deseja dar baixa nos itens selecionados?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            let examesColetaIds = gridDetalhamentoColetaExameOptions.getSelectedRows();
                            if(examesColetaIds.length) {
                                examesColetaIds = examesColetaIds.map(x=> x.id);
                            }

                            baixaExamesModal.open({
                                resultadoId: $("input[name='LabResultadoId']").val(),
                                resultadoExameIds: examesColetaIds
                            });
                        }

                        btn.buttonBusy(false);
                    }
                );
            })
            
            $(".btn-voltar-status-anterior").on("click",() => {
                const btn = $(this);
                btn.buttonBusy(true);
                abp.message.confirm(
                    "Deseja voltar o status dos itens selecionados?",function(isConfirmed) {
                        if (isConfirmed) {
                            let examesColetaIds = gridDetalhamentoColetaExameOptions.getSelectedRows();
                            if(examesColetaIds.length) {
                                examesColetaIds = examesColetaIds.map(x=> x.id);
                            }
                            resultadoExameAppService
                                .voltarStatusAnterior($("input[name='LabResultadoId']").val(),examesColetaIds)
                                .then(data=>{
                                if (data.errors.length > 0) {
                                    errorHandler(data.errors, 'Erro ao voltar status dos exames ');
                                    return;
                                }
                                abp.notify.success("Status dos exames selecionados atualizados com sucesso!", "Voltar Status dos Exames")

                                atualizarColeta();
                            })
                        }
                    }
                );
            })

            $(".btn-add-pendencia").on("click", (event) => {
                const btn = $(event.target);
                btn.buttonBusy(true);

                abp.message.confirm(
                    "Deseja adicionar pendencia nos itens selecionados?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            let examesColetaIds = gridDetalhamentoColetaExameOptions.getSelectedRows();
                            if(examesColetaIds.length) {
                                examesColetaIds = examesColetaIds.map(x=> x.id);
                            }

                            addPendenciaModal.open({
                                resultadoId: $("input[name='LabResultadoId']").val(),
                                resultadoExameIds: examesColetaIds
                            });
                        }

                        btn.buttonBusy(false);
                    }
                );
            })

            $(".btn-resolver-pendencia").on("click", (event) => {
                const btn = $(event.target);
                btn.buttonBusy(true);

                abp.message.confirm(
                    "Deseja resolver as pendências dos itens selecionados?",
                    function (isConfirmed) {
                        let examesColetaIds = gridDetalhamentoColetaExameOptions.getSelectedRows();
                        if(examesColetaIds.length) {
                            examesColetaIds = examesColetaIds.map(x=> x.id);
                        }
                        resultadoExameAppService
                            .resolverPendencias($("input[name='LabResultadoId']").val(),examesColetaIds)
                            .then(data=>{
                                // if (data.errors.length > 0) {
                                //     errorHandler(data.errors, 'Erro ao resolver pendências dos exames ');
                                //     return;
                                // }
                                abp.notify.success("Pendências resolvidas com sucesso!", "Resolver pendências")

                                atualizarColeta();
                            })
                            btn.buttonBusy(false);
                        
                    }
                );
            })

            $(".btn-excluir-exame-coleta").on("click", (event) => {
                const btn = $(event.target);
                btn.buttonBusy(true);
                abp.message.confirm(
                    "Deseja excluir os exames selecionados da coleta?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            abp.notify.success("Exames excluídos com sucesso!", "Exclusão exames")
                        }

                        btn.buttonBusy(false);
                    }
                );
            })

            $(".btn-excluir-exame").on("click", (event) => {
                const btn = $(event.target);
                btn.buttonBusy(true);
                abp.message.confirm(
                    "Deseja excluir os exames selecionados?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            abp.notify.success("Exames excluídos com sucesso!", "Exclusão exames")
                        }

                        btn.buttonBusy(false);
                    }
                );
            })

            $(".btn-adicionar-exame-coleta").on("click",() => {
                const btn = $(this);
                btn.buttonBusy(true);
                abp.message.confirm(
                    "Deseja adicionar os exames selecionados na coleta?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            let rows = gridDetalhamentoSolicitacaoExamesOptions.getSelectedRows();
                            if (rows.length) {
                                rows = rows.map(x => x.id);
                            }
                            resultadoExameAppService.adicionarExameColeta($("input[name='LabResultadoId']").val(),$("input[name='SolicitacaoExameId']").val(),rows).then(data=>{
                                if (data.errors.length > 0) {
                                    errorHandler(data.errors, 'Erro ao adicionar exames na coleta ');
                                    return;
                                }
                                abp.notify.success("Exames selecionados adicionados na coleta com sucesso!", "Adicionar exames na coleta")
                                atualizarColeta();
                        });
                    }
                });
            })

            $(".btn-criar-coleta").on("click", (event) => {
                const btn = $(event.target);
                btn.buttonBusy(true);
                abp.message.confirm(
                    "Deseja criar a coleta dos selecionados?",
                    function (isConfirmed) {
                        if (isConfirmed) {
                            let rows = gridDetalhamentoSolicitacaoExamesOptions.getSelectedRows();
                            if(rows.length) {
                                rows = rows.map(x=> x.id);
                            }
                            const data = {
                                id : $("input[name='LabResultadoId']").val(),
                                solicitacaoExameId: $("input[name='SolicitacaoExameId']").val(),
                                atendimentoId: $("input[name='atendimentoId']").val(),
                                solicitacaoExameItems: rows,
                                imprimirColeta:true
                            }
                            createOrEditColetaPorSolicitacaoModal.open(data);
                        }

                        btn.buttonBusy(false);
                    }
                );
            })

            $(".btn-editar-coleta").on("click", (event) => {
                const btn = $(event.target);
                btn.buttonBusy(true);
                const data = {
                    id : $("input[name='LabResultadoId']").val(),
                    atendimentoId : $("input[name='AtendimentoId']").val(),
                    solicitacaoExameId: null,
                    solicitacaoExameItems: null,
                    imprimirColeta:false,
                }
                createOrEditColetaPorSolicitacaoModal.open(data);
                btn.buttonBusy(false);
            })

            $(".btn-resultados").on("click", (event) => {
                const btn = $(event.target);
                btn.buttonBusy(true);
                $('.nav.nav-tabs a[href=".tabColetaResultado"]').tab("show");
            })

            $(".btn-voltar").on("click", (event) => {
                $('.nav.nav-tabs a[href=".tabColeta"]').tab("show");
            })
            
            $(".btn-reimprimir").on("click", (event)=> {
                imprimirEtiquetaExames([],false);
            });
            
            abp.event.on('app.CriarOuEditarResultadoPorSolicitacaoModalSaved', onCriarOuEditarResultadoModalSaved);
            abp.event.on('app.atualizaColeta', onAtualizaColeta)
        }
        
        function onAtualizaColeta(event) {
            resultadoAppService.obter(event.resultadoId).then(res => {
                const resultado = res;
                $("input[name='LabResultadoId']").val(resultado.id);
                $(".data-coleta").text(moment(resultado.dataColeta).format("DD/MM/YYYY hh:mm:ss"))
                if(resultado.resultadoStatus) {
                    $(".coleta-status").text(resultado.resultadoStatus.descricao);
                    $(".coleta-status").css('background-color',`#${resultado.resultadoStatus.corFundo}`);
                    $(".coleta-status").css('color',`#${resultado.resultadoStatus.corFonte}`);
                }
                if(resultado.responsavel) {
                    $(".coleta-responsavel").text(resultado.responsavel.descricao);
                }
                carregarDashboardColeta();
            })
        }
        
        function onCriarOuEditarResultadoModalSaved(resultObj) {
            $("input[name='LabResultadoId']").val(resultObj.resultado.id);
            $(".data-coleta").text(moment(resultObj.resultado.dataColeta).format("DD/MM/YYYY hh:mm:ss"))
            if(resultObj.resultado.resultadoStatus) {
                $(".coleta-status").text(resultObj.resultado.resultadoStatus.descricao);
                $(".coleta-status").css('background-color',`#${resultObj.resultado.resultadoStatus.corFundo}`);
                $(".coleta-status").css('color',`#${resultObj.resultado.resultadoStatus.corFonte}`);
            }

            if(resultObj.resultado.responsavel) {
                $(".coleta-responsavel").text(resultObj.resultado.responsavel.descricao);
            }
            
            $(".dashboard-coleta").removeClass("hidden");
            $(".coleta-warning").addClass("hidden");
            carregarDashboardColeta();
            const aHref = $("a[href='.tabColetaExame']");
            aHref.parent().removeClass("hide");
            aHref.trigger("click");
            if (resultObj.imprimirColeta) {
                imprimirEtiquetaExames(resultObj.solicitacaoExameItems, true);
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

        function imprimirEtiquetaExames(solicitacaoExameItems, isSolicitacao) {
            if(isSolicitacao) {
                abp.message.confirm(
                    "Deseja imprimir as etiquetas dos exames?", function (confirm) {
                        if(confirm) {
                            const data = {
                                resultadoId:$("input[name='LabResultadoId']").val(),
                                solicitacaoExameItems
                            }
                            imprimirEtiquetasPorSolicitacaoModal.open(data)
                        }
                    });
            } else {
                abp.message.confirm(
                    "Deseja imprimir as etiquetas dos exames?", function (confirm) {
                        if(confirm) {
                            let examesColetaIds = gridDetalhamentoColetaExameOptions.getSelectedRows();
                            if(examesColetaIds.length) {
                                examesColetaIds = examesColetaIds.map(x=> x.id);
                            }
                            const data = {
                                resultadoId:$("input[name='LabResultadoId']").val(),
                                resultadoExameIds:examesColetaIds
                            }
                            imprimirExamesColetaModal.open(data)
                        }
                    });
            }
        }
        
        function carregarDashboardColeta () {
            if($("input[name='LabResultadoId']").val()) {
                solicitacaoExameService.retornaContadores($("input[name='LabResultadoId']").val()).then(res => {
                    $(".dashboard-stat.inicial .valor").text(res.inicialValor);
                    $(".dashboard-stat.emColeta .valor").text(res.emColetaValor);
                    $(".dashboard-stat.coletado .valor").text(res.coletadoValor);
                    $(".dashboard-stat.digitado .valor").text(res.digitadoValor);
                    $(".dashboard-stat.interfaceado .valor").text(res.interfaceadoValor);
                    $(".dashboard-stat.pendente .valor").text(res.pendenteValor);
                    $(".dashboard-stat.conferido .valor").text(res.conferidoValor);
                })
            }
        }

        const gridDetalhamentoColetaOptions = {
            enableBrowserTooltips: false,
            tooltipShowDelay: 0,
            rowData: null,
            columnDefs: null,
            rowSelection: 'single',
            onSelectionChanged: (event) => {
                const selectedRows = gridDetalhamentoColetaOptions.api.getSelectedRows();
                if (selectedRows != null && selectedRows.length !== 0) {
                    selectedRow = selectedRows[0];
                    gridDetalhamentoColetaOptions.api.deselectAll();
                    $('a[href=".tabColetaExame"]').tab("show");
                }
            },
            onGridReady: () => {
                const params = {
                    force: true
                };
                gridDetalhamentoColetaOptions.api.refreshCells(params);
                eGridColetaDiv.style.width = '100%';
                //gridOptions.api.setRowData(lista);
                gridDetalhamentoColetaOptions.api.doLayout();
                gridDetalhamentoColetaOptions.api.sizeColumnsToFit();
            }
        };
        function init() {
            carregarDados()
            carregarEventos()

            setTimeout(() => {
                $('a.active[data-toggle="tab"]').trigger("click");

                gridDetalhamentoSolicitacaoExamesOptions.render(gridDetalhamentoSolicitacaoExamesDiv[0])
            }, 500)
            
            abp.event.on("app.atualizaGridDetalhamentoColetaExame", () => {
                atualizarColeta();
            })
            abp.event.on("app.baixaColetaExame", (data) => {
                onVerificaExamesBaixa(data)
            })
        }

        function createTabColetaResultados() {
            eGridColetaResultadoDiv.style.width = $('.modal-detalhamento').width();
            obterResultados();
        }

        function obterResultados() {
            lista = [];
            let _args = {
                atendimentoId: 265234,
                dateEnd: "2020-09-26T02:59:59.999Z",
                dateStart: "2020-08-26T03:00:00.000Z",
                filtro: "",
                id: 131936,
                isAmbulatorioEmergencia: true,
                isDesseAtendimento: false,
                isDessePaciente: true,
                isInternacao: true,
                nomePaciente: "ANA MARIA TRINDADE da COSTA",
                pacienteId: 131936
            };
            
            evolucaoResultadosService.listaEvolucaoResultado(_args)
                .done(function (dt) {
                    if (gridDetalhamentoColetaResultadoOptions.api && gridDetalhamentoColetaResultadoOptions.api.destroy) {
                        gridDetalhamentoColetaResultadoOptions.api.destroy();
                    }
                    lista = dt.items;
                    let datas = [];

                    _.forEach(lista,
                        function (item, index) {
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

                    const columnDefs = [
                        {
                            headerName: 'Nome do exame',
                            field: "itemDescricao",
                            tooltip: (params) => params.value,
                            pinned: 'left', lockPinned: true,
                        },
                        {
                            headerName: 'Informação',
                            field: "itemInfo",
                            tooltip: (params) => params.value,
                            pinned: 'left', lockPinned: true,
                        },
                        {
                            headerName: 'Valor de referência',
                            field: "referencia",
                            tooltip: (params) => params.value,
                            pinned: 'left', lockPinned: true,
                        }
                    ];

                    _.forEach(datas, (item, index) => {
                        columnDefs.push({
                            headerName: moment(item).format("DD/MM/YYYY HH:mm:ss"),
                            tooltipComponent: 'customTooltip',
                            tooltipField: 'referencia',
                            maxWidth: 140,
                            tooltipComponentParams: { color: '#ececec' },
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
                                            return resultadoItem.resultado;
                                        } else {

                                            return { color: resultadoItem.corTexto, backgroundColor: resultadoItem.corFundo };
                                        }
                                    }
                                }
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
                            }
                        });
                    });
                    gridDetalhamentoColetaResultadoOptions.rowData = null;

                    gridDetalhamentoColetaResultadoOptions.columnDefs = columnDefs;


                    eGridColetaResultadoDiv.style.width = $('#grupoContaAdministrativaInformationsTab').width();

                    new agGrid.Grid(eGridColetaResultadoDiv, gridDetalhamentoColetaResultadoOptions);

                    setTimeout(function () {
                        const params = {
                            force: true
                        };
                        gridDetalhamentoColetaResultadoOptions.api.refreshCells(params);
                        eGridColetaResultadoDiv.style.width = '100%';
                        gridDetalhamentoColetaResultadoOptions.api.setRowData(lista);
                        gridDetalhamentoColetaResultadoOptions.api.doLayout();

                    }, 1000);

                    App.stopPageLoading();
                    document.querySelector('.loadingCommon').style.display = 'none';
                    return;
                });
        }

        function createDateRangePickerResultado(inputTag, selectedDateRange) {
            const baseOptions = app.createDateRangePickerOptions();
            const options = $.extend(true, baseOptions, selectedDateRange);
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

        function createDetalhamentoSolicitacaoExameColumns() {
            return [
                AgGridHelper.columns.base('codigo','Código', {width: 80,checkboxSelection: true,headerCheckboxSelection: true}),
                AgGridHelper.columns.base('status','Status', {width: 120}),
                AgGridHelper.columns.base('exame','Exame', {
                    tooltipComponent: 'exameTooltip',
                    headerTooltip: 'Pendente?',
                    tooltipComponentParams: {color: '#ececec'},
                } ),
                AgGridHelper.columns.base('codigoMaterial','Código Material', {width:90}),
                AgGridHelper.columns.base('descricaoMaterial','Material'),
                AgGridHelper.columns.dateTime('dataSolicitacao','Data Solicitação' ),
                AgGridHelper.columns.base('medicoSolicitante','Solicitante',{
                    tooltipComponent: 'medicoSolicitanteTooltip',
                    tooltipField: 'numeroConselho',
                    headerTooltip: 'Solicitante',
                    tooltipComponentParams: {color: '#ececec'},
                }),
                AgGridHelper.columns.boolean('isPendente','Pendente?', {
                    tooltipComponent: 'pendenciaTooltip',
                    tooltipField: 'motivoPendencia',
                    headerTooltip: 'Pendente?',
                    tooltipComponentParams: {color: '#ececec'},
                }),
                AgGridHelper.columns.boolean('existeResultadoExame','Incluso na coleta?'),
                
            ];
        }

        function createDetalhamentoColetaExameColumns() {
            return [
                AgGridHelper.columns.base('codigo','Código', {width: 80,checkboxSelection: true,headerCheckboxSelection: true}),
                AgGridHelper.columns.status('statusDescricao','Status', {
                    width: 120,
                    cellRendererParams: {
                        corFundo: 'statusCor'
                    }
                }),
                AgGridHelper.columns.base('exame','Exame', {
                    tooltipComponent: 'exameTooltip',
                    headerTooltip: 'Pendente?',
                    tooltipComponentParams: {color: '#ececec'},
                }),
                AgGridHelper.columns.base('codigoMaterial','Código Material', {width:90}),
                AgGridHelper.columns.base('descricaoMaterial','Material'),
                AgGridHelper.columns.dateTime('dataColeta','Data Coleta' ),
                AgGridHelper.columns.dateTime('dataColetaBaixa','Data Baixa',{
                    tooltipField:'usuarioColetaBaixa'
                } ),
                AgGridHelper.columns.dateTime('dataDigitado','Data Digitado', {
                    tooltipField:'usuarioDigitado'
                } ),
                AgGridHelper.columns.dateTime('dataConferido','Data Conferido', {
                    tooltipField:'usuarioConferido'
                }),
                // AgGridHelper.columns.base('responsavel','Responsável'),
                // AgGridHelper.columns.base('medicoSolicitante','Solicitante',{
                //     tooltipComponent: 'medicoSolicitanteTooltip',
                //     tooltipField: 'numeroConselho',
                //     headerTooltip: 'Solicitante',
                //     tooltipComponentParams: {color: '#ececec'},
                // }),
                AgGridHelper.columns.boolean('isPendencia','Pendente?', {
                    tooltipComponent: 'pendenciaTooltip',
                    tooltipField: 'motivoPendencia',
                    headerTooltip: 'Pendente?',
                    tooltipComponentParams: {color: '#ececec'},
                }),
                AgGridHelper.columns.base('observacao','Observação'),
            ];
        }
    });
})();