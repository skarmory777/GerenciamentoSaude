(function ($) {

    var _agendamentoConsultasService = abp.services.app.agendamentoConsulta;

    var _$agendamentoConsultaInformationForm = null;

    _$agendamentoConsultaInformationForm = $('form[name=AgendamentoConsultaInformationsForm]');
    _$agendamentoConsultaInformationForm.validate();

    function _save() {

        _$agendamentoConsultaInformationForm = $('form[name=AgendamentoConsultaInformationsForm]');
        _$agendamentoConsultaInformationForm.validate();
        if (!_$agendamentoConsultaInformationForm.valid()) {
            abp.notify.error(app.localize('ErroSalvar'));
        }
        else {
            var agendamentoConsulta = _$agendamentoConsultaInformationForm.serializeFormToObject();
            $('#btn-salvar-agendamento').buttonBusy(true);

            _agendamentoConsultasService.criarOuEditar(agendamentoConsulta)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     abp.event.trigger('app.CriarOuEditarAgendamentoConsultaModalSaved');
                     $('#form-titulo').html(app.localize('CreateNewAgendamentoConsulta'))
                     $('#form-medico-id').val('');
                     $('#form-medico-especialidade-id').val('');
                     $('#form-data-agendamento').val(moment().format('L'));
                     $('#form-data-agendamento').data('daterangepicker').setStartDate(moment().format('L'));
                     $('#form-data-agendamento').data('daterangepicker').setEndDate(moment().format('L'));
                     $('#form-hora-agendamento').val('');
                     $('#form-paciente-id').val('');
                     $('#form-convenio-id').val('');
                     $('#form-notas').val('');
                     $('#id').val('');
                     $('#div-btn-excluir').addClass('hidden');

                     $('.chosen-select').trigger("chosen:updated");
                     $('#calendar').fullCalendar('refetchEvents');
                 })
                .always(function () {
                    $('#btn-salvar-agendamento').buttonBusy(false);
                });
        }
    }

    function _delete() {
        abp.message.confirm(
            app.localize('DeleteWarning', app.localize('AgendamentoConsulta') + ' ' + $('#form-data-agendamento').val() + ' ' + $('#hora-agendamento').val()),
            function (isConfirmed) {
                if (isConfirmed) {
                    $('#btn-excluir-agendamento').buttonBusy(true);
                    _agendamentoConsultasService.excluir($('#id').val())
                    .done(function () {
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                        $('#form-titulo').html(app.localize('CreateNewAgendamentoConsulta'))
                        $('#form-medico-id').val('');
                        $('#form-medico-especialidade-id').val('');
                        $('#form-data-agendamento').val(moment().format('L'));
                        $('#form-data-agendamento').data('daterangepicker').setStartDate(moment().format('L'));
                        $('#form-data-agendamento').data('daterangepicker').setEndDate(moment().format('L'));
                        $('#form-hora-agendamento').val('');
                        $('#form-paciente-id').val('');
                        $('#form-convenio-id').val('');
                        $('#form-notas').val('');
                        $('#id').val('');
                        $('#div-btn-excluir').addClass('hidden');
                        $('.chosen-select').trigger("chosen:updated");
                        $('#calendar').fullCalendar('refetchEvents');
                    })
                    .always(function () {
                        $('#btn-excluir-agendamento').buttonBusy(false);
                    });
                }
            }
        );
    }

    $('#form-medico-id').on('change', function (e) {
        e.preventDefault();
        var id = $(this).val();
        var myDate = $('input[name="DataAgendamento"]').val();
        var aDate = myDate.split('/');
        $('#div-medico-especialidades').load('/mpa/AgendamentoConsultas/_MontarComboMedicoEspecialidades',
            {
                medicoId: id,
                date: aDate[2] + '-' + aDate[1] + '-' + aDate[0]
            },
            function () {
                $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' }).trigger("chosen:updated");
            }
        );

        //$("#filtro-medico-id").val(id);
        //$.ajax({
        //    url: "/Mpa/AgendamentoConsultas/MedicoEspecialidades/" + id,
        //    success: function (data) {
        //        $("#filtro-medico-especialidade-id").empty().append('<option value="">' + app.localize('SelecioneLista') + '</option>');
        //        $("#form-medico-especialidade-id").empty().append('<option value="">' + app.localize('SelecioneLista') + '</option>');
        //        $.each(data.result, function (index, element) {
        //            $("#filtro-medico-especialidade-id").append('<option value="' + element.id + '">' + element.nome + '</option>');
        //            $("#form-medico-especialidade-id").append('<option value="' + element.id + '">' + element.nome + '</option>');
        //        });
        //        $("#filtro-medico-especialidade-id").trigger("chosen:updated")
        //        $("#form-medico-id").trigger("chosen:updated")
        //        $("#form-medico-especialidade-id").trigger("chosen:updated")

        //    }
        //});
    });

    $('#form-medico-especialidade-id').on('change', function (e) {
        e.preventDefault();
        //loadCriarEditarModal();
        var medicoEspecialidadeId = $(this).val();
        var id = $('#id').length > 0 ? $('#id').val() : 0;
        var myDate = $('input[name="DataAgendamento"]').val();
        var aDate = myDate.split('/');
        var medicoId = $('form-medico-id').val();
        $('#div-horarios').load('/mpa/AgendamentoConsultas/_MontarComboHorarios',
            {
                medicoEspecialidadeId: medicoEspecialidadeId,
                medicoId: medicoId,
                date: aDate[2] + '-' + aDate[1] + '-' + aDate[0],
                id: id
            },
            function () {
                $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            }
        );

    });

    $('#btn-salvar-agendamento').on('click', function (e) {
        e.preventDefault();
        _save()
    });

    $('#btn-excluir-agendamento').on('click', function (e) {
        e.preventDefault();
        _delete();
    });

    $('#opt-paciente-cadastrado').on('click', function (e) {
        if ($(this).is(':checked')) {
            $('#nome-reservante').val('');
            $('#telefone-reservante').val('');
            $('#data-nascimento-reservante').val('');
            $('#paciente-cadastrado').removeClass('hidden').css('display', 'block');
            $('#paciente-nao-cadastrado').addClass('hidden');
        }
        else {
            $('#paciente-cadastrado').addClass('hidden');
            $('#paciente-id').val('');
            $('#paciente-nao-cadastrado').removeClass('hidden').css('display', 'block');

        }
    });

    function loadCriarEditarModal() {
        $('#novo-agendamento').load(
            '/mpa/AgendamentoConsultas/_CriarOuEditarAgendamentoConsulta',
            {
                Id: $('#id').val(),
                MedicoId: $('#form-medico-id').length == 0 ? null : $('#form-medico-id').val(),
                MedicoEspecialidadeId: $('#form-medico-especialidade-id').length == 0 ? null : $('#form-medico-especialidade-id').val(),
                AgendamentoConsultaMedicoDisponibilidadeId: $('#form-agendamento-consulta-medico-disponibilidade-id').length == 0 ? null : $('#form-agendamento-consulta-medico-disponibilidade-id').val(),
                PacienteId: $('#form-pacinete-id').length == 0 ? null : $('#form-pacinete-id').val(),
                ConvenioId: $('#form-convenio-id').length == 0 ? null : $('#form-convenio-id').val(),
                PlanoId: $('#form-plano-id').length == 0 ? null : $('#form-plano-id').val(),
                Notas: $('#form-notas').length == 0 ? null : $('#form-notas').val(),
                NomeReservante: $('#form-nome-reservante').length == 0 ? null : $('#form-nome-reservante').val(),
                TelefoneReservante: $('#form-telefone-reservante').length == 0 ? null : $('#form-telefone-reservante').val(),
                DataNascimentoReservante: $('#form-data-nascimento-reservante').length == 0 ? null : $('#form-data-nascimento-reservante').val(),
                ConvenioReservante: $('#form-convenio-reservante').length == 0 ? null : $('#form-convenio-reservante').val(),
                PlanoReservante: $('#form-plano-reservante').length == 0 ? null : $('#form-plano-reservante').val(),
                DataAgendamento: $('#form-data-agendamento').length == 0 ? data : $('#form-data-atendamento').val(),
                HoraAgendamento: $('#form-hora-agendamento').length == 0 ? hora : $('#form-hora-agendamento').val(),
            },
            function () {
                $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            }
        )
    }

    $('input[name="DataAgendamento"]')
        .on('input', function () {
            if ($(this).val().length == 10) {
                var date = $(this).val();
                if (IsValid(date)) {
                    $('#div-medico').load(
                        '/mpa/AgendamentoConsultas/_MontarComboMedicos',
                        {
                            date: date
                        }
                    );
                }
            }
        })
        .on('keyup', function () {
            barraData(this);
        })
        .daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            changeYear: true,
            minDate: moment(),
            maxDate: moment().add('year', 1),
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
        $('input[name="DataAgendamento"]').val(moment(selDate).format('L')).addClass('form-control edited');
        $('#calendar').fullCalendar('gotoDate', selDate);
        //atualizar a lista de horários para a data selecionada
        updateCriarOuEditarAgendamentoConsultaViewModel();
    });


    $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });

})(jQuery);