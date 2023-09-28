(function ($) {
    app.modals.CriarOuEditarFeriadoModal = function () {

        var _FeriadosService = abp.services.app.feriado;

        var _modalManager;
        var _$FeriadoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$FeriadoInformationForm = _modalManager.getModal().find('form[name=FeriadoInformationsForm]');
            _$FeriadoInformationForm.validate();
        };

        this.save = function () {
            if (!_$FeriadoInformationForm.valid()) {
                return;
            }

            var feriado = _$FeriadoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _FeriadosService.criarOuEditar(feriado)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
                     //location.reload();//seguindo o projeto pronto
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
        aplicarDateRange();
        aplicarDateSingle();
        aplicarSelect2Padrao();
        CamposRequeridos();
    };
})(jQuery);