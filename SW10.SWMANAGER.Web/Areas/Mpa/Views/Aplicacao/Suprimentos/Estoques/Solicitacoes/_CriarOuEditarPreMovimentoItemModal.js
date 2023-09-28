    (function ($) {
    app.modals.CriarOuEditarPreMovimentoItemModal = function () {

        var _preMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _produtoService = abp.services.app.produto;

        var _modalManager;
        var _$PreMovimentoItemInformationForm = null;

        $(document).ready(function () {

            $('#QuantidadeItemid').mask('000.000.000', { reverse: true });
            $('#custoUnitarioId').mask('000.000.000,00', { reverse: true });
            $('#custoTotalId').mask('000.000.000,00', { reverse: true });
            $('#perIPIId').mask('000.000.000,00', { reverse: true });
            $('#valorIPIId').mask('000.000.000,00', { reverse: true });

        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$PreMovimentoItemInformationForm = _modalManager.getModal().find('form[name=PreMovimentoItemInformationsForm]');
            _$PreMovimentoItemInformationForm.validate();
            $('ul.ui-autocomplete').css('z-index', '2147483647 !important');

            $('.selectpicker').selectpicker('refresh');
        };


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
            //  scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_InformarLoteValidade.js',
            //  modalClass: 'EstoquePreMovimentoLoteValidadeProduto'
        });

        var lista = [];
        this.save = function () {

            if (!_$PreMovimentoItemInformationForm.valid()) {
                return;
            }
            var preMovimentoItem = _$PreMovimentoItemInformationForm.serializeFormToObject();

            preMovimentoItem.Quantidade = retirarMascara(preMovimentoItem.Quantidade);

            if ($('#itens').val() != '') {
                lista = JSON.parse($('#itens').val());
            }

            if ($('#idGrid').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGrid').val()) {
                        lista[i].Quantidade = $('#QuantidadeItemid').val();
                        lista[i].ProdutoId = $('#ProdutoId').val();
                      //  lista[i].NumeroSerie = $('#NumeroSerie').val();
                        lista[i].ProdutoUnidadeId = $('#ProdutoUnidadeId').val();
                    }

                }
            }
            else {
                preMovimentoItem.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                lista.push(preMovimentoItem);
            }

            $('#itens').val(JSON.stringify(lista));
           
            $('#ProdutoId').val('');
            $('#QuantidadeItemid').val('');
            $('#NumeroSerie').val('');
            $('#Id').val('');
            $('#idGrid').val('');
            $('#ProdutoUnidadeId').val('').selectpicker('refresh');
           abp.event.trigger('app.CriarOuEditarPreMovimentoItemModalSaved');
            $('#ProdutoId').focus();
        };

        //$('#produto-search').autocomplete({
        //    minLength: 2,
        //    delay: 0,
        //    source: function (request, response) {
        //        var term = $('#produto-search').val();
        //        var url = '/mpa/produtos/autocomplete';
        //        var fullUrl = url + '/?term=' + term;
        //        $.getJSON(fullUrl, function (data) {
        //            //console.log('retorno controller: ' + JSON.stringify(data));
        //            if (data.result.length == 0) {
        //                $('#ProdutoId').val(0);
        //                $("#produto-search").focus();
        //                abp.notify.info(app.localize("ListaVazia"));
        //                return false;
        //            };
        //            response($.map(data.result, function (item) {
        //                //console.log('response: ' + JSON.stringify(data));
        //                //console.log('item: ' + JSON.stringify(item));
        //                $('#ProdutoId').val(0);
        //                return {
        //                    label: item.nome,
        //                    value: item.nome,
        //                    realValue: item.id
        //                };
        //            }));
        //        });
        //    },
        //    select: function (event, ui) {
        //        //console.log('select: ' + JSON.stringify(ui));
        //        $('#ProdutoId').val(ui.item.realValue);
        //        $('#produto-search').val(ui.item.value);
        //        return false;
        //    },
        //    change: function (event, ui) {
        //        event.preventDefault();
        //        //console.log('change: ' + JSON.stringify(ui));
        //        if (ui.item == null) {
        //            $('#ProdutoId').val(0);
        //            $("#produto-search").val('').focus();
        //            abp.notify.info(app.localize("ProdutoInvalido"));
        //            return false;
        //        }
        //    },
        //});

        $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });

     

        //$('#QuantidadeItemid').change(function () {
        //    calcularCustoTotal();
        //});

        //$('#produtoId').on('change', function () {

        //   
        //    var valor = $('#produtoId').val();

        //    $("#ProdutoUnidadeId").empty();
        //    $.ajax({
        //        url: "/mpa/preMovimentos/SelecionarUnidades/" + valor,
        //        success: function (data) {

        //            $("#ProdutoUnidadeId").append('<option value>Selecione um valor</option>');

        //            var selected = (data.Items.length == 1) ? " selected='selected' " : "";

        //            $.each(data.Items, function (index, element) {
        //                $("#ProdutoUnidadeId").append('<option ' + selected + ' value="' + element.Id + '">' + element.Descricao + '</option>');
        //            });
        //            $('.selectpicker').selectpicker('refresh');
        //            $('#QuantidadeItemid').focus();
        //        }
        //    });

            
           

        //});

        $('#custoTotalId').blur(function () {

            var custoTotal = ($('#custoTotalId').val() != '') ? parseFloat(retirarMascara($('#custoTotalId').val())) : 0;
            var quantidadeItem = ($('#QuantidadeItemid').val() != '') ? parseFloat(retirarMascara($('#QuantidadeItemid').val())) : 0;
            var custoUnitario = parseFloat(custoTotal / quantidadeItem);

            $('#custoUnitarioId').val(custoUnitario.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', ''));
            calcularValorIPI();
        });

        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        $('#perIPIId').blur(function (e) {
            e.preventDefault();
            calcularValorIPI();
        });

        function calcularValorIPI() {
            var perIPI = ($('#perIPIId').val() != '') ? parseFloat(retirarMascara($('#perIPIId').val())) : 0;
            var custoTotal = ($('#custoTotalId').val() != '') ? parseFloat(retirarMascara($('#custoTotalId').val())) : 0;


            var valorIPI = perIPI * (custoTotal / 100);

            $('#valorIPIId').val(valorIPI.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', ''));
        }

        $('#valorIPIId').blur(function (e) {
            e.preventDefault();
            calcularPerIPI();
        });

        function calcularPerIPI() {
            var custoTotal = ($('#custoTotalId').val() != '') ? parseFloat(retirarMascara($('#custoTotalId').val())) : 0;
            var valorIPI = ($('#valorIPIId').val() != '') ? parseFloat(retirarMascara($('#valorIPIId').val())) : 0;

            var perIPI = parseFloat((valorIPI * 100) / custoTotal);

            $('#perIPIId').val(perIPI.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', ''));
        }

        selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoDropdown");

    };
})(jQuery);