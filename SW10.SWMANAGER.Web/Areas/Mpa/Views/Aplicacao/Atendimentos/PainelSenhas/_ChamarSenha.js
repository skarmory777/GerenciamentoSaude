(function ($) {
    app.modals.ChamarSenhaModal = function () {

        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        selectSW('.selectTipoLocalChamada', "/api/services/app/TipoLocalChamada/ListarTipoLocalChamadaDropdown");
        selectSW('.selectLocalChamada', "/api/services/app/LocalChamadas/ListarLocalChamadaPorTipoDropdown", $('#cbo-tipo-local-chamada-id'));
        //selectSW('.selectSenha', "/api/services/app/Senha/ListarSenhasPorlocalChamadaDropdown", $('#local-chamada-id'));


        $('#btn-chamar-senha').on('click', function (e) {
            e.preventDefault();
            var _terminalSenhasService = abp.services.app.terminalSenhas;

            _terminalSenhasService.chamarSenha(
                     $('#cbo-tipo-local-chamada-id').val(),
                     $('#cbo-local-chamada-id').val(),
                     $('#txt-senha-id').val() //$('#movimentacao-senha-id').val()
                 );

            debugger;
          
           // $.cookie('localChamada', $('#@localChamadaId').val());

            _modalManager.close();
            abp.notify.success(app.localize('SenhaEnviada'));

            $.cookie("localChamada", $('#cbo-local-chamada-id').val());

        });

        CamposRequeridos();
    };
})(jQuery);