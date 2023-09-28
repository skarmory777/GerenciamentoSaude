(function ($) {
    // app.modals.CriarOuEditarModal = function () {

    $(document).ready(function () {
        CamposRequeridos();

        //   $('.modal-dialog').css('width', '900px');


        $('#motivoDiscordancia').summernote('code', $('#hdnMotivoDiscordancia').val());
        $('#justificativaContraste').summernote('code', $('#hdnJustificativaContraste').val());
        $('#comentario2').summernote('code', $('#hdnComentario').val());
        $('#laudo').summernote('code', $('#hdnLaudo').val());

    });

    var _registroExemesService = abp.services.app.registroExemes;
    var _atendimentoService = abp.services.app.atendimento;
    var _laudoModeloLaudoService = abp.services.app.modeloLaudo;

    var _modalManager;
    var _$registroExameInformationsForm = null;
    var _$exameTable = $('#exameTable');

    var _ErrorModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
    });

    //this.init = function (modalManager) {
    //    _modalManager = modalManager;

    //    _$registroExameInformationsForm = $('form[name=RegistroExameInformationsForm]');
    //    _$registroExameInformationsForm.validate();
    //};

    $('#salvar').click(function (e) {
        e.preventDefault()




        _$registroExameInformationsForm = $('form[name=RegistroExameInformationsForm]');
        _$registroExameInformationsForm.validate();

        if (!_$registroExameInformationsForm.valid()) {
            return;
        }


        var registroExame = _$registroExameInformationsForm.serializeFormToObject();
        registroExame.parecer = $('#parecer').summernote('code');
        registroExame.laudo = $('#laudo').summernote('code');
        registroExame.revisao = $('#revisao').summernote('code');
        registroExame.comentarioLaudo = $('#comentario2').summernote('code');
        registroExame.IsSolicitacaoRevisao = $('#isSolicitacaoRevisao')[0].checked;
        registroExame.isIndicativo = $('#isIndicativo')[0].checked;
        registroExame.justificativaContraste = $('#justificativaContraste').summernote('code');
        registroExame.motivoDiscordancia = $('#motivoDiscordancia').summernote('code');



        _registroExemesService.registrarLaudo(registroExame)
             .done(function (data) {
                 if (data.errors.length > 0) {
                     _ErrorModal.open({ erros: data.errors });
                 }
                 else {

                     abp.notify.info(app.localize('SavedSuccessfully'));

                     location.href = '/mpa/GestaoLaudos';

                 }
             })
            .always(function () {
            });
    });

    $('.close').on('click', function () {
        location.href = '/mpa/GestaoLaudos';
    });

    $('.close-button').on('click', function () {
        location.href = '/mpa/GestaoLaudos';
    });

    var lista = [];

    $('input[name="LaudoData"]').daterangepicker({
        "singleDatePicker": true,
        "showDropdowns": true,
        autoUpdateInput: false,
        //  maxDate: new Date(),
        changeYear: true,
        yearRange: 'c-10:c+10',
        showOn: "both",

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
    $('input[name="LaudoData"]').val(selDate.format('L LT')).addClass('form-control edited');
});






    $('#laudo').summernote({
        height: 150,
        minHeight: 30,

    });

    $('#revisao').summernote({
        height: 150,
        minHeight: 30,

    });

    $('#parecer').summernote({
        height: 150,
        minHeight: 30,
    });

    $('#comentario2').summernote({
        height: 150,
        minHeight: 30,
    });

    $('#justificativaContraste').summernote({
        height: 150,
        minHeight: 30,
    });

    $('#motivoDiscordancia').summernote({
        height: 150,
        minHeight: 30,
    });



    function carregarTextLaudo() {
        $('#txtParecer').prepend(' <div class="col-sm-11" style="padding:20px;"><div class="row"><div class="col-sm-11"><span>' + $('#hdnParecer').val() + '</span></div></div></div>');
        $('#txtLaudo').prepend(' <div class="col-sm-11" style="padding:10px;"><div class="row"><div class="col-sm-12"><span>' + $('#hdnLaudo').val() + '</span></div></div></div>');
        $('#txtRevisao').prepend(' <div class="col-sm-11" style="padding:10px;"><div class="row"><div class="col-sm-12"><span>' + $('#hdnRevisao').val() + '</span></div></div></div>');
        $('#txtComentario').prepend(' <div class="col-sm-11" style="padding:10px;"><div class="row"><div class="col-sm-12"><span>' + $('#hdnComentario').val() + '</span></div></div></div>');
        $('#txtJustificativaContraste').prepend(' <div class="col-sm-11" style="padding:10px;"><div class="row"><div class="col-sm-12"><span>' + $('#hdnJustificativaContraste').val() + '</span></div></div></div>');
        $('#txtMotivoDiscordancia').prepend(' <div class="col-sm-11" style="padding:10px;"><div class="row"><div class="col-sm-12"><span>' + $('#hdnMotivoDiscordancia').val() + '</span></div></div></div>');

    }

    carregarTextLaudo();

    selectSWMultiplosFiltros('.selectModeloLaudo', "/api/services/app/ModeloLaudo/ListarDropdownPorExame", ['exameId']);

    $('#modeloLaudoId').on('change', function (e) {
        e.preventDefault();

        var summernote = ''

        if ($('#status').val() == 1 && $('#isParecer').val() == 'True') {
            summernote = $('#parecer');
        }
        else {
            summernote = $('#laudo');
        }



        if ($('#modeloLaudoId').val() != '' && $('#modeloLaudoId').val() != null) {
            _laudoModeloLaudoService.obter($('#modeloLaudoId').val())
               .done(function (data) {
                   //var snVal = $('.note-editable.panel-body p').text(); //summernote.val() != '';
                   //if (snVal) {
                   //    var laudo = summernote.summernote('code');
                   //    summernote.summernote("code", laudo + "<br />" + data.modelo);
                   //}
                   //else {
                   var laudo = summernote.summernote('code');
                   laudo.replace(/[&\/\\#,+()$~%.'":*?<>{}]/g, '_');
                   laudo.replace('_p__br___p_', '');
                   summernote.summernote("code", laudo + "<br />" + data.modelo);
                   //}

                   //var snVal = $('.note-editable.panel-body p').text(); //summernote.val() != '';
                   //if (snVal) {
                   //var laudo = summernote.summernote('code');
                   //}
                   //summernote.summernote();
                   //summernote.summernote('reset');

                   //if (snVal) {
                   //var code = summernote.summernote('code');
                   //summernote.summernote('editor.pasteHTML', '<br />');
                   //summernote.summernote('insertNode', node);
                   //summernote.summernote('editor.pasteHTML', laudo);
                   //data.modelo = '<br>' + data.modelo;

                   //}
                   //summernote.summernote('editor.pasteHTML', data.modelo);
                   //summernote.summernote('editor.pasteHTML', data.modelo);
                   //summernote.summernote('code', laudo + data.modelo);
               });
        }
    });










    // }
})(jQuery);