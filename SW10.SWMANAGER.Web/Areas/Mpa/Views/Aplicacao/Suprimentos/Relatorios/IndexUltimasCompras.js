(function() {
    $(function() {
        let _selectedDateRange = {
            startDate: moment().add(-1, "days").startOf('day'),
            endDate: moment().endOf('day')
        };

        const pickerOptions = app.createDateRangePickerOptions();

        const numberMaskTemplate = {
            mask: [
                { mask: '' },
                {
                    mask: 'num %',
                    lazy: false,
                    blocks: {
                        num: {
                            mask: Number,
                            thousandsSeparator: '.',
                            scale: 2,	// digits after decimal
                            signed: true, // allow negative
                            normalizeZeros: true,  // appends or removes zeros at ends
                            radix: ',',  // fractional delimiter
                            padFractionalZeros: true,  // if true, then pads zeros at end to the length of scale
                            allowDecimal: true,

                        }
                    }
                }
            ]
        };

        const variacaoInicial = IMask($("#variacaoInicial")[0], numberMaskTemplate);
        const variacaoFinal = IMask($("#variacaoFinal")[0], numberMaskTemplate);

        $('.date-range-picker').daterangepicker(
            $.extend(true, pickerOptions, _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
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
            
           const data = {
               empresaId: $("#empresa-id").val(),
               dataInicio: _selectedDateRange.startDate,
               dataFinal: _selectedDateRange.endDate,
               grupoId: $("#grupo-id").val(),
               estoqueId: $("#estoque-id").val(),
               produtoId: $("#produto-id").val(),
               produtoDescricao: $("#produto-descricao").val(),
               laboratorioId: $("#laboratorio-id").val(),
               fornecedorId: $("#fornecedor-id").val(),
               rank: $("#rank").val(),
               casasDecimais: $("#casasDecimais").val() || '4',
               variacaoInicial: variacaoInicial.unmaskedValue,
               variacaoFinal: variacaoFinal.unmaskedValue
               
           }
            const dataParam = $.param(data);
            let caminho = `/Mpa/Relatorios/RetornaUltimasCompras?${dataParam}`;
            const urlPath = window.location.href.split(window.location.pathname)[0].split("//")[1];
            $('#dvVisualizar').show();
            setTimeout(() => {
                $('#fVisualizar').attr('src', "//" + urlPath + "/libs/pdfjs/web/viewer.html?file=" + encodeURIComponent(caminho) + "&locale=pt-BR");
            },1000);
            
            
        });
    })
})();