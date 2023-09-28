(function ($) {
    app.modals.CriarOuEditarPreMovimentoItemModal = function () {

        var _preMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _produtoService = abp.services.app.produto;

        var _modalManager;
        var _$PreMovimentoItemInformationForm = null;
        //var maskFields = {};
        //const currencyMaskTemplate = {
        //    mask: 'R$num',
        //    blocks: {
        //        num: {
        //            mask: Number,
        //            thousandsSeparator: '.',
        //            scale: 5,	// digits after decimal
        //            signed: true, // allow negative
        //            normalizeZeros: true,  // appends or removes zeros at ends
        //            radix: ',',  // fractional delimiter
        //            padFractionalZeros: true,  // if true, then pads zeros at end to the length of scale
        //            allowDecimal: true
        //        }
        //    },
        //};

        //const numberMaskTemplate = {
        //    mask: 'num',
        //    blocks: {
        //        num: {
        //            mask: Number,
        //            thousandsSeparator: '.',
        //            scale: 2,	// digits after decimal
        //            signed: true, // allow negative
        //            normalizeZeros: true,  // appends or removes zeros at ends
        //            radix: ',',  // fractional delimiter
        //            padFractionalZeros: true,  // if true, then pads zeros at end to the length of scale
        //            allowDecimal: true,

        //        }
        //    },
        //};

        this.init = function (modalManager) {

            //$('#QuantidadeItemid').maskMoney({ allowNegative: true, thousands: '.', decimal: ',' }).maskMoney('mask');
            //$('#custoUnitarioId').show();
            //$('#custoUnitarioId').val(numeral($('#custoUnitarioId').val()).format("0.00,00"));
            //$('#custoUnitarioId').maskMoney({ prefix: 'R$ ', allowNegative: true, thousands: '.', decimal: ',', precision: 5, allowZero: true }).maskMoney('mask');
            //$('#custoTotalId').show();
            //$('#custoTotalId').val(numeral($('#custoTotalId').val()).format("0.00,00"));
            //$('#custoTotalId').maskMoney({ prefix: 'R$ ', allowNegative: true, thousands: '.', decimal: ',', precision: 5, allowZero: true }).maskMoney('mask');
            //$('#perIPIId').show();
            //$('#perIPIId').maskMoney({ allowNegative: true, thousands: '.', decimal: ',', allowZero: true }).maskMoney('mask');
            //$('#valorIPIId').show();
            //$('#valorIPIId').val(numeral($('#valorIPIId').val()).format("0.00,00"));
            //$('#valorIPIId').maskMoney({ prefix: 'R$ ', allowNegative: true, thousands: '.', decimal: ',', precision: 5, allowZero: true }).maskMoney('mask');

            //$('#custoUnitarioId').val(formatarValor($('#custoUnitarioId').val()));
            //$('#custoTotalId').val(formatarValor($('#custoTotalId').val()));
            //$('#perIPIId').val(formatarValor($('#perIPIId').val()));
            //$('#valorIPIId').val(formatarValor($('#valorIPIId').val()));

            //Quando for Inventário não mostrar esses campos
            if (localStorage['inventario'] == '19') {
                //maskFields['custoUnitarioId'].unmaskedValue = '0';
                //maskFields['custoTotalId'].unmaskedValue = '0';
                //maskFields['perIPIId'].unmaskedValue = '0';
                //maskFields['valorIPIId'].unmaskedValue = '0';
                document.querySelector('#custoUnitarioId').parentElement.style.display = 'none';
                document.querySelector('#custoTotalId').parentElement.style.display = 'none';
                document.querySelector('.ipiValorIpi').style.display = 'none';
            }

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

        this.save = function () {

            if (!_$PreMovimentoItemInformationForm.valid()) {
                return;
            }

            var preMovimentoItem = _$PreMovimentoItemInformationForm.serializeFormToObject();

            //preMovimentoItem.Quantidade = maskFields['QuantidadeItemid'].unmaskedValue;
            //preMovimentoItem.CustoUnitario = maskFields['custoUnitarioId'].unmaskedValue;
            //preMovimentoItem.CustoTotal = maskFields['custoTotalId'].unmaskedValue;
            //preMovimentoItem.PerIPI = maskFields['perIPIId'].unmaskedValue;
            //preMovimentoItem.ValorIPI = maskFields['valorIPIId'].unmaskedValue;
            debugger;
            _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();
            _preMovimentoItemService.criarOuEditar(preMovimentoItem)
                .done(function (data) {
                    debugger;
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        if ($('#Id').val() != undefined && $('#Id').val() != '') {
                            _modalManager.close();
                        }

                        $('#codigoBarrasId').val('');
                        $('#ProdutoId').val(null).trigger("change");
                        $('#QuantidadeItemid').val('');
                        $('#custoUnitarioId').val('');
                        $('#custoTotalId').val('');
                        $('#NumeroSerie').val('');
                        $('#Id').val('');
                        $('#perIPIId').val('');
                        $('#valorIPIId').val('');
                        //$('#ProdutoUnidadeId').val('').selectpicker('refresh');
                        $('#ProdutoId').focus();
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        abp.event.trigger('app.CriarOuEditarPreMovimentoItemModalSaved');
                        //$('form[name=PreMovimentoItemInformationsForm]').trigger('reset');

                        _modalManager.close();
                    }
                }
                ).always(function () {
                    _modalManager.setBusy(false);
                }
                )
        };

        $('#produto-search').autocomplete({
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                var term = $('#produto-search').val();
                var url = '/mpa/produtos/autocomplete';
                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    //console.log('retorno controller: ' + JSON.stringify(data));
                    if (data.result.length == 0) {
                        $('#ProdutoId').val(0);
                        $("#produto-search").focus();
                        abp.notify.info(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        //console.log('response: ' + JSON.stringify(data));
                        //console.log('item: ' + JSON.stringify(item));
                        $('#ProdutoId').val(0);
                        return {
                            label: item.nome,
                            value: item.nome,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                //console.log('select: ' + JSON.stringify(ui));
                $('#ProdutoId').val(ui.item.realValue);
                $('#produto-search').val(ui.item.value);
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                //console.log('change: ' + JSON.stringify(ui));
                if (ui.item == null) {
                    $('#ProdutoId').val(0);
                    $("#produto-search").val('').focus();
                    abp.notify.info(app.localize("ProdutoInvalido"));
                    return false;
                }
            },
        });

        $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });

        //$('#custoUnitarioId, #QuantidadeItemid').focusout(function () {

        //    calcularCustoTotal();
        //});

        //$('#QuantidadeItemid').change(function () {
        //    calcularCustoTotal();
        //});

        $('#ProdutoId').on('change', function () {

            var valor = $('#ProdutoId').val();

            selectSWMultiplosFiltros('.selectProdutoUnidade', "/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown", ['ProdutoId']);

            $("#ProdutoUnidadeId").empty();
            $.ajax({
                url: "/mpa/preMovimentos/SelecionarUnidades/" + valor,
                success: function (data) {

                    $("#ProdutoUnidadeId").append('<option value>Selecione um valor</option>');

                    var selected = (data.Items.length == 1) ? " selected='selected' " : "";

                    $.each(data.Items, function (index, element) {
                        $("#ProdutoUnidadeId").append('<option ' + selected + ' value="' + element.Id + '">' + element.Descricao + '</option>');
                    });
                    $('.selectpicker').selectpicker('refresh');

                }
            });

            if (valor != '' && valor != null) {


                _produtoService.obter(valor).done(function (produto) {
                    if (produto.isSerie) {
                        $('#divNumeroSerie').show();
                        $('#QuantidadeItemid').val(1);

                        $('#QuantidadeItemid').attr('readonly', true);
                        $('#NumeroSerie').attr('required', true);
                        $('#NumeroSerie').focus();

                    }
                    else {
                        $('#divNumeroSerie').hide();
                        $('#QuantidadeItemid').attr('readonly', false);
                        $('#NumeroSerie').attr('required', false);

                        $("#ProdutoUnidadeId").focus();
                    }
                });
            }

        });

        //$('#custoTotalId').focusout(function () {
        //    var custoTotal = ($('#custoTotalId').val() != '') ? numeral(parseFloat(maskFields['custoTotalId'].unmaskedValue)).value() : 0;
        //    var quantidadeItem = ($('#QuantidadeItemid').val() != '') ? numeral(parseFloat(maskFields['QuantidadeItemid'].unmaskedValue)).value() : 0;
        //    var custoUnitario = parseFloat(custoTotal / quantidadeItem);
        //    if (isNaN(custoUnitario)) {
        //        custoUnitario = 0;
        //    }

        //    maskFields['custoUnitarioId'].unmaskedValue = custoUnitario.toString();
        //    calcularValorIPI();
        //});

        $('#codigoBarrasId').blur(function (e) {

            let search = e.target.value;
            if (!search) {
                return false;
            }

            _produtoService.obterProdutoPorCodigoBarrasDropdown(search)
                .done(function (data) {
                    debugger;
                    //$("#EstoqueId").empty();

                    //$("#EstoqueId").append('<option value>Selecione um valor</option>');

                    if (data.id != 0) {
                        var selected = (data != null) ? " selected='selected' " : "";
                        $("#ProdutoId").append('<option ' + selected + ' value="' + data.id + '">' + data.text + '</option>');
                    } else {
                        $("#ProdutoId").append('<option selected="selected" value="0">Informe um produto</option>');
                    }

                }).always(function () {
                    debugger;
                    var valor = $('#ProdutoId').val();

                    $("#ProdutoUnidadeId").empty();

                    $.ajax({
                        url: "/mpa/preMovimentos/SelecionarUnidades/" + valor,
                        success: function (data) {

                            $("#ProdutoId").append('<option value>Informe um produto</option>');

                            $("#ProdutoUnidadeId").append('<option value>Selecione um valor</option>');

                            var selected = (data.Items.length == 1) ? " selected='selected' " : "";

                            $.each(data.Items, function (index, element) {
                                $("#ProdutoUnidadeId").append('<option ' + selected + ' value="' + element.Id + '">' + element.Descricao + '</option>');
                            });

                        }
                    });

                });

        });

        //function calcularCustoTotal() {
        //    var custoUnitario = ($('#custoUnitarioId').val() != '') ? numeral(parseFloat(maskFields['custoUnitarioId'].unmaskedValue)).value() : 0;
        //    var quantidadeItem = ($('#QuantidadeItemid').val() != '') ? numeral(parseFloat(maskFields['QuantidadeItemid'].unmaskedValue)).value() : 0;
        //    var custoTotal = (custoUnitario * quantidadeItem);

        //    if (isNaN(custoTotal)) {
        //        custoTotal = 0;
        //    }

        //    maskFields['custoTotalId'].unmaskedValue = custoTotal.toString();
        //    calcularValorIPI();
        //}

        //function retirarMascara(valor) {

        //    while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
        //    while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

        //    valor = valor.replace(',', '.');

        //    return valor;
        //}

        //$('#perIPIId').focusout(function (e) {
        //    e.preventDefault();
        //    calcularValorIPI();
        //});

        //function calcularValorIPI() {
        //    var perIPI = ($('#perIPIId').val() != '') ? numeral(parseFloat(maskFields['perIPIId'].unmaskedValue)).value() : 0;
        //    var custoTotal = ($('#custoTotalId').val() != '') ? numeral(parseFloat(maskFields['custoTotalId'].unmaskedValue)).value() : 0;
        //    var valorIPI = perIPI * (custoTotal / 100);

        //    if (isNaN(valorIPI)) {
        //        valorIPI = 0;
        //    }
        //    debugger;
        //    maskFields['valorIPIId'].unmaskedValue = valorIPI.toString();
        //}

        //$('#valorIPIId').focusout(function (e) {
        //    e.preventDefault();
        //    calcularPerIPI();
        //});

        //function calcularPerIPI() {
            
        //    var custoTotal = ($('#custoTotalId').val() != '') ? numeral(parseFloat(maskFields['custoTotalId'].unmaskedValue)).value() : 0;
        //    var valorIPI = ($('#valorIPIId').val() != '') ? numeral(parseFloat(maskFields['valorIPIId'].unmaskedValue)).value() : 0;
        //    var perIPI = (valorIPI * 100) / custoTotal;

        //    if (isNaN(perIPI)) {
        //        perIPI = 0;
        //    }
        //    maskFields['perIPIId'].unmaskedValue = perIPI.toString();
        //}

        selectSWMultiplosFiltros('.selectProduto', "/api/services/app/produto/ListarProdutoPorEstoqueIdDropdown", ['EstoqueId']);

        selectSWMultiplosFiltros('.selectProdutoUnidade', "/api/services/app/ProdutoUnidade/ListarUnidadePorProdutoDropdown", ['ProdutoId']);
    };
})(jQuery);