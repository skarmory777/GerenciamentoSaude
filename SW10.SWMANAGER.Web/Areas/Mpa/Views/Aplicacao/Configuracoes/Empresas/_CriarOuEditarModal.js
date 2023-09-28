(function ($) {
    app.modals.CriarOuEditarEmpresaModal = function () {

        var _empresasService = abp.services.app.empresa;

        var _modalManager;
        var _$empresaInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$empresaInformationForm = _modalManager.getModal().find('form[name=EmpresaInformationsForm]');
            _$empresaInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('div.form-group select').addClass('form-control selectpicker');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        };

        this.save = function () {
            if (!_$empresaInformationForm.valid()) {
                return;
            }

            var empresa = _$empresaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _empresasService.criarOuEditar(empresa)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarEmpresaModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        //date picker

        $('input[name="DataInicialContrato"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date() + 720,
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-50:c+5',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY/MM/DD",
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
            $('input[name="DataInicialContrato"]').val(selDate.format('L')).addClass('form-control edited');
        });

        $('input[name="DataUltimaRenovacaoContrato"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date() + 7200,
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-50:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY/MM/DD",
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
            $('input[name="DataUltimaRenovacaoContrato"]').val(selDate.format('L')).addClass('form-control edited');
        });

        $('input[name="DataProximaRenovacaoContrato"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            maxDate: new Date() + 20000,
            autoUpdateInput: false,
            changeYear: true,
            yearRange: 'c-5:c+50',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY/MM/DD",
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
            $('input[name="DataProximaRenovacaoContrato"]').val(selDate.format('L')).addClass('form-control edited');
        });

        $('#btn-buscar-cep').click(function (e) {
            e.preventDefault();
            var cep = $('#cep').val().replace('-', '');
            if (isNaN(cep)) {
                abp.notify.info(app.localize("CepInvalido"));
                return false;
            }
            if (cep === '') {
                abp.notify.info(app.localize("InformarCep"));
                return false;
            }
            if (cep.length !== 8) {
                abp.notify.info(app.localize("TamanhoCep"));
                return false;
            }
            buscarCep(cep);
        });

        $('#capturar-imagem').click(function (e) {
            e.preventDefault();
            //if (typeof ($("input#file")) === "undefined") {
            $('<input>', {
                'id': 'file',
                'class': 'hidden',
                'name': 'File',
                'type': 'file',
                'onchange': lerImagemForm(this, 'logotipo', 'logotipo-mime-type', 'logotipo-img')
            }).appendTo('body');
            //}
            $('#file').change(function () {
                lerImagemForm(this, 'logotipo', 'logotipo-mime-type', 'logotipo-img');
            })
                .click();
        });

    };
})(jQuery);