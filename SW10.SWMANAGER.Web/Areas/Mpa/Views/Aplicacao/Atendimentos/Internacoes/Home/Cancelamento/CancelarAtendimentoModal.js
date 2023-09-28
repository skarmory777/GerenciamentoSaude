(function ($) {
    app.modals.CancelarAtendimentoModal = function () {

        var _modalManager;


        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        $(document).ready(function () {
            CamposRequeridos();
           
        });

        var _AtendimentosService = abp.services.app.atendimento;

        this.save = function () {

            //alert('kdsjfkasdfasdf');

            _AtendimentosService.excluir($('#atendimentoId').val(), $('#motivoCancelamentoId').val())
                .done(function () {
                  //  getAtendimentos();
                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                    _modalManager.close();
                    abp.event.trigger('app.AltaModalViewModel');
                });


           
            //_$contaAdministrativaInformationsForm.validate()
            //if (!_$contaAdministrativaInformationsForm.valid()) {
            //    return;
            //}

            //var contaAdministrativa = _$contaAdministrativaInformationsForm.serializeFormToObject();

            //contaAdministrativa.IsReceita = contaAdministrativa.options == 'isReceita';
            //contaAdministrativa.IsDespesa = contaAdministrativa.options == 'isDespesa';

            //_modalManager.setBusy(true);
            //_contaAdministrativaService.criarOuEditar(contaAdministrativa)
            //     .done(function (data) {
            //         if (data.errors.length > 0) {
            //             _ErrorModal.open({ erros: data.errors });
            //         }
            //         else {

            //             abp.notify.info(app.localize('SavedSuccessfully'));
            //             _modalManager.close();
            //             abp.event.trigger('app.CriarOuEditarFeriadoModalSaved');
            //             //location.reload();//seguindo o projeto pronto
            //         }
            //     })
            //    .always(function () {
            //        _modalManager.setBusy(false);
            //    });
        };

        selectSW('.selectMotivoCancelamento', "/api/services/app/AteMotivoCancelamento/ListarDropdown");

};
})(jQuery);