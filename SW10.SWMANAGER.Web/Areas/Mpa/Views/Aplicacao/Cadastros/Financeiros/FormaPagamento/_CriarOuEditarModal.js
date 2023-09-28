(function ($) {
    app.modals.CriarOuEditarFormaPagamentoModal = function () {


        $(document).ready(function () {

            $('#percentualDesconto').mask('000,00', { reverse: true });
        });


        var _formaPagamentoService = abp.services.app.formaPagamento;

        var _modalManager;
        var _$FormaPagamentoInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$FormaPagamentoInformationsForm = _modalManager.getModal().find('form[name=FormaPagamentoInformationsForm]');
            _$FormaPagamentoInformationsForm.validate();
        };

        this.save = function () {


           
            //if (!_$FormaPagamentoInformationsForm.valid()) {
            //    return;
            //}

            var formaPagamento = _$FormaPagamentoInformationsForm.serializeFormToObject();


            formaPagamento.PercentualDesconto = retirarMascara(formaPagamento.PercentualDesconto);

            _modalManager.setBusy(true);
            _formaPagamentoService.criarOuEditar(formaPagamento)
                 .done(function (data) {
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {

                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                         //location.reload();//seguindo o projeto pronto
                     }
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('input[name="DiaMesAno"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date() + 1456,
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
    $('input[name="DiaMesAno"]').val(selDate.format('L')).addClass('form-control edited');
});


  

        $('.meuMaxlength').on('keyup', function (e) {
            if (this.value > 99) { this.value = '99'; } else if (this.value < 0) { this.value = '0'; }
        });


        function retirarMascara(valor) {

            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }


    };
})(jQuery);