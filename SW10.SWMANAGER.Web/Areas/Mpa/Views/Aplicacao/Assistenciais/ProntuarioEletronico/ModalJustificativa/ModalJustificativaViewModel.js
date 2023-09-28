(function ($) {

    app.modals.JustificativaProntuarioEletronicoModal = function () {
        const prontuarioEletronicoAppService = abp.services.app.prontuarioEletronico;
        const form = $("#formModalJustificativa");
        let _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            $(".save-button").click(saveButton);
        };

        function saveButton () {
            $(this).buttonBusy(true);
            form.validate();
            if (!form.valid()) {
                $(this).buttonBusy(false);
                return;
            }
            let data = form.serializeFormToObject();
            let metodo = data.estaInativo === "true" ? prontuarioEletronicoAppService.reativar : prontuarioEletronicoAppService.excluir;

            metodo(data.id, data.justificativa).done(function () {
                abp.notify.success("Salvo com sucesso.");
                abp.event.trigger("UpdateModalJustificativaViewModel");
                _modalManager.close();
            }).always(function () {
                $(this).buttonBusy(false);
            });            
        };
    };

})(jQuery);