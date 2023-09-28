(function ($) {
    app.modals.CriarOuEditarVersaoTissModal = function () {

        var _versoesTissService = abp.services.app.versaoTiss;

        var _modalManager;
        var _$versaoTissInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$versaoTissInformationForm = _modalManager.getModal().find('form[name=VersaoTissInformationsForm]');
            _$versaoTissInformationForm.validate({ ignore: "" });
            //$('.modal-dialog').css('width', '1100px');
            //$('select').addClass('form-control edited');
            
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        };

        this.save = function () {
           
            if (!_$versaoTissInformationForm.valid()) {
                return;
            }

            var versaoTiss = _$versaoTissInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _versoesTissService.criarOuEditar(versaoTiss)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarVersaoTissModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        // Date pickers
        $('input[name="DataInicio"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //minDate: moment(),
            maxDate: moment().add('year', 15),
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
           $('input[name="DataInicio"]').val(selDate.format('L')).addClass('form-control edited');
       });

        $('input[name="DataFim"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //minDate: moment(),
            maxDate: moment().add('year', 15),
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
           $('input[name="DataFim"]').val(selDate.format('L')).addClass('form-control edited');
       });

    };
})(jQuery);