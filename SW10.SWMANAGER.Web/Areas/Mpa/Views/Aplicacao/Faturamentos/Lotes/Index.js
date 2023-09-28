(function () {
    $(function () {
        $('body').addClass('page-sidebar-closed');
        
        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        const gridPrincipal = $('.grid-principal');
        const btnCriarLote = $(".btn-criar-lote");
        const faturamentoLoteAppService = abp.services.app.faturamentoEntregaLote;;


        let _periodoContas = {
            autoUpdateInput: false,
            startDate: undefined,
            endDate: undefined
        };

        let _periodoEntrega = {
            autoUpdateInput: false,
            startDate: undefined,
            endDate: undefined
        };

        const pickerOptions = app.createDateRangePickerOptions();

        let gridPrincipalOptions = AgGridHelper.createAgGrid('grid-principal-lotes',
        {
            rowSelection: 'single',
            columnDefs: defColumnsPrincipal(),
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridPrincipal.css('width', $('.portlet.light').width());
            },
            data: {
                autoInitialLoad: true,
                enablePagination: true,
                callback: faturamentoLoteAppService.listarLotes,
                getData() {
                    if (typeof (_periodoContas.startDate) !== "string" ) {
                        _periodoContas.startDate = !_periodoContas.startDate ? undefined : _periodoContas.startDate.format('YYYY-MM-DDT00:00:00Z');
                    }
                    if (typeof (_periodoContas.endDate) !== "string" ) {
                        _periodoContas.endDate = !_periodoContas.endDate ? undefined : _periodoContas.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                    }

                    if (typeof (_periodoEntrega.startDate) !== "string") {
                        _periodoEntrega.startDate = !_periodoEntrega.startDate ? undefined : _periodoEntrega.startDate.format('YYYY-MM-DDT00:00:00Z');
                    }
                    if (typeof (_periodoEntrega.endDate) !== "string") {
                        _periodoEntrega.endDate = !_periodoEntrega.endDate ? undefined : _periodoEntrega.endDate.format('YYYY-MM-DDT23:59:59.999Z');
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
                        startDate: _periodoContas.startDate,
                        endDate: _periodoContas.endDate,
                        startDateEntrega: _periodoEntrega.startDate,
                        endDateEntrega: _periodoEntrega.endDate,
                    };
                },
            },
            editarItem(data) {
                if (data) {
                    window.open(`/Mpa/FaturamentoLotes/lote?loteId=${data.id}`);
                }
            }
        });
        gridPrincipalOptions.render(gridPrincipal[0]);

        btnCriarLote.on("click", onBtnCriarLote)
        
        
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


        $('.periodo-contas').daterangepicker(
            $.extend(true, pickerOptions, _periodoContas),
            function (start, end, label) {
                _periodoContas.startDate = start != undefined ? start.format('YYYY-MM-DDT00:00:00Z') : undefined;
                _periodoContas.endDate = end != undefined ? end.format('YYYY-MM-DDT23:59:59.999Z') : undefined;

                gridPrincipalOptions.refresh();
            });

        $('.periodo-entrega').daterangepicker(
            $.extend(true, pickerOptions, _periodoEntrega),
            function (start, end, label) {
                _periodoEntrega.startDate = start != undefined ? start.format('YYYY-MM-DDT00:00:00Z') : undefined;
                _periodoEntrega.endDate = end != undefined ? end.format('YYYY-MM-DDT23:59:59.999Z') : undefined;

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
        updatePrincipalSize(false);
        
        function defColumnsPrincipal() {
            const disableFilterAndMenu = { filter: true, suppressMenu: false, sortable: true,resizable:true};
            return [
                AgGridHelper.columns.action({ enableEdit: true }),
                //AgGridHelper.columns.status('loteStatus', app.localize('Status'), disableFilterAndMenu, {
                //    cellRendererParams: {
                //        corFundo: 'ateCorFundo',
                //        corTexto: 'ateCorTexto'
                //    }
                //}),
                AgGridHelper.columns.base('empresaNomeFantasia', app.localize('Empresa'), disableFilterAndMenu),
                AgGridHelper.columns.base('convenioNomeFantasia', app.localize('Convenio'), disableFilterAndMenu),
                AgGridHelper.columns.base('codEntregaLote', app.localize('Código Entrega'), disableFilterAndMenu),
                AgGridHelper.columns.base('numeroProcesso', app.localize('Número do processo'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataInicial', app.localize('Periodo Inicial'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataFinal', app.localize('Periodo Final'), disableFilterAndMenu),
                AgGridHelper.columns.dateTime('dataEntrega', app.localize('entrega'), disableFilterAndMenu),
                AgGridHelper.columns.integer('totalContas', app.localize('Total Contas'), disableFilterAndMenu),
                AgGridHelper.columns.money('valorFatura', app.localize('Fatura'), disableFilterAndMenu),
                AgGridHelper.columns.boolean('isInternacao', app.localize('Internação'), disableFilterAndMenu),
                AgGridHelper.columns.boolean('isAmbulatorio', app.localize('Emergência'), disableFilterAndMenu)
            ];
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
        

        function onBtnCriarLote(e) {
            debugger;

            btnCriarLote.buttonBusy(true);

            customConfirmModalHelper.CreateModal({
                title: "Criar Lote",
                message: "Deseja criar um lote novo ou reinviar uma conta?",
                icon: "fas fa-exclamation-triangle text-info",
                buttons: [
                    //customConfirmModalHelper.CreateButton("Cancelar", "btn btn-danger", null, (event, confirmModalInstance) => {
                    //    console.log(confirmModalInstance);
                    //}),
                    customConfirmModalHelper.CreateButton("Criar Lote", "btn btn-primary", null, (event, confirmModalInstance) => {
                        const btnInnerCriarLote = $(event.target);
                        window.open('/Mpa/FaturamentoLotes/criar');
                        btnInnerCriarLote.buttonBusy(true);
                        btnCriarLote.buttonBusy(false);
                        confirmModalInstance.close();
                    }),
                    customConfirmModalHelper.CreateButton("Reinviar Conta", "btn btn-default", null, (event, confirmModalInstance) => {
                        const btnReinviarConta = $(event.target);
                        window.open('/Mpa/FaturamentoLotes/reenviarConta');
                        btnReinviarConta.buttonBusy(true);
                        btnCriarLote.buttonBusy(false);
                        confirmModalInstance.close();
                    })
                ],
                styles: {
                    "modal-dialog": { 'min-width': '500px' }
                },
                confirmModalOptions: {
                    cancelButton: {
                        enable: true,
                        callback: (event, confirmModalInstance) => {
                            btnCriarLote.buttonBusy(false);
                            confirmModalInstance.close()
                        }
                    },
                    onHideModal: (event, confirmModalInstance) => {
                        btnCriarLote.buttonBusy(false);
                        confirmModalInstance.removeModal()
                    }
                }
            });
        }
        
    });
})();