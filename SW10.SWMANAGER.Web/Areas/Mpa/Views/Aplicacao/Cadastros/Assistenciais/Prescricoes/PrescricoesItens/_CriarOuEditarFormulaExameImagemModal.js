var _$formFormulaExameImagem = null;

$('#salvar-formula-exame-imagem').on('click', function (e) {
    e.preventDefault();
    var itemProcessado = false;
    var _$formulaExameImagemForm = $('form[name="FormulaExameImagemInformationsForm"]');
    _$formulaExameImagemForm.validate();
    if (!_$formulaExameImagemForm.valid()) {
        return;
    }
    var formulaExameImagem = _$formulaExameImagemForm.serializeFormToObject();

    var formulaExameImagemList = $('#formula-exame-imagem-list').val();
    localStorage["FormulaExameImagemList"] = formulaExameImagemList;
    if (!localStorage["FormulaExameImagemList"] || (localStorage["FormulaExameImagemList"] && localStorage["FormulaExameImagemList"] == '[]')) {
        localStorage["FormulaExameImagemList"] = '';
    }
    if (localStorage["FormulaEstoqueList"] != '') {
        var lista = JSON.parse($('#formula-exame-imagem-list').val());
    }
    else {
        var lista = [];
    }
    if (lista.length > 0) {
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].IdGridFormulasExameImagem == $('#id-grid-formulas-exame-imagem').val()) {
                //editando o registro
                lista[i].Id = formulaExameImagem.Id;
                lista[i].Codigo = $('#codigo-formula-exame-imagem').val();
                lista[i].Descricao = $('#descricao-formula-exame-imagem').val();
                lista[i].ExameImagemItemId = formulaExameImagem.ExameImagemItemId;
                lista[i].MaterialId = formulaExameImagem.MaterialId;
                lista[i].PrescricaoItemId = formulaExameImagem.PrescricaoItemId;
                lista[i].IsFatura = formulaExameImagem.IsFatura;
                lista[i].IdGridFormulasExameImagem = formulaExameImagem.IdGridFormulasExameImagem;
                itemProcessado = true;
                break;
            }
        }
        if (!itemProcessado) {
            formulaExameImagem.IdGridFormulasExameImagem = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridFormulasExameImagem + 1;
            lista.push(formulaExameImagem);
        }
    }
    else {
        formulaExameImagem.IdGridFormulasExameImagem = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridFormulasExameImagem + 1;
        lista.push(formulaExameImagem);
    }
    $('#formula-exame-imagem-list').val(JSON.stringify(lista));
    localStorage["FormulaExameImagemList"] = JSON.stringify(lista);
    abp.notify.info(app.localize('ListaAtualizada'));
    $('#FormulasExamesImagensTable').jtable('load', {
        prescricaoItemId: $('#prescricao-item-id').val()
    });
    $('#CreateNewFormulaExameImagemButton').trigger('click');

});

$('#faturamento-item-id-formula-exame-imagem').on('change', function (e) {
    //$('#material-id-formula-exame-imagem').empty();
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
    $('#descricao-formula-exame-imagem').val(texto);
    $('#codigo-formula-exame-imagem').val(parseInt(codigo));
    if ($('#descricao-prescricao-item').val() === "" || $('#descricao-prescricao-item').val() == undefined) {
        $('#descricao-prescricao-item').val(texto);
        $('#codigo-prescricao-item').val(parseInt(codigo));
    }
}).select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: '/api/services/app/faturamentoitem/ListarExameImagemDropdown',
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