(function ($) {
    app.modals.CriarOuEditarAgendamentoConsultaModal = function () {

        var _agendamentoConsultasService = abp.services.app.agendamentoConsulta;
        var _agendamentoConsultasDisponibilidadeService = abp.services.app.agendamentoConsultaMedicoDisponibilidade;
        var _medicoEspecialidadeAppService = abp.services.app.medicoEspecialidade;

        var _modalManager;
        var _$agendamentoConsultaInformationsForm = null;


        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$agendamentoConsultaInformationsForm = _modalManager.getModal().find('form[name=AgendamentoConsultaInformationsForm]');
            _$agendamentoConsultaInformationsForm.validate();

            $('#loader-div').hide().ajaxStart(function () {
                $(this).show();  // show Loading Div
            }).ajaxStop(function () {
                $(this).hide(); // hide loading div
            }).ajaxError(function () {
                $(this).hide(); // hide loading div
            });

            $('.select2').css('width', '100%');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };


            if ($('#medico-id').val() > 0) {
                $('#medico-id').trigger('change');
            }


            if ($('#id').val() > 0) {
                $('#medico-especialidade-id').trigger('change');
            }

        };

        this.save = function () {
            if (!_$agendamentoConsultaInformationsForm.valid()) {
                return;
            }

            var agendamentoConsulta = _$agendamentoConsultaInformationsForm.serializeFormToObject();
            _modalManager.setBusy(true);

            _agendamentoConsultasService.criarOuEditar(agendamentoConsulta)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarAgendamentoConsultaModalSaved');
                     $('#calendar').fullCalendar('refetchEvents');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        function _delete() {
            abp.message.confirm(
                app.localize('DeleteWarning', app.localize('AgendamentoConsulta') + ' ' + $('#form-data-agendamento').val() + ' ' + $('#hora-agendamento').val()),
                function (isConfirmed) {
                    if (isConfirmed) {
                        $('#btn-excluir-agendamento').buttonBusy(true);
                        _agendamentoConsultasService.excluir($('#id').val())
                        .done(function () {
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                            _modalManager.close();
                            abp.event.trigger('app.CriarOuEditarAgendamentoConsultaModalSaved');
                            $('#calendar').fullCalendar('refetchEvents');
                        })
                        .always(function () {
                            $('#btn-excluir-agendamento').buttonBusy(false);
                        });
                    }
                }
            );
        }

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
                $('#form-paciente-id').val('');
                $('#paciente-nao-cadastrado').removeClass('hidden').css('display', 'block');

            }
        });

        $('input[name="DataAgendamento"]')
            .on('input', function () {
                if ($(this).val().length === 10) {
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

                    $('input[name="DataAgendamento"]').val(moment(selDate).format('L')).addClass('form-control');
                    $('#calendar').fullCalendar('gotoDate', selDate);
                    //atualizar a lista de horários para a data selecionada
                    //updateCombos();
                    updateCriarOuEditarAgendamentoConsultaViewModel();
                });

        aplicarSelect2Padrao();

        $('#medico-id').select2({
            ajax: {
                url: '/api/services/app/agendamentoconsultamedicodisponibilidade/ListarMedicosDisponiveisDropdown',
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
        })
            .on('change', function () {

               

                if ($('#id').val() == 0 || $('#id').val() == null) {

                    _agendamentoConsultasDisponibilidadeService.obterSomenteUmaEspecialidade($('#medico-id').val())
                   .done(function (data) {

                       if (data != null) {

                           $('#medico-especialidade-id')


                           .append($("<option>") //add option tag in select
                      .val(data.id) //set value for option to post it
                      .text(data.nome)
                    ) //set a text for show in select
              .val(data.id)
                            .trigger("change")//select option of select2
                       }
                       else {
                           $('#medico-especialidade-id').val(null).trigger('change');
                       }

                   });
                }

                if ($(this).val() > 0) {
                    $('#medico-especialidade-id').removeAttr('disabled');
                }
                else {
                    $('#medico-especialidade-id').attr('disabled', 'disabled');
                }
            });

        $('#medico-especialidade-id').select2({
            ajax: {
                url: '/api/services/app/agendamentoconsultamedicodisponibilidade/ListarEspecialidadesMedicoDropdown',
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
        })
            .on('change', function () {

                var medicoEspecialidadeId = $(this).val();
                var id = $('#id').length > 0 ? $('#id').val() : 0;
                var myDate = $('#data-agendamento').val();
                var aDate = myDate.split('/');
                var medicoId = $('#medico-id').val();
                $('#div-horarios').load('/mpa/AgendamentoConsultas/_MontarComboHorarios',
                    {
                        medicoEspecialidadeId: medicoEspecialidadeId,
                        medicoId: medicoId,
                        date: aDate[2] + '-' + aDate[1] + '-' + aDate[0],
                        id: id,
                        dataHora: $('#hora-agendamento').val()
                    }, function () {

                        $('#agendamento-consulta-medico-disponibilidade-id').on('change', function (e) {
                            var hora = $('#agendamento-consulta-medico-disponibilidade-id option:selected').text();
                            var data = $('#data-agendamento').val();
                            $('#hora-agendamento').val(data + ' ' + hora);

                            carregarDivQuantidadeHorarios();

                        }).trigger('change');

                    }
                );
            });

        $('#plano-id').select2({
            ajax: {
                url: '/api/services/app/plano/ListarPorConvenioDropdown',
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
                        filtro: $('#convenio-id').val()
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

        $('#convenio-id').select2({
            ajax: {
                url: '/api/services/app/convenio/ListarDropdown',
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
                        filtro: $('#convenio-id').val()
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
        })
            .on('change', function () {
                if ($(this).val() > 0) {
                    $('#plano-id').removeAttr('disabled');
                }
                else {
                    $('#plano-id').attr('disabled', 'disabled');
                }
            });



        $('#hora-agendamento').on('change', function (){
           carregarDivQuantidadeHorarios();
        });

        function carregarDivQuantidadeHorarios() {

            var medicoEspecialidadeId = $('#medico-especialidade-id').val();
            var id = $('#id').length > 0 ? $('#id').val() : 0;
            var myDate = $('#data-agendamento').val();
            var aDate = myDate.split('/');
            var medicoId = $('#medico-id').val();


            $('#divQuantidadehorarios').load('/mpa/AgendamentoConsultas/_MontarComboQuantidadeHorarios',
                   {
                       medicoEspecialidadeId: medicoEspecialidadeId,
                       medicoId: medicoId,
                       date: aDate[2] + '-' + aDate[1] + '-' + aDate[0],
                       id: id,
                       dataHora: $('#hora-agendamento').val(),
                       quantidadeHorarios: $('#quantidadeHorarios').val()
                   }, function () {

                       $('#agendamento-consulta-medico-quantidadeHorarios-id').on('change', function (e) {
                           var qtd = $('#agendamento-consulta-medico-quantidadeHorarios-id option:selected').text();
                         //  var data = $('#data-agendamento').val();
                             $('#quantidadeHorarios').val(qtd);
                       }).trigger('change');

                   }
            );

        }

        //Novo Botão de editar na tela de atendimento
        $('#editPacienteButton').click(function (e) {
            e.preventDefault();
            var id = $('#comboPaciente').val();
            if (id != "") {
                _editModal.open({ id: id });
            }

        });


        $('#pacienteButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

    };
})(jQuery);