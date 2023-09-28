(function () {
    $(function () {

        
        // Contas Medicas
        $('#btn-contas-medicas').on('click', function(e){
            e.preventDefault();
            var registroSelecionado = $('#AtendimentosTable').jtable('registroSelecionado');
            var atendimentoId = registroSelecionado.id;

          abp.services.app.conta.obterUltimaContaAtendimentoId(atendimentoId)
             .done(function (data) {
                 //debugger
                 //if (data == 0) {
                 //    abp.notify.warn('Nenhuma conta registrada para este atendimento.');
                 //    return;
                 //}

                 var _createOrEditModal = new app.ModalManager({
                     viewUrl: abp.appPath + 'Mpa/ContasMedicas/ContasMedicasPorAtendimentoModal',
                     scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Faturamentos/ContasMedicas/_CriarOuEditarModal.js',
                     modalClass: 'CriarOuEditarContaMedicaModal'
                 });

                 _createOrEditModal.open({ id: data, atendimentoId: atendimentoId });
             })
            .always(function () {

            });

      });


    });
})();
