(function () {
    $(function () {
        $(document).ready(function () {
        });

        var _$filterForm = $('#ContaPagarFilterForm');

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
            exibirRelatorio();
        });

        function exibirRelatorio() {
            createRequestParams();
            var caminho = $('#tipo-rel').val() == 1 ? '/mpa/contaspagar/VisualizarRptContaPagarResumidoPDF' : '/mpa/contaspagar/VisualizarRptContaPagarDetalhadoPDF';
            var vData = $('#periodo').val().split(' - ');
            var dataIni = vData[0];
            debugger
            var dataFim = vData[1];
            $.ajax({
                url: caminho,
                method: 'post',
                data: {
                    EmpresaId: $('#empresa-id').val() != null ? $('#empresa-id').val() : 0,
                    StartDate: dataIni, //_selectedDateRange.startDate.format('L'),
                    EndDate: dataFim, //_selectedDateRange.endDate.format('L'),
                    TipoPessoaId: $('#tipo-pessoa-id').val() != null ? $('#tipo-pessoa-id').val() : 0,
                    PessoaId: $('#pessoa-id').val() != null ? $('#pessoa-id').val() : 0,
                    SituacaoNotaFiscalId: $('#situacao-nota-fiscal-id').val() != null ? $('#situacao-nota-fiscal-id').val() : 0,
                    Situacao: $('#situacao').val() != null ? $('#situacao').val() : 0,
                    TipoData: $('#tipo-data').val() != null ? $('#tipo-data').val() : 0,
                    TipoRel: $('#tipo-rel').val(),
                    TipoDocumentoId: $('#tipo-documento-id').val() != null ? $('#tipo-documento-id').val() : 0,
                    IsCredito: $('#is-credito').val()
                },
                cache: false,
                async: false,
                beforeSend: function () {
                    abp.ui.setBusy();
                },
                complete: function () {
                    abp.ui.clearBusy();
                },
                error: function (msg) {
                   
                    alert('Erro na exibição do relatorio', msg.responseText);
                    abp.ui.clearBusy();
                },
                fail: function () {
                    abp.ui.clearBusy();
                },
                success: function (data) {
                    var path = data;
                    var urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
                    $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + path + "&locale=pt-BR");
                    $('#dvVisualizar').show();
                }
            });
        }

        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

        $('.select2').css('width', '100%');

        aplicarSelect2Padrao();
        aplicarDateRange();
    });
})();