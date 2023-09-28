(function ($) {
    app.modals.ClonarGeradorFormularioModal = function () {

        var _geradorFormulariosService = abp.services.app.geradorFormulario;

        var _modalManager;
        var _$geradorFormularioInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$geradorFormularioInformationForm = _modalManager.getModal().find('form[name=GeradorFormularioInformationsForm]');
            _$geradorFormularioInformationForm.validate({ ignore: "" });
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        };

        //this.save = function () {
        //    if (!_$geradorFormularioInformationForm.valid()) {
        //        return;
        //    }
        //    var geradorFormulario = _$geradorFormularioInformationForm.serializeFormToObject();
        //    _modalManager.setBusy(true);
        //    _geradorFormulariosService.criarOuEditar(geradorFormulario)
        //         .done(function () {
        //             abp.notify.info(app.localize('SavedSuccessfully'));
        //             _modalManager.close();
        //             abp.event.trigger('app.CriarOuEditarGeradorFormularioModalSaved');
        //         })
        //        .always(function () {
        //            _modalManager.setBusy(false);
        //        });
        //};

        //date picker

        //$('input[name="DataInicialContrato"]').daterangepicker({
        //    "singleDatePicker": true,
        //    "showDropdowns": true,
        //    maxDate: new Date() + 720,
        //    autoUpdateInput: false,
        //    changeYear: true,
        //    yearRange: 'c-50:c+5',
        //    showOn: "both",
        //    "locale": {
        //        "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY/MM/DD",
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
        //    $('input[name="DataInicialContrato"]').val(selDate.format('L')).addClass('form-control edited');
        //});

        //$('input[name="DataUltimaRenovacaoContrato"]').daterangepicker({
        //    "singleDatePicker": true,
        //    "showDropdowns": true,
        //    maxDate: new Date() + 7200,
        //    autoUpdateInput: false,
        //    changeYear: true,
        //    yearRange: 'c-50:c+10',
        //    showOn: "both",
        //    "locale": {
        //        "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY/MM/DD",
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
        //    $('input[name="DataUltimaRenovacaoContrato"]').val(selDate.format('L')).addClass('form-control edited');
        //});

        //$('input[name="DataProximaRenovacaoContrato"]').daterangepicker({
        //    "singleDatePicker": true,
        //    "showDropdowns": true,
        //    maxDate: new Date() + 20000,
        //    autoUpdateInput: false,
        //    changeYear: true,
        //    yearRange: 'c-5:c+50',
        //    showOn: "both",
        //    "locale": {
        //        "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY/MM/DD",
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
        //    $('input[name="DataProximaRenovacaoContrato"]').val(selDate.format('L')).addClass('form-control edited');
        //});

        //function readURL(input) {
        //    if (input.files && input.files[0]) {
        //        var reader = new FileReader();

        //        reader.onload = function (e) {
        //            var dados = {};
        //            var base64 = e.target.result;
        //            dados.base64 = base64.substr(base64.indexOf(',') + 1, base64.length);
        //            var type = base64.substr(base64.indexOf(':') + 1, base64.indexOf(';') - 5);
        //            $('#logotipo').val(dados.base64);
        //            $('#logotipo-mime-type').val(type);
        //            $('#logotipo-img').attr({
        //                'src': 'data:' + type + ';base64,' + dados.base64
        //            });
        //        }
        //        reader.readAsDataURL(input.files[0]);
        //    }
        //}

        //$('#btn-buscar-cep').click(function (e) {
        //    e.preventDefault();
        //    var cep = $('#cep').val().replace('-', '');
        //    if (isNaN(cep)) {
        //        abp.notify.info(app.localize("CepInvalido"));
        //        return false;
        //    }
        //    if (cep === '') {
        //        abp.notify.info(app.localize("InformarCep"));
        //        return false;
        //    }
        //    if (cep.length !== 8) {
        //        abp.notify.info(app.localize("TamanhoCep"));
        //        return false;
        //    }
        //    buscarCep(cep);
        //});

        //$('#capturar-imagem').click(function (e) {
        //    e.preventDefault();
        //    //if (typeof ($("input#file")) === "undefined") {
        //    $('<input>', {
        //        'id': 'file',
        //        'class': 'hidden',
        //        'name': 'File',
        //        'type': 'file',
        //        'onchange': readURL(this)
        //    }).appendTo('body');
        //    //}
        //    $('#file').change(function () {
        //        readURL(this)
        //    })
        //        .click();
        //});

        //$("#file").change(function () {
        //    readURL(this);
        //});

    };
})(jQuery);