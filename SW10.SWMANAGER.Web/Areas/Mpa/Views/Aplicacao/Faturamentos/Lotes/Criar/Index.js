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
        const faturarContaAppService = abp.services.app.conta;
        const faturamentoLoteAppService = abp.services.app.faturamentoEntregaLote;
        const btnGerarLote = $('.btn-gerar-lote');

        let _selectedDateRange = {
            startDate: moment().add(-1, "days").startOf('day'),
            endDate: moment().endOf('day')
        };
        const pickerOptions = app.createDateRangePickerOptions();

        const _criarLoteModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoLotes/CriarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/Lotes/Criar/Modal.js',
            modalClass: 'FaturarLoteCriarModal',
            focusFunction:()=> {
                $.fn.modal.Constructor.prototype.enforceFocus = function () {};
            }
        });

        let gridPrincipalOptions = AgGridHelper.createAgGrid('grid-principal-atendimentos',
        {
            rowSelection: 'multiple',
            columnDefs: defColumnsPrincipal(),
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridPrincipal.css('width', $('.portlet.light').width());
            },
            data: {
                autoInitialLoad: true,
                enablePagination: true,
                callback: faturarContaAppService.listarContasParaLotes,
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
                        startDate: _selectedDateRange.startDate,
                        endDate: _selectedDateRange.endDate,
                    };
                },
            }
        });
        gridPrincipalOptions.render(gridPrincipal[0]);
       
        btnGerarLote.on("click", (e) => {
            btnGerarLote.buttonBusy(true);
            debugger;
            const selectedRows = gridPrincipalOptions.getSelectedRows();

            if (!selectedRows || !selectedRows.length) {
                abp.notify.error("Nenhuma conta selecionada");
                btnGerarLote.buttonBusy(false);
                return;
            }
            debugger;
            const convenioIds = _.uniq(selectedRows.map(x => x.convenioId));
            if (convenioIds.length > 1) {
                abp.notify.error("Não é possível gerar um lote com multiplos convenios");
                btnGerarLote.buttonBusy(false);
                return;
            }

            const tipoInternacaoValue = $(".chk-tipo-internacao:checked").map((x, el) => $(el).val()).get();
            let tipoInternacao = null;
            if (tipoInternacaoValue.some((x) => x == "internacao") && tipoInternacaoValue.length != 2) {
                tipoInternacao = 0;
            } else if (tipoInternacaoValue.some((x) => x == "emergencia") && tipoInternacaoValue.length != 2) {
                tipoInternacao = 1;
            }

            const data = {
                empresaId: $(".selectEmpresa").val(),
                convenioId: convenioIds[0],
                numeroProcesso: $(".numeroProcesso").val(),
                tipoInternacao,
                startDate: _selectedDateRange.startDate,
                endDate: _selectedDateRange.endDate,
                contaIds: selectedRows.map(x => x.id)
            };

            abp.message.confirm("Deseja gerar o lote com as contas selecionadas?", "Gerar Lote", (confirm) => {
                if (confirm) {
                    faturamentoLoteAppService.criarLote(data).then((x) => {
                        window.location = `/Mpa/FaturamentoLotes/lote?loteId=${x.returnObject}`;
                        btnGerarLote.buttonBusy(false);
                    })
                } else {
                    btnGerarLote.buttonBusy(false);
                }
            })
        })
        
        
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
        

        
        function defColumnsPrincipal() {
            const disableFilterAndMenu = { filter: true, suppressMenu: false, sortable: true,resizable:true};
            return [
                AgGridHelper.columns.action({
                    enableEdit: false,
                    enableDelete: false,
                    customAction: [{
                        title: 'Entrar Conta Medica',
                        action: onEnterContaMedica,
                        icon: 'fas fa-file-medical',
                        class: 'btn-info'
                    }]
                }, disableFilterAndMenu, { width: 100, checkboxSelection: true }),
                AgGridHelper.columns.base('codigo', app.localize('Codigo'), disableFilterAndMenu, { width: 50 }),
                //AgGridHelper.columns.status('fatStatus', app.localize('Status Faturamento'), disableFilterAndMenu, {
                //    cellRendererParams: {
                //        corFundo: 'fatContaStatusCor',
                //        corTexto: 'FFFFFF'
                //    }
                //}),
                AgGridHelper.columns.base('pacienteNomeCompleto', app.localize('Paciente'), disableFilterAndMenu, {
                    filter: 'agTextColumnFilter',
                    filterParams: {
                        buttons: ['reset', 'apply'],
                    },
                }),
                AgGridHelper.columns.dateTime('dataRegistro', app.localize('Data Atendimento'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataAlta', app.localize('Data Alta'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataInicial', app.localize('Data Inicio'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataFinal', app.localize('Data Final'), disableFilterAndMenu),
                AgGridHelper.columns.money('valorConta', app.localize('Valor Total'), disableFilterAndMenu),
                AgGridHelper.columns.base('convenioNomeFantasia', app.localize('Convenio'), disableFilterAndMenu),
                AgGridHelper.columns.base('planoDescricao', app.localize('Plano'), disableFilterAndMenu),
                AgGridHelper.columns.base('tipoDeGuia', app.localize('Tipo de Guia'), disableFilterAndMenu),
                AgGridHelper.columns.base('numeroDeGuia', app.localize('Numero de Guia'), disableFilterAndMenu),
                AgGridHelper.columns.base('matricula', app.localize('Matricula'), disableFilterAndMenu)
            ];
        }
        
        function defColumnsContasMedicas() {
            const disableFilterAndMenu = {filter: false,  suppressMenu: true};
            return [
                
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
        
        
        function updatePrincipalSize(show) {
            const principalContent = $(".row-grid-content");
            if(!show) {
                gridPrincipal.css("height", $(window).height() - principalContent.position().top - 100 - $("page-header.navbar.navbar-fixed-top").height());
            }
            else {
                gridPrincipal.css("height", $(window).height() - principalContent.position().top - parseInt(relationCollapse.css('max-height'))  - 100 - $("page-header.navbar.navbar-fixed-top").height());
                setTimeout(()=> {
                    const nodes = gridPrincipalOptions.getApi().getSelectedNodes();
                    gridPrincipalOptions.getApi().ensureNodeVisible(nodes[0],'middle');
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
       

        function onAddOcorrencias(e) {
            $(this).buttonBusy(true);
        }
        
        function onEnterContaMedica(e,data) {
            const urlData = {
                atendimentoId: data.atendimentoId,
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