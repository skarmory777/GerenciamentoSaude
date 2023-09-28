(function() {
    $(function() {
        let _selectedDateRange = {
            startDate: moment().add(-1, "days").startOf('day'),
            endDate: moment().endOf('day'),
            timePicker: true,
            timePicker24Hour: true,
            timePickerSeconds: true,
            locale: {
                format: 'DD/MM/YYYY hh:mm:ss'
            }
        };

        const pickerOptions = app.createDateRangePickerOptions();

        $('.date-range-picker').daterangepicker(
            $.extend(true, pickerOptions, _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDTHH:mm:ssZ');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDTHH:mm:ssZ');
            });
        $('.date-range-picker').trigger("change");

        aplicarSelect2Padrao();
        selectSW('#unidade-organizacional-id', "/api/services/app/UnidadeOrganizacional/ListarDropdownPorUsuario");
        selectSW('.selectEmpresa', '/api/services/app/empresa/ListarDropdownPorUsuario');
        
        $("#btnVisualizar").on("click", function () {
            debugger;
            if(typeof(_selectedDateRange.startDate) !== "string" ) {
                _selectedDateRange.startDate = _selectedDateRange.startDate.format('YYYY-MM-DDTHH:mm:ssZ');
            }
            if(typeof(_selectedDateRange.endDate) !== "string" ) {
                _selectedDateRange.endDate = _selectedDateRange.endDate.format('YYYY-MM-DDTHH:mm:ssZ');
            }
            
            const data = {
               unidadeId: $('#unidade-organizacional-id').val() ?? 0,
               empresaId: $('.selectEmpresa').val() ?? 0,
               dataInicio: _selectedDateRange.startDate,
               dataFinal: _selectedDateRange.endDate,
            }
            const dataParam = $.param(data);
            let caminho = `/Mpa/Relatorios/RetornaMapaDispensacao?${dataParam}`;
            const urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
            $('#dvVisualizar').show();
            setTimeout(() => {
                $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + encodeURIComponent(caminho) + "&locale=pt-BR");
            },1000);
            
            
        });
    })
})();