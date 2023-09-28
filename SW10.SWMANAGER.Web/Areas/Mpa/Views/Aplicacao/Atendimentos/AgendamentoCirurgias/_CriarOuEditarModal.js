(function ($) {
    app.modals.CriarOuEditarAgendamentoCirurgiasModal = function () {

        var _agendamentoConsultasService = abp.services.app.agendamentoConsulta;
        var _agendamentoConsultasDisponibilidadeService = abp.services.app.agendamentoConsultaMedicoDisponibilidade;
        var _medicoEspecialidadeAppService = abp.services.app.medicoEspecialidade;
        var _planoAppService = abp.services.app.plano;
        var _pacienteAppService = abp.services.app.paciente;




        var _agendamentoSalaCirurgicaDisponibilidade = abp.services.app.agendamentoSalaCirurgicaDisponibilidade;
        var _agendamentoSalaCirurgicaService = abp.services.app.agendamentoSalaCirurgica;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        var _modalManager;
        var _$agendamentoConsultaInformationsForm = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;


            $('.modal-dialog').css({ 'width': '90%', 'max-width': '700px' });

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



            if ($('#sala-id').val() > 0) {
                $('#sala-id').trigger('change');
            }


            if ($('#tipoCirurgia-id').val() > 0) {
                $('#tipoCirurgia-id').trigger('change');
            }

            $('#sala-id').focus();

        };

       
        $(".cpf").mask('000.000.000-00', { reverse: true });

        $(document).ready(function () {

           // $('#valorNota').mask('000.000.000,00', { reverse: true });
        });


        this.save = function () {
            if (!_$agendamentoConsultaInformationsForm.valid()) {
                return;
            }

            var agendamentoConsulta = _$agendamentoConsultaInformationsForm.serializeFormToObject();
            _modalManager.setBusy(true);

            agendamentoConsulta.Cpf = RetirarMascaraPadrao(agendamentoConsulta.Cpf);

           
            _agendamentoSalaCirurgicaService.criarOuEditar(agendamentoConsulta)
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


        $('#cpf').on('blur', function (e) {
            e.preventDefault();

            debugger;

            _pacienteAppService.obterPorCpf(RetirarMascaraPadrao($('#cpf').val()))
                                            .done(function (data) {

                                                if (data != null) {
                                                    $('#comboPaciente').append($("<option>")
                                                                                      .val(data.id) //set value for option to post it
                                                                                      .text(data.nomeCompleto)) //set a text for show in select
                                                                      .val(data.id)
                                                                      .trigger("change")//select option of select2


                                                    $('#opt-paciente-cadastrado').attr("checked", true);
                                                    $('#nome-reservante').val('');
                                                    $('#telefone-reservante').val('');
                                                    $('#data-nascimento-reservante').val('');
                                                    $('#paciente-cadastrado').removeClass('hidden').css('display', 'block');
                                                    $('#paciente-nao-cadastrado').addClass('hidden');

                                                }
                                                else {
                                                    $('#comboPaciente').val(null).trigger('change');
                                                }

                                            });

        });



        $('#opt-paciente-cadastrado').on('click', function (e) {
            if ($(this).is(':checked')) {
                //$('#nome-reservante').val('');
                //$('#telefone-reservante').val('');
                //$('#data-nascimento-reservante').val('');
                $('#paciente-cadastrado').removeClass('hidden').css('display', 'block');
                $('#paciente-nao-cadastrado').addClass('hidden');
            }
            else {
                $('#paciente-cadastrado').addClass('hidden');
               // $('#form-pacienteId').val('');
                $('#paciente-nao-cadastrado').removeClass('hidden').css('display', 'block');
               // $('#comboPaciente').val(null).trigger('change');

            }
        });

        $('input[name="DataAgendamento"]')
            .on('input', function () {
                if ($(this).val().length === 10) {
                    var date = $(this).val();
                    if (IsValid(date)) {

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
                    updateCriarOuEditarAgendamentoConsultaViewModel();
                });



        $('input[name="DataNascimentoReservante"]')
          .on('input', function () {
              if ($(this).val().length === 10) {
                  var date = $(this).val();
                  if (IsValid(date)) {

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
              //minDate: moment(),
              //maxDate: moment().add('year', 1),
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

                  $('input[name="DataNascimentoReservante"]').val(moment(selDate).format('L')).addClass('form-control');
                  // $('#calendar').fullCalendar('gotoDate', selDate);
              });

       // aplicarSelect2Padrao();


        $('#tipoCirurgia-id').select2({

        })
            .on('change', function (e) {
                e.preventDefault();

                debugger;

                var tipoCirurgiaId = $(this).val();
                var id = $('#id').length > 0 ? $('#id').val() : 0;
                var myDate = $('#data-agendamento').val();
                var aDate = myDate.split('/');
                var salaId = $('#sala-id').val();
                $('#div-horarios').load('/mpa/AgendamentoCirurgias/_MontarComboHorarios',
                    {
                        tipoCirurgiaId: tipoCirurgiaId,
                        salaId: salaId,
                        date: aDate[2] + '-' + aDate[1] + '-' + aDate[0],
                        id: id,
                        dataHora: $('#hora-agendamento').val()
                    }, function () {

                        $('#agendamento-consulta-medico-disponibilidade-id').on('change', function (e) {
                            var hora = $('#agendamento-consulta-medico-disponibilidade-id option:selected').text();
                            var data = $('#data-agendamento').val();
                            $('#hora-agendamento').val(data + ' ' + hora);

                            carregarDivQuantidadeHorarios();
                            $('#btnSalvarAgendamento').prop("disabled", false)

                        }).trigger('change');

                    }
                );



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


               

                selectSW('.selectPlano', "/api/services/app/Plano/ListarPorConvenioExclusivoDropdown", $('#convenio-id'));



                if ($('#id').val() == 0 || $('#id').val() == null) {

                    //   if ($('#plano-id').val() == 0 || $('#plano-id').val() == null || $('#plano-id').val() == '') {

                    _planoAppService.obterSomenteUmPlano($('#convenio-id').val())
                   .done(function (data) {

                       if (data != null) {

                           $('#plano-id').append($("<option>")
                                                    .val(data.id) //set value for option to post it
                                                    .text(data.nome)) //set a text for show in select
                                                .val(data.id)
                                                .trigger("change")//select option of select2

                       }
                       else {
                           $('#plano-id').val(null).trigger('change');
                       }
                   });
                    // }
                }






                if ($(this).val() > 0) {
                    $('#plano-id').removeAttr('disabled');
                }
                else {
                    $('#plano-id').attr('disabled', 'disabled');
                }





            });


        $('#hora-agendamento').on('change', function () {
            carregarDivQuantidadeHorarios();
        });

        function carregarDivQuantidadeHorarios() {



            var tipoCirurgiaId = $('#tipoCirurgia-id').val();
            var id = $('#id').length > 0 ? $('#id').val() : 0;
            var myDate = $('#data-agendamento').val();
            var aDate = myDate.split('/');
            var salaId = $('#sala-id').val();


            $('#divQuantidadehorarios').load('/mpa/AgendamentoCirurgias/_MontarComboQuantidadeHorarios',
                   {
                       tipoCirurgiaId: tipoCirurgiaId,
                       salaId: salaId,
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


      

        $('#comboPaciente').on('change', function () {

            if ($('#comboPaciente').val() != null) {

                _pacienteAppService.obter($('#comboPaciente').val())
                                             .done(function (data) {

                                                 if (data != null) {

                                                     $('#cpfCadastrado').val(data.cpf);
                                                     $('#telefoneCadastrado').val(data.telefone1);
                                                     $('#data-nascimentoCadastrado').val(moment(data.nascimento).format('L'));
                                                     if (!_.isUndefined(data.sexo) && data.sexo != null) {
                                                         $('#sexoCadastrado').val(data.sexo.descricao);
                                                     }

                                                     if (data.sexoId == 1) {
                                                         $('#sexoCadastrado').val("Masculino");
                                                     }
                                                     else if (data.SexoId == 2) {
                                                         $('#sexoCadastrado').val("Feminino");
                                                     }
                                                     else if (data.SexoId == 3) {
                                                         $('#sexoCadastrado').val("Ambos");
                                                     }
                                                 }

                                             });
            }

        });


        $('#sala-id').on('change', function (e) {

            e.preventDefault();

            debugger;

            $('#btnSalvarAgendamento').prop("disabled", true)

            selectSW('.selectTipoCirurgia', "/api/services/app/AgendamentoSalaCirurgicaDisponibilidade/ListarTiposCirurugiasDisponiveisDropdown", $('#sala-id'));


            // if ($('#id').val() == 0 || $('#id').val() == null) {

            //   if ($('#tipoCirurgia-id').val() == 0 || $('#tipoCirurgia-id').val() == null || $('#tipoCirurgia-id').val() == '') {
            if ($('#sala-id').val() != null) {

                _agendamentoSalaCirurgicaDisponibilidade.obterSomenteUmTipoCirurgia($('#sala-id').val())
               .done(function (data) {

                   if (data != null) {

                       $('#tipoCirurgia-id').append($("<option>")
                                                .val(data.id) //set value for option to post it
                                                .text(data.nome)) //set a text for show in select
                                            .val(data.id)
                                            .trigger("change")//select option of select2

                   }
                   else {
                       //$('#tipoCirurgia-id').val(null).trigger('change');
                   }


               });
            }
            
            //  }
            //  }



        });




        var _$materiaisOPMETable = $('#materiaisOPMETable');

        _$materiaisOPMETable.jtable({
            title: app.localize('Materiais'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,

            fields: {
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '15%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');


                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                       .appendTo($span)
                       .click(function (e) {
                           e.preventDefault();
                           editMaterialOPME(data.record);
                       });


                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                             .appendTo($span)
                             .click(function (e) {
                                 e.preventDefault();
                                 deleteMaterialOPME(data.record);
                             });


                        return $span;
                    }
                },

                Material: {
                    title: app.localize('Material'),
                    width: '85%',
                    display: function (data) {
                        return data.record.Material;
                    }
                },

            }
        });



        var listaMateriaisOPME = [];

        $('#inserirMateriaisOPME').click(function (e) {
            e.preventDefault();

            var matgeriaisOPME = {};
           

            if ($('#materiaisOPMEJson').val() != '') {
                listaMateriaisOPME = JSON.parse($('#materiaisOPMEJson').val());
            }

            if ($('#idGridMatgeriaisOPME').val() != '') {


                for (var i = 0; i < listaMateriaisOPME.length; i++) {
                    if (listaMateriaisOPME[i].IdGrid == $('#idGridMatgeriaisOPME').val()) {

                        listaMateriaisOPME[i].Material = $('#material').val();

                        listaMateriaisOPME[i].QuantidadeMaterial = $('#quantidadeMaterial').val();
                        listaMateriaisOPME[i].DataPrevista = $('#dataPrevista').val();
                        listaMateriaisOPME[i].DataRecebimento = $('#dataRecebimento').val();
                        listaMateriaisOPME[i].NumeroNota = $('#numeroNota').val();
                        var valorNota = $('#valorNota').val();

                       
                        listaMateriaisOPME[i].ValorNota = retirarMascara(valorNota);

                        listaMateriaisOPME[i].IsCobraPeloHospital = $('#isCobraPeloHospital')[0].checked;
                        listaMateriaisOPME[i].FornecedorId = $('#fornecedorId').val();



                        var fornecedor = $('#fornecedorId').select2('data');
                        if (fornecedor && fornecedor.length > 0) {

                            listaMateriaisOPME[i].FornecedorDescricao = fornecedor[0].text;
                        }


                        _$materiaisOPMETable.jtable('updateRecord', {
                            record: listaMateriaisOPME[i]
                          , clientOnly: true
                        });

                    }
                }
            }
            else {
                matgeriaisOPME.IdGrid = listaMateriaisOPME.length == 0 ? 1 : listaMateriaisOPME[listaMateriaisOPME.length - 1].IdGrid + 1;


                matgeriaisOPME.Material = $('#material').val();
                matgeriaisOPME.QuantidadeMaterial = $('#quantidadeMaterial').val();
                matgeriaisOPME.DataPrevista = $('#dataPrevista').val();
                matgeriaisOPME.DataRecebimento = $('#dataRecebimento').val();
                matgeriaisOPME.NumeroNota = $('#numeroNota').val();
                var valorNota = $('#valorNota').val();

               
                matgeriaisOPME.ValorNota = retirarMascara(valorNota);

                matgeriaisOPME.IsCobraPeloHospital = $('#isCobraPeloHospital')[0].checked;
                matgeriaisOPME.FornecedorId = $('#fornecedorId').val();

                var fornecedor = $('#fornecedorId').select2('data');
                if (fornecedor && fornecedor.length > 0) {

                    matgeriaisOPME.FornecedorDescricao = fornecedor[0].text;
                }


                listaMateriaisOPME.push(matgeriaisOPME);

                _$materiaisOPMETable.jtable('addRecord', {
                    record: matgeriaisOPME
                  , clientOnly: true
                });

            }

            $('#materiaisOPMEJson').val(JSON.stringify(listaMateriaisOPME))



            $('#idGridMatgeriaisOPME').val('');
            $('#material').val('');
            $('#quantidadeMaterial').val('');
            $('#dataPrevista').val('');
            $('#dataRecebimento').val('');
            $('#numeroNota').val('');
            $('#valorNota').val('');
            $('#isCobraPeloHospital').attr("checked", false);
            $('#fornecedorId').val(null).trigger('change');


            $('#inserirMateriaisOPME > i').removeClass('fa-check');
            // $('#inserir > i').addClass('glyphicon');
            $('#inserirMateriaisOPME > i').addClass('fa-plus');



        });

        function getMateriaisOPME() {

            listaMateriaisOPME = JSON.parse($('#materiaisOPMEJson').val());

            var allRows = _$materiaisOPMETable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');
                _$materiaisOPMETable.jtable('deleteRecord', { key: id, clientOnly: true });
            });

            for (var i = 0; i < listaMateriaisOPME.length; i++) {
                var item = listaMateriaisOPME[i];

                item.DataPrevista = moment(item.DataPrevista).format('L');
                item.DataRecebimento = moment(item.DataRecebimento).format('L');

                _$materiaisOPMETable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        function deleteMaterialOPME(item) {
            abp.message.confirm(
                app.localize('DeleteWarning', item.Material),
                function (isConfirmed) {
                    if (isConfirmed) {

                        listaMateriaisOPME = JSON.parse($('#materiaisOPMEJson').val());

                        for (var i = 0; i < listaMateriaisOPME.length; i++) {
                            if (listaMateriaisOPME[i].IdGrid == item.IdGrid) {
                                listaMateriaisOPME.splice(i, 1);
                                $('#materiaisOPMEJson').val(JSON.stringify(listaMateriaisOPME));

                                _$materiaisOPMETable.jtable('deleteRecord', {
                                    key: item.IdGrid
                                , clientOnly: true
                                });

                                break;
                            }
                        }

                    }
                }
            );
        }

        function editMaterialOPME(material) {

           

            $('#idGridMatgeriaisOPME').val(material.IdGrid);

            $('#material').val(material.Material);

            $('#quantidadeMaterial').val(material.QuantidadeMaterial);
            $('#dataPrevista').val(material.DataPrevista);
            $('#dataRecebimento').val(material.DataRecebimento);
            $('#numeroNota').val(material.NumeroNota);
            $('#valorNota').val(formatarValor(material.ValorNota));
            $('#isCobraPeloHospital').attr("checked", material.IsCobraPeloHospital);
            $('#fornecedorId').val(material.FornecedorId);



            $('#fornecedorId')
               .append($("<option>") //add option tag in select
             .val(material.FornecedorId) //set value for option to post it
             .text(material.FornecedorDescricao)
           ) //set a text for show in select
     .val(material.FornecedorId) //select option of select2
           .trigger("change");




            $('#inserirMateriaisOPME > i').removeClass('fa-plus');
            // $('#inserir > i').addClass('glyphicon');
            $('#inserirMateriaisOPME > i').addClass('fa-check');


            //calcularTotal();
        }

        getMateriaisOPME();


        var _$itemTable = $('#itemTable');


        _$itemTable.jtable({
            title: app.localize('Materiais'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,

            fields: {
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');


                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                       .appendTo($span)
                       .click(function (e) {
                           e.preventDefault();
                           editMaterial(data.record);
                       });


                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                             .appendTo($span)
                             .click(function (e) {
                                 e.preventDefault();
                                 deleteMaterial(data.record);
                             });


                        return $span;
                    }
                },

                Descricao: {
                    title: app.localize('Material'),
                    width: '60%',
                    display: function (data) {
                        return data.record.Descricao;
                    }
                },


                Quantidade: {
                    title: app.localize('Quantidade'),
                    width: '60%',
                    display: function (data) {
                        return data.record.QuantidadeMaterial;
                    }
                },


            }
        });


        var listaMateriais = [];

        $('#inserirMaterial').click(function (e) {
            e.preventDefault();


            function existe(registro) {
               
                return registro.FaturamentoItemId == $('#itemId').val();
            }



            if ($('#itemId').val() != '' && $('#itemId').val() != null) {

                var matgeriais = {};
               

                if ($('#materiaisJson').val() != '') {


                }

                if ($('#idGridMatgeriais').val() != '') {


                    for (var i = 0; i < listaMateriais.length; i++) {
                        if (listaMateriais[i].IdGrid == $('#idGridMatgeriais').val()) {

                            listaMateriais[i].QuantidadeMaterial = $('#quantidadeItem').val();
                            listaMateriais[i].FaturamentoItemId = $('#itemId').val();

                            var material = $('#itemId').select2('data');
                            if (material && material.length > 0) {
                                listaMateriais[i].Descricao = material[0].text;
                            }

                            _$itemTable.jtable('updateRecord', {
                                record: listaMateriais[i]
                              , clientOnly: true
                            });

                        }
                    }
                }
                else {

                    var resultado = listaMateriais.filter(existe);


                    if (resultado == null || resultado.length == 0) {


                        matgeriais.IdGrid = listaMateriais.length == 0 ? 1 : listaMateriais[listaMateriais.length - 1].IdGrid + 1;


                        matgeriais.QuantidadeMaterial = $('#quantidadeItem').val();
                        matgeriais.FaturamentoItemId = $('#itemId').val();

                        var material = $('#itemId').select2('data');
                        if (material && material.length > 0) {
                            matgeriais.Descricao = material[0].text;
                        }

                        listaMateriais.push(matgeriais);

                        _$itemTable.jtable('addRecord', {
                            record: matgeriais
                          , clientOnly: true
                        });
                    } else {
                        alert('Item já relacionado.');
                        $('#itemId').val(null).trigger("change");
                    }


                }

                $('#materiaisJson').val(JSON.stringify(listaMateriais))

                $('#idGridMatgeriais').val('');
                $('#quantidadeItem').val('1');
                //$('#quantidadeMaterial').val('');
                $('#itemId').val(null).trigger('change');

                $('#inserirMaterial > i').removeClass('fa-check');
                // $('#inserir > i').addClass('glyphicon');
                $('#inserirMaterial > i').addClass('fa-plus');
            }

        });


        function getMateriais() {

            listaMateriais = JSON.parse($('#materiaisJson').val());

            var allRows = _$itemTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');
                _$itemTable.jtable('deleteRecord', { key: id, clientOnly: true });
            });

            for (var i = 0; i < listaMateriais.length; i++) {
                var item = listaMateriais[i];

                _$itemTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getMateriais();

        function editMaterial(material) {

           

            $('#idGridMatgeriais').val(material.IdGrid);


            $('#quantidadeItem').val(material.QuantidadeMaterial);
            $('#itemId').val(material.FaturamentoItemId);



            $('#itemId')
               .append($("<option>") //add option tag in select
             .val(material.FaturamentoItemId) //set value for option to post it
             .text(material.Descricao)
           ) //set a text for show in select
     .val(material.FaturamentoItemId) //select option of select2
           .trigger("change");




            $('#inserirMateriais > i').removeClass('fa-plus');
            $('#inserirMateriais > i').addClass('fa-check');


        }


        function deleteMaterial(item) {

           

            abp.message.confirm(
                app.localize('DeleteWarning', item.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {



                        listaMateriais = JSON.parse($('#materiaisJson').val());

                        for (var i = 0; i < listaMateriais.length; i++) {
                            if (listaMateriais[i].IdGrid == item.IdGrid) {
                                listaMateriais.splice(i, 1);
                                $('#materiaisJson').val(JSON.stringify(listaMateriais));

                                _$itemTable.jtable('deleteRecord', {
                                    key: item.IdGrid
                                , clientOnly: true
                                });

                                break;
                            }
                        }

                    }
                }
            );
        }


        selectSW('.selectSalaCirurgica', "/api/services/app/SalaCirurgica/ListarDropdown");
        selectSW('.selectTipoCirurgia', "/api/services/app/AgendamentoSalaCirurgicaDisponibilidade/ListarTiposCirurugiasDisponiveisDropdown", $('#sala-id'));
        selectSW('.selectCirurugia', "/api/services/app/FaturamentoItem/ListarCirurgiaAgendamentoDropdown");
        selectSW('.selectMaterial', "/api/services/app/FaturamentoItem/ListarMateriaisOPMEDropdown");
        selectSW('.selectMedico', "/api/services/app/Medico/ListarDropdown");
        selectSW('.selectPlano', "/api/services/app/Plano/ListarPorConvenioExclusivoDropdown", $('#convenio-id'));
        selectSW('.selectEspecialidade', "/api/services/app/MedicoEspecialidade/ListarDropdownPorMedico", $('#medico-id'));
        selectSW('.selectFornecedor', "/api/services/app/Fornecedor/ListarDropdown");
        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selectMatrial', "/api/services/app/FaturamentoItem/ListarMaterialAgendamentoDropdown");

        selectSW('.selectPaciente', "/api/services/app/Paciente/ListarIncluindoCPFDropdown");
        selectSW('.selectStatus', "/api/services/app/AgendamentoStatus/ListarDropdown");


        var _$cirurgiasTable = $('#cirurgiasTable');

        _$cirurgiasTable.jtable({
            title: app.localize('Cirurgias'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,

            fields: {
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                             .appendTo($span)
                             .click(function (e) {
                                 e.preventDefault();
                                 deleteRegistro(data.record);
                             });


                        return $span;
                    }
                },

                Descricao: {
                    title: app.localize('Descricao'),
                    width: '90%',
                    display: function (data) {
                        return data.record.Descricao;
                    }
                },

            }
        });


        var listaCirurgias = [];

        function getRegistros() {




            listaCirurgias = JSON.parse($('#cirurgiasJson').val());

            var allRows = _$cirurgiasTable.find('.jtable-data-row')

            $.each(allRows, function () {
                var id = $(this).attr('data-record-key');
                _$cirurgiasTable.jtable('deleteRecord', { key: id, clientOnly: true });
            });

            for (var i = 0; i < listaCirurgias.length; i++) {
                var item = listaCirurgias[i];


                _$cirurgiasTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getRegistros();

        $('#inserirCirurgia').click(function (e) {
            e.preventDefault();

            function existe(registro) {
               
                return registro.RelacionadoId == $('#cirurgia-id').val();
            }



            if ($('#cirurgia-id').val() != '' && $('#cirurgia-id').val() != null) {

                var cirurgia = {};

                if ($('#cirurgiasJson').val() != '') {
                    listaCirurgias = JSON.parse($('#cirurgiasJson').val());
                }


                if ($('#idGridCirurgia').val() != '') {
                }
                else {

                   
                    var resultado = listaCirurgias.filter(existe);


                    if (resultado == null || resultado.length == 0) {

                        cirurgia.IdGrid = listaCirurgias.length == 0 ? 1 : listaCirurgias[listaCirurgias.length - 1].IdGrid + 1;

                        cirurgia.RelacionadoId = $('#cirurgia-id').val();

                        var campo = $('#cirurgia-id').select2('data');
                        if (campo && campo.length > 0) {

                            cirurgia.Descricao = campo[0].text;
                        }


                        listaCirurgias.push(cirurgia);

                        _$cirurgiasTable.jtable('addRecord', {
                            record: cirurgia
                          , clientOnly: true
                        });

                        $('#cirurgia-id').val(null).trigger("change");
                    }
                    else {
                        alert('Cirurgia já relacionada.');
                        $('#cirurgia-id').val(null).trigger("change");
                    }
                }

                $('#cirurgiasJson').val(JSON.stringify(listaCirurgias))
            }
        });

        $('#recalcularTempo').click(function (e) {
            e.preventDefault();

           

            var cirurgia = {};

            if ($('#cirurgiasJson').val() != '') {
                listaCirurgias = JSON.parse($('#cirurgiasJson').val());
            }



            _agendamentoSalaCirurgicaService.recalcularQuantidadeHorarios($('#cirurgiasJson').val()
                                                                        , $('#id').val(), $('#agendamento-consulta-medico-disponibilidade-id').val()
                                                                        , $('#data-agendamento').val()
                                                                        , $('#hora-agendamento').val()
                                                                        , $('#sala-id').val())
              .done(function (data) {
                  if (data.warnings.length > 0) {
                      _ErrorModal.open({ erros: data.warnings });
                  }



                  if (data.returnObject != null) {
                      $('#quantidadeHorarios').val(data.returnObject.quantidadeHorarios);
                      $('#agendamento-consulta-medico-quantidadeHorarios-id').val(data.returnObject.quantidadeHorarios);

                  }

              })
             .always(function () {
                 _modalManager.setBusy(false);
             });



        });

        function deleteRegistro(item) {
            abp.message.confirm(
                app.localize('DeleteWarning', item.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {



                        listaCirurgias = JSON.parse($('#cirurgiasJson').val());

                        for (var i = 0; i < listaCirurgias.length; i++) {
                            if (listaCirurgias[i].IdGrid == item.IdGrid) {
                                listaCirurgias.splice(i, 1);
                                $('#cirurgiasJson').val(JSON.stringify(listaCirurgias));

                                _$cirurgiasTable.jtable('deleteRecord', {
                                    key: item.IdGrid
                                , clientOnly: true
                                });

                                break;
                            }
                        }

                    }
                }
            );
        }

        $('#confirmarAtendimento').click(function (e) {
            e.preventDefault();


            var id = $('#id').val();

            _agendamentoConsultasService.confirmarAtendimento(id)
             .done(function (data) {
                 if (data == null) {
                     window.open('Internacoes?id=' + id, '_blank');
                 }

             })
            .always(function () {
            });


        });

        $('#medico-id')
            .on('change', function () {

                selectSW('.selectEspecialidade', "/api/services/app/MedicoEspecialidade/ListarDropdownPorMedico", $('#medico-id'));

               


                if ($('#medico-id').val() != '' && $('#medico-id').val() != null) {
                    abp.services.app.medico.obter($('#medico-id').val())
                     .done(function (medico) {
                         if (medico != null) {
                             $('#observacao').val(medico.sisPessoa.observacao);
                         }
                         else {
                             $('#observacao').val('');
                         }
                     })


                    if ($('#id').val() == 0 || $('#id').val() == null) {

                        _medicoEspecialidadeAppService.obterSomenteUmaEspecialidade($('#medico-id').val())
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
                }
                else {
                    $('#observacao').val('');
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
        });


        $('input[name="DataPrevista"]')
            .on('input', function () {
                if ($(this).val().length === 10) {
                    var date = $(this).val();
                    if (IsValid(date)) {

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
                //minDate: moment(),
                //maxDate: moment().add('year', 1),
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

                    $('input[name="DataPrevista"]').val(moment(selDate).format('L')).addClass('form-control');
                    $('#calendar').fullCalendar('gotoDate', selDate);
                });


        $('input[name="DataRecebimento"]')
                  .on('input', function () {
                      if ($(this).val().length === 10) {
                          var date = $(this).val();
                          if (IsValid(date)) {

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
                      //minDate: moment(),
                      //maxDate: moment().add('year', 1),
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

                          $('input[name="DataRecebimento"]').val(moment(selDate).format('L')).addClass('form-control');
                          $('#calendar').fullCalendar('gotoDate', selDate);
                      });




        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Pacientes/CriarOuEditarModalAtendimento',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPacienteModal'
        });

        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Pacientes/EditarModalAtendimento',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarPacienteModal'
        });


        //Novo Botão de editar na tela de atendimento
        $('#editPacienteButton').click(function (e) {
            e.preventDefault();

            debugger;

            var id = $('#comboPaciente').val();
            if (id != "") {
                _editModal.open({ id: id });
            }

        });


        $('#pacienteButton').click(function (e) {
            e.preventDefault();
            // _createOrEditModal.open({ id: null, nomePaciente: $('#nome-reservante').val(), cpf: $('#cpf').val(), dataNascimento: $('#data-nascimento').val(), telefone: $('#telefone-reservante').val() });

            $('#pacienteButton').buttonBusy(true);


            if (($('#nome-reservante').val() == null || $('#nome-reservante').val() == '')
                || ($('#cpf').val() == null || $('#cpf').val() == '')
                || ($('#data-nascimento').val() == null || $('#data-nascimento').val() == '')
                || ($('#telefone-reservante').val() == null || $('#telefone-reservante').val() == '')
                 || ($('#cbo-sexoReservanteId').val() == '0' || $('#cbo-sexoReservanteId').val() == '')
                ) {

                alert('Existem campos do cadastro de paciente não preenchidos.');
                $('#pacienteButton').buttonBusy(false);
            }
            else {



                if ($('#cpf').val() != null && $('#cpf').val() != '') {
                    _pacienteAppService.obterPorCpf(RetirarMascaraPadrao($('#cpf').val()))
                                                    .done(function (data) {
                                                       
                                                        if (data) {
                                                            $('#comboPaciente').append($("<option>") //add option tag in select
                                                                               .val(data.id) //set value for option to post it
                                                                               .text(data.codigoPaciente + ' - ' +  data.nomeCompleto + ' - ' + data.cpf)
                                                                                       ) //set a text for show in select
                                                                               .val(data.id) //select option of select2
                                                                               .trigger("change");

                                                            $('#opt-paciente-cadastrado').attr("checked", true);
                                                            // $('#nome-reservante').val('');
                                                            //  $('#telefone-reservante').val('');
                                                            //  $('#data-nascimento-reservante').val('');
                                                            $('#paciente-cadastrado').removeClass('hidden').css('display', 'block');
                                                            $('#paciente-nao-cadastrado').addClass('hidden');

                                                            $('#cpfCadastrado').val(data.cpf)
                                                            $('#telefoneCadastrado').val(data.telefone1)
                                                            $('#data-nascimentoCadastrado').val(moment(data.ascimento).format('L'));
                                                            $('#sexoCadastrado').val(data.sexo.descricao)
                                                            $('#pacienteButton').buttonBusy(false);
                                                          

                                                            return;

                                                        }
                                                        else {

                                                            var paciente = { NomeCompleto: $('#nome-reservante').val(), Cpf: RetirarMascaraPadrao($('#cpf').val()), Nascimento: $('#data-nascimento').val(), Telefone1: $('#telefone-reservante').val(), SexoId: $('#cbo-sexoReservanteId').val() };

                                                            _pacienteAppService.criarOuEditar(paciente)
                                                                               .done(function (data) {

                                                                                   if (paciente) {
                                                                                       $('#comboPaciente').append($("<option>") //add option tag in select
                                                                                                          .val(data) //set value for option to post it
                                                                                                          .text($('#nome-reservante').val() + ' - ' + $('#data-nascimento').val())
                                                                                                                  ) //set a text for show in select
                                                                                                          .val(data) //select option of select2
                                                                                                          .trigger("change");

                                                                                       $('#opt-paciente-cadastrado').attr("checked", true);
                                                                                       // $('#nome-reservante').val('');
                                                                                       //  $('#telefone-reservante').val('');
                                                                                       //  $('#data-nascimento-reservante').val('');
                                                                                       $('#paciente-cadastrado').removeClass('hidden').css('display', 'block');
                                                                                       $('#paciente-nao-cadastrado').addClass('hidden');

                                                                                   }
                                                                                    
                                                                               });


                                                         }


                                                    }


                                       )
                                        .always(function () {
                                            $('#pacienteButton').buttonBusy(false);
                                        });;

                }

            }
        });


        abp.event.on('app.AtualizaModalAgendamento', function () {
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '700px' });
        });



        $('#imprimirOrcamento').on('click', function (e) {
            e.preventDefault();
           

            var convenioId = $('#convenio-id').val() != null ? $('#convenio-id').val() : "";
            var planoId = $('#plano-id').val() != null ? $('#plano-id').val() : "";
            var disponibilidadeId = $('#agendamento-consulta-medico-disponibilidade-id').val();
            //var listItemFaturamento = $('#cirurgiasJson').val();


            listaCirurgias = JSON.parse($('#cirurgiasJson').val())
            listaMateriais = JSON.parse($('#materiaisJson').val());


            //array.map: 
            var ids = listaCirurgias.map(function (v) { return v.RelacionadoId; });
            var strIds = JSON.stringify(ids)


            var idsMateriais = listaMateriais.map(function (v) { return { QuantidadeMaterial: v.QuantidadeMaterial, FaturamentoItemId: v.FaturamentoItemId }; });


            var strMateriaisIds = JSON.stringify(idsMateriais)

            //jQuery.map: var ids2 = $.map(this.fruits, function (v){ return v.Id; }); 




            //var paciente = '';

            //if ($('#comboPaciente').val() != null)
            //{
            //    paciente = $('#comboPaciente').val();
            //}
            //else
            //{
            //    paciente = $('#nome-reservante').val();
            //}


            var url = "/Mpa/AgendamentoCirurgias/IndexRelatorioOrcamento?convenioId=" + convenioId
                                                                        + "&planoId=" + planoId
                                                                        + "&disponibilidadeId=" + disponibilidadeId
                                                                        + "&listItemFaturamento=" + strIds
                                                                        + "&listItemMateriais=" + strMateriaisIds
                                                                        + "&pacienteId=" + ($('#comboPaciente').val() != null ? $('#comboPaciente').val() : "")
                                                                        + "&dataHoraAgendamento=" + $('#hora-agendamento').val()
                                                                        + "&pacienteReservante=" + $('#nome-reservante').val()
                                                                        + "&agendamentoId=" + $('#id').val()

            ;


            window.open(url);


        });





        var exibirDescontoAgendamento = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/AgendamentoCirurgias/ExibirDescontoAgendamento',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/Desconto.js',
             modalClass: 'DescontoModal'
        });

        


        $('#desconto').on('click', function (e) {
            e.preventDefault();

            exibirDescontoAgendamento.open({ id: $('#id').val() });
           
        });





    };
})(jQuery);