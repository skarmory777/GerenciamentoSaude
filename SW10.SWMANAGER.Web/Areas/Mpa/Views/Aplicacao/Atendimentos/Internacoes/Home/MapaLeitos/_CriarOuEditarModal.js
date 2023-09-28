(function ($) {
    app.modals.TransferenciaLeitoMovModalViewModel = function () {

        var _AtendimentosLeitosMovAppService = abp.services.app.atendimentoLeitoMov;
        var _Leitos = abp.services.app.leito;
        var _modalManager;
        var _$AtendimentoLeitoMovInformationForm = null;
        var _$LeitoTable = $('#LeitoTable');
        var movLeitos = [];
        var desocuparLeito;
        var dataRegistro;
        var dataInclusao;

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

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        //=====================================
        this.save = function () {
            // criar o objeto do tipo SolicitacaoExameItem
            if (movLeitos.length > 0) {
                _modalManager.setBusy(true);

                var leitoOrigemId = $('#desocuparLeito').val();
                var leitoDestinoId = movLeitos[0].id;
                var atendimentoDestinoId = movLeitos[0].atendimentoId;
                var atendimentoId = $('#atendimentoId').val();
                var dataHoraTransferencia = $('#dataInicial').val();

                _AtendimentosLeitosMovAppService.transferirLeito(leitoOrigemId, leitoDestinoId, atendimentoId, dataHoraTransferencia, atendimentoDestinoId)
                .done(function (data) {
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {

                        abp.notify.info(app.localize('SavedSuccessfully'));

                        _modalManager.setBusy(false);
                        abp.event.trigger('app.CriarOuEditarLeitoMovimentoModalSaved');
                        $('#aba-principal2 a').trigger('click');

                    }
                })
          .always(function () {
          });




                //for (var i = 0; i < movLeitos.length; i++) {

                //    movLeitos[i].isAmbulatorioEmergencia = 0;
                //    movLeitos[i].isInternacao = 1;
                //    movLeitos[i].isHomeCare = 0;
                //    movLeitos[i].isPreatendimento = 0;
                //    //movLeitos[i].dataRegistro =  moment().format('L');

                //    var ArrayAtendiMoviLeito = new Object();
                //    ArrayAtendiMoviLeito.id = Number($('#atendimentoId').val());
                //    ArrayAtendiMoviLeito.leitoId = Number(movLeitos[i].id);
                //    ArrayAtendiMoviLeito.unidadeOrganizacionalId = Number(movLeitos[i].unidadeOrganizacionalId);
                //    debugger
                //    // CORRECAO DATA INICIAL AO TRANSERIR LEITO
                //    var dtInicial = $('#dataInicial').val();
                //    ArrayAtendiMoviLeito.dataInicial = dtInicial; // new Date(dtInicial);

                //    ArrayAtendiMoviLeito.dataInclusao = dataInclusao;

                //    var ocuparLeito;

                //    //DESOCUPA LEITO
                //    leitoInput = $('#desocuparLeito').val();
                //    ocuparLeito = false;
                //    operarLeito(leitoInput, ocuparLeito)
                //    //EDITA MOV LEITO
                //    editarMovLeito(atendimentoId.value, leitoInput, ArrayAtendiMoviLeito.dataInicial)

                //    //OCUPAR LEITO
                //    leitoInput = ArrayAtendiMoviLeito.leitoId;
                //    ocuparLeito = true;
                //    operarLeito(leitoInput, ocuparLeito)

                //    //CRIAR MOV LEITO
                //    criarMovLeito(atendimentoId.value, leitoInput, ArrayAtendiMoviLeito.unidadeOrganizacionalId, ArrayAtendiMoviLeito.dataInicial, ArrayAtendiMoviLeito.dataInclusao)
                //    window.carregarLeitos();

                //}
                // window.carregarLeitos();
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
                _$LeitoTable.jtable('reload');
            } else {
                _$LeitoTable.jtable('load', {
                    filtro: $('#PreAtendimentosFilter').val()
                    , somenteInternados: $('#cbLeitosOcupados').is(':checked')
                });
            }
        }

        _$LeitoTable.jtable({

            title: app.localize('MapasLeitos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            selectingCheckboxes: true,

            actions: {
                listAction: {
                    method: abp.services.app.leito.listar,
                    data: 'Internado'
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                }
                ,


                status: {
                    title: app.localize('Status'),
                    width: '5%',
                    display: function (data) {

                        if (data.record.leitoStatus) {
                            var cor = data.record.leitoStatus.cor;
                            var descricao = data.record.leitoStatus.descricao;
                            return '<div style="text-align:center; display:inline-block;">   <span style="display:inline-block; width:20px; height:20px;  text-align:center; background-color: ' + cor + '; border-radius: 25px;">  </span> </div>    ' + data.record.leitoStatus.descricao;
                        }
                    }
                }
                ,
                leito: {
                    title: app.localize('Leito'),
                    width: '4%',
                    display: function (data) {

                        return data.record.descricao;
                        //return data.record.leito.unidadeOrganizacional.organizationUnit.displayName;

                    }
                }
                ,
                tipoLeito: {
                    title: app.localize('tipoLeito'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.tipoAcomodacao) {
                            return data.record.tipoAcomodacao.descricao;
                        }
                    }
                }
                ,

                unidadeOrganizacional: {
                    title: app.localize('Local'),
                    width: '6%',
                    display: function (data) {
                        if (data.record.unidadeOrganizacional) {
                            return data.record.unidadeOrganizacional.localizacao;
                        }
                    }
                },

                paciente: {
                    title: app.localize('Paciente'),
                    width: '20%',
                    display: function (data) {
                        if (data.record.unidadeOrganizacional) {
                            return data.record.paciente;
                        }
                    }
                }
            }
            ,

            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = $('#LeitoTable').jtable('selectedRows');
                if ($selectedRows.length > 0) {
                    //Show selected rows
                    var list = [];
                    var i = 0;
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        list[i] = record;
                        i++;
                    })
                    movLeitos = [];
                    movLeitos = list;
                }
            }
        });

        getLeitos();

        $('.tipoLeito').click(function (e) {
            e.preventDefault();
            _$LeitoTable.jtable('load', { tipoAcomodacao: this.id, somenteInternados: $('#cbLeitosOcupados').is(':checked') });
        });

        $('.unidade').click(function (e) {
            e.preventDefault();
            _$LeitoTable.jtable('load', { unidadeId: this.id, somenteInternados: $('#cbLeitosOcupados').is(':checked') });
        });




        $('#todosid').click(function (e) {
            e.preventDefault();
            _$LeitoTable.jtable('load', {
                somenteInternados: $('#cbLeitosOcupados').is(':checked')
            });
        });


        $('#cbLeitosOcupados').change(function (e) {
            getLeitos();
        })


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
                    abp.event.trigger('app.CriarOuEditarLeitoMovimentoModalSaved');
                    $('#aba-principal2 a').trigger('click');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                },
                complete: function () {
                    abp.event.trigger('app.CriarOuEditarLeitoMovimentoModalSaved');
                    $('#aba-principal2 a').trigger('click');
                }
            });

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
                    abp.event.trigger('app.CriarOuEditarLeitoMovimentoModalSaved');
                    $('#aba-principal2 a').trigger('click');
                },

                error: function (xhr, ajaxOptions, thrownError) {
                },
                complete: function () {
                    abp.event.trigger('app.CriarOuEditarLeitoMovimentoModalSaved');
                }
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




        $('input[name="dtIni"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            //  maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            timePicker: true,
            timePicker24Hour: true,

            onChange: function (date) {
                alert(date)
            }, //changeSolicitacao(),

            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY HH:mm" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD HH:mm",
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
        $('input[name="dtIni"]').val(selDate.format('L LT')).addClass('form-control edited');
    });

    };

})(jQuery);
