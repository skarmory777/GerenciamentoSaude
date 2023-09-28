(function ($) {
    app.modals.CriarOuEditarParentescoModal = function () {

        var _ParentescosService = abp.services.app.parentesco;

        var _modalManager;
        var _$ParentescoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$ParentescoInformationForm = _modalManager.getModal().find('form[name=ParentescoInformationsForm]');
            _$ParentescoInformationForm.validate();
        };

        this.save = function () {
            if (!_$ParentescoInformationForm.valid()) {
                return;
            }

            var Parentesco = _$ParentescoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _ParentescosService.criarOuEditar(Parentesco)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarParentescoModalSaved');
                    //location.reload();//seguindo o projeto pronto
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        //        $('input[name="DiaMesAno"]').daterangepicker({
        //            "singleDatePicker": true,
        //            "showDropdowns": true,
        //            maxDate: new Date() + 1456,
        //            autoUpdateInput: false,
        //            changeYear: true,
        //            yearRange: 'c-10:c+10',
        //            showOn: "both",
        //            "locale": {
        //                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
        //                "separator": " - ",
        //                "applyLabel": "Apply",
        //                "cancelLabel": "Cancel",
        //                "fromLabel": "From",
        //                "toLabel": "To",
        //                "customRangeLabel": "Custom",
        //                "daysOfWeek": [
        //                    app.localize('Dom'),
        //                    app.localize('Seg'),
        //                    app.localize('Ter'),
        //                    app.localize('Qua'),
        //                    app.localize('Qui'),
        //                    app.localize('Sex'),
        //                    app.localize('Sab')
        //                ],
        //                "monthNames": [
        //                    app.localize("Jan"),
        //                    app.localize("Fev"),
        //                    app.localize("Mar"),
        //                    app.localize("Abr"),
        //                    app.localize("Mai"),
        //                    app.localize("Jun"),
        //                    app.localize("Jul"),
        //                    app.localize("Ago"),
        //                    app.localize("Set"),
        //                    app.localize("Out"),
        //                    app.localize("Nov"),
        //                    app.localize("Dez"),
        //                ],
        //                "firstDay": 0
        //            }
        //        },
        //function (selDate) {
        //    $('input[name="DiaMesAno"]').val(selDate.format('L')).addClass('form-control edited');
        //});

    };
    aplicarDateRange();
    aplicarDateSingle();
    aplicarSelect2Padrao();
    CamposRequeridos();
})(jQuery);