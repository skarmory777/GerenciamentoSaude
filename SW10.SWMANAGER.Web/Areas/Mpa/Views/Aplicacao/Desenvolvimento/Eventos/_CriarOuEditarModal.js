(function ($) {
    app.modals.CriarOuEditarEventoModalViewModel = function () {
               
        var _VisitantesService = abp.services.app.visitante;
        var _AtendimentosService = abp.services.app.atendimento;
        var _modalManager;
        var _$VisitanteInformationForm = null;

          //passagem de valores para as variaveis
        //var emergencia = $('#chk-isEmergencia').is(':checked');
        //var internado = $('#chk-isInternado').is(':checked');
        //var setor = $('#chk-isSetor').is(':checked');
        //var fornecedor = $('#chk-isFornecedor').is(':checked');
        //var PacienteId = $('#PacienteId').val();
        //var dataSaida = $('#dataSaida').val();

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$VisitanteInformationForm = _modalManager.getModal().find('form[name=VisitanteInformationsTab]');
            _$VisitanteInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '900px' });
            $('div.form-group select').addClass('form-control selectpicker');

            ////console.log("_$VisitanteInformationForm: ",_$VisitanteInformationForm);

           // //console.log(app.modals.CriarOuEditarVisitanteModalViewModel.internado);
          
            if ($('#chk-isFornecedor').is(':checked')) {

                $('.Internados').hide("slow");

                $('.Emergência').hide("slow");
                $('.divUnidadeOrganizacional').show("slow");
                $('.divFornecedores').show("slow");
            }

            if ($('#chk-isSetor').is(':checked')) {

                $('.Internados').hide("slow");

                $('.Emergência').hide("slow");
                $('.divUnidadeOrganizacional').show("slow");
                $('.divFornecedores').hide("slow");
            }

            if ($('#chk-isInternado').is(':checked')) {

                $('.Internados').show("slow");

                $('.Emergência').hide("slow");
                $('.divUnidadeOrganizacional').hide("slow");
                $('.divFornecedores').hide("slow");
            }

            if ($('#chk-isEmergencia').is(':checked')) {

                $('.Internados').hide("slow");

                $('.Emergência').show("slow");
                $('.divUnidadeOrganizacional').hide("slow");
                $('.divFornecedores').hide("slow");
            }

            if (!$('#chk-isFornecedor').is(':checked') && !$('#chk-isInternado').is(':checked') && !$('#chk-isEmergencia').is(':checked') && !$('#chk-isSetor').is(':checked')) {
                $('.Internados').hide("slow");

                $('.Emergência').hide("slow");
                $('.divUnidadeOrganizacional').hide("slow");
                $('.divFornecedores').hide("slow");
            }

        };

        this.save = function () {

            //valida formulario
            if (!_$VisitanteInformationForm.valid()) {
                return;
            }

            //passagem de valores para as variaveis
             emergencia = $('#chk-isEmergencia').is(':checked');
             internado = $('#chk-isInternado').is(':checked');
             setor = $('#chk-isSetor').is(':checked');
             fornecedor = $('#chk-isFornecedor').is(':checked'); 
             PacienteId = $('#PacienteId').val();
             dataSaida = $('#dataSaida').val();
            
             ////console.log('emergencia: ',emergencia);
            //teste das variaveis
            if (emergencia || setor || fornecedor || internado) {

                if (emergencia || internado) {
                    if (PacienteId == 0 && PacienteId2 == 0) {
                        abp.message.info('', 'Selecione um Paciente!');
                        return;
                    }
                }
               
                var visitante = _$VisitanteInformationForm.serializeFormToObject();
                //console.log("visitante:", visitante);

                _modalManager.setBusy(true);
                _VisitantesService.criarOuEditar(visitante)
                     .done(function () {
                         abp.notify.success(app.localize('SavedSuccessfully'));
                         _modalManager.close();
                         abp.event.trigger('app.CriarOuEditarVisitanteModalSaved');
                     })
                    .always(function () {
                        _modalManager.setBusy(false);
                    });

            } else {
                abp.message.info('', 'Selecione o setor da visita !');
            }
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

        $('#capturar-foto').click(function (e) {
            e.preventDefault();
            if ($('#area-captura').html() === '') {
                $('#area-captura').load("/mpa/pacientes/_CarregarFoto", function () {
                    $(this).removeClass('hidden');
                    $('#capturar-foto').html(app.localize('EncerrarCaptura'));
                })
            }
            else {
                if (localMediaStream) {
                    localMediaStream.getVideoTracks()[0].stop();
                }
                $(this).html(app.localize('CapturarFoto'));
                $('#area-captura').html('').addClass('hidden');
            }
        });

        $('.chk').click(function () {
            ////console.log("teste");
            var tipoVisita = $(this).attr("id");
            controleSelec(tipoVisita);
        });

        var controleSelec = function (tipoVisita) {

            if (tipoVisita == "chk-isFornecedor") {

                $('.Internados').hide("slow");

                $('.Emergência').hide("slow");

                $('.divUnidadeOrganizacional').show("slow");

                $('.divFornecedores').show("slow");
            }

            if (tipoVisita == "chk-isSetor") {

                $('.Internados').hide("slow");

                $('.Emergência').hide().slow;

                $('.divFornecedores').hide("slow");

                $('.divUnidadeOrganizacional').show("slow");
            }

            if (tipoVisita == "chk-isInternado") {
             
                $('.Internados').show("slow");

                $('.Emergência').hide("slow");

                $('.divFornecedores').hide("slow");

                $('.divUnidadeOrganizacional').hide("slow");
            }

            if (tipoVisita == "chk-isEmergencia") {

                $('.Emergência').show("slow");

                $('.Internados').hide("slow");

                $('.divFornecedores').hide("slow");

                $('.divUnidadeOrganizacional').hide("slow");
            }

            if (!$('#chk-isFornecedor').is(':checked') && !$('#chk-isInternado').is(':checked') && !$('#chk-isEmergencia').is(':checked') && !$('#chk-isSetor').is(':checked')) {   
                $('.Internados').hide("slow");
                $('.Emergência').hide("slow");
                $('.divUnidadeOrganizacional').hide("slow");
                $('.divFornecedores').hide("slow");
            }
        }

    };

})(jQuery);

//MANIPULANDO DIV PAI DO ELEMENTO
//$('#UnidadeOrganizacionalId')
//  .closest("div")
//  .show("slow");