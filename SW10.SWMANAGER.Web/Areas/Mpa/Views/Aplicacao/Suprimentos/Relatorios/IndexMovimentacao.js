(function () {
    $(function () {
        /*var urlGrupoClasse = "@Url.Action("ListarGrupoClasse")";
    var urlGrupoSubClasse = "@Url.Action("ListarGrupoSubClasse")";
    var urlVisualizar = "@Url.Action("Visualizar")";
    var urlExportar = "@Url.Action("Exportar")";

    var grupo = $("#GrupoProduto");
    var classe = $("#Classe");
    var subClasse = $("#SubClasse");
    var painel = $("#dvVisualizar");
    painel.hide();*/
        $(document).ready(function () {
            selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");

            if ($('#tipo-rel').val() == 1) {
                $('#div-lote').hide();
            }
            else {
                $('#div-lote').show();
            }
        });

        $('#tipo-rel').on('change', function (e) {
            if ($(this).val() == 1) {
                $('#div-lote').hide();
            }
            else {
                $('#div-lote').show();
            }
        });

        var _$filterForm = $('#MovimentacaoFilterForm');

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };
        //_$filterForm.find('.date-range-picker').daterangepicker(
        $('#periodo').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        $("#btnVisualizar").click(function (e) {
            e.preventDefault();
            abp.ui.setBusy();
            exibirRelatorio();
            abp.ui.clearBusy();
        });

        function exibirRelatorio() {
            createRequestParams();
            //var caminho = $('#tipo-rel').val() == 1 ? '@Url.Action("VisualizarEstoqueMovimentoResumidoPDF", "Produtos")' : '@Url.Action("VisualizarEstoqueMovimentoDetalhadoPDF", "Produtos")';
            //var caminho = $('#tipo-rel').val() == 1 ? '/mpa/produtos/visualizarestoquemovimentoresumidopdf' : '/mpa/produtos/visualizarestoquemovimentodetalhadopdf';
            //caminho += '/?empresaId=' + $('#empresa-id').val();
            //caminho += '&startDate=' + moment(_selectedDateRange.startDate).format('L');
            //caminho += '&endDate=' + moment(_selectedDateRange.endDate).format('L');
            //caminho += '&grupoId=';
            //caminho += $('#grupo-id').val() != null ? $('#grupo-id').val() : 0;
            //caminho += "&produto='" + $('#produto').val() + "'";
            //caminho += "&lote='" + $('#lote').val() + "'";
            //caminho += "&tipoRel=" + $('#tipo-rel').val();
            //caminho += '&ClasseId=' + $('#classe-id').val();
            ////caminho += '&SubclasseId=' + $('#subclasse-id').val();

            //PDFObject.embed(caminho, "#relatorio-leitos");
            //$('#relatorio-leitos').load('@Url.Action("_Viewer","Produtos",new { path = Url.Action("VisualizarSaldoProdutoPdf","Produtos",new { EmpresaId=1,EstoqueId=0,GrupoId=0}) })');
            //$('#div-relatorio').load('/mpa/produtos/_viewer?path=' + caminho);
            var vData = $('#periodo').val().split(' - ');
            var dataIni = vData[0];
            var dataFim = vData[1];
            $.ajax({
                url: '/mpa/produtos/RelatorioEstoqueMovimento',
                method: 'post',
                data: {
                    EmpresaId: $('#EmpresaId').val() != null ? $('#EmpresaId').val() : 0,
                    EstoqueId: $('#estoque-id').val() != null ? $('#estoque-id').val() : 0,
                    StartDate: dataIni, //_selectedDateRange.startDate.format('L'),
                    EndDate: dataFim, //_selectedDateRange.endDate.format('L'),
                    GrupoId: $('#grupo-id').val() != null ? $('#grupo-id').val() : 0,
                    Produto: $('#produto').val(),
                    Lote: $('#lote').val(),
                    TipoRel: $('#tipo-rel').val()
                },
                cache: false,
                async: false,
                //beforeSend: function () {
                //    abp.ui.setBusy();
                //},
                //complete: function () {
                //    abp.ui.clearBusy();
                //},
                //error: function () {
                //    abp.ui.clearBusy();
                //},
                //fail: function () {
                //    abp.ui.clearBusy();
                //},
                success: function (data) {
                    debugger;
                    var path = data;
                    var urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
                    $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + path + "&locale=pt-BR");
                    $('#dvVisualizar').show();
                }
            });
        }

        /*//Exportar arquivos Word, PDF, Excel
        $(".btnExportar").click(function () {
            window.open(urlExportar + montarParam() + "&formato=" + $(this).data("formato"));
        });


    grupo.change(function () {
        carregar(urlGrupoClasse + "/" + $(this).val(), classe);
    });

    classe.change(function () {
        carregar(urlGrupoSubClasse + "/" + $(this).val(), $("#SubClasse"));
    });

    function carregar(url, destino) {
        $.get(url, function (result) {
            if (result) {
                destino.html("");
                for (var i in result) {
                    destino.append(new Option(result[i].Text, result[i].Value));
                }
            }
        });
    }

    function montarParam() {
        return "?GrupoProduto=" + grupo.val() + "&Classe=" + classe.val() + "&SubClasse=" + subClasse.val() + "&EhMovimentacao=" + "@Model.EhMovimentacao.ToString()";
    }*/



        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAssistencialAtendimentosFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAssistencialAtendimentosFiltersArea').slideUp();
        });

        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

        $('.select2').css('width', '100%');

        aplicarSelect2Padrao();
        aplicarDateRange();
    });
})();