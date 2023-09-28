(function () {
    $(function () {
        var _$FaturamentosTable = $('#FaturamentosTable');
        var _$FaturamentosFilterForm = $('#FaturamentosFilterForm');
        var _DashBoard = abp.services.app.dashboard;
        //        var strLinhasFaturamentoAberto = JSON.parse('["ValorEntrega", "ValorQuitacaoAmbulatorio", "ValorQuitacaoInternacao", "ValorDifEntregaGlosa"]');
        var strLinhasFaturamentoAberto = JSON.parse('["ValorEntrega", "ValorLancamentoAberto", "ValorGlosa", "ValorDifEntregaGlosa"]');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.TipoAtendimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.TipoAtendimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.TipoAtendimento.Delete')
        };
        var now = new Date;
        var meses = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
        var primeiroMes = now.getMonth();
        var segundoMes = new Date(now.setMonth(now.getMonth() - 1)).getMonth();
        var terceiroMes = new Date(now.setMonth(now.getMonth() - 1)).getMonth();
        var quartoMes = new Date(now.setMonth(now.getMonth() - 1)).getMonth();
        var quintoMes = new Date(now.setMonth(now.getMonth() - 1)).getMonth();
        var sextoMes = new Date(now.setMonth(now.getMonth() - 1)).getMonth();

        _$FaturamentosTable.jtable({
            title: app.localize('FaturamentoAbertoSeisMeses'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _DashBoard.listarFaturamentoAbertoSeisMeses
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                convenio: {
                    title: app.localize('Convenio'),
                    width: '30%'
                },
                primeiroMes: {
                    title: app.localize(meses[primeiroMes]),
                    width: '10%',
                    display: function (data) {
                        return data.record.primeiroMes.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
                    }
                },
                segundoMes: {
                    title: app.localize(meses[segundoMes]),
                    width: '10%',
                    display: function (data) {
                        return data.record.segundoMes.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
                    }
                },
                terceiroMes: {
                    title: app.localize(meses[terceiroMes]),
                    width: '10%',
                    display: function (data) {
                        return data.record.terceiroMes.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
                    }
                },
                quartoMes: {
                    title: app.localize(meses[quartoMes]),
                    width: '10%',
                    display: function (data) {
                        return data.record.quartoMes.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
                    }
                },
                quintoMes: {
                    title: app.localize(meses[quintoMes]),
                    width: '10%',
                    display: function (data) {
                        return data.record.quintoMes.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
                    }
                },
                sextoMes: {
                    title: app.localize(meses[sextoMes]),
                    width: '10%',
                    display: function (data) {
                        return data.record.sextoMes.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
                    }
                },
                valorTotal: {
                    title: app.localize('Total'),
                    width: '20%',
                    display: function (data) {
                        return data.record.valorTotal.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
                    }
                }
            }
        });

        function getFaturamentos(reload) {
            if (reload) {
                _$FaturamentosTable.jtable('reload');
            } else {
                _$FaturamentosTable.jtable('load', {
                    filtro: $('#FaturamentosTableFilter').val(),
                    empresaId: $('#cbo-empresas').val()
                });
            }
        }

        $('#GetFaturamentosButton, #RefreshFaturamentosListButton').click(function (e) {
            e.preventDefault();
            getFaturamentos();
        });

        function loadGraficoFaturamentoEntregue() {
            $.get('/mpa/dashboard/ListarFaturamentoEntregue', function (result) {
                $('#faturamento_entregue').html('');
                Morris.Line({
                    element: 'faturamento_entregue',
                    padding: 0,
                    parseTime: false,
                    //behaveLikeLine: false,
                    //gridEnabled: true,
                    //gridLineColor: false,
                    //axes: false,
                    fillOpacity: 1,
                    data: result,
                    lineColors: ['#ed6b75', '#659be0', '#F1C40F', 'green'],
                    labels: [app.localize('ValorEntregue'), app.localize('ValorRecebido')],
                    xkey: 'AnoMes',
                    ykeys: ["ValorTotalEntregue", "ValorTotalRecebido"],
                    //pointSize: 0,
                    //lineWidth: 0,
                    hideHover: 'auto',
                    resize: true
                });
            });
        };

        function loadGraficoFaturamentoAberto() {
            $.get('/mpa/dashboard/ListarFaturamentoAberto', function (result) {
                $('#faturamento_aberto').html('');
                Morris.Line({
                    element: 'faturamento_aberto',
                    padding: 0,
                    parseTime: false,
                    //behaveLikeLine: false,
                    //gridEnabled: true,
                    //gridLineColor: false,
                    //axes: false,
                    fillOpacity: 1,
                    data: result,
                    lineColors: ['#ed6b75', '#659be0', '#F1C40F', 'green'],
                    //labels: [app.localize('ValorEntrega'), app.localize('ValorQuitacaoAmbulatorio'), app.localize('ValorQuitacaoInternacao'), app.localize('ValorDifEntregaGlosa')],
                    labels: [app.localize('ValorEntrega'), app.localize('ValorLancamentoAberto'), app.localize('ValorGlosa'), app.localize('ValorDifEntregaGlosa')],
                    xkey: 'AnoMesVenc',
                    ykeys: strLinhasFaturamentoAberto,
                    //pointSize: 0,
                    //lineWidth: 0,
                    hideHover: 'auto',
                    resize: true
                });
            });
        };

        function loadGraficoFaturamentoRecebido() {
            var campo1, campo2, campo3, campo4;

            campo1 = app.localize("ValorQuitacaoInternacao");
            campo2 = app.localize("ValorQuitacaoLancamento");
            campo3 = app.localize('ValorQuitacaoAmbulatorio');
            campo4 = app.localize('ValorQuitacaoSemIdentificacao');

            var jsonLinhas = JSON
                .parse(
                    getChkLinhasTrocaSelecao('#chkRecebimentoInternacao'
                    , '"ValorQuitacaoInternacao","ValorQuitacaoLancamento"'
                    , '#chkRecebimentoAmbulatorio'
                    , '"ValorQuitacaoAmbulatorio","ValorQuitacaoSemIdentificacao"'
                    )
                );

            campo1 = app.localize("ValorInternacao");
            campo2 = app.localize("ValorLancamento");
            campo3 = app.localize('ValorAmbulatorio');
            campo4 = app.localize('ValorSemIdentificacao');

            var jsonLabels = JSON
                .parse(
                    getChkLinhasTrocaSelecao('#chkRecebimentoInternacao'
                    , '"' + campo1 + '", "' + campo2 + '"'
                    , '#chkRecebimentoAmbulatorio'
                    , '"' + campo3 + '", "' + campo4 + '"'
                    )
                );

            $.get('/mpa/dashboard/ListarFaturamentoRecebimento', function (result) {
                $('#faturamento_recebimento').html('');
                Morris.Line({
                    element: 'faturamento_recebimento',
                    padding: 0,
                    parseTime: false,
                    //behaveLikeLine: false,
                    //gridEnabled: true,
                    //gridLineColor: false,
                    //axes: false,
                    fillOpacity: 1,
                    data: result,
                    lineColors: ['#ed6b75', '#659be0', '#F1C40F', 'green'],
                    labels: jsonLabels,
                    xkey: 'AnoMesPG',
                    ykeys: jsonLinhas,
                    //pointSize: 0,
                    //lineWidth: 0,
                    hideHover: 'auto',
                    resize: true
                });
            });
        };

        function getChkLinhasFatEntregueXRecebido() {
            //"ValorTotal"
            var strEntregueXrecebido = ""
            if ($('#chkEntregue').is(':checked')) {
                strEntregueXrecebido += '"ValorTotalEntregue"';
            };
            if ($('#chkRecebido').is(':checked')) {
                if (strEntregueXrecebido != "") {
                    strEntregueXrecebido += ',';
                };
                strEntregueXrecebido += '"ValorTotalRecebido"';
            };

            if (!($('#chkEntregue').is(':checked')) && !($('#chkRecebido').is(':checked'))) {
                strEntregueXrecebido = '"ValorTotalEntregue", "ValorTotalRecebido"'
            }

            return "[" + strEntregueXrecebido + "]";

        };

        function getChkLinhasTrocaSelecao(ckBox1, campos1, ckBox2, campos2) {
            //"ValorTotal"
            //ckBox1='#chkEntregue'
            //campos1 = '"ValorTotalEntregue"'
            //ckBox2,'#chkRecebido'
            //campos2='"ValorTotalRecebido"'

            var strResult = ""

            if ($(ckBox1).is(':checked')) {
                strResult += campos1; //'"ValorTotalEntregue"';
            };
            if ($(ckBox2).is(':checked')) {
                if (strResult != "") {
                    strResult += ',';
                };
                strResult += campos2; //'"ValorTotalRecebido"';
            };

            if (!($(ckBox1).is(':checked')) && !($(ckBox2).is(':checked'))) {
                strResult = campos1 + ',' + campos2 //'"ValorTotalEntregue", "ValorTotalRecebido"'
            }

            return "[" + strResult + "]";

        };


        $('#chkRecebimentoInternacao').click(function () {
            //alert($('#cbInternacao').is(':checked'));
            loadGraficoFaturamentoRecebido();
        });

        $('#chkRecebimentoAmbulatorio').click(function () {
            //alert($('#cbInternacao').is(':checked'));
            loadGraficoFaturamentoRecebido();
        });

        $('#GetDashboardButton').click(function (e) {
            e.preventDefault();
            getTabelaPreenchida();
        });

        $('#refresh-relatorio-despesas').on('click', function (e) {
            e.preventDefault();
            if ($('#show-relatorio-despesas').hasClass('expand')) {
                $('#show-relatorio-despesas').trigger('click');
            }
            refreshRelatorioDespesas();
        });

        $('#refresh-grid-faturamento').on('click', function (e) {
            e.preventDefault();
            if ($('#show-grid-faturamento').hasClass('expand')) {
                $('#show-grid-faturamento').trigger('click');
            }
            getFaturamentos();
        });

        $('#refresh-graficos').on('click', function (e) {
            e.preventDefault();
            if ($('#show-graficos').hasClass('expand')) {
                $('#show-graficos').trigger('click');
            }
            atualizarGraficos();
        });

        function atualizarGraficos() {
            loadGraficoFaturamentoEntregue();
            loadGraficoFaturamentoAberto();
            loadGraficoFaturamentoRecebido();
        }

        //window.setInterval(refreshRelatorioDespesas, 60000);
        //atualiza os gráficos na inicialização

        //loadGraficoFaturamentoEntregue();
        //loadGraficoFaturamentoAberto();
        //loadGraficoFaturamentoRecebido();

        //getFaturamentos();

        //$(function () {
        //    var _tenantDashboardService = abp.services.app.tenantDashboard;

        //();

        //faturamento_aberto

        //faturamento_recebimento


        //var getMemberActivity = function () {
        //    _tenantDashboardService
        //        .getMemberActivity({})
        //        .done(function (result) {
        //            $("#totalMembersChart").sparkline(result.totalMembers, {
        //                type: 'bar',
        //                width: '100',
        //                barWidth: 6,
        //                height: '45',
        //                barColor: '#F36A5B',
        //                negBarColor: '#e02222',
        //                chartRangeMin: 0
        //            });

        //            $("#newMembersChart").sparkline(result.newMembers, {
        //                type: 'bar',
        //                width: '100',
        //                barWidth: 6,
        //                height: '45',
        //                barColor: '#5C9BD1',
        //                negBarColor: '#e02222',
        //                chartRangeMin: 0
        //            });
        //        });
        //};

        //$('#RefreshMemberActivityButton').click(function () {
        //    getMemberActivity();
        //});

        //getMemberActivity();

        //});
        //getFaturamentos();
        aplicarSelect2Padrao();
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };

    });
})();