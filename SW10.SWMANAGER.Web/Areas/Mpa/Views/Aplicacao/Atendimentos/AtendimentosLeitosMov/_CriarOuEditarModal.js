(function ($) {
    app.modals.CriarOuEditarAtendimentoLeitoMovModalViewModel = function () {

        var _AtendimentosLeitosMovAppService = abp.services.app.atendimentoLeitoMov;
        var _Leitos = abp.services.app.leito;

        var _modalManager;
        var _$AtendimentoLeitoMovInformationForm = null;

        var _$LeitoTable = $('#LeitoTable');

        var movLeitos = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$AtendimentoLeitoMovInformationForm = _modalManager.getModal().find('form[name=AtendimentoLeitoMovInformationsTab]');
            _$AtendimentoLeitoMovInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '900px' });
            $('div.form-group select').addClass('form-control selectpicker');

        };

        $('input[name="DataEntrada"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "timePicker" : true,
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

            // criar o objeto do tipo SolicitacaoExameItem
            if (movLeitos.length > 0) {
                _modalManager.setBusy(true);

                for (var i = 0; i < movLeitos.length; i++) {

                    movLeitos[i].isAmbulatorioEmergencia = 0;
                    movLeitos[i].isInternacao = 1;
                    movLeitos[i].isHomeCare = 0;
                    movLeitos[i].isPreatendimento = 0;
                    //movLeitos[i].dataRegistro =  moment().format('L');

                    var ArrayAtendiMoviLeito = new Object();
                    ArrayAtendiMoviLeito.id = Number($('#atendimentoId').val());
                    ArrayAtendiMoviLeito.leitoId = Number(movLeitos[i].id);
                    ArrayAtendiMoviLeito.unidadeOrganizacionalId = Number(movLeitos[i].unidadeOrganizacionalId);
                    //ArrayAtendiMoviLeito.dataRegistro = moment(new Date()).format("L");

                    _AtendimentosLeitosMovAppService.atendimentoEditar(ArrayAtendiMoviLeito.id, ArrayAtendiMoviLeito.leito, ArrayAtendiMoviLeito.unidadeOrganizacionalId);

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
                _$LeitoTable.jtable('reload');
            } else {
                _$LeitoTable.jtable('load', {
                    filtro: $('#PreAtendimentosFilter').val()
                });
            }
        }

        _$LeitoTable.jtable({

            title: app.localize('MapasLeitos'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting'                                                                                                                                                                                                                                                                                                                                                                                                                
            //multiselect: true, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column

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

                unidadeOrganizacional: {
                    title: app.localize('Local'),
                    width: '6%',
                    display: function (data) {
                        if (data.record.unidadeOrganizacional) {
                            return data.record.unidadeOrganizacional.localizacao;
                            //return data.record.leito.unidadeOrganizacional.organizationUnit.displayName;
                        }
                    }
                }
                //,
                //unidadeOrganizacionalID: {
                //    title: app.localize('Local'),
                //    width: '6%',
                //    visible: false,
                //    display: function (data) {
                //        if (data.record.unidadeOrganizacional) {
                //            return data.record.unidadeOrganizacional.id;
                //            //return data.record.leito.unidadeOrganizacional.organizationUnit.displayName;
                //        }
                //    }
                //}
               
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

        //=====================
    };

})(jQuery);
