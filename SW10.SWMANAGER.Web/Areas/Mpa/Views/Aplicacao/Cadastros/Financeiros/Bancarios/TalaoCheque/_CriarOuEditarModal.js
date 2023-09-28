(function ($) {
    app.modals.CriarOuEditarTalaoCheque = function () {

        let _args = null;

        $(document).ready(function () {
            CamposRequeridos();
            //$('#limiteCredito').mask('000.000.000,00', { reverse: true });
        });

        var _talaoChequeService = abp.services.app.talaoCheque;

        var _modalManager;
        var _$talaoChequeInformationsForm = null;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            
            _args = modalManager.getArgs();
            _modalManager = modalManager;
            App.stopPageLoading(); document.querySelector('.loadingCommon').style.display = 'none';
           
            $('input[name="DataAbertura"]').val(moment().format('L'));

            _$talaoChequeInformationsForm = _modalManager.getModal().find('form[name=talaoChequeInformationsForm]');
            _$talaoChequeInformationsForm.validate();
        };

        this.save = function () {

            if (!_$talaoChequeInformationsForm.valid()) {
                return;
            }
            
            var talaoCheque = _$talaoChequeInformationsForm.serializeFormToObject();

            _modalManager.setBusy(true);

            App.startPageLoading({ animate: true });document.querySelector('.loadingCommon').style.display = null;
            _talaoChequeService.criarOuEditar(talaoCheque)
                 .done(function (data) {
                     App.stopPageLoading();document.querySelector('.loadingCommon').style.display = 'none';
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {
                         abp.notify.info(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                     }
                 })
                .always(function () {
                    App.stopPageLoading();document.querySelector('.loadingCommon').style.display = 'none';
                    _modalManager.setBusy(false);
                });
        };  

        $('input[name="DataAbertura"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
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
            $('input[name="DataAbertura"]').val(selDate.format('L')).addClass('form-control edited');
        });

        function retirarMascara(valor) {
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');
            valor = valor.replace(',', '.');
            return valor;
        }

        document.getElementById("numeroInicial").oninput = function () { fnInicial() };
        document.getElementById("numeroFinal").oninput = function (ev) { fnFinal(ev) };
        document.getElementById("numeroInicial").onblur = function () {
            let inicial = document.getElementById("numeroInicial");
            if (inicial.value == '') {
                inicial.value = '0';
            }
        };
        document.getElementById("numeroFinal").onblur = function () {
            let inicial = document.getElementById("numeroInicial");
            let final = document.getElementById("numeroFinal");
            if (Number(inicial.value) > Number(final.value)) {
                final.value = inicial.value
            }
        };

        function fnInicial() {
            let inicial = document.getElementById("numeroInicial");
            let final = document.getElementById("numeroFinal");            
            if (Number(inicial.value) > Number(final.value)) {
                final.value = inicial.value
            }
            if (inicial.value.indexOf('-') == 0) {
                inicial.value = '0';
            }
            fnZerar(inicial, final);
        }

        function fnFinal(evt) {
            evt = evt || window.event;
            var key = evt.keyCode || evt.which;
            if (key == undefined) {
                let inicial = document.getElementById("numeroInicial");
                let final = document.getElementById("numeroFinal");
                if (Number(inicial.value) > Number(final.value)) {
                    final.value = inicial.value
                }
                fnZerar(inicial, final);
            }
        }

        function fnZerar(inicial, final){
            if (inicial.value == '') {
                inicial.value = '';
            }
            if (final.value == '') {
                final.value = '0';
            }
        }

        selectSW('.selectContaCorrente', "/api/services/app/ContaCorrente/ListarDropdown");
    };
})(jQuery);