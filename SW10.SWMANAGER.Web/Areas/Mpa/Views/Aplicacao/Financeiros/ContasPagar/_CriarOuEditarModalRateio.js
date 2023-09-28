(function ($) {


    var _$rateioTable = $('#rateioTable');




    _$rateioTable.jtable({

        title: app.localize('Rateio'),
        sorting: true,
        edit: false,
        create: false,
        multiSorting: true,


        rowInserted: function (event, data) {
            select: true
        },

        fields: {
            IdGrid: {
                key: true,
                list: false
            },
            actions: {
                title: app.localize('Actions'),
                width: '10%',
                sorting: false,
                display: function (data) {
                    var $span = $('<span></span>');
                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                        .appendTo($span)
                        .click(function (e) {
                            e.preventDefault();
                            editRateio(data.record)
                        });

                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                        .appendTo($span)
                        .click(function (e) {
                            e.preventDefault();
                            deleteRateio(data.record);
                        });

                    return $span;
                }
            },
            Empresa: {
                title: app.localize('Empresa'),
                width: '15%',
                display: function (data) {
                    return data.record.EmpresaDescricao;
                }
            },

            ContaAdministrativa: {
                title: app.localize('ContaAdministrativa'),
                width: '15%',
                display: function (data) {
                    return data.record.ContaAdministrativaDescricao;
                }
            },
            CentroCusto: {
                title: app.localize('CentroCusto'),
                width: '15%',
                display: function (data) {
                    return data.record.CentroCustoDescricao;
                }
            },

            Valor: {
                title: app.localize('Valor'),
                width: '10%',
                display: function (data) {
                    if (data.record.Valor) {
                        return posicionarDireita(formatarValor(data.record.Valor));
                    }
                }
            },

        }
    });


    var lista = [];

    function getRegistrosRateio() {

       

        lista = JSON.parse($('#rateioJson').val());

        var allRows = _$rateioTable.find('.jtable-data-row')

        $.each(allRows, function () {
            var id = $(this).attr('data-record-key');
            _$rateioTable.jtable('deleteRecord', { key: id, clientOnly: true });
        });

        for (var i = 0; i < lista.length; i++) {
            var item = lista[i];

            _$rateioTable.jtable('addRecord', {
                record: item
                , clientOnly: true
            });
        }
    }

    getRegistrosRateio();

    $('#inserirRateio').click(function (e) {
        e.preventDefault();

        var _$rateioForm = $('form[name=RateioForm]');
        //_$rateioItemInformationsForm.validate();

        //if (!_$rateioItemInformationsForm.valid()) {
        //    return;
        //}



        var rateio = _$rateioForm.serializeFormToObject();


        if ($('#rateioJson').val() != '') {
            lista = JSON.parse($('#rateioJson').val());
        }

        if ($('#idGridRateio').val() != '') {

            for (var i = 0; i < lista.length; i++) {
                if (lista[i].IdGrid == $('#idGridRateio').val()) {

                    var empresa = $('#empresaRateioId').select2('data');
                    if (empresa && empresa.length > 0) {

                        lista[i].EmpresaDescricao = empresa[0].text;
                    }

                    lista[i].EmpresaId = $('#empresaRateioId').val();


                    var contaAdministrativa = $('#contaAdministrativaId').select2('data');
                    if (contaAdministrativa && contaAdministrativa.length > 0) {

                        lista[i].ContaAdministrativaDescricao = contaAdministrativa[0].text;
                    }

                    lista[i].ContaAdministrativaId = $('#contaAdministrativaId').val();


                    var centroCusto = $('#centroCustoId').select2('data');
                    if (centroCusto && centroCusto.length > 0) {

                        lista[i].CentroCustoDescricao = centroCusto[0].text;
                    }

                    lista[i].CentroCustoId = $('#centroCustoId').val();

                    lista[i].Valor = parseFloat(retirarMascara($('#valorRateio').val()));
                    lista[i].Observacao = $('#observacao').val();
                    lista[i].IsImposto = $('#isImposto')[0].checked;


                    _$rateioTable.jtable('updateRecord', {
                        record: lista[i]
                    , clientOnly: true
                    });

                }
            }
        }
        else {
            rateio.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;

            var empresa = $('#empresaRateioId').select2('data');
            if (empresa && empresa.length > 0) {

                rateio.EmpresaDescricao = empresa[0].text;
            }

            rateio.EmpresaId = $('#empresaRateioId').val();


            var contaAdministrativa = $('#contaAdministrativaId').select2('data');
            if (contaAdministrativa && contaAdministrativa.length > 0) {

                rateio.ContaAdministrativaDescricao = contaAdministrativa[0].text;
            }

            rateio.ContaAdministrativaId = $('#contaAdministrativaId').val();


            var centroCusto = $('#centroCustoId').select2('data');
            if (centroCusto && centroCusto.length > 0) {

                rateio.CentroCustoDescricao = centroCusto[0].text;
            }

            rateio.CentroCustoId = $('#centroCustoId').val();

            rateio.Valor = retirarMascara($('#valorRateio').val());
            rateio.Observacao = $('#observacao').val();
            rateio.IsImposto = $('#isImposto')[0].checked;

            lista.push(rateio);

           

            _$rateioTable.jtable('addRecord', {
                record: rateio
              , clientOnly: true
            });

        }

        var valorRestante = 0;
        var valorRateado = 0;

       

        for (var i = 0; i < lista.length; i++) {

            valorRateado = parseFloat(valorRateado) + parseFloat(lista[i].Valor);
        }

        valorRestante = parseFloat(retirarMascara($('#valorTotal').val())) - valorRateado;


        $('#rateioJson').val(JSON.stringify(lista));

        $('#idGridRateio').val('');
        $('#empresaRateioId').val('').trigger('change');
        $('#contaAdministrativaId').val('').trigger('change');
        $('#centroCustoId').val('').trigger('change');
        $('#observacao').val('');
        $('#valorRateio').val(formatarValor(valorRestante));
        $('#isImposto').attr("checked", false);
        CalculaValorRateio();
        $('#valor').focus();

        $('#inserirRateio > i').removeClass('fa-check');
        $('#inserirRateio > i').addClass('fa-plus');

    });

    //function retirarMascara(valor) {
    //    while (valor.indexOf('.') != -1) valor = valor.replace('.', '');
    //    while (valor.indexOf(' ') != -1) valor = valor.replace(' ', '');
    //    valor = valor.replace(',', '.');
    //    return valor;
    //}

    //function formatarValor(valor) {

    //    if (valor != '' && valor != null) {
    //        var numero = parseFloat( valor).toFixed(2).split('.');
    //        numero[0] = numero[0].split(/(?=(?:...)*$)/).join('.');
    //        return numero.join(',');

    //    }
    //    return '';

    //}


    
    function editRateio(rateio) {

       

        $('#empresaRateioId')
                  .append($("<option>") //add option tag in select
                .val(rateio.EmpresaId) //set value for option to post it
                .text(rateio.EmpresaDescricao)
              ) //set a text for show in select
        .val(rateio.EmpresaId) //select option of select2
              .trigger("change");



        $('#contaAdministrativaId')
          .append($("<option>") //add option tag in select
        .val(rateio.ContaAdministrativaId) //set value for option to post it
        .text(rateio.ContaAdministrativaDescricao)
      ) //set a text for show in select
.val(rateio.ContaAdministrativaId) //select option of select2
      .trigger("change");

        $('#centroCustoId')
       .append($("<option>") //add option tag in select
     .val(rateio.CentroCustoId) //set value for option to post it
     .text(rateio.CentroCustoDescricao)
   ) //set a text for show in select
.val(rateio.CentroCustoId) //select option of select2
   .trigger("change");



        $('#valorRateio').val(formatarValor(rateio.Valor));//.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', ''));
        $('#observacao').val(rateio.Observacao);
        $('#idGridRateio').val(rateio.IdGrid);

        $('#isImposto').attr("checked", rateio.IsImposto);

        // $('#inserir > i').removeClass('fa');
        $('#inserirRateio > i').removeClass('fa-plus');
        // $('#inserir > i').addClass('glyphicon');
        $('#inserirRateio > i').addClass('fa-check');


    }

    function deleteRateio(rateio) {
        abp.message.confirm(
            app.localize('DeleteWarning', rateio.EmpresaDescricao),
            function (isConfirmed) {
                if (isConfirmed) {

                   

                    lista = JSON.parse($('#rateioJson').val());

                    for (var i = 0; i < lista.length; i++) {
                        if (lista[i].IdGrid == rateio.IdGrid) {
                            lista.splice(i, 1);
                            $('#rateioJson').val(JSON.stringify(lista));

                            _$rateioTable.jtable('deleteRecord', {
                                key: rateio.IdGrid
                            , clientOnly: true
                            });

                            break;
                        }
                    }
                    CalculaValorRateio();
                    CarregarValoreRestanteRateio();
                }
            }
        );
    }

    function CalculaValorRateio() {
        var totalRateio = 0;

        for (var i = 0; i < lista.length; i++) {
            totalRateio += parseFloat(lista[i].Valor);
        }

        $('#valorTotalRateio').val(formatarValor(totalRateio));
    }

    function CarregarValoreRestanteRateio() {
       
        var valorRestante = 0;
        var valorRateado = 0;

        for (var i = 0; i < lista.length; i++) {

            valorRateado = parseFloat(valorRateado) + parseFloat(lista[i].Valor);
        }

        valorRestante = parseFloat(retirarMascara($('#valorTotal').val())) - valorRateado;

        $('#valorRateio').val(formatarValor(valorRestante));
    }




    $("#tabRateioRef").on('click', function () {
        CarregarValoreRestanteRateio();

    });



})(jQuery);