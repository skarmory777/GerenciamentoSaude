(function ($) {
    app.modals.AltaModalViewModel = function () {
    
        var _atendimentoService = abp.services.app.atendimento;

        var _modalManager;
        var _$AltasInformationForm = null;
        var _$AltaMedicaInformationsForm = null;
       
        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$AltaInformationForm = _modalManager.getModal().find('form[name=AltaInformationsForm]');
            _$AltaInformationForm.validate();
        };

        $('input[name="Data"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "timePicker": true,
            "timePicker24Hour": true,
            "startDate": moment(),
            "endDate": moment(),
            autoUpdateInput: false,
            maxDate: moment().add(1, 'days'),
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
                $('input[name="Data"]').val(selDate.format('L LT')).addClass('form-control edited');
            });


        $('#alta-medica-parcial').load('/AltaMedica/_CriarOuEditarAltaMedicaModal?altaMedicaId=' + $('#id').val());
                
    };
})(jQuery);