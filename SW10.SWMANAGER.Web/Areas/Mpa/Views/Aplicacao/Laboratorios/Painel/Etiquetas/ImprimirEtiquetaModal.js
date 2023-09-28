(function ($) {
    
    app.modals.ImprimirEtiquetaModal = function () {
        const resultadoExame = abp.services.app.resultadoExame;
        let modalManager;
        let $modal;
        this.init = function (_modalManager) {
            modalManager = _modalManager;
            $modal = $(_modalManager.getModal());
            $modal.find(".modal-dialog").css({ 'width': '85%'});
        }
        
        this.save = function() {
            const btn = $(this);
            btn.buttonBusy(true);
            const setorBox = $(".setor_box");
            const data = {
                resultadoId: $("#impressora_resultado_id").val(),
                etiquetas:[],
                impressora: $.cookie("impressora_laboratorio")
            };
            
            setorBox.each(function() {
                const currentSetor = $(this);
                let resultadoExameIds;
                if(currentSetor.find(".setor_exames").val() && currentSetor.find(".setor_exames").val().split(',').length ) {
                    resultadoExameIds = currentSetor.find(".setor_exames").val().split(',').map(Number);
                }
                if(currentSetor.find(".checkbox-item-mneumonico").attr("checked")) {
                    data.etiquetas.push({
                        setorId: currentSetor.find(".setor").val(),
                        resultadoExameIds: resultadoExameIds,
                        modelo:"Mneumonico",
                        qtd:  currentSetor.find(".mneumonico_qtd").val(),
                    })
                }

                if(currentSetor.find(".checkbox-item-equipamento").attr("checked")) {
                    data.etiquetas.push({
                        setorId: currentSetor.find(".setor").val(),
                        resultadoExameIds: resultadoExameIds,
                        modelo:"Equipamento",
                        qtd:  currentSetor.find(".equipamento_qtd").val(),
                    })
                }
            })
            resultadoExame.imprimirEtiquetas(data).then(() => {
                abp.message.success("Aguarde enquanto as etiquetas estão sendo impressas","Etiquetas na fila de impressão.");
                modalManager.close();
                abp.event.trigger("app.atualizaGridDetalhamentoColetaExame");
                abp.event.trigger("app.atualizaColeta", {
                    resultadoId: $modal.find("#impressora_resultado_id").val()
                });
            }).always(() => {
                btn.buttonBusy(false);
            })
            
        }

        $('[data-toggle="popover"]').popover({
            container: 'body',
            html:true
        }).on('shown.bs.popover', function () {
            const $pop = $(this);
            setTimeout(function () {
                $pop.popover('hide');
            }, 2000);
        });
        
        $(".checkbox-item").on("change", function() {
           const item = $(this);
           const target = item.data("target");
           if(item.attr('checked')) {
               $(target).removeAttr("readonly")
           }
           else {
               $(target).attr("readonly","readonly")
           }
        });
    }
})(jQuery)