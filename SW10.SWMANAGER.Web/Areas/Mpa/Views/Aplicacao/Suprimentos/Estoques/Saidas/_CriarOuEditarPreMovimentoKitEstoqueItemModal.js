(function ($) {
    app.modals.CriarOuEditarPreMovimentoKitEstoqueItemModal = function () {

        var _preMovimentoItemService = abp.services.app.estoquePreMovimentoItem;

        var _modalManager;
        var _$PreMovimentoKitEstoqueItemInformationForm = null;

        $(document).ready(function () {
            $('#Quantidade').mask('000.000.000,00', { reverse: true });
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$PreMovimentoKitEstoqueItemInformationForm = _modalManager.getModal().find('form[name=PreMovimentoKitEstoqueItemInformationsForm]');
            _$PreMovimentoKitEstoqueItemInformationForm.validate();
            $('ul.ui-autocomplete').css('z-index', '2147483647 !important');
        };

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros'
        });

        this.save = function () {

            if (!_$PreMovimentoKitEstoqueItemInformationForm.valid()) {
                return;
            }

            var kitEstoqueItem = _$PreMovimentoKitEstoqueItemInformationForm.serializeFormToObject();

            kitEstoqueItem.Quantidade = retirarMascara(kitEstoqueItem.Quantidade);

            _modalManager.setBusy(true);

            kitEstoqueItem.EstoqueId = $('#EstoqueId').val();

            _preMovimentoItemService.criarKitEstoqueItem(kitEstoqueItem)
                .done(function (data) {
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {
                        _modalManager.close();

                        abp.notify.info(app.localize('SavedSuccessfully'));
                        abp.event.trigger('app.CriarOuEditarPreMovimentoItemModalSaved');

                        $('#Quantidade').val('');
                    }
                }
                ).always(function () {
                    _modalManager.setBusy(false);
                }
                )
        };

        $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });

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
    };
})(jQuery);