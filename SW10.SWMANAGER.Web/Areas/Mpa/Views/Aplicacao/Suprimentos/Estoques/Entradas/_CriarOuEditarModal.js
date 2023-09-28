
(function ($) {
    $(function () {
        iValidador.init();
    });
    var iValidador = {
        init: function () {
            // Execute seus códigos iniciais
            // ...
            //alert('Entrou no validador agora!');
            // Chame as funções desejadas...
            iValidador.outraFuncao();
        },
        outraFuncao: function () {
            // Códigos desejados...
        }
    };

    $('#txtTotal').on('click', function () {
        fnCalcularTotal();
    });
    $('#acrescimo-desconto').on('input', function () {
        fnCalcularTotal();
    });
    $('#frete').on('input', function () {
        fnCalcularTotal();
    });
    $('#valor-documento').on('input', function () {
        fnCalcularTotal();
    });

    //$('#Data').on('load', function () {
    //    var d = new Date();
    //    var n = d.getDate();
    //    $('#Data').val(moment().format("L LT"));
    //});
    
    function fnCalcularTotal(){
        //var n1 =  parseInt(document.getElementById('Frete').value, 10);
        //var n2 = parseInt(document.getElementById('AcrescimoDesconto').value, 10);
        //var n3 = parseInt(document.getElementById('ValorDocumento').value, 10);
        // document.getElementById('txtTotal').value
        var result = parseFloat($('#frete').val()) +
                     parseFloat($('#acrescimo-desconto').val()) +
                     parseFloat($('#valor-documento').val());
        $('#txtTotal').val(parseFloat(result).toFixed(2));
       // alert('fun!');
    };

    var _$EntradasItemTable = $('#EntradasItemTable');

    var _entradaService = abp.services.app.entrada;
    //var _entradasItemService = abp.services.app.entradaItem;

    var _modalManager;
    var _$entradasInformationForm = null;

    //var _permissions = {
    //    create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.Entrada.Create'),
    //    edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.Entrada.Edit'),
    //    'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.Entrada.Delete')
    //};

    this.init = function (modalManager) {
        _modalManager = modalManager;
      //  $('#Data').val(moment().format("L LT"));

        _$entradasInformationForm = $('form[name=EntradaInformationsForm]'); //_modalManager.getModal().find('form[name=EntradaInformationsForm]');
        //    //_$entradasInformationForm.validate();
        $('.modal-dialog').css('width', '1300px');
        //    //$('select').addClass('form-control edited');
        //    // $('.modal-content').css('z-index', '50');
    };

    $('.close').on('click', function () {
        location.href = '/mpa/entradas';
    });

    $('.close-button').on('click', function () {
        location.href = '/mpa/entradas';
    });
    //this.save = function () {
    $('.save-button').on('click', function () {
        //if (!_$entradasInformationForm.valid()) {
        //    return;
        //}
        var entradas = $('form[name=EntradaInformationsForm]').serializeFormToObject(); //_$entradasInformationForm.serializeFormToObject();


        console.log(JSON.stringify(entradas));
        //_modalManager.setBusy(true);
        $(this).buttonBusy(true);
        _entradaService.criarOuEditar(entradas)
             .done(function () {
                 abp.notify.info(app.localize('SavedSuccessfully'));
                 //_modalManager.close();
                 abp.event.trigger('app.CriarOuEditarEntradaModalSaved');
                 location.href = '/mpa/entradas';
             })
            .always(function () {
                //_modalManager.setBusy(false);
                $(this).buttonBusy(false);
            });
    });

    _$EntradasItemTable.jtable
    ({

        title: app.localize('Item'),
        paging: true,
        sorting: true,
        useBootstrap: true,
        multiSorting: true,

        actions:
        {
            listAction:
            {
                method: _entradaService.listarItens
            },
            createAction: '/Mpa/Entradas/CriarEntradaItem',
            updateAction: '/Mpa/Entradas/EditarEntradaItem',
            deleteAction: '/Mpa/Entradas/ExcluirEntradaItem'
        },
        fields:
        {
            id: {
                key: true,
                list: false
            },
            entradaId: {
                type: 'hidden',
                defaultValue: function (data) {
                    return $('#id').val();
                },
            },
            //prescricao: {
            //    title: app.localize('Prescricao'),
            //    width: '40%',
            //    type: 'checkbox',
            //    values: { 'false': '', 'true': '' },
            //    display: function (data) {
            //        if ((data.record.prescricao === true) || (data.record.prescricao == "true")) {
            //            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
            //        } else {
            //            return '<span class="label label-default">' + app.localize('No') + '</span>';
            //        }
            //    }
            //},
            ProdutoId: {
                title: app.localize('Produto'),
                width: '40%',
                display: function (data) {
                    if (data.record.produto) {
                        return data.record.produto.descricao;
                    }
                },
                options: function (data) {
                    if (data.source == 'list') {
                        //Return url all options for optimization. 
                        return '/Mpa/Produtos/ListarNomeProdutosExcetoId?id=' + data.record.produto.id;
                    } else {
                        //This code runs when user opens edit/create form to create combobox.
                        //data.source == 'edit' || data.source == 'create'
                        return '/Mpa/Produtos/ListarNomeProdutosExcetoId?id=0';
                    }
                }
            },
            quantidade: {
                title: app.localize('Quantidade'),
                width: '20%',
                display: function (data) {
                    if (data.record.quantidade) {
                        return data.record.quantidade;
                    }
                }
            },
            custoUnitario: {
                title: app.localize('CustoUnitario'),
                sorting: false,
                width: '40%',

                display: function (data) {
                    if (data.record.custoUnitario) {
                        var formato = { minimumFractionDigits: 2, style: 'currency', currency: 'BRL' }
                        //$("#totalEntrada").append($entrada.toLocaleString('pt-BR', formato));
                        display_value = data.record.custoUnitario.toLocaleString("pt-BR", formato);
                        if (!display_value) { display_value = ''; } //handles showing blank cell for null values
                        return '<div style="text-align:left;">' + display_value + '</div>';
                        //return '<div style="text-align:left;">' + data.record.custoUnitario.toLocaleString("pt-BR", formato) + '</div>'; // { style: "currency" });
                    }
                }
            },
        }
    });

    function getEntradaItem(reload) {

        if (reload) {
            _$EntradasItemTable.jtable('reload');
        } else {
            _$EntradasItemTable.jtable('load', { filtro: $('#id').val() });
        }
    }
    getEntradaItem();

})(jQuery);