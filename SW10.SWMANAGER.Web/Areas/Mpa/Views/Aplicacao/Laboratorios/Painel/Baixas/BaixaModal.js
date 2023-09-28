(function ($) {
    
    app.modals.BaixaModal = function () {
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
            $modal.find(".remove-item").click(onRemoveItem);
            $modal.find(".undo-item").click(onUndoItem);

            selectSW("#tecnico-id","/api/services/app/tecnico/ListarDropdown")
            let index = 0;
            $modal.find(".resultadoExameAnchor").each(function() {
                const resultadoExameAnchor = $(this);
                dataExames.push({
                    isActive:false,
                    selected:true,
                    resultadoExameId: resultadoExameAnchor.find(".resultadoExameId").val(),
                    materialId: resultadoExameAnchor.find(".resultadoExameMaterialId").val(),
                    resultadoExameMaterialDesc: resultadoExameAnchor.find(".resultadoExameMaterialDesc").val(),
                    observacao: resultadoExameAnchor.find(".observacao").val()
                })
            })

            $modal.find(".resultadoExameAnchor").click(function () {
                const resultadoExameAnchor = $(this);
                onResultadoExameAnchor(resultadoExameAnchor)
            })
        }
        
        function onResultadoExameAnchor(resultadoExameAnchor) {
            const dataExame = _.find(dataExames, x=> x.resultadoExameId == resultadoExameAnchor.find(".resultadoExameId").val());
            resultadoExameAnchor.data("active", !resultadoExameAnchor.data("active"))

            if(resultadoExameAnchor.data("active")) {
                resultadoExameAnchor.addClass("active");
            } else {
                resultadoExameAnchor.removeClass("active");
            }
            dataExame.isActive = resultadoExameAnchor.data("active")
            upsert(dataExames,{resultadoExameId: resultadoExameAnchor.find(".resultadoExameId").val()}, dataExame)
            atualizaListaExames();

            function upsert(arr, key, newval) {
                const match = _.find(arr, key);
                if(match){
                    const index = _.indexOf(arr, _.find(arr, key));
                    arr.splice(index, 1, newval);
                } else {
                    arr.push(newval);
                }
            }
        }

        function onResultadoExameAnchorOnDelete(resultadoExameAnchor) {
            const dataExame = _.find(dataExames, x=> x.resultadoExameId == resultadoExameAnchor.find(".resultadoExameId").val());
            resultadoExameAnchor.data("active", false)

            if(resultadoExameAnchor.data("active")) {
                resultadoExameAnchor.addClass("active");
            } else {
                resultadoExameAnchor.removeClass("active");
            }
            dataExame.isActive = resultadoExameAnchor.data("active")
            upsert(dataExames,{resultadoExameId: resultadoExameAnchor.find(".resultadoExameId").val()}, dataExame)
            atualizaListaExames();

            
        }

        function upsert(arr, key, newval) {
            const match = _.find(arr, key);
            if(match){
                const index = _.indexOf(arr, _.find(arr, key));
                arr.splice(index, 1, newval);
            } else {
                arr.push(newval);
            }
        }
        
        this.save = function () {
            const btn = $(this);
            btn.buttonBusy(true);

            const materiais = [];
            
            $(".material").each(function () {
                const materialItem = $(this);
                materiais.push({
                    materialId: materialItem.data("material-id"),
                    observacao: materialItem.find(".text-editor").val()
                });
            })
            
            const dataExamesFiltered =  _.filter(dataExames,x => x.selected);
            debugger;
            if(dataExamesFiltered.length == 0) {
                abp.notify.error("Não há nenhum exame para dar baixa!", "Baixa Exames")
                return;
            }
            
            const data = [];
            _.forEach(dataExamesFiltered, (exame) => {
                const material = _.findLast(materiais, x=> x.materialId == exame.materialId);
                const observacao = exame.isActive ? $modal.find(`.panel-exames-${exame.resultadoExameId} .text-editor`).val(): exame.observacao;
                data.push({
                    resultadoExameId: exame.resultadoExameId,
                    observacao: observacao,
                    materialDescricaoLocal : material ? material.observacao : "",
                })
            })
            
            resultadoExame.registrarBaixa($modal.find("#resultado_id").val(), $modal.find("#tecnico-id").val(), data).then(data => {
                    if (data.errors.length > 0) {
                        errorHandler(data.errors, 'Erro ao dar baixa nos exames ');
                        return;
                    }
                    abp.notify.success("Baixa dos exames selecionados com sucesso!", "Baixa Exames")
                    modalManager.close();
            }).always(() => {
                
                abp.event.trigger("app.atualizaGridDetalhamentoColetaExame");
                abp.event.trigger("app.baixaColetaExame", {
                    resultadoId: $modal.find("#resultado_id").val(),
                    resultadoExameIds: dataExamesFiltered.map(x=> x.resultadoExameId)
                });
                abp.event.trigger("app.atualizaColeta", {
                    resultadoId: $modal.find("#resultado_id").val()
                });
                btn.buttonBusy(false);    
            })
            
            
        }
        
        function atualizaListaExames() {
            let html = "";
            _.forEach(_.filter(dataExames, x=> x.isActive) , (item) => {
                html += " "+ getTemplate(item.resultadoExameId, item.resultadoExameMaterialDesc,item.observacao);
            })
            if(html == "") {
                $modal.find(".row-content").addClass("hidden");
            } else {
                $modal.find(".row-content").removeClass("hidden");
                $modal.find(".content-exames").empty().html(html);
            }
            function getTemplate(exameId,exameDesc,observacao) {
                return template = `
                <div class="col-md-6">
                    <div class="panel panel-default panel-exames-${exameId}" data-exame-id="${exameId}" >
                        <div class="panel-heading font-weight-bold" style="font-size: 15px"> ${exameDesc}</div>
                        <div class="panel-body">
                             <textarea class="form-control text-editor">${observacao}</textarea>
                        </div>
                    </div>
                </div>`
            }
        }

        function onRemoveItem(e) {
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const anchor = btn.parents("a.resultadoExameAnchor");
            
            anchor.data("selected",false);
            anchor.data("active",false);
            anchor.find(".undo-item").removeClass("hidden");
            btn.addClass("hidden").buttonBusy(false);

            atualizaListaExames()
            onResultadoExameAnchorOnDelete(anchor);

            const dataExame = _.find(dataExames, x=> x.resultadoExameId == anchor.find(".resultadoExameId").val());
            dataExame.selected = anchor.data("selected")
            upsert(dataExames,{resultadoExameId: anchor.find(".resultadoExameId").val()}, dataExame)
            
        }

        function onUndoItem(e) {
            e.stopImmediatePropagation();
            const btn = $(this);
            btn.buttonBusy(true);
            const anchor = btn.parents("a.resultadoExameAnchor");
            anchor.data("selected",true);
            anchor.find(".remove-item").removeClass("hidden");
            btn.addClass("hidden").buttonBusy(false);
            
            const dataExame = _.find(dataExames, x=> x.resultadoExameId == anchor.find(".resultadoExameId").val());
            dataExame.selected = anchor.data("selected")
            upsert(dataExames,{resultadoExameId: anchor.find(".resultadoExameId").val()}, dataExame)
        }

    }
})(jQuery)