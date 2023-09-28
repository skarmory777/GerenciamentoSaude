(function ($) {
    app.modals.CriarOuEditarContaMedicaModal = function () {
        let modalManager;
        let $modal = null;
        const contaAppService = abp.services.app.conta;
        this.init = function (_modalManager) {
            modalManager = _modalManager;
            $modal = $(_modalManager.getModal());
            $modal.find(".modal-dialog").css({ 'width': '80%'});
            selectSWWithDefaultValue($modal.find('.selectMedico'),'/api/services/app/medico/ListarDropdown');
            selectSWWithDefaultValue($modal.find('.selectConvenio'),'/api/services/app/convenio/ListarDropdown');
            selectSWWithDefaultValue($modal.find('.selectPlano'), "/api/services/app/plano/ListarDropdown", $modal.find('.selectConvenio'));
            selectSWWithDefaultValue($modal.find('.selectTipoLeito'), "/api/services/app/TipoAcomodacao/ListarDropdown");
            selectSWWithDefaultValue($modal.find(".selectMotivoAlta"),"/api/services/app/motivoAlta/ListarDropdown")
            selectSWWithDefaultValue($modal.find(".selectFatGuia"),'/api/services/app/FaturamentoGuia/ListarDropdown')
            calcDiff();
            
            CamposRequeridos();
            createSingleDatePicker($modal);
        }
        
        $(".data-inicio").on('change',calcDiff);

        $(".data-fim").on('change', calcDiff);
        function calcDiff() {
            let inicio = $modal.find(".data-inicio").val();
            let fim = $modal.find(".data-fim").val();
            let momentInicio = moment(inicio,"DD/MM/YYYY HH:mm:ss");
            let momentFim = moment(fim,"DD/MM/YYYY HH:mm:ss");
            if(!momentInicio.isValid() || !momentFim.isValid()) {
                $modal.find(".diffDatas").text("0 dia");
                return;
            }
            const diffDays = momentFim.diff(momentInicio,"days");
            $modal.find(".diffDatas").text(`${diffDays} ${diffDays <= 0? "dia":"dias"}`);
        }
        this.save = function (e) {
            $(this).buttonBusy(true);

            let contaMedicaForm = $('form[name=contaMedicaForm]');

            contaMedicaForm.validate();
            
            if(contaMedicaForm.valid()) {
                contaAppService.criarOuEditar(contaMedicaForm.serializeFormToObject()).then(res=> {
                    abp.message.confirm('Deseja entrar na conta medica?','Conta Medica Criada Com Sucesso', (confirm) => {
                        debugger;
                        const urlData = {
                            atendimentoId: res.atendimentoId,
                            contaMedicaId: res.id
                        };
                        if(confirm) {
                            window.open(`/Mpa/FaturarAtendimento/ContaMedica?${$.param(urlData)}`);
                        }
                        $(this).buttonBusy(false);
                        modalManager.close();
                        abp.event.trigger("app.indexFaturarAtendimentoReload");
                    })    
                });
            }
            else {
                $(this).buttonBusy(false);
            }
        }
    }
})(jQuery);