(function ($) {
    app.modals.CriarOuEditarAgendamentoConsultaMedicoDisponibilidadeModal = function () {

        var _agendamentoConsultaMedicoDisponibilidadesService = abp.services.app.agendamentoConsultaMedicoDisponibilidade;

        var _modalManager;
        var _$agendamentoConsultaMedicoDisponibilidadesInformationForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$agendamentoConsultaMedicoDisponibilidadeInformationForm = _modalManager.getModal().find('form[name=AgendamentoConsultaMedicoDisponibilidadeInformationsForm]');
            _$agendamentoConsultaMedicoDisponibilidadeInformationForm.validate();
            $('.modal-dialog').css({ 'width': '80%', 'max-width': '1100px' });
            $('.select2').css('width', '100%');
        };

        this.save = function () {
            if (!_$agendamentoConsultaMedicoDisponibilidadeInformationForm.valid()) {
                return;
            }

            var agendamentoConsultaMedicoDisponibilidade = _$agendamentoConsultaMedicoDisponibilidadeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _agendamentoConsultaMedicoDisponibilidadesService.criarOuEditar(agendamentoConsultaMedicoDisponibilidade)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarAgendamentoConsultaMedicoDisponibilidadeModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('input[name="DataInicio"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: moment().add('year',2),
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
            maxDate: moment().add('year',2),
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





        $('#medico-id').change(function (e) {
            e.preventDefault()

            $('#medico-especialidade-id').val(null).trigger("change");

        }
        );




        $('#medico-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/medico/ListarDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: data.result.items,
                        pagination: {
                            more: (params.page * 10) < data.result.totalCount
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0
        });

        function especialidadeSelect2()
        {

        }


        $('#medico-especialidade-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/medicoespecialidade/ListarDropdownPorMedico',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                        filtro: $('#medico-id').val()
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: data.result.items,
                        pagination: {
                            more: (params.page * 10) < data.result.totalCount
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0
        });
    };
})(jQuery);