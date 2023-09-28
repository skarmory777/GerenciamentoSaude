var _$formFormulaExameLaboratorial = null;

$('#salvar-formula-exame-laboratorial').on('click', function (e) {
    e.preventDefault();
    var itemProcessado = false;
    var _$formulaExameLaboratorialForm = $('form[name="FormulaExameLaboratorialInformationsForm"]');
    _$formulaExameLaboratorialForm.validate();
    if (!_$formulaExameLaboratorialForm.valid()) {
        return;
    }
    var formulaExameLaboratorial = _$formulaExameLaboratorialForm.serializeFormToObject();

    var formulaExameLaboratorialList = $('#formula-exame-laboratorial-list').val();
    localStorage["FormulaExameLaboratorialList"] = formulaExameLaboratorialList;
    if (!localStorage["FormulaExameLaboratorialList"] || (localStorage["FormulaExameLaboratorialList"] && localStorage["FormulaExameLaboratorialList"] == '[]')) {
        localStorage["FormulaExameLaboratorialList"] = '';
    }
    if (localStorage["FormulaEstoqueList"] != '') {
        var lista = JSON.parse($('#formula-exame-laboratorial-list').val());
    }
    else {
        var lista = [];
    }
    if (lista.length > 0) {
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].IdGridFormulasExameLaboratorial == $('#id-grid-formulas-exame-laboratorial').val()) {
                //editando o registro
                lista[i].Id = formulaExameLaboratorial.Id;
                lista[i].Codigo = $('#codigo-formula-exame-laboratorial').val();
                lista[i].Descricao = $('#descricao-formula-exame-laboratorial').val();
                lista[i].ExameLaboratorialItemId = formulaExameLaboratorial.ExameLaboratorialItemId;
                lista[i].MaterialId = formulaExameLaboratorial.MaterialId;
                lista[i].PrescricaoItemId = formulaExameLaboratorial.PrescricaoItemId;
                lista[i].IsFatura = formulaExameLaboratorial.IsFatura;
                lista[i].IdGridFormulasExameLaboratorial = formulaExameLaboratorial.IdGridFormulasExameLaboratorial;
                itemProcessado = true;
                break;
            }
        }
        if (!itemProcessado) {
            formulaExameLaboratorial.IdGridFormulasExameLaboratorial = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridFormulasExameLaboratorial + 1;
            lista.push(formulaExameLaboratorial);
        }
    }
    else {
        formulaExameLaboratorial.IdGridFormulasExameLaboratorial = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridFormulasExameLaboratorial + 1;
        lista.push(formulaExameLaboratorial);
    }
    $('#formula-exame-laboratorial-list').val(JSON.stringify(lista));
    localStorage["FormulaExameLaboratorialList"] = JSON.stringify(lista);
    abp.notify.info(app.localize('ListaAtualizada'));
    $('#FormulasExamesLaboratoriaisTable').jtable('load', {
        prescricaoItemId: $('#prescricao-item-id').val()
    });
    $('#CreateNewFormulaExameLaboratorialButton').trigger('click');

});


$('#faturamento-item-id-formula-exame-laboratorial').on('change', function (e) {
    //$('#material-id-formula-exame-laboratorial').empty();
    var data = $(this).select2('data');
    var txt = '';
    if (data && data.length>0) {
        txt = data[0].text;
    }
    var arr = txt.split(' - ');
    var codigo = arr[0];
    var texto = '';
    for (var i = 1; i < arr.length; i++) {
        texto += arr[i] + ' ';
    }
    $('#descricao-formula-exame-laboratorial').val(texto);
    $('#codigo-formula-exame-laboratorial').val(parseInt(codigo));
    if ($('#descricao-prescricao-item').val() === "" || $('#descricao-prescricao-item').val() == undefined) {
        $('#descricao-prescricao-item').val(texto);
        $('#codigo-prescricao-item').val(parseInt(codigo));
    }
}).select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: '/api/services/app/faturamentoitem/ListarExameLaboratorialDropdown',
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