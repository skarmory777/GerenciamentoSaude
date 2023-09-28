(function ($) {
    app.modals.CriarOuEditarPreMovimentoItemModal = function () {

        var _movimentoService = abp.services.app.estoqueMovimento;
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


        this.save = function () {
            if (!_$PreMovimentoItemInformationForm.valid()) {
                return;
            }
            var preMovimentoItem = _$PreMovimentoItemInformationForm.serializeFormToObject();

            preMovimentoItem.QuantidadeBaixa = preMovimentoItem.Quantidade;
            //preMovimentoItem.QuantidadeBaixa = retirarMascara(preMovimentoItem.Quantidade);
            //preMovimentoItem.CustoUnitario = retirarMascara(preMovimentoItem.CustoUnitario);
            //preMovimentoItem.CustoTotal = retirarMascara(preMovimentoItem.CustoTotal);
            //preMovimentoItem.PerIPI = retirarMascara(preMovimentoItem.PerIPI);


            _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();


           

            _movimentoService.criarOuEditarBaixaItem(preMovimentoItem)
                  .done(function (data) {

                      if (data.errors.length > 0) {
                          _ErrorModal.open({ erros: data.errors });
                      }
                      else {
                          if ( $('#Id').val() != undefined && $('#Id').val() != '')
                          {
                              _modalManager.close();
                          }

                      
                          $('#ProdutoId').val('').selectpicker('refresh');
                          $('#QuantidadeItemid').val('');
                          $('#custoUnitarioId').val('');
                          $('#custoTotalId').val('');
                          $('#NumeroSerie').val('');
                          $('#Id').val('');
                          $('#perIPIId').val('');
                          $('#valorIPIId').val('');
                          $('#ProdutoUnidadeId').val('').selectpicker('refresh');
                          $('#ProdutoId').focus();
                          abp.notify.info(app.localize('SavedSuccessfully'));
                          abp.event.trigger('app.CriarOuEditarBaixaItem');
                          //$('form[name=PreMovimentoItemInformationsForm]').trigger('reset');
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

        $('#custoUnitarioId, #QuantidadeItemid').blur(function () {

           calcularCustoTotal();
        });

        //$('#QuantidadeItemid').change(function () {
        //    calcularCustoTotal();
        //});

        $('#ProdutoId').on('change', function () {
            var valor = $('#ProdutoId').val();

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

            if (valor != '') {

              
                _produtoService.obter(valor).done(function (produto) {
                    if (produto.isSerie) {
                        $('#divNumeroSerie').show();
                        $('#QuantidadeItemid').val(1);

                        $('#QuantidadeItemid').attr('readonly', true);
                        $('#NumeroSerie').attr('required', true);

                    }
                    else {
                        $('#divNumeroSerie').hide();
                        $('#QuantidadeItemid').attr('readonly', false);
                        $('#NumeroSerie').attr('required', false);
                    }
                });
            }

        });

        $('#custoTotalId').blur(function () {

            var custoTotal = ($('#custoTotalId').val() != '') ? parseFloat(retirarMascara($('#custoTotalId').val())) : 0;
            var quantidadeItem = ($('#QuantidadeItemid').val() != '') ? parseFloat(retirarMascara($('#QuantidadeItemid').val())) : 0;
            var custoUnitario = parseFloat(custoTotal / quantidadeItem);

            $('#custoUnitarioId').val(custoUnitario.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', ''));
            calcularValorIPI();
        });


        function calcularCustoTotal() {

            var custoUnitario = ($('#custoUnitarioId').val() != '') ? parseFloat(retirarMascara($('#custoUnitarioId').val())) : 0;
            var quantidadeItem = ($('#QuantidadeItemid').val() != '') ? parseFloat(retirarMascara($('#QuantidadeItemid').val())) : 0;
         
            var custoTotal = (custoUnitario * quantidadeItem);

            var valor = custoTotal.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R','').replace('$','');
            $('#custoTotalId').val(valor);
            calcularValorIPI();
        }

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

        function calcularValorIPI()
        {
            var perIPI = ($('#perIPIId').val() != '') ? parseFloat(retirarMascara($('#perIPIId').val())) : 0;
            var custoTotal = ($('#custoTotalId').val() != '') ? parseFloat(retirarMascara($('#custoTotalId').val())) : 0;


            var valorIPI = perIPI * (custoTotal / 100);

            $('#valorIPIId').val(valorIPI.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', ''));
        }

        $('#valorIPIId').blur(function (e) {
            e.preventDefault();
            calcularPerIPI();
        });

        function calcularPerIPI()
        {
            var custoTotal = ($('#custoTotalId').val() != '') ? parseFloat(retirarMascara($('#custoTotalId').val())) : 0;
            var valorIPI = ($('#valorIPIId').val() != '') ? parseFloat(retirarMascara($('#valorIPIId').val())) : 0;

            var perIPI = parseFloat((valorIPI * 100)/custoTotal);

            $('#perIPIId').val(perIPI.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }).replace('R', '').replace('$', ''));
        }

        selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoDropdown");
      
    };
})(jQuery);