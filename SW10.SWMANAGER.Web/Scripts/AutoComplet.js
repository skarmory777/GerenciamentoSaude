




function CriarAutoComplete(idSearch, idCampo, url, cadastro) {

    var search = '#' + idSearch;
    var campo = '#' + idCampo;

    $(search)
    .autocomplete({
        minLength: 3,
        delay: 0,
        source: function (request, response) {
            var term = $(search).val();
            // var url = ;
            var fullUrl = url + '/?term=' + term;
            $.getJSON(fullUrl, function (data) {
                if (data.length == 0) {
                    $(campo).val(0);
                    $(search).focus();
                    abp.notify.info(app.localize("ListaVazia"));
                    return false;
                };
                response($.map(data, function (item) {
                    $(campo).val(0);
                    return {
                        label: item.Nome,
                        value: item.Nome,
                        realValue: item.Id,

                    };
                }));
            });
        },
        select: function (event, ui) {
            $(campo).val(ui.item.realValue);
            $(search).val(ui.item.value);
            //$('.save-button').removeAttr('disabled');
            return false;
        },
        change: function (event, ui) {
            event.preventDefault();
            if (ui.item == null) {
                //$('.save-button').attr('disabled', 'disabled');
                $(campo).val(0);
                $(search).val('').focus();
                abp.notify.info(app.localize("AutoConpletInvalido").replace('$cadastro', cadastro));
                return false;
            }
        },
    });

}










//function CriarAutoComplete(idSearch, idCampo, url, cadastro) {

//    var search = '#' + idSearch;
//    var campo = '#' + idCampo;

   

//    $(search)
//    .autocomplete({
//        minLength: 3,
//        delay: 0,
//        source: function (request, response) {
//            var term = $(search).val();
//            // var url = ;
//            var fullUrl = url + '/?term=' + term;
//            $.getJSON(fullUrl, function (data) {
//                if (data.length == 0) {
//                    $(campo).val(0);
//                    $(search).focus();
//                    abp.notify.info(app.localize("ListaVazia"));
//                    return false;
//                };
//                response($.map(data, function (item) {
//                    $(campo).val(0);
//                    return {
//                        label: item.Nome,
//                        value: item.Nome,
//                        realValue: item.id,

//                    };
//                }));
//            });
//        },
//        select: function (event, ui) {
//            $(campo).val(ui.item.realValue);
//            $(search).val(ui.item.value);
//            //$('.save-button').removeAttr('disabled');
//            return false;
//        },
//        change: function (event, ui) {
//            event.preventDefault();
//            if (ui.item == null) {
//                //$('.save-button').attr('disabled', 'disabled');
//                $(campo).val(0);
//                $(search).val('').focus();
//                abp.notify.info(app.localize("AutoConpletInvalido").replace('$cadastro', cadastro));
//                return false;
//            }
//        },
//    });

//}
