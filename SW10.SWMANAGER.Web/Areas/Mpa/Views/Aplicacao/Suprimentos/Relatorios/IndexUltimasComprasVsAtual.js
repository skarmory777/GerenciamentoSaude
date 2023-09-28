(function() {
    $(function() {
        let _selectedDateRange = {
            startDate: moment().add(-1, "days").startOf('day'),
            endDate: moment().endOf('day')
        };

        let _selectedDateRangeAtual = {
            startDate: moment().add(-1, "days").startOf('day'),
            endDate: moment().endOf('day')
        };

        const pickerOptions = app.createDateRangePickerOptions();

        $('.date-range-picker.base').daterangepicker(
            $.extend(true, pickerOptions, _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });
        $('.date-range-picker.atual').daterangepicker(
            $.extend(true, pickerOptions, _selectedDateRangeAtual),
            function (start, end, label) {
                _selectedDateRangeAtual.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRangeAtual.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });
        $('.date-range-picker').trigger("change");

        aplicarSelect2Padrao();
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        selectSW('#produto-id','/api/services/app/produto/ListarProdutoDropdown')
        selectSW('#grupo-id','/api/services/app/grupo/listarDropdown')
        selectSW('#laboratorio-id','/api/services/app/produtoLaboratorio/listarDropdown')
        selectSW('#fornecedor-id','/api/services/app/fornecedor/ListarDropdownSisFornecedor')
        
        $("#btnVisualizar").on("click", function() {
            if(typeof(_selectedDateRange.startDate) !== "string" ) {
                _selectedDateRange.startDate = _selectedDateRange.startDate.format('YYYY-MM-DDT00:00:00Z');
            }
            if(typeof(_selectedDateRange.endDate) !== "string" ) {
                _selectedDateRange.endDate = _selectedDateRange.endDate.format('YYYY-MM-DDT23:59:59.999Z');
            }

            if(typeof(_selectedDateRangeAtual.startDate) !== "string" ) {
                _selectedDateRangeAtual.startDate = _selectedDateRangeAtual.startDate.format('YYYY-MM-DDT00:00:00Z');
            }
            if(typeof(_selectedDateRangeAtual.endDate) !== "string" ) {
                _selectedDateRangeAtual.endDate = _selectedDateRangeAtual.endDate.format('YYYY-MM-DDT23:59:59.999Z');
            }
            
           const data = {
               empresaId: $("#empresa-id").val(),
               dataInicioBase: _selectedDateRange.startDate,
               dataFimBase: _selectedDateRange.endDate,
               dataInicioAtual: _selectedDateRangeAtual.startDate,
               dataFimAtual: _selectedDateRangeAtual.endDate,
               casasDecimaisCusto: $("#casasDecimaisCusto").val() || '4',
               casasDecimaisVariacao: $("#casasDecimaisVariacao").val() || '2',
               
           }
            const dataParam = $.param(data);
            let caminho = `/Mpa/Relatorios/RetornaUltimasComprasVsAtual?${dataParam}`;
            const urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
            $('#dvVisualizar').show();
            setTimeout(() => {
                $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + encodeURIComponent(caminho) + "&locale=pt-BR");
            },1000);
            
            
        });
    })
})();