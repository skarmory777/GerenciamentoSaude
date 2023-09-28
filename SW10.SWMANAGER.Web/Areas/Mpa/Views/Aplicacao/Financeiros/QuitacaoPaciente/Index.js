(function () {
    $(function () {
        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');
        const gridPrincipal = $('.grid-principal');
        const faturamentoEntregaContaAppService = abp.services.app.faturamentoEntregaConta;

        let _periodoEntrega = {
            autoUpdateInput: false,
            startDate: undefined,
            endDate: undefined
        };

        var _consolidacaoModal = new app.ModalManager({
            viewUrl: abp.appPath + "Mpa/QuitacaoPaciente/AbrirConsolidacaoModal",
            scriptUrl: abp.appPath + "Areas/Mpa/Views/Aplicacao/Financeiros/QuitacaoPaciente/_ConsolidacaoModal.js",
            modalClass: 'AbrirConsolidacaoModal'
        });

        const pickerOptions = app.createDateRangePickerOptions();

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
                    callback: faturamentoEntregaContaAppService.listarContasQuitacaoPaciente,
                    getData() {
                        if (typeof (_periodoEntrega.startDate) !== "string") {
                            _periodoEntrega.startDate = !_periodoEntrega.startDate ? undefined : _periodoEntrega.startDate.format('YYYY-MM-DDT00:00:00Z');
                        }
                        if (typeof (_periodoEntrega.endDate) !== "string") {
                            _periodoEntrega.endDate = !_periodoEntrega.endDate ? undefined : _periodoEntrega.endDate.format('YYYY-MM-DDT23:59:59.999Z');
                        }

                        return {
                            convenioId: $(".selectConvenio").val(),
                            pacienteId: $(".selectPaciente").val(),
                            modalidadeAtendimento: $("input[name=ModalidadeAtendimento]:checked").val(),
                            ignorarDataEntrega: $('#ignorarDataEntrega')[0].checked,
                            Situacao: $('select[name=selectSituacao]').val(),
                            startDateEntrega: _periodoEntrega.startDate,
                            endDateEntrega: _periodoEntrega.endDate
                        };
                    },
                }
            });
        gridPrincipalOptions.render(gridPrincipal[0]);
        $(".tooltip-info").tooltip({
            html: true,
            container: 'body',
            template: `<div class='tooltip' role='tooltip'><div class='tooltip-arrow'></div><div class='tooltip-inner' style='width:100% !important;max-width: 350px;'></div></div>`
        });

        selectSW('.selectPaciente', "/api/services/app/paciente/ListarDropdown");
        selectSW('.selectConvenio', '/api/services/app/convenio/ListarDropdown');


        $(".select2").on("change", (event) => {
            gridPrincipalOptions.refresh();
        });
        $("input[name=ModalidadeAtendimento]").on("change", (event) => {
            gridPrincipalOptions.refresh();
        });
        $('.periodo-entrega').daterangepicker(
            $.extend(true, pickerOptions, _periodoEntrega),
            function (start, end, label) {
                _periodoEntrega.startDate = start != undefined ? start.format('YYYY-MM-DDT00:00:00Z') : undefined;
                _periodoEntrega.endDate = end != undefined ? end.format('YYYY-MM-DDT23:59:59.999Z') : undefined;

                gridPrincipalOptions.refresh();
            });

        updatePrincipalSize(false);

        function alterarValorRecebidoAtual(data) {
            var id = data.data.id;
            var newValue = data.newValue;

            faturamentoEntregaContaAppService.alterarValorRecebidoAtual(id, newValue);

            return;
        }

        function alterarValorGlosaRecuperavelTemp(data) {
            var id = data.data.id;
            var newValue = data.newValue;

            faturamentoEntregaContaAppService.alterarValorGlosaRecuperavelTemp(id, newValue);

            return;
        }

        function alterarValorGlosaIrrecuperavelTemp(data) {
            var id = data.data.id;
            var newValue = data.newValue;

            faturamentoEntregaContaAppService.alterarValorGlosaIrrecuperavelTemp(id, newValue);

            return;
        }

        function defColumnsPrincipal() {
            const disableFilterAndMenu = { filter: true, suppressMenu: false, sortable: true, resizable: true };
            return [
                AgGridHelper.columns.action({
                    enableEdit: false,
                    enableDelete: false,
                }, disableFilterAndMenu, { width: 100, checkboxSelection: true }),
                AgGridHelper.columns.base('id', app.localize('Id'), disableFilterAndMenu, {hide: true}),
                AgGridHelper.columns.base('entregaLoteId', app.localize('Lote'), disableFilterAndMenu, {width: 100 }),
                AgGridHelper.columns.dateTime('dataEntrega', app.localize('Entrega'), disableFilterAndMenu, {width:150}),
                AgGridHelper.columns.dateTime('dataInicio', app.localize('Inicio'), disableFilterAndMenu, { width: 150 }),
                AgGridHelper.columns.dateTime('dataFim', app.localize('Final'), disableFilterAndMenu, { width: 150 }),
                AgGridHelper.columns.base('matricula', app.localize('Matricula'), disableFilterAndMenu, { width: 150}),
                AgGridHelper.columns.base('tipoGuia', app.localize('Guia'), disableFilterAndMenu, {width:200}),
                AgGridHelper.columns.base('numeroGuia', app.localize('Numero'), disableFilterAndMenu, {width:150}),
                AgGridHelper.columns.base('pacienteNomeCompleto', app.localize('Paciente'), disableFilterAndMenu, {
                    filter: 'agTextColumnFilter',
                    filterParams: {
                        buttons: ['reset', 'apply'],
                    },
                    width: 150
                    },
                ),
                AgGridHelper.columns.base('convenioNomeFantasia', app.localize('Convenio'), disableFilterAndMenu, {width:150}),
                AgGridHelper.columns.money('valorEntregue', app.localize('Entregue'), disableFilterAndMenu, { width: 110}),
                AgGridHelper.columns.money('glosaRecuperavel', app.localize('Glosa Recuperavel'), disableFilterAndMenu, { width: 170},
                    { editable: true, onCellValueChanged: alterarValorGlosaRecuperavelTemp }),
                AgGridHelper.columns.money('glosaIrrecuperavel', app.localize('Glosa Irrecuperavel'), { width: 170},
                    disableFilterAndMenu,
                    { editable: true, onCellValueChanged: alterarValorGlosaIrrecuperavelTemp }),
                AgGridHelper.columns.money('valorRecebidoAnterior', app.localize('Anterior'), disableFilterAndMenu, {width:100}),
                AgGridHelper.columns.money('valorRecebidoAtual', app.localize('Atual'), disableFilterAndMenu,
                    { editable: true, onCellValueChanged: alterarValorRecebidoAtual })

            ];
        }

        $("#btnConsolidacao").click(function () {            
            var input = [];
            var selectedRows = gridPrincipalOptions.getSelectedRows();

            if (selectedRows.length == 0) {
                abp.notify.warn("Selecione ao menos um registro.");
                return;
            }

            selectedRows.forEach(function (element) {
                var entregaContaInput = {
                    entregaContaId: element.id,
                    entregaLoteId: element.entregaLoteId,
                    valorRecebido: element.valorRecebidoAtual
                }
                input.push(entregaContaInput);
            });
            
            _consolidacaoModal.open({ input });
        });

        function updatePrincipalSize(show) {
            const principalContent = $(".row-grid-content");
            if (!show) {
                gridPrincipal.css("height", $(window).height() - principalContent.position().top - 100 - $("page-header.navbar.navbar-fixed-top").height());
            }
            else {
                gridPrincipal.css("height", $(window).height() - principalContent.position().top - parseInt(relationCollapse.css('max-height')) - 100 - $("page-header.navbar.navbar-fixed-top").height());
                setTimeout(() => {
                    const nodes = gridPrincipalOptions.getApi().getSelectedNodes();
                    gridPrincipalOptions.getApi().ensureNodeVisible(nodes[0], 'middle');
                }, 100)

            }
        }

        function getFiltros() {
             DataEntrega = $("#dataEntrega").val();
             ConvenioId = $("#convenioId").val();
             PacienteId = $("#pacienteId").val();
        }
    });
})();