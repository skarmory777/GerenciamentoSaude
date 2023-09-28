﻿(function ($) {
    app.modals.CriarOuEditarTransferenciaItemModal = function () {

        var _preMovimentoItemService = abp.services.app.estoquePreMovimentoItem;
        var _produtoService = abp.services.app.produto;
        var _estoqueLoteValidadeService = abp.services.app.estoqueLoteValidade;


        var _modalManager;
        var _$TransferenciaItemInformationsForm = null;

        $(document).ready(function () {
            //  $('#Quantidade').mask('000.000.000', { reverse: true });
         
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TransferenciaItemInformationsForm = _modalManager.getModal().find('form[name=TransferenciaItemInformationsForm]');
            _$TransferenciaItemInformationsForm.validate();
            $('ul.ui-autocomplete').css('z-index', '2147483647 !important');

        };


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        this.save = function () {

            debugger;
            if (!_$TransferenciaItemInformationsForm.valid()) {
                return;
            }

           
            var preMovimentoItem = _$TransferenciaItemInformationsForm.serializeFormToObject();
       
            preMovimentoItem.Quantidade = retirarMascara(preMovimentoItem.Quantidade);

            _modalManager.setBusy(true);
            var editMode = $('#is-edit-mode').val();

            preMovimentoItem.EstoqueId = $('#EstoqueSaidaId').val();

           

            _preMovimentoItemService.criarOuEditarTransferencia(preMovimentoItem, $('#id').val(), $('#transferenciaItemId').val())
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
                          $('#LoteValidadeId').val('');
                          
                          $('#ProdutoId').focus();
                          //$('form[name=PreMovimentoItemInformationsForm]').trigger('reset');
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

            if (valor != null && valor != '') {

                $("#ProdutoUnidadeId").empty();

                $.ajax({
                    url: "/mpa/preMovimentos/SelecionarUnidades/" + valor,
                    success: function (data) {

                        $("#ProdutoUnidadeId").append('<option value>Selecione um valor</option>');

                        $.each(data.Items, function (index, element) {
                            $("#ProdutoUnidadeId").append("<option value='" + element.Id + "'>" + element.Descricao + "</option>");
                        });

                        $('#Quantidade').focus();
                    }
                });

                _produtoService.obter(valor).done(function (data) {

                    if (data.isValidade || data.isLote) {
                        $("#loteValidade").show();

                        $("#LoteValidadeId").empty();


                        $.ajax({
                            url: "/mpa/saidas/SelecionarLotesValidades/",
                            data: {
                                produtoId: valor,
                                estoqueId: $("#EstoqueSaidaId").val(),
                                preMovimentoId: $("#PreMovimentoId").val()
                            },
                            success: function (data) {

                                $("#LoteValidadeId").append('<option value>Selecione um valor</option>');

                                $.each(data, function (index, element) {

                                    $("#LoteValidadeId").append("<option value='" + element.Id + "'>" + element.Nome + "</option>");
                                });

                                $('#Quantidade').focus();
                            }
                        });


                    }
                    else {
                        $("#loteValidade").hide();
                    }
                });
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

        selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoDropdown");

    };
})(jQuery);