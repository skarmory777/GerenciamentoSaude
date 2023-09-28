(function ($) {
    const faturamentoEntregaContaRecebidaAppService = abp.services.app.faturamentoEntregaContaRecebida;

    app.modals.ConsolidarQuitacaoPaciente = function () {
        this.init = function (modalManager) {
            _modalManager = modalManager;
        };
    }
    selectSW('.selectContaCorrente', "/api/services/app/ContaCorrente/ListarDropdown");
    const gridConsolidacao = $('.grid-consolidacao');
    const quitacaoAppService = abp.services.app.quitacao;

    function defColumnsConsolidacao() {
        const disableFilterAndMenu = { filter: true, suppressMenu: false, sortable: true, resizable: true };
        return [
            AgGridHelper.columns.base('id', app.localize('Id'), disableFilterAndMenu, { hide: true }),
            AgGridHelper.columns.dateTime('dataMovimento', app.localize('Movimento'), disableFilterAndMenu, { width: 200}),
            AgGridHelper.columns.base('pessoaNome', app.localize('Fornecedor'), disableFilterAndMenu, { width: 200 }),
            AgGridHelper.columns.base('meioPagamentoDescricao', app.localize('MeioPagamento'), disableFilterAndMenu, { width: 200 }),
            AgGridHelper.columns.money('valorTotal', app.localize('ValorQuitacao'), disableFilterAndMenu, { width: 200 }),
            AgGridHelper.columns.base('contaCorrenteDescricao', app.localize('ContaCorrente'), disableFilterAndMenu, { width: 200 })
        ];
    };


    var gridConsolidacaoOptions = AgGridHelper.createAgGrid('grid-consolidacao-quitacoes',
        {
            rowSelection: 'multiple',
            columnDefs: defColumnsConsolidacao(),            
            data: {
                autoInitialLoad: true,
                enablePagination: true,
                callback: quitacaoAppService.listarQuitacoesNaoConsolidadas,
                getData() {
                    return {
                        ContaCorrenteId: $(".selectContaCorrente").val(),
                        DataMovimento: $("#dataMovimento").val(),
                    }
                }
            }
        });

    gridConsolidacaoOptions.render(gridConsolidacao[0]);

    $("#btnListarQuitacoes").click(function () {
        gridConsolidacaoOptions.refresh();
    })

    $(".save-button").click(function () {
        var consolidacaoForm = $("form[name=ConsolidacaoForm]");
        consolidacaoForm.validate();

        if (!consolidacaoForm.valid()) {
            return;
        }

        if (gridConsolidacaoOptions.getSelectedRows().length == 0) {
            abp.notify.warn("É necessário selecionar uma quitação na grid.");
            return;
        }


        var valorImpostos = $("#valorImposto").val() == '' ? 0 : parseFloat($("#valorImposto").val().replace(',', '.'));
        var valorLiquido = (parseFloat($("#totalQuitacao").val().replace(',', '.')) - valorImpostos);
        var valorQuitacaoSelecionada = gridConsolidacaoOptions.getSelectedRows()[0].valorTotal;
        console.log('valorImpostos', valorImpostos);
        console.log('valorLiquido', valorLiquido);
        console.log('valorQuitacaoSelecionada:', valorQuitacaoSelecionada);

        if (valorLiquido != valorQuitacaoSelecionada) {            
            abp.notify.warn("O valor a ser conciliado deve ser igual ao valor da quitação.");
            return;
        }

        var quitacaoId = gridConsolidacaoOptions.getSelectedRows()[0].id;        
        var entregaContas = JSON.parse($("#ViewModel").val());

        var input = {
            quitacaoId: quitacaoId,
            dataConsolidado: $("#dataMovimento").val(),
            valorImposto: $("#valorImposto").val(),
            entregaContas: entregaContas
        };

        $('.save-button').prop("disabled", true);
        faturamentoEntregaContaRecebidaAppService.conciliarContasRecebidas(input)
            .done(function (data) {
                if (data != null && data.errors.length > 0) {
                    _ErrorModal.open({ erros: data.errors });
                }
                else {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $(".close-button").click();
                }
            })
            .always(function () {
                $('.save-button').prop("disabled", false);
            });
    })

})(jQuery);


