(function ($) {
    app.modals.HistoricoLeitoModalViewModel = function () {

        var _AtendimentosLeitosMovAppService = abp.services.app.atendimentoLeitoMov;
        var _Leitos = abp.services.app.leito;

        var _modalManager;
        var _$AtendimentoLeitoMovInformationForm = null;

        var _$AtendimentosLeitosMovTable = $('#AtendimentosLeitosMovTable');

        var movLeitos = [];

        var desocuparLeito;
        var dataRegistro;
        var dataInclusao;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TrasferirAtendimentosInformationsTab = _modalManager.getModal().find('form[name=TrasferirAtendimentosInformationsTab]');
            _$TrasferirAtendimentosInformationsTab.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '900px' });
            $('div.form-group select').addClass('form-control selectpicker');

            desocuparLeito = _$TrasferirAtendimentosInformationsTab[0].Id.value;
            dataInclusao = _$TrasferirAtendimentosInformationsTab[0].DataInclusao.value;
            dataRegistro = _$TrasferirAtendimentosInformationsTab[0].DataRegistro.value;
            AtendimentosInformations = _$TrasferirAtendimentosInformationsTab.serialize();

        };

        $('input[name="DataEntrada"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "timePicker": true,
            "timePicker24Hour": true,
            "startDate": moment(),
            "endDate": moment(),
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
                $('input[name="DataEntrada"]').val(selDate.format('L LT')).addClass('form-control edited');
            });

        $('input[name="DataSaida"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "timePicker": true,
            "timePicker24Hour": true,
            "startDate": moment(),
            "endDate": moment(),
            autoUpdateInput: false,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY H:mm:ss" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
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
                $('input[name="DataSaida"]').val(selDate.format('L LT')).addClass('form-control edited');
                //$('input[name="DataAlta"]').val(selDate.format('L')).addClass('form-control edited');
            });

        $('.chk').click(function () {
            var tipoVisita = $(this).attr("id");
            controleSelec(tipoVisita);
        });

        var controleSelec = function (tipoVisita) {

            if (tipoVisita == "chk-isFornecedor") {

                $('.divPaciente').hide("slow");

                $('.divUnidadeOrganizacional').show("slow");

                $('.divFornecedores').show("slow");
            }

            if (tipoVisita == "chk-isSetor") {

                $('.divPaciente').hide("slow");

                $('.divFornecedores').hide("slow");

                $('.divUnidadeOrganizacional').show("slow");
            }

            if (tipoVisita == "chk-isInternado" || tipoVisita == "chk-isEmergencia") {

                $('.divPaciente').show().slow;

                $('.divFornecedores').hide().slow;

                $('.divUnidadeOrganizacional').hide().slow;
            }

            if (!$('#chk-isFornecedor').is(':checked') && !$('#chk-isInternado').is(':checked') && !$('#chk-isEmergencia').is(':checked') && !$('#chk-isSetor').is(':checked')) {
                $('.divPaciente').hide("slow");
                $('.divUnidadeOrganizacional').hide("slow");
                $('.divFornecedores').hide("slow");
            }
        }

        //=====================================
        this.save = function () {
            //debugger
            // criar o objeto do tipo SolicitacaoExameItem
            if (movLeitos.length > 0) {
                _modalManager.setBusy(true);

                for (var i = 0; i < movLeitos.length; i++) {

                    // //console.log(i, " vezes");

                    movLeitos[i].isAmbulatorioEmergencia = 0;
                    movLeitos[i].isInternacao = 1;
                    movLeitos[i].isHomeCare = 0;
                    movLeitos[i].isPreatendimento = 0;
                    //movLeitos[i].dataRegistro =  moment().format('L');

                    var ArrayAtendiMoviLeito = new Object();
                    ArrayAtendiMoviLeito.id = Number($('#atendimentoId').val());
                    ArrayAtendiMoviLeito.leitoId = Number(movLeitos[i].id);
                    ArrayAtendiMoviLeito.unidadeOrganizacionalId = Number(movLeitos[i].unidadeOrganizacionalId);
                    ArrayAtendiMoviLeito.dataInicial = moment(new Date()).format("L LT");
                    ArrayAtendiMoviLeito.dataInclusao = dataInclusao;

                    ////console.log("ArrayAtendiMoviLeito: ", ArrayAtendiMoviLeito);

                    ////console.log("Ínicio");
                    var ocuparLeito;

                    //DESOCUPA LEITO
                    leitoInput = desocuparLeito;
                    ocuparLeito = false;
                    operarLeito(leitoInput, ocuparLeito)
                    ////console.log("desocuparLeito", leitoInput);

                    //EDITA MOV LEITO
                    editarMovLeito(atendimentoId.value, leitoInput, ArrayAtendiMoviLeito.dataInicial)
                    ////console.log("edita mov - Atend:", atendimentoId.value, " LeitoId:", leitoInput, " DataFinal:", ArrayAtendiMoviLeito.dataInicial);

                    //OCUPAR LEITO
                    leitoInput = ArrayAtendiMoviLeito.leitoId;
                    ocuparLeito = true;
                    operarLeito(leitoInput, ocuparLeito)
                    ////console.log("ocuparLeito", leitoInput);

                    //CRIAR MOV LEITO
                    criarMovLeito(atendimentoId.value, leitoInput, ArrayAtendiMoviLeito.unidadeOrganizacionalId, ArrayAtendiMoviLeito.dataInicial, ArrayAtendiMoviLeito.dataInclusao)
                    ////console.log("criar mov - Atend:", atendimentoId.value, " LeitoId:", leitoInput, " DataFinal:", ArrayAtendiMoviLeito.dataInicial, " DataInclusao:", ArrayAtendiMoviLeito.dataInclusao);

                    ////console.log("unidadeOrganizacionalId: ", ArrayAtendiMoviLeito.unidadeOrganizacionalId)
                    window.carregarLeitos();

                }
                _modalManager.close();
            }

            else {
                abp.notify.warn(app.localize('SelecioneLista'));
            }
            //carregra o grib de atendimento
            // window.carregaAtendimento();

        };

        function getLeitos(reload) {
            //_$PreAtendimentosTable.jtable('load');
            if (reload) {
                //console.log("sim");
                _$AtendimentosLeitosMovTable.jtable('reload', {
                    id: localStorage["HistoricoAtendimentoId"],
                    //sorting: "DataInicial"
                });
            } else {
                //console.log("não. At: ", atendimentoId.value);
                _$AtendimentosLeitosMovTable.jtable('load', {
                    id: localStorage["HistoricoAtendimentoId"],
                    //sorting: "DataInicial"
                });
                //_$AtendimentosLeitosMovTable.jtable('load', {
                //    filtro: $('#PreAtendimentosFilter').val()
                //});
            }
        }

        _$AtendimentosLeitosMovTable.jtable({
            title: app.localize('AtendimentosLeitosMov'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: abp.services.app.atendimentoLeitoMov.listarFiltro
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '6%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        debugger;

                        if (!$('#isAlta').val()) {
                            if (data.record.isUltimoHistorico) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Trasnferir') + '"><i class="fa fa-edit"></i></button>')
                                   .appendTo($span)
                                   .click(function (e) {
                                       e.preventDefault();
                                       //console.log("data: ",data)
                                       //_createOrEditModal.open({ data: data.record });
                                       editarMovimentoLeito(data.record);
                                   });
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        deleteMovimentoLeito(data.record);
                                    });
                            }
                        }
                        return $span;
                    }
                },
                //codigoAtendimento: {
                //    title: app.localize('CodigoIntenacao'),
                //    width: '3%',
                //    //display: function (data) {
                //    //    if (data.record.atendimento) {
                //    //        return data.record.atendimento.codigo;
                //    //    }
                //    //}
                //},
                leito: {
                    title: app.localize('Leito'),
                    width: '6%',
                    //display: function (data) {

                    //    if (data.record.leito) {
                    //        return data.record.leito.descricao;
                    //    }
                    //}
                },
                tipoLeito: {
                    title: app.localize('tipoLeito'),
                    width: '5%',
                    //display: function (data) {
                    //    if (data.record.leito) {
                    //        if (data.record.leito.tipoAcomodacao) {
                    //            return data.record.leito.tipoAcomodacao.descricao;
                    //        }
                    //    }
                    //}
                }
                ,
                paciente: {
                    title: app.localize('Paciente'),
                    width: '7%',
                    //display: function (data) {
                    //    if (data.record.atendimento) {
                    //        if (data.record.atendimento.paciente) {
                    //            return data.record.atendimento.paciente.nomeCompleto;
                    //        }
                    //    }
                    //}
                }
                ,
                dataInicial: {
                    title: app.localize('DataInicial'),
                    width: '4%',
                    display: function (data) {
                        if (data.record.dataInicial) {
                            return moment(data.record.dataInicial).format('L LT');

                        }
                    },
                },
                dataFinal: {
                    title: app.localize('DataFinal'),
                    width: '4%',
                    display: function (data) {
                        if (data.record.dataFinal) {
                            return moment(data.record.dataFinal).format('L LT');
                        }

                    }
                },
                dataInclusao: {
                    title: app.localize('dataInclusao'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.dataInclusao) {
                            return moment(data.record.dataInclusao).format('L LT');
                        }
                    }
                },
                dataAlta: {
                    title: app.localize('Alta'),
                    width: '6%',
                    display: function (data) {
                        if (data.record.atendimento) {
                            if (data.record.atendimento.dataAlta) {
                                return moment(data.record.atendimento.dataAlta).format('L LT');
                            }
                        } else {
                            return ("");
                        }
                    }
                },
            }
        });

        //_$AtendimentosLeitosMovTable.jtable('load', {
        //    id: atendimentoId.value, sorting: "DataInicial"
        //});

        getLeitos();

        $('.tipoLeito').click(function (e) {
            e.preventDefault();
            _$LeitoTable.jtable('load', { tipoAcomodacao: this.id });
            ////console.log('tipoLeitoId: ', this.id);
        });

        $('#todos').click(function (e) {
            e.preventDefault();
            _$LeitoTable.jtable('load', null);
            ////console.log('todos: ');
        });

        function operarLeito(leito, ocuparLeito) {
            var _url;
            if (ocuparLeito) {
                _url = '/Mpa/Leitos/OcuparLeito';
            } else {
                _url = '/Mpa/Leitos/DesocuparLeito';
            }

            $.ajax({
                type: "POST",
                url: _url,
                data: { leitoId: leito },
                success: function (result) {
                },
                error: function (xhr, ajaxOptions, thrownError) {
                },
                complete: function () { }
            });
            ////console.log("DESOCUPA: ", leitoInput);

        }

        function criarMovLeito(atenId, leitoId, unidOrg, dataInicial, dataInclusao) {
            var _url = '/Mpa/AtendimentoLeitoMov/SalvarAtendimentoLeitoMov';
            $.ajax({
                type: "POST",
                url: _url,
                data: {
                    AtendimentoId: atenId,
                    LeitoId: leitoId,
                    UnidOrg: unidOrg,
                    DataInicial: dataInicial,
                    DataFinal: "",
                    DataInclusao: dataInclusao,
                    Edita: 0
                },
                success: function (result) {
                },

                error: function (xhr, ajaxOptions, thrownError) {
                },
                complete: function () { }
            });
        }

        function editarMovLeito(atenId, leitoId, dataFinal) {
            var _url = '/Mpa/AtendimentoLeitoMov/SalvarAtendimentoLeitoMov';
            $.ajax({
                type: "POST",
                url: _url,
                data: {
                    AtendimentoId: atenId,
                    LeitoId: leitoId,
                    DataFinal: dataFinal,
                    Edita: 1
                },
                success: function (result) {
                },

                error: function (xhr, ajaxOptions, thrownError) {
                },
                complete: function () { }
            });
        }


        function deleteMovimentoLeito(movimento)
        {
            _AtendimentosLeitosMovAppService.excluirMovimentoLeito(movimento.id)
             .done(function (data) {


                 if (data.errors.length > 0) {
                     _ErrorModal.open({ erros: data.errors });
                 }
                 else {

                     getLeitos();


                 }
             })
        }


        

        $('input[name="DataNova"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "timePicker": true,
            "timePicker24Hour": true,
            "startDate": moment(),
            "endDate": moment(),
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
           $('input[name="DataNova"]').val(selDate.format('L LT')).addClass('form-control edited');
       });


        function editarMovimentoLeito(record) {

            $('#dataNova').val(moment(record.dataInicial).format('L LT'));
            $('#movimentoId').val(record.id);

            $('#divDataNova').show();
            
        }


        $('#btnAtualizarMovimento').click(function (e) {
            e.preventDefault();

            _AtendimentosLeitosMovAppService.altarDataMovimentoLeito($('#movimentoId').val(), $('#dataNova').val())
             .done(function (data) {

                 if (data.errors.length > 0) {
                     _ErrorModal.open({ erros: data.errors });
                 }
                 else {
                     $('#divDataNova').hide();
                     getLeitos();

                 }
             })

        });

    };

})(jQuery);
