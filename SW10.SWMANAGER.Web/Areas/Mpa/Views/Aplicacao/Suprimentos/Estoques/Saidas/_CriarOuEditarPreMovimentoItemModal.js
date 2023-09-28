(function ($) {
    app.modals.CriarOuEditarPreMovimentoItemModal = function () {

        var _preMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _produtoService = abp.services.app.produto;
        
        var _modalManager;
        var _$PreMovimentoItemInformationForm = null;

        $(document).ready(function () {
            $('#Quantidade').mask('000.000.000,00', { reverse: true });
        });


        selectSWWithDefaultValue('.selectLoteValidade', "/api/services/app/estoqueLoteValidade/listarDropdownSaldo", [$('#ProdutoId'), $('#EstoqueId'), $('#PreMovimentoId'), $('.selectLoteValidade')]);

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$PreMovimentoItemInformationForm = _modalManager.getModal().find('form[name=PreMovimentoItemInformationsForm]');
            _$PreMovimentoItemInformationForm.validate();
            $('ul.ui-autocomplete').css('z-index', '2147483647 !important');
        };


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros'
        });


        this.save = function () {

            if (!_$PreMovimentoItemInformationForm.valid()) {
                return;
            }

            var preMovimentoItem = _$PreMovimentoItemInformationForm.serializeFormToObject();

            preMovimentoItem.Quantidade = retirarMascara(preMovimentoItem.Quantidade);

            _modalManager.setBusy(true);

            preMovimentoItem.EstoqueId = $('#EstoqueId').val();

            _preMovimentoItemService.criarOuEditarSaida(preMovimentoItem)
                  .done(function (data) {
                      if (data.errors.length > 0) {
                          _ErrorModal.open({ erros: data.errors });
                      }
                      else {
                          abp.notify.info(app.localize('SavedSuccessfully'));
                          abp.event.trigger('app.CriarOuEditarPreMovimentoItemModalSaved');
                          $('#ProdutoId').val(null).trigger('change');
                          $('#Quantidade').val('');
                          $('#Id').val('');
                          $('#CodigoBarra').val('');
                          $('#ProdutoUnidadeId').val('');
                          $("#EstoquePreMovimentoLoteValidadeId").val('');
                          $('#LoteValidadeId').val('');
                          $('#LoteValidadeId').trigger("change");
                          $('#ProdutoId').focus();
                      }
                  }
                  ).always(function () {
                      _modalManager.setBusy(false);
                  }
           )
        };


        $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });

        $('#ProdutoId').change(function () {
            var valor = $('#ProdutoId').val();
            $("#ProdutoUnidadeId").empty();

            if (valor != '' && valor != null) {

                $.ajax({
                    url: "/mpa/preMovimentos/SelecionarUnidades/" + valor,
                    success: function (data) {

                        $("#ProdutoUnidadeId").append('<option value>Selecione um valor</option>');

                        var selected = (data.Items.length == 1) ? " selected='selected' " : "";

                        $.each(data.Items, function (index, element) {
                            $("#ProdutoUnidadeId").append("<option " + selected + " value='" + element.Id + "'>" + element.Descricao + "</option>");
                        });

                        $('.selectpicker').selectpicker('refresh');
                    }
                });

                //_produtoService.obter(valor).done(function (data) {

                //    if (data.isValidade || data.isLote) {
                //        $("#loteValidade").show();

                //        $("#LoteValidadeId").empty();


                //        $.ajax({
                //            url: "/mpa/saidas/SelecionarLotesValidades/",
                //            data: {
                //                produtoId: valor,
                //                estoqueId: $("#EstoqueId").val(),
                //                preMovimentoId: $("#PreMovimentoId").val()
                //            },
                //            success: function (data) {

                //                $("#LoteValidadeId").append('<option value>Selecione um valor</option>');

                //                var selectedLoteValidade = (data.length == 1) ? " selected='selected' " : "";


                //                $.each(data, function (index, element) {

                //                    $("#LoteValidadeId").append("<option  " + selectedLoteValidade + " value='" + element.Id + "'>" + element.Nome + "</option>");
                //                });
                //            }
                //        });



                //    }
                //    else {
                //        $("#loteValidade").hide();
                //    }



                //    if (valor != '') {


                //        _produtoService.obter(valor).done(function (produto) {
                //            if (produto.isSerie) {
                //                $('#divNumeroSerie').show();
                //                $('#QuantidadeItemid').val(1);

                //                $('#QuantidadeItemid').attr('readonly', true);
                //                $('#NumeroSerie').attr('required', true);

                //            }
                //            else {
                //                $('#divNumeroSerie').hide();
                //                $('#QuantidadeItemid').attr('readonly', false);
                //                $('#NumeroSerie').attr('required', false);
                //            }
                //        });
                //    }

                //    if (data.isSerie) {
                //        $('#divNumeroSerie').show();
                //        $('#Quantidade').val(1);

                //        $('#Quantidade').attr('readonly', true);
                //        $('#numeroSerieId').attr('required', true);

                //    }
                //    else {
                //        $('#divNumeroSerie').hide();
                //        $('#Quantidade').attr('readonly', false);
                //        $('#numeroSerieId').attr('required', false);
                //    }

                //});
            }
        });

        function calcularCustoTotal() {

            var custoUnitario = ($('#custoUnitarioId').val() != '') ? parseFloat(retirarMascara($('#custoUnitarioId').val())) : 0;
            var quantidadeItem = ($('#QuantidadeItemid').val() != '') ? parseFloat(retirarMascara($('#QuantidadeItemid').val())) : 0;
            var custoTotal = (custoUnitario * quantidadeItem);

            $('#custoTotalId').val(custoTotal.toFixed(2));
        }

        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        $('input[name="Validade"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
           function (selDate) {
               $('input[name="Validade"]').val(selDate.format('L')).addClass('form-control edited');
           });

        selectSWMultiplosFiltros('.selectProduto', "/api/services/app/produto/listarProdutoPorEstoqueIdDropdown", ['EstoqueId']);

        

        
        $('#CodigoBarra').on('keypress', function (event) {
            //Tecla 13 = Enter
            debugger;

            if (event.which == 13) {
                event.preventDefault();
                inserirProdutoCodigoBarra();
            }
        });

        function inserirProdutoCodigoBarra() {
            var estoquePreMovimentoItemAppService = abp.services.app.estoquePreMovimentoItem;
            estoquePreMovimentoItemAppService.criarSaidaPorCodigoBarra($('#CodigoBarra').val(), $('#EstoqueId').val(), $('#PreMovimentoId').val(), $('#Quantidade').val())
                .done(function (data) {
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        if (data.warnings.length > 0) {
                            _ErrorModal.open({ erros: data.warnings });
                        }

                        abp.notify.info(app.localize('SavedSuccessfully'));
                        abp.event.trigger('app.CriarOuEditarPreMovimentoItemModalSaved');
                       
                    }

                    $('#ProdutoId').val(null).trigger('change');
                    $('#Id').val('');
                    $('#CodigoBarra').val('');
                    $('#ProdutoUnidadeId').val('');
                    $("#EstoquePreMovimentoLoteValidadeId").val('');
                    $('#LoteValidadeId').val('');
                    $('#LoteValidadeId').trigger("change");

                    $('#CodigoBarra').val('');
                    $('#Quantidade').val('1');
                    $('#CodigoBarra').focus();
                });
        }
    };
})(jQuery);