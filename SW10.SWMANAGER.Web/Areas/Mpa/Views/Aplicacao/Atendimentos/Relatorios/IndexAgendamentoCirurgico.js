(function () {
    $(function () {
        var _$filterForm = $('#AgendamentoCirurgicoFilterForm');

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
            if (_$filterForm.valid()) {
                createRequestParams();
                var vData = $('#periodo').val().split(' - ');
                var dataInicial = vData[0];
                var dataFinal = vData[1];
                debugger;
                var path = encodeURIComponent(`/mpa/AtendimentoRelatorio/ImprimiRelatorioAgendamentoCirurgico?dataInicial=${moment(dataInicial, "DD/MM/YYYY").format("YYYY-MM-DD")}&dataFinal=${moment(dataFinal, "DD/MM/YYYY").format("YYYY-MM-DD")}`);
                var urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
                $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + path + "&locale=pt-BR");
                $('#dvVisualizar').show();
            }
        }

        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

        $('.select2').css('width', '100%');

        CamposRequeridos();
    });
})();