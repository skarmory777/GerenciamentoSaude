(function () {
    $(function () {
        var _agendamentoSalaCirurgicaService = abp.services.app.agendamentoSalaCirurgica;
     
       // $('.modal-dialog').css('width', '1800px');

        $('.modal-dialog').css({ 'width': '90%', 'max-width': '700px' });

        selectSW('.selectPaciente2', "/api/services/app/paciente/ListarIncluindoCPFDropdown");
        selectSW('.selectMedico2', "/api/services/app/medico/ListarDropdown");
        selectSW('.selectSala2', "/api/services/app/salaCirurgica/ListarDropdown");
        selectSW('.selectCirurugia2', "/api/services/app/FaturamentoItem/ListarCirurgiaAgendamentoDropdown");
        selectSW('.selectTipoCirurgia2', "/api/services/app/TipoCirurgia/ListarDropdown");
        

        $('#btnVisualizar').on('click', function (e) {
            e.preventDefault();

            getAgendamentos();
        });


       

        



        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AgendamentoCirurgias/_EditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarAgendamentoCirurgiasModal'
        });

        var _$agendamentoFilterForm = $('#agendamentoFilterForm');

        debugger;

        //var _selectedDateRange = {
        //    startDate: moment().startOf('day'),
        //    endDate: moment().endOf('day')
        //};
        
        //_$agendamentoFilterForm.find('input.date-range-picker').daterangepicker(
        //  $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
        //  function (start, end, label) {
        //      _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
        //      _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
        //  });



        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        $('#dateRangeAgendamento').daterangepicker(
      $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
      function (start, end, label) {
          _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
          _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
      });






        function createRequestParams() {
            var prms = {};
            _$agendamentoFilterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
        }


        var _$AgendamentosTable = $('#AgendamentosTable');
        _$AgendamentosTable.jtable({

            title: app.localize('Agendamentos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _agendamentoSalaCirurgicaService.obterListagem
                }
            },

            rowInserted: function (event, data) {
                if (data) {

                    if (data.record.statusId == 4)// cancelado
                    {
                        data.row[0].cells[1].setAttribute('bgcolor', '#db3636');
                       // data.row[0].cells[1].setAttribute('color', '#e1a8c4');
                    }
                }
                },


            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                       // if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                     _editModal.open({ id: data.record.id });
                                });
                      //  }

                       

                        return $span;
                    }
                },

                "AgendamentoConsulta.StatusId": {
                    sorting: true,
                    title: app.localize('Status'),
                    width: '5%',
                    display: (data) => data.record.status
                },

                "AgendamentoConsulta.Paciente.NomeCompleto": {
                    sorting: true,
                    title: app.localize('Paciente'),
                    width: '25%',
                    display: (data) => data.record.paciente
                },

                "AgendamentoConsulta.DataAgendamento": {
                    sorting: true,
                    title: app.localize('Data'),
                    width: '7%',
                        display: function (data) {
                            return moment(data.record.data).format('L');
                        }
                },

                "AgendamentoConsulta.HoraAgendamento": {
                    sorting: true,
                    title: app.localize('Hora'),
                    width: '5%',
                        display: function (data) {
                            return moment(data.record.hora).format('LT');
                        }
                },

                "AgendamentoConsulta.Medico.NomeCompleto": {
                    sorting: true,
                    title: app.localize('Medico'),
                    width: '25%',
                    display:(data)=> data.record.medico
                },
                "AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgia.Descricao": {
                    sorting: true,
                    title: app.localize('TipoCirurgia'),
                    width: '15%',
                    display: (data) => data.record.tipoCirurgia
                },
                "AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica.Descricao": {
                    sorting: true,
                    title: app.localize('sala'),
                    width: '20%',
                    display: (data) => data.record.sala
                },
            }
        });

        function getAgendamentos(reload) {
            if (reload) {
                _$ConveniosTable.jtable('reload');
            } else {
                var vData = $('#dateRangeAgendamento').val().split(' - ');
                var dataIni = vData[0];
                var dataFim = vData[1];


                _$AgendamentosTable.jtable('load', {
                    pacienteId: $('#pacienteId').val(),
                    medicoId: $('#medicoId').val(),
                    salaId: $('#salaId').val(),
                    procedimentoId: $('#procedimentoId').val(),
                    inicio: dataIni,//  $('#PeridoDe').val(),
                    fim: dataFim, //$('#PeridoAte').val()
                    tipoCirurgiaId: $('#tipoCirurgiaId').val(),
                    
                    
                });
            }
        }
        _$AgendamentosTable
        getAgendamentos();

        aplicarDateRange();
    });

})();