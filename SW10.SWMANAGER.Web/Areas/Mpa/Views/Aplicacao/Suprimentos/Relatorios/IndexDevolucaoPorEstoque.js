(function() {
    $(function() {
        let _selectedDateRange = {
            startDate: moment().add(-1, "days").startOf('day'),
            endDate: moment().endOf('day')
        };

        const pickerOptions = app.createDateRangePickerOptions();

        $('.date-range-picker').daterangepicker(
            $.extend(true, pickerOptions, _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });
        $('.date-range-picker').trigger("change");

        aplicarSelect2Padrao();
        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdownPorUsuario");
        
        
        $("#btnVisualizar").on("click", function() {
            if(typeof(_selectedDateRange.startDate) !== "string" ) {
                _selectedDateRange.startDate = _selectedDateRange.startDate.format('YYYY-MM-DDT00:00:00Z');
            }
            if(typeof(_selectedDateRange.endDate) !== "string" ) {
                _selectedDateRange.endDate = _selectedDateRange.endDate.format('YYYY-MM-DDT23:59:59.999Z');
            }
            
           const data = {
               dataInicio: _selectedDateRange.startDate,
               dataFinal: _selectedDateRange.endDate,
               pacienteId: $("#paciente-id").val(),
               empresaId: $("#empresa-id").val(),
               estoqueId: $("#estoque-id").val()
           }
            const dataParam = $.param(data);
            let caminho = `/Mpa/Relatorios/RetornaDevolucaoPorEstoque?${dataParam}`;
            const urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
            $('#dvVisualizar').show();
            setTimeout(() => {
                $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + encodeURIComponent(caminho) + "&locale=pt-BR");
            },1000);
            
            
        });
    })
})();