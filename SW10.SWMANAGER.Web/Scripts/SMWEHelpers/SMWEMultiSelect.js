// Multiselect
$('#swmulti').multiSelect(
    {
        dblClick: true,
        cssClass: 'smwe-ms',
        afterSelect: function (values) { },
        selectableHeader: headerEsquerda,
        selectionHeader: headerDireita
    });

SMWETagschamarServico();

function inserirOpcao(item, index) {

    var descritivo = item.nomeFantasia || item.nome || item.Nome || item.descricao;
    //console.log(descritivo);

    $('#swmulti').multiSelect('addOption', {
        value: item.id, text: descritivo
    });
}

$('#filtro-multi').keyup(function () {
    //console.log('k u p');
    filtrar();
});

$('#ms-swmulti').css('width', '100%');
var inp = $('#filtro-multi');
inp.css('width', '45%');
$('#ms-swmulti').prepend('<br><br>');
$('#ms-swmulti').prepend(inp);

function filtrar() {
    var input, filter, ul, li, i;
    input = $('#filtro-multi');
    filter = input.val().toUpperCase();
    ul = $('#ms-swmulti').find('ul:first-of-type');

    ul.find('li').each(function () {
        var current = $(this);
        var span = current.find('span');

        if (span.text().toUpperCase().indexOf(filter) > -1) {
            if (current.hasClass('ms-elem-selectable')) {
                //console.log('eh selectable');
                current.show();
            }
        } else {
            if (current.hasClass('ms-elem-selectable')) {
                current.hide();
            }
        }

        ////console.log('filtrando');

        //var itensSelecionados = $('.ms-elem-selection.ms-selected');
        //itensSelecionados.each(function () {
        //    var este = $(this);
        //    var hiddenInput = este.find('input:first');

        //    //console.log('item selecionado val: ' + este);
        //});

    });
}

function SMWETagsMultiSelecionados(debug) {
  
    var itensSelecionados = $('.ms-elem-selection.ms-selected');
    var objs = [];

    itensSelecionados.each(function () {
        var este = $(this);
        var hiddenInput = este.find('input:first');
        var chk = este.children('input').eq(1);

        var obj = {
            id: hiddenInput.val(),
            checado: chk.is(':checked')
        };

    //    //console.log(JSON.stringify(obj));
        objs.push(obj);
    });

    if (debug) {
        //console.log(JSON.stringify(objs));
    }
  
    return objs;
}

// FIM - Multiselect

// SMWE forms - funcoes auxiliares para set/get elementos de forms e componentes SMWE



// FIM - Inputs