(function () {
    $(function () {
        $('body').addClass('page-sidebar-closed');
        
        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        const gridPrincipal = $('.grid-principal');
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

        let gridPrincipalOptions = AgGridHelper.createAgGrid('grid-principal-contas-por-lote',
        {
            rowSelection: 'multiple',
            columnDefs: defColumnsPrincipal(),
            [AgGridHelper.HOOKS.BEFORE_CREATED](hookData) {
                gridPrincipal.css('width', $('.portlet.light').width());
            },
            data: {
                autoInitialLoad: true,
                enablePagination: true,
                callback: faturamentoLoteAppService.listarContasPorLote,
                getData() {
                    return {
                        loteId: $("#loteId").val()
                    };
                },
            }
        });
        gridPrincipalOptions.render(gridPrincipal[0]);
       
        btnGerarLote.on("click", (e) => {
            //abp.ui.setBusy()
            //window.open(`/Mpa/FaturamentoLotes/GerarLote?fatEntregaLoteId=${$("#loteId").val()}`,"_blank")
            $.ajax({
                url: `/Mpa/FaturamentoLotes/GerarLote?fatEntregaLoteId=${$("#loteId").val()}`,
                method: 'post',
                cache: false,
                
                error: function(xhr, textStatus, errorThrown) {
                    // Here you are able now to access to the property "responseText"
                    // as you have the type set to "text" instead of "blob".
                    console.error(xhr.responseText);
                },
            }).done((data, status, xhr) => {
                console.log(status, xhr);
                var filename = "";
                var disposition = xhr.getResponseHeader('Content-Disposition');
                if (disposition && disposition.indexOf('attachment') !== -1) {
                    var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                    var matches = filenameRegex.exec(disposition);
                    if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                }
                var blobData = new Blob([data], { type: 'text/xml' })
                console.log(blobData);
                saveAs(blobData, filename);
                abp.ui.clearBusy()
            });
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
        
        function onEnterContaMedica(e,data) {
            const urlData = {
                atendimentoId: data.atendimentoId,
                contaMedicaId: data.fatContaMedicaId
            };
            window.open(`/Mpa/FaturarAtendimento/ContaMedica?${$.param(urlData)}`);
        }
    });
})();