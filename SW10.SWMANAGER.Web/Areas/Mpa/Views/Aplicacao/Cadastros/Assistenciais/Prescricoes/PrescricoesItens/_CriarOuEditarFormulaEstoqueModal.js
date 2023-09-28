var _$formFormulaEstoque = null;





$('#salvar-formula-estoque').on('click', function (e) {
    e.preventDefault();
    var itemProcessado = false;
    var _$formulaEstoqueForm = $('form[name="FormulaEstoqueInformationsForm"]');
    _$formulaEstoqueForm.validate();
    if (!_$formulaEstoqueForm.valid()) {
        return;
    }
    var formulaEstoque = _$formulaEstoqueForm.serializeFormToObject();

    var formulaEstoqueList = $('#formula-estoque-list').val();
    localStorage["FormulaEstoqueList"] = formulaEstoqueList;
    if (!localStorage["FormulaEstoqueList"] || (localStorage["FormulaEstoqueList"] && localStorage["FormulaEstoqueList"] == '[]')) {
        localStorage["FormulaEstoqueList"] = '';
    }
    if (localStorage["FormulaEstoqueList"] != '') {
        var lista = JSON.parse(localStorage["FormulaEstoqueList"]);
    }
    else {
        var lista = [];
    }
    //if ($('#id-grid-formulas-estoque').val() != '' && $('#formula-estoque-list').val() != '[]') {
    if (lista.length > 0) {
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].IdGridFormulasEstoque == $('#id-grid-formulas-estoque').val()) {
                //editando o registro
                lista[i].Id = formulaEstoque.Id;
                lista[i].Codigo = $('#codigo-formula-estoque').val();
                lista[i].Descricao = $('#descricao-formula-estoque').val();
                lista[i].EstoqueId = formulaEstoque.EstoqueId;
                lista[i].ProdutoId = formulaEstoque.ProdutoId;
                lista[i].UnidadeId = formulaEstoque.UnidadeId;
                lista[i].PrescricaoItemId = formulaEstoque.PrescricaoItemId;
                lista[i].IsPrincipal = formulaEstoque.IsPrincipal;
                lista[i].IdGridFormulasEstoque = formulaEstoque.IdGridFormulasEstoque;
                itemProcessado = true;
                break;
            }
        }
        if (!itemProcessado) {
            formulaEstoque.IdGridFormulasEstoque = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridFormulasEstoque + 1;
            lista.push(formulaEstoque);
        }
    }
    else {
        formulaEstoque.IdGridFormulasEstoque = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridFormulasEstoque + 1;
        lista.push(formulaEstoque);
    }

    $('#formula-estoque-list').val(JSON.stringify(lista));
    localStorage["FormulaEstoqueList"] = JSON.stringify(lista);
    abp.notify.info(app.localize('ListaAtualizada'));
    //abp.event.trigger('app.CriarOuEditarFormulaEstoqueModalSaved');
    $('#FormulasEstoquesTable').jtable('load', {
        prescricaoItemId: $('#prescricao-item-id').val()
    });
    //getFormulasEstoques();
    $('#CreateNewFormulaEstoqueButton').trigger('click');
});

$('#estoque-origem-id-formula-estoque').on('change', function (e) {
    $('#produto-id-formula-estoque').empty();
    $('#unidade-requisicao-id-formula-estoque').empty();
});

$('#unidade-requisicao-id-formula-estoque').on('change', function (e) {
    var data = $(this).select2('data');
    var txt = '';
    if (data && data.length > 0) {
        txt = data[0].text;
        var cod = $(this).val();
        $('<option value=' + cod + '>' + txt + '</option>').appendTo($('#unidade-requisicao-id-prescricao-item'));
        $('#unidade-requisicao-id-prescricao-item').val(cod).change();
    }
}).select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: '/api/services/app/produtounidade/ListarUnidadePorProdutoDropdown',
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
                filtro: $('#produto-id-formula-estoque').val()
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

$('#unidade-id-formula-estoque').on('change', function (e) {
    var data = $(this).select2('data');
    var txt = '';
    if (data && data.length > 0) {
        txt = data[0].text;
        var cod = $(this).val();
        $('<option value=' + cod + '>' + txt + '</option>').appendTo($('#unidade-id-prescricao-item'));
        $('#unidade-id-prescricao-item').val(cod).change();
    }
}).select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: '/api/services/app/produtounidade/ListarUnidadePorProdutoDropdown',
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
                filtro: $('#produto-id-formula-estoque').val()
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

$('#produto-id-formula-estoque').on('change', function (e) {
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
    //$('#descricao-prescricao-item').val($('#produto-id-formula-estoque').val());
    $('#descricao-formula-estoque').val(texto);
    $('#codigo-formula-estoque').val(codigo);
    if ($('#descricao-prescricao-item').val() === "" || $('#descricao-prescricao-item').val() == undefined) {
        $('#descricao-prescricao-item').val(texto);
        $('#codigo-prescricao-item').val(codigo);
    }
}).select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: '/api/services/app/produto/listardropdown',
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
                filtro: $('#estoque-origem-id-formula-estoque').val()
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

