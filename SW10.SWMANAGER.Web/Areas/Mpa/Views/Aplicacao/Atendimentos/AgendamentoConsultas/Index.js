(function () {
    $(function () {
        var _agendamentoConsultasService = abp.services.app.agendamentoConsulta;

        var myLocale = moment.locale();

        function lerLegendas() {
            $('#legendas-agendamento').load('/mpa/AgendamentoConsultas/_ListarMedicoDisponibilidades',
            {
                medicoId: $('#filtro-medico-id').length > 0 ? $('#filtro-medico-id').val() : null,
                especialidadeId: $('#filtro-medico-especialidade-id').length > 0 ? $('#filtro-medico-especialidade-id').val() : null
            },
            function () {
                $(".accordion").accordion({
                    active: false,
                    collapsible: true,
                    heightStyle: "content",
                    //event: false
                });

                $('.filtro-medico').on('click', function (e) {
                    e.preventDefault();
                    $('#ultimo-medico').val($(this).attr('id'));
                    var active = $(".accordion").accordion("option", "active");
                    if (active !== false) {
                        $('#ultima-tab').val($(".accordion").accordion("option", "active"));
                    }
                    $('#filtro-todos-medicos').trigger('click');
                });

                $('#filtro-todos-medicos').on('click', function () {
                    if ($(this).is(':checked')) {
                        $('#filtro-medico-id').val('');
                        $(".accordion").accordion("option", "active", false);
                    }
                    else {
                        if ($('#ultimo-medico').val() > 0) {
                            $('#filtro-medico-id').val($('#ultimo-medico').val());
                            var active = $(".accordion").accordion("option", "active");
                            var lastActive = $('#ultima-tab').val();
                            if (active === false && lastActive >= 0) {
                                $(".accordion").accordion("option", "active", parseInt(lastActive));
                            }
                        }
                        else {
                            $(this).attr('checked', 'checked');
                        }
                    }
                    $('#calendar').fullCalendar('refetchEvents');
                });
                $('.ui-accordion .ui-accordion-header').css('font-size', '1.2rem');
            });
            //$('#calendar').fullCalendar('refetchEvents');
        }

        lerLegendas();

        $('#calendar').fullCalendar({
            header: {
                left: 'prevYear,prev,next,nextYear today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay,listMonth'
            },
            draggable: true,
            eventStartEditable: true,
            eventDurationEditable: false,
            eventOverlap: false,
            defaultDate: moment(),
            navLinks: true, // can click day/week names to navigate views
            editable: true,
            eventLimit: true, // allow "more" link when too many events
            locale: myLocale,
            //lang: moment.locale(),
            theme: true,
            slotDuration: '00:10:00',
            slotLabelFormat: 'HH:mm',
            weekends: true,
            businessHours: {
                // days of week. an array of zero-based day of week integers (0=Sunday)
                dow: [1, 2, 3, 4, 5], // Monday - Thursday

                start: '07:00', // a start time (10am in this example)
                end: '21:00', // an end time (6pm in this example)
            },
            //editable: true,

            events: function (start, end, timezone, callback) {

            

                var medicoId = $('#filtro-medico-id').length > 0 ? $('#filtro-medico-id').val() : null;
                var medicoEspecialidadeId = $('#filtro-medico-especialidade-id').length > 0 ? $('#filtro-medico-especialidade-id').val() : null;
                $.ajax({
                    url: '/Mpa/AgendamentoConsultas/EventosPorMedico/',
                    dataType: 'json',
                    data: {
                        // our hypothetical feed requires UNIX timestamps
                        medicoId: medicoId,
                        medicoEspecialidadeId: medicoEspecialidadeId,
                        start: start.format('YYYY-MM-DD'),
                        end: end.format('YYYY-MM-DD')
                    },
                    success: function (doc) {
                        var events = [];
                        if (doc.result) {
                            $.map(doc.result, function (r) {
                                events.push({
                                    id: r.id,
                                    title: r.title,
                                    start: r.start,
                                    end: r.end,
                                    color: r.color
                                });
                            });
                        }
                        callback(events);
                    }
                });
            },
            dayClick: function (date) { //, novoEvento, view) {
                //$('#form-data-agendamento').val(date.format('L'));
                //$('#form-data-agendamento').data('daterangepicker').setStartDate(moment(date).format('L'));
                //$('#form-data-agendamento').data('daterangepicker').setEndDate(moment(date).format('L'));
                //updateCriarOuEditarAgendamentoConsultaViewModel();
                //if (date.format('HH:mm') !== '00:00') {
                //    var hora = date.format('HH:mm');
                //    $('#hora-agendamento')
                //    .val(
                //        $('#hora-agendamento option')
                //            .filter(function () {
                //                return $(this).html() === hora;
                //            })
                //            .val()
                //        );
                //    $('.chosen-select').trigger("chosen:updated");

                //}
                criarAgendamento(date);
            },
            eventRender: function (event, element) {

            

                element.bind('mousedown', function (e) {
                    e.preventDefault();
                    if (e.which === 3) {
                        abp.message.confirm(
                            app.localize('DeleteWarning', 'O agendamento de ' + event.title + ' de ' + event.start + ' até ' + event.end),
                            function (isConfirmed) {
                                if (isConfirmed) {
                                    _agendamentoConsultasService.excluir(event.id)
                                        .done(function () {
                                            //getAgendamentoConsulta(true);
                                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                                            $('#calendar').fullCalendar('refetchEvents');
                                        });
                                }
                            }
                        );
                    }
                });
            },
            eventClick: function (event) {
                editarAgendamento(event.id);
            },

            eventDrop: function (event, jsEvent, ui, view) {
               

                _agendamentoConsultasService.alterarAgendamento(event.id, jsEvent._data.days, jsEvent._data.hours, jsEvent._data.minutes);

              
               
            },



            loading: function (bool) {
                if (bool)
                    $('#loading-calendar').removeClass('hidden').css('display', 'block');
                else
                    $('#loading-calendar').addClass('hidden');
            },
        });

        var _createModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AgendamentoConsultas/_CriarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarAgendamentoConsultaModal'
        });
        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AgendamentoConsultas/_EditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarAgendamentoConsultaModal'
        });


        //$('#filtro-medico-id').on('change', function (e) {
        //    e.preventDefault();
        //    var id = $(this).val();
        //    //$("#form-medico-id").val(id);
        //    $.ajax({
        //        url: "/Mpa/AgendamentoConsultas/MedicoEspecialidades/" + id,
        //        success: function (data) {
        //            $("#filtro-medico-especialidade-id").empty().append('<option value="">' + app.localize('FiltroEspecialidade') + '</option>');
        //            $.each(data.result, function (index, element) {
        //                $("#filtro-medico-especialidade-id").append('<option value="' + element.id + '">' + element.nome + '</option>');
        //                //$("#form-medico-especialidade-id").append('<option value="' + element.id + '">' + element.nome + '</option>');
        //            });
        //            $(".chosen-select").trigger("chosen:updated");
        //            $('#calendar').fullCalendar('refetchEvents');
        //        }
        //    });
        //});

        $('#filtro-medico-especialidade-id').on('change', function (e) {
            e.preventDefault();
            lerLegendas();
            $('#calendar').fullCalendar('refetchEvents');
        });

        function editarAgendamento(id) {
            _editModal.open({ id });
        }

        function criarAgendamento(date) {
           
            _createModal.open({ date: moment(date).format('YYYY-MM-DD HH:mm'), medicoId: $('#ultimo-medico').val() });
        }

        $('#CreateNewAgendamentoConsulta').on('click', function (e) {
            e.preventDefault();
            criarAgendamento(moment().format('YYYY-MM-DD'));
        });

        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

    });
})();