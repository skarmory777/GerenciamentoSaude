(function () {
    $(document).ready(function () {
        var contasRecebidasTable = $('#contasRecebidasTable');
        var entregaContaRecebidaService = abp.services.app.faturamentoEntregaContaRecebida;

        contasRecebidasTable.jtable({
            paging: true,
            edit: false,
            create: false,
            selecting: false,
            multiSorting: false,
            
            actions: {
                listAction: {
                    method: entregaContaRecebidaService.listarContasRecebidasPorQuitacao
                }
            },

            fields: {
                Id: {
                    key: true,
                    list: false
                },

                Paciente: {
                    title: app.localize('Paciente'),
                    width: '20%',
                    display: function (data) {
                        console.log(data);
                        if (data.record.paciente) {
                            return (data.record.paciente);
                        }
                    }
                },

                ValorRecebido: {
                    title: app.localize('ValorRecebido'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.valorRecebido) {
                            return posicionarDireita(formatarValor(data.record.valorRecebido));
                        }
                    }
                },

                ValorGlosaRecuperavel: {
                    title: app.localize('GlosaRecuperavel'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.valorGlosaRecuperavel) {
                            return posicionarDireita(formatarValor(data.record.valorGlosaRecuperavel));
                        }
                    }
                },

                ValorGlosaIrrecuperavel: {
                    title: app.localize('GlosaIrrecuperavel'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.valorGlosaIrrecuperavel) {
                            return posicionarDireita(formatarValor(data.record.valorGlosaIrrecuperavel));
                        }
                    }
                }

            }
        });

        function getContasRecebidas(reload) {
            if (reload) {
                contasRecebidasTable.jtable('reload');
            } else {
                    contasRecebidasTable.jtable('load', { QuitacaoId: $("#quitacaoId").val() });
            }
        }

        function formatarValor(valor) {
            if (valor != '' && valor != null) {
                var retorno = valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', '');
                return retorno;
            }
            return '';

        }

        getContasRecebidas();
    });
})();