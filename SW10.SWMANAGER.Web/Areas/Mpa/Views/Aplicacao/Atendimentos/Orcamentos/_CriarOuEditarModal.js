(function ($) {
    app.modals.CriarOuEditarOrcamentoModal = function () {
        
        var _orcamentosService = abp.services.app.orcamento;
        var _modalManager;
        var _$orcamentoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            $('.modal-dialog').css({ 'width': '80%', 'max-width': '1100px' });

            _$orcamentoInformationForm = _modalManager.getModal().find('form[name=OrcamentoInformationsForm]');
            _$orcamentoInformationForm.validate({ ignore: "" });
            //$('select').addClass('form-control edited');
        };

        this.save = function () {
            
            //if (!_$orcamentoInformationForm.valid()) {
            //    return;
            //}

            //var orcamento = _$orcamentoInformationForm.serializeFormToObject();
            
            //_modalManager.setBusy(true);

            //_orcamentosService.criarOuEditar(orcamento)
            //     .done(function () {
            //         abp.notify.info(app.localize('SavedSuccessfully'));
            //         _modalManager.close();
            //         abp.event.trigger('app.CriarOuEditarOrcamentoModalSaved');
            //     })
            //    .always(function () {
            //        _modalManager.setBusy(false);
            //    });
        };

        // Date Picker
        //$('input[name="Nascimento"]').daterangepicker({
        //    "singleDatePicker": true,
        //    "showDropdowns": true,
        //    autoUpdateInput: false,
        //    maxDate: new Date(),
        //    changeYear: true,
        //    yearRange: 'c-10:c+10',
        //    showOn: "both",
        //    "locale": {
        //        "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
        //        "separator": " - ",
        //        "applyLabel": "Apply",
        //        "cancelLabel": "Cancel",
        //        "fromLabel": "From",
        //        "toLabel": "To",
        //        "customRangeLabel": "Custom",
        //        "daysOfWeek": [
        //            app.localize('Dom'),
        //            app.localize('Seg'),
        //            app.localize('Ter'),
        //            app.localize('Qua'),
        //            app.localize('Qui'),
        //            app.localize('Sex'),
        //            app.localize('Sab')
        //        ],
        //        "monthNames": [
        //            app.localize("Jan"),
        //            app.localize("Fev"),
        //            app.localize("Mar"),
        //            app.localize("Abr"),
        //            app.localize("Mai"),
        //            app.localize("Jun"),
        //            app.localize("Jul"),
        //            app.localize("Ago"),
        //            app.localize("Set"),
        //            app.localize("Out"),
        //            app.localize("Nov"),
        //            app.localize("Dez"),
        //        ],
        //        "firstDay": 0
        //    }
        //},
        //function (selDate) {
        //    $('input[name="Nascimento"]').val(selDate.format('L')).addClass('form-control edited');
        //});

    };
})(jQuery);