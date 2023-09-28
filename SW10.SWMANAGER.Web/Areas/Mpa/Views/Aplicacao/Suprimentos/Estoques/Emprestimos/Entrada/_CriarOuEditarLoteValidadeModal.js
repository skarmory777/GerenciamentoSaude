
(function ($) {
    app.modals.CriarOuEditarEstoquePreMovimentoLoteValidadeDtoModalViewModel = function () {

        var _modalManagerLote;

        var _createOrEditLoteValidadeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PreMovimentos/InformarLoteValidadeModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_InformarLoteValidade.js',
            modalClass: 'EstoquePreMovimentoLoteViewModel'
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
            //  scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_InformarLoteValidade.js',
            //  modalClass: 'EstoquePreMovimentoLoteValidadeProduto'
        });


        var _estoqueLoteValidade = abp.services.app.estoqueLoteValidade;
        var _estoquePreMovimentoLoteValidade = abp.services.app.estoquePreMovimentoLoteValidade;


        this.init = function (modalManager) {
            _modalManagerLote = modalManager;

            $('#QuantidadeLote').mask('000.000.000,00', { reverse: true });

            $('.modal-dialog').css('width', '800px');
            $('.selectpicker').selectpicker('refresh');
        }

        $('#salvar-PreMovimentoLoteValidade').click(function (e) {
            e.preventDefault()

            var _$preMovimentoLoteValidadeForm = $('form[name=preMovimentoLoteValidadeForm');

            if (!_$preMovimentoLoteValidadeForm.valid()) {
                return;
            }

            var _estoqueLoteValidadeForm = _$preMovimentoLoteValidadeForm.serializeFormToObject();

            _estoqueLoteValidadeForm.Quantidade = retirarMascara(_estoqueLoteValidadeForm.Quantidade);

            _estoqueLoteValidade.criarOuEditar(_estoqueLoteValidadeForm)
                  .done(function (data) {

                      if (data.errors.length > 0) {
                          _ErrorModal.open({ erros: data.errors });
                      }
                      else {
                          abp.notify.info(app.localize('SavedSuccessfully'));

                    

                          if ($('#Id').val() != undefined && $('#Id').val() != '' && $('#Id').val() != '0') {
                              _modalManagerLote.close();
                          }

                          $('#Id').val('');
                          $('#LaboratorioId').val('').selectpicker('refresh');;
                          $('#LoteId').val('');
                          $('#validadeId').val('');
                          $('#QuantidadeLote').val('');
                          $('#LoteId').focus();


                          _estoquePreMovimentoLoteValidade.obterQuantidadeRestanteLoteValidade(_estoqueLoteValidadeForm.EstoquePreMovimentoItemId)
                                                    .done(function (dataQuantidade) {
                                                     
                                                        var quanitadade = dataQuantidade;

                                                        if (quanitadade != 0) {
                                                            $('#QuantidadeLote').val(dataQuantidade);
                                                        }
                                                        else
                                                        {
                                                            _modalManagerLote.close();
                                                        }
                                                    });




                          abp.event.trigger('app.preMovimentoLoteValidadeModalSaved');
                      }
                  })
                      .always(function () {
                      });

        });


        $('input[name="Validade"]').daterangepicker({

            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            minDate: new Date(),
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
                "firstDay": 0,

                onClose: function (dateText, inst) {
                    var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    $(this).val($.datepicker.formatDate('yy-mm', new Date(year, month, 1)));
                }

            }
        },
      function (selDate) {
          $('input[name="Validade"]').val(selDate.format('L')).addClass('form-control edited');
          // obterIdade(selDate);
      });

        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        selectSW('.selectLaboratorio', "/api/services/app/produtoLaboratorio/ListarDropdown");
        


    };
})(jQuery);