var _$formFormulaFaturamento = null;

$('#salvar-formula-faturamento').on('click', function (e) {
    e.preventDefault();
    var itemProcessado = false;
    var _$formulaFaturamentoForm = $('form[name="FormulaFaturamentoInformationsForm"]');
    _$formulaFaturamentoForm.validate();
    if (!_$formulaFaturamentoForm.valid()) {
        return;
    }
    var formulaFaturamento = _$formulaFaturamentoForm.serializeFormToObject();

    var formulaFaturamentoList = $('#formula-faturamento-list').val();
    localStorage["FormulaFaturamentoList"] = formulaFaturamentoList;
    if (!localStorage["FormulaFaturamentoList"] || (localStorage["FormulaFaturamentoList"] && localStorage["FormulaFaturamentoList"] == '[]')) {
        localStorage["FormulaFaturamentoList"] = '';
    }
    if (localStorage["FormulaEstoqueList"] != '') {
        var lista = JSON.parse($('#formula-faturamento-list').val());
    }
    else {
        var lista = [];
    }
    if (lista.length > 0) {
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].IdGridFormulasFaturamento == $('#id-grid-formulas-faturamento').val()) {
                //editando o registro
                lista[i].Id = formulaFaturamento.Id;
                lista[i].Codigo = $('#codigo-formula-faturamento').val();
                lista[i].Descricao = $('#descricao-formula-faturamento').val();
                lista[i].FaturamentoItemId = formulaFaturamento.FaturamentoItemId;
                lista[i].MaterialId = formulaFaturamento.MaterialId;
                lista[i].PrescricaoItemId = formulaFaturamento.PrescricaoItemId;
                lista[i].IsFatura = formulaFaturamento.IsFatura;
                lista[i].IdGridFormulasFaturamento = formulaFaturamento.IdGridFormulasFaturamento;
                itemProcessado = true;
                break;
            }
        }
        if (!itemProcessado) {
            formulaFaturamento.IdGridFormulasFaturamento = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridFormulasFaturamento + 1;
            lista.push(formulaFaturamento);
        }
    }
    else {
        formulaFaturamento.IdGridFormulasFaturamento = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridFormulasFaturamento + 1;
        lista.push(formulaFaturamento);
    }
    $('#formula-faturamento-list').val(JSON.stringify(lista));
    localStorage["FormulaFaturamentoList"] = JSON.stringify(lista);
    abp.notify.info(app.localize('ListaAtualizada'));
    $('#FormulasFaturamentosTable').jtable('load', {
        prescricaoItemId: $('#prescricao-item-id').val()
    });
    $('#CreateNewFormulaFaturamentoButton').trigger('click');

});

$('#faturamento-item-id-formula-faturamento').on('change', function (e) {
    //$('#material-id-formula-faturamento').empty();
    var data = $(this).select2('data');
    var txt = '';
    if (data && data.length > 0) {
        txt = data[0].text;
    }
    var arr = txt.split(' - ');
    var codigo = arr[0];
    var texto = '';
    for (var i = 1; i < arr.length; i++) {
        texto += arr[i] + ' ';
    }
    $('#descricao-formula-faturamento').val(texto);
    $('#codigo-formula-faturamento').val(parseInt(codigo));
    if ($('#descricao-prescricao-item').val() === "" || $('#descricao-prescricao-item').val() == undefined) {
        $('#descricao-prescricao-item').val(texto);
        $('#codigo-prescricao-item').val(parseInt(codigo));
    }
}).select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: '/api/services/app/faturamentoitem/ListarFatItemDropdown',
        dataType: 'json',
        delay: 250,
        method: 'Post',
        data: function (params) {
            if (params.page == undefined)
                params.page = '1';
            return {
                search: params.term,
                page: params.page,
                totalPorPagina: 10,
                //filtro: $('#estoque-origem-id-formula-estoque').val()
            };
        },
        processResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.result.items,
                pagination: {
                    more: (params.page * 10) < data.result.totalCount
                }
            };
        },
        cache: true
    },
    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
    minimumInputLength: 0
});
$('.select2').css('width', '100%');