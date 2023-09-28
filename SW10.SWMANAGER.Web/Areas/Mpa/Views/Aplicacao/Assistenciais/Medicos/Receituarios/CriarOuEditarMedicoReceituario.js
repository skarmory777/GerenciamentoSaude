(function ($) {
    var _receituariosService = abp.services.app.receituarioMedico;

    function initMemed() {
        var script = document.createElement('script');

        script.dataset.color = '#3598dc';
        script.dataset.token = $("#medicoPrescritorMemedToken").val();
        script.dataset.container = 'memedContainer';

        script.src = 'https://integrations.memed.com.br/modulos/plataforma.sinapse-prescricao/build/sinapse-prescricao.min.js';

        script.addEventListener('load', function () {
            initEventsMemed();
        });

        document.body.appendChild(script);
    }

    function initEventsMemed() {
        MdSinapsePrescricao.event.add('core:moduleInit', (module) => {
            if (module.name === 'plataforma.prescricao') {
                MdHub.command.send('plataforma.prescricao', 'setFeatureToggle', {
                    deletePatient: false,
                    removePatient: false,
                    editPatient: false,
                    conclusionModalEdit: false,
                    buttonClose: true,
                    allowShareModal: true
                });

                MdHub.command.send('plataforma.prescricao', 'setPaciente', {
                    external_id: $("#pacienteExternalId").val(),
                    nome: $("#pacienteNome").val(),
                    cpf: $("#pacienteCpf").val(),
                    telefone: $("#pacienteTelefone").val()
                }).then(function () {
                    $('.pre-loading').hide();
                    MdHub.module.show('plataforma.prescricao');
                });

                MdHub.event.add('prescricaoImpressa', function (prescriptionData) {
                    console.log(prescriptionData);

                    var receitaDocumentoCompletoId = prescriptionData.prescricao.documents.filter(function (obj) {
                        return (obj.type === "full");
                    })[0].uuid;

                    var data = {
                        atendimentoId: $("#atendimentoId").val(),
                        receituarioId: $("#receituarioId").val(),
                        pacienteMemedId: prescriptionData.prescricao.paciente.id,
                        prescricaoMemedId: prescriptionData.prescricao.id,
                        receitaDocumentoCompletoId: receitaDocumentoCompletoId
                    }	

                    _receituariosService.salvarDadosDaReceitaMemed(data);
                });
            }
        });

        MdSinapsePrescricao.event.add('core:moduleHide', (memedModule) => {
            if (memedModule.name === 'plataforma.prescricao') {
                //$("#link-atendimento-" + $("#atendimentoId").val() + '-' + $("#receituarioId").val()).tab("hide");
                console.log('====== Módulo fechado ======', memedModule);
            }
        });
    }

    initMemed();
})(jQuery)