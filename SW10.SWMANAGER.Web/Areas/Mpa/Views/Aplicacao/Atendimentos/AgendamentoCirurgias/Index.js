(function () {
    $(function () {
        var _agendamentoConsultasService = abp.services.app.agendamentoConsulta;


        var myLocale = moment.locale();

        function lerLegendas() {

            $('#legendas-agendamento').load('/mpa/AgendamentoCirurgias/_ListarSalasCirurgicas',
            {
                salaCirurgicaId: $('#filtro-medico-id').length > 0 ? $('#filtro-medico-id').val() : null,
                tipoCirurgiaId: $('#tipoCirurgiaId').length > 0 ? $('#tipoCirurgiaId').val() : null,
                empresaId: $('#empresaId').length > 0 ? $('#empresaId').val() : null
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
            slotDuration: '00:30:00',
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
                var medicoEspecialidadeId = $('#tipoCirurgiaId').length > 0 ? $('#tipoCirurgiaId').val() : null;
                var empresaId = $('#empresaId').length > 0 ? $('#empresaId').val() : null;
                $.ajax({
                    url: '/Mpa/AgendamentoCirurgias/EventosPorSala/',
                    dataType: 'json',
                    data: {
                        // our hypothetical feed requires UNIX timestamps
                        medicoId: medicoId,
                        tipoCirurgiaId: medicoEspecialidadeId,
                        start: start.format('YYYY-MM-DD'),
                        end: end.format('YYYY-MM-DD'),
                        empresaId:empresaId
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
            viewUrl: abp.appPath + 'Mpa/AgendamentoCirurgias/_CriarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarAgendamentoCirurgiasModal'
        });
        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AgendamentoCirurgias/_EditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarAgendamentoCirurgiasModal'
        });



        $('#tipoCirurgiaId').on('change', function (e) {
            e.preventDefault();
            lerLegendas();
            $('#calendar').fullCalendar('refetchEvents');
        });

        $('#filtro-medico-id').on('change', function (e) {
            e.preventDefault();
            lerLegendas();
            $('#calendar').fullCalendar('refetchEvents');
        });

        function editarAgendamento(id) {
            _editModal.open({ id });
        }

        function criarAgendamento(date) {
            
            _createModal.open({ date: moment(date).format('YYYY-MM-DD HH:mm'), salaCirurgicaId: $('#ultimo-medico').val(), tipoCirurgiaId: $('#tipoCirurgiaId').val() });
        }

        $('#CreateNewAgendamentoConsulta').on('click', function (e) {
            e.preventDefault();
            criarAgendamento(moment().format('YYYY-MM-DD'));
        });

        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

        selectSW('.selectTipoCirurgia2', "/api/services/app/TipoCirurgia/ListarDropdown");
        selectSW('.selectEmpresa', "/api/services/app/Empresa/ListarDropdownPorUsuario");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");
        selectSW('.selectPaciente', "/api/services/app/paciente/ListarIncluindoCPFDropdown");

        $('#imprimirAgendamento').on('click', function (e) {
            e.preventDefault();
           

            window.open("/Mpa/AgendamentoCirurgias/IndexRelatorio");


        });


        $('#listarAgendamento').on('click', function (e) {
            e.preventDefault();

            window.open("/Mpa/ListagemAgendamentoCirurgias");
        });




    });
})();