(function ($) {
    app.modals.impressaoAcrescimosESuspensoesModal = function () {
        let _modalManager = null
        this.init = function (modalManager) {
            _modalManager = modalManager
            $("#impressaoAcrescimosESuspensoesModal .modal-dialog").css("width", '85vw');
            
            
            $(".list-group-item").click(function()  {
                let item = $(this);
                $(".list-group-item").not(item).removeClass("active");
                if(item.hasClass('active')) {
                    item.removeClass('active');
                } else {
                    item.addClass('active');
                }
                exibirRelatorio($(".prescricaoMedicaId").val());
            })

            exibirRelatorio($(".prescricaoMedicaId").val());
        }

        function exibirRelatorio(prescricaoId) {
            let datasAgrupamento = [];
            $(".list-group-item.active").each(function() {
                datasAgrupamento.push($(this).data('dt-agrupamento'));
            })
            
            const obj = { prescricaoId, datasAgrupamento}
            PDFObject.embed(`/mpa/AssistenciaisRelatorios/ImprimirAcrescimosESuspensos?${$.param(obj, true)}`, ".prescricaoAcrescimos");
        }
    }
})(jQuery)