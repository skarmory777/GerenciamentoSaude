(function ($) {

    app.modals.PendenciaModal = function () {
        const resultadoExame = abp.services.app.resultadoExame;
        let modalManager;
        let $modal;
        const dataExames = [];
        const _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });
        this.init = function (_modalManager) {
            modalManager = _modalManager;
            $modal = $(_modalManager.getModal());
            $modal.find(".modal-dialog").css({ 'width': '85%'});
            console.log(modalManager);
            debugger;
            if(modalManager.getArgs().blockClose) {
                $modal.find("#fechar-modal").hide();
                $modal.find(".resultado-exame .text-editor").attr("required","required");
            }
        }

        this.save = function () {
            const resultadoExameForm = $modal.find('form[name=resultadoExameForm]');
            resultadoExameForm.validate();

            if (!resultadoExameForm.valid()) {
                abp.notify.warn("As pendências precisam ser preenchidas!", "Pendências")
                return;
            }
            
            const btn = $(this);
            btn.buttonBusy(true);
            const data = [];
            $(".resultado-exame").each(function () {
                const resultadoExame = $(this);
                data.push({
                    resultadoExameId: resultadoExame.data('id'),
                    observacao: resultadoExame.find(".text-editor").val()
                })
            })

            resultadoExame.adicionarPendencias($modal.find("#resultado_id").val(), data).then(() => {
                abp.notify.success("Pendências adicionadas selecionados com sucesso!", "Pendências")
                modalManager.close();
            }).always(() => {
                abp.event.trigger("app.atualizaGridDetalhamentoColetaExame");
                btn.buttonBusy(false);
            })
        }
    }
})(jQuery)