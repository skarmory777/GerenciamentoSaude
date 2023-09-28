(function () {
    $(function () {
        app.modals.SelecionarModelePrescricaoModal = function () {


            var _modalManager;

            this.init = function (modalManager) {
                _modalManager = modalManager;
            }


            $('#modeloId').on('select2:select', function () {
                copiarPrescricao($('#modeloId').val(), $('#atendimentoId').val());

            });

            function copiarPrescricao(prescricaoId, atendimentoId) {
                //            abp.message.confirm(
                //                app.localize('CopiarPrescricaoWarning'),
                //                function (isConfirmed) {
                //                    if (isConfirmed) {
                swal({
                    title: "Modelo de prescrição",
                    text: "Gerar prescrição a partir do modelo",
                    type: "info",
                    showCancelButton: true,
                    closeOnConfirm: false,
                    showLoaderOnConfirm: true
                }, function () {
                        debugger;

                    $.ajax({
                        url: '/api/services/app/prescricaoMedica/copiar?id=0&atendimentoId=' + atendimentoId + '&modeloPrescricaoId' + prescricaoId,
                        method: 'POST',
                        //data: {
                        //    id: id,
                        //    atendimentoId: localStorage["AtendimentoId"]
                        //},
                        success: function (data) {
                            if (data != null && data != '' && data != undefined) {

                                debugger;
                                swal(app.localize('OperacaoConcluida'), data.result, 'success');
                            }
                            else {
                                abp.notify.success(app.localize('Successfully'));
                            }

                            _modalManager.close();

                        },
                        error: function (request, status, error) {
                            var req = JSON.parse(request.responseText);
                            swal(app.localize('Error'), req.message, 'error');
                        }
                    });
                   
                });
              
            }






            selectSW('.selectModelo', "/api/services/app/ModeloPrescricao/ListarDropdown");

        };










    });
})();