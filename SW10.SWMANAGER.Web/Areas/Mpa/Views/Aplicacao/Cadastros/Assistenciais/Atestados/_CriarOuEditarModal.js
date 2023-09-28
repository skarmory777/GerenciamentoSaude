(function ($) {
    app.modals.CriarOuEditarAtestadoModal = function () {

        var _AtestadosService = abp.services.app.atestado;
        var _modeloAtestadoService = abp.services.app.modeloAtestado;
        var _modalManager;
        var _$AtestadoInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$AtestadoInformationForm = _modalManager.getModal().find('form[name=AtestadoInformationsForm]');
            _$AtestadoInformationForm.validate();
            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '1100px' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');
            var textarea = $('#conteudo');
            textarea.Editor();
            textarea.Editor('setText', textarea.text());
        };

        this.save = function () {
            var $textarea = $("#conteudo");
            $textarea.text($textarea.Editor("getText"));

            if (!_$AtestadoInformationForm.valid()) {
                return;
            }

            var atestado = _$AtestadoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _AtestadosService.criarOuEditar(atestado)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarAtestadoModalSaved');
                     //location.reload();//seguindo o projeto pronto
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('#paciente-search').autocomplete({
            minLength: 3,
            delay: 0,
            source: function (request, response) {
                var term = $('#paciente-search').val();
                var url = '/mpa/pacientes/autocomplete';
                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.result.length == 0) {
                        $('#paciente-id').val(0);
                        $("#paciente-search").focus();
                        abp.notify.error(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        $('#paciente-id').val(0);
                        return {
                            value: item.nomeCompleto,
                            label: item.nomeCompleto,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                $('#paciente-id').val(ui.item.realValue);
                $('#paciente-search').val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $('#paciente-id').val(0);
                    $("#paciente-search").val('').focus();
                    abp.notify.error(app.localize("PacienteInvalido"));
                    return false;
                }
            },
        });

        $('#medico-search').autocomplete({
            minLength: 3,
            delay: 0,
            source: function (request, response) {
                var term = $('#medico-search').val();
                var url = '/mpa/medicos/autocomplete';
                var fullUrl = url + '/?term=' + term;
                $.getJSON(fullUrl, function (data) {
                    if (data.result.length == 0) {
                        $('#medico-id').val(0);
                        $("#medico-search").focus();
                        abp.notify.error(app.localize("ListaVazia"));
                        return false;
                    };
                    response($.map(data.result, function (item) {
                        $('#medico-id').val(0);
                        return {
                            value: item.nomeCompleto,
                            label: item.nomeCompleto,
                            realValue: item.id
                        };
                    }));
                });
            },
            select: function (event, ui) {
                $('#medico-id').val(ui.item.realValue);
                $('#medico-search').val(ui.item.value);
                //$('.save-button').removeAttr('disabled');
                return false;
            },
            change: function (event, ui) {
                event.preventDefault();
                if (ui.item == null) {
                    //$('.save-button').attr('disabled', 'disabled');
                    $('#medico-id').val(0);
                    $("#medico-search").val('').focus();
                    abp.notify.error(app.localize("MedicoInvalido"));
                    return false;
                }
            },
        });

        $('input[name="DataAtendimento"]').daterangepicker({
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
            $('input[name="DataAtendimento"]').val(selDate.format('L')).addClass('form-control edited');
        });

        $('#modelo-atestado-id').on('change', function (e) {
            e.preventDefault();
            _modeloAtestadoService.obter($(this).val())
            .done(function (data) {
                $('#conteudo').Editor('setText', data.conteudo);
            });
        });
    };
})(jQuery);